﻿using Cosmos.Common.Extensions;
using Cosmos.Core;
using Cosmos.HAL;
using System;
using Waterfall.System.Core.CLI;

namespace Waterfall.System.Core.Drivers.PS2
{
    public class PS2Controller
    {
        public CLILogs logs;
        private enum Command : byte
        {
            GetConfigurationByte = 0x20,
            SetConfigurationByte = 0x60,
            DisableSecondPS2Port = 0xA7,
            EnableSecondPS2Port = 0xA8,
            TestSecondPS2Port = 0xA9,
            TestPS2Controller = 0xAA,
            TestFirstPS2Port = 0xAB,
            DisableFirstPS2Port = 0xAD,
            EnableFirstPS2Port = 0xAE,
            WriteNextByteToSecondPS2PortInputBuffer = 0xD4,
            PulseOutputLineBase = 0xF0
        }

        private enum DeviceCommand : byte
        {
            IdentifyDevice = 0xF2,
            DisableScanning = 0xF5
        }

        [Flags]
        private enum OutputLines
        {
            None = 0x00,
            First = 0x01,
            Second = 0x02,
            Third = 0x04,
            Fourth = 0x08,
            All = First | Second | Third | Fourth
        }

        public const byte Ack = 0xFA;
        public const uint WAIT_TIMEOUT = 100000;

        public bool IsDualChannel;
        public bool SelfTestPassed;
        public bool FirstPortTestPassed;
        public bool SecondPortTestPassed;

        public Device FirstDevice;
        public Device SecondDevice;

        /// <summary>
        /// Initializes the PS/2 controller.
        /// </summary>
        public void Initialize(bool InitScrollWheel)
        {
            // http://wiki.osdev.org/%228042%22_PS/2_Controller#Initialising_the_PS.2F2_Controller
            // Modifed version of CosmosPS2
            logs.WriteInfo("Initializing PS2 Controller");
            // Disable Devices
            SendCommand(Command.DisableFirstPS2Port);
            SendCommand(Command.DisableSecondPS2Port);

            // Flush The Output Buffer
            while (WaitToReadData())
            {
                ReadData();
            }

            // Set the Controller Configuration Byte
            SendCommand(Command.GetConfigurationByte);

            var xConfigurationByte = ReadData();
            // check if the controller is dual channel
            IsDualChannel = (xConfigurationByte & (1 << 5)) != 0;
            xConfigurationByte = (byte)(xConfigurationByte & ~0b0000_0011);

            SendCommand(Command.SetConfigurationByte, xConfigurationByte);

            // Perform Controller Self Test
            SendCommand(Command.TestPS2Controller);
            SelfTestPassed = ReadData() == 0x55;

            // Determine If There Are 2 Channels
            if (IsDualChannel)
            {
                SendCommand(Command.EnableSecondPS2Port);

                SendCommand(Command.GetConfigurationByte);
                xConfigurationByte = ReadData();

                IsDualChannel = (xConfigurationByte & (1 << 5)) == 0;

                if (IsDualChannel)
                {
                    SendCommand(Command.DisableSecondPS2Port);
                }
            }

            // Perform Interface Tests
            FirstPortTestPassed = TestPort(1);

            if (IsDualChannel)
            {
                SecondPortTestPassed = TestPort(2);
            }

            // Enable Devices
            if (FirstPortTestPassed)
            {
                SendCommand(Command.EnableFirstPS2Port);
                // enable interrupt
                xConfigurationByte |= 0b01;
            }

            if (SecondPortTestPassed)
            {
                SendCommand(Command.EnableSecondPS2Port);
                // enable interrupt
                xConfigurationByte |= 0b10;
            }

            SendCommand(Command.SetConfigurationByte, xConfigurationByte);

            if (FirstPortTestPassed)
            {
                IdentifyDevice(1, out FirstDevice, InitScrollWheel);
            }

            if (SecondPortTestPassed)
            {
                IdentifyDevice(2, out SecondDevice, InitScrollWheel);
            }
        }

        /// <summary>
        /// Identifies a PS/2 device.
        /// </summary>
        /// <param name="aPort">The port of the PS/2 device to identify.</param>
        /// <param name="aDevice">An instance of the identified device.</param>
        private void IdentifyDevice(byte aPort, out Device aDevice, bool InitScrollWheel)
        {
            aDevice = null;

            if (aPort == 1 || aPort == 2)
            {
                var xSecondPort = aPort == 2;

                WaitToWrite();
                SendDeviceCommand(DeviceCommand.DisableScanning, xSecondPort);

                WaitToWrite();
                SendDeviceCommand(DeviceCommand.IdentifyDevice, xSecondPort);

                byte xFirstByte = 0;
                byte xSecondByte = 0;

                if (ReadByteAfterAckWithTimeout(ref xFirstByte))
                {
                    /*
                     * |--------|---------------------------|
                     * |  Byte  |  Device Type              |
                     * |--------|---------------------------|
                     * |  0x00  |  Standard PS/2 mouse      |
                     * |--------|---------------------------|
                     * |  0x03  |  Mouse with scroll wheel  |
                     * |--------|---------------------------|
                     * |  0x04  |  5-button mouse           |
                     * |--------|---------------------------|
                     * |  0x50  |  Laptop Touchpad          |
                     * |--------|---------------------------|
                     */
                    bool InitBytes;
                    if (InitScrollWheel)
                    {
                        InitBytes = xFirstByte == 0x00 || xFirstByte == 0x03 || xFirstByte == 0x04 || xFirstByte == 0x50;
                    }
                    else
                    {
                        InitBytes = xFirstByte == 0x00 || xFirstByte == 0x04 || xFirstByte == 0x50;
                        logs.WriteWarn("Mousewheel detection disabled");
                    }
                    if (InitBytes)
                    {
                        var xDevice = new PS2Mouse(aPort, xFirstByte, this, logs);
                        xDevice.Initialize();

                        aDevice = xDevice;
                    }
                    /*
                     * |-----------------|----------------------------------------------------------------|
                     * |  Bytes          |  Device Type                                                   |
                     * |-----------------|----------------------------------------------------------------|
                     * |  0xAB, 0x41     |  MF2 keyboard with translation enabled in the PS/2 Controller  |
                     * |  or 0xAB, 0xC1  |  (not possible for the second PS/2 port)                       |
                     * |-----------------|----------------------------------------------------------------|
                     * |  0xAB, 0x83     |  MF2 keyboard                                                  |
                     * |-----------------|----------------------------------------------------------------|
                     */
                    else if (xFirstByte == 0xAB && ReadDataWithTimeout(ref xSecondByte))
                    {
                        var xTest = xSecondByte == 0x41 || xSecondByte == 0xC1;

                        if (xTest && aPort == 1)
                        {
                            var xDevice = new PS2Keyboard(aPort, logs, this);
                            xDevice.Initialize();

                            aDevice = xDevice;
                        }
                        else if (xSecondByte == 0x83)
                        {
                            var xDevice = new PS2Keyboard(aPort, logs, this);
                            xDevice.Initialize();

                            aDevice = xDevice;
                        }
                    }
                }
                /*
                 * |--------|---------------------------------------------------------------------|
                 * |  Byte  |  Device Type                                                        |
                 * |--------|---------------------------------------------------------------------|
                 * |  None  |  Ancient AT keyboard with translation enabled in the PS/Controller  |
                 * |        |  (not possible for the second PS/2 port)                            |
                 * |--------|---------------------------------------------------------------------|
                 */
                else if (aPort == 1)
                {
                    var xDevice = new PS2Keyboard(aPort, logs, this);
                    xDevice.Initialize();

                    aDevice = xDevice;
                }

                if (aDevice == null)
                {
                    logs.WriteError("FATAL: (PS/2 Controller) Device detection failed: FirstByte: " + xFirstByte + ", SecondByte: " + xSecondByte + ". Device: " + aPort);
                }
                else
                    logs.WriteOk("(PS/2 Controller) Device found: " + aPort);
            }
            else
            {
                throw new Exception("(PS/2 Controller) Port " + aPort + " doesn't exist");
            }
        }

        /// <summary>
        /// Tests a PS/2 device port.
        /// </summary>
        /// <param name="aPort">The PS/2 device port to test.</param>
        /// <returns></returns>
        private bool TestPort(byte aPort)
        {
            if (aPort == 1 || aPort == 2)
            {
                if (aPort == 1)
                {
                    SendCommand(Command.TestFirstPS2Port);
                }
                else
                {
                    SendCommand(Command.TestSecondPS2Port);
                }

                var xTestByte = ReadData();

                if (xTestByte == 0x00)
                {
                    return true;
                }
                else
                {
                    string xErrorMessage;

                    /*
                     * |--------|-------------------------|
                     * |  Byte  |  Error                  |
                     * |--------|-------------------------|
                     * |  0x01  |  clock line stuck low   |
                     * |--------|-------------------------|
                     * |  0x02  |  clock line stuck high  |
                     * |--------|-------------------------|
                     * |  0x03  |  data line stuck low    |
                     * |--------|-------------------------|
                     * |  0x04  |  data line stuck high   |
                     * |--------|-------------------------|
                     */
                    switch (xTestByte)
                    {
                        case 0x01:
                            xErrorMessage = "Clock Line Stuck Low";
                            break;
                        case 0x02:
                            xErrorMessage = "Clock Line Stuck High";
                            break;
                        case 0x03:
                            xErrorMessage = "Data Line Stuck Low";
                            break;
                        case 0x04:
                            xErrorMessage = "Data Line Stuck High";
                            break;
                        default:
                            throw new Exception("(PS/2 Controller) (Port " + aPort + ") Device test status unknown: " + xTestByte.ToHex());
                    }

                    logs.WriteError("(PS/2 Controller) (Port " + aPort + ") Device test error: '" + xErrorMessage + "'");
                    return false;
                }
            }
            else
            {
                throw new Exception("(PS/2 Controller) Port " + aPort + " doesn't exist");
            }
        }

        /// <summary>
        /// Pulses the PS/2 controller's output line.
        /// </summary>
        /// <param name="aPulseResetLine">The reset line.</param>
        /// <param name="aPulseSecondLine">The second line.</param>
        /// <param name="aPulseThirdLine">The third line.</param>
        /// <param name="aPulseFourthLine">The fourth line.</param>
        private void PulseOutputLine(OutputLines aOutputLines)
        {
            byte xMask = (byte)aOutputLines;


            WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Command, (byte)((byte)Command.PulseOutputLineBase | xMask));

        }

        /// <summary>
        /// Waits for the acknowledgement byte (0xFA). Returns false if the timeout expires, true otherwise.
        /// </summary>
        /// <returns>Returns false if the timeout expires, true otherwise.</returns>
        public bool WaitForAck()
        {
            int i = 0;

            while (IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data) != Ack)
            {
                i++;

                if (i >= WAIT_TIMEOUT)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads the byte after acknowledgement.
        /// </summary>
        /// <returns>The byte read after acknowledgement.</returns>
        public byte ReadByteAfterAck()
        {
            byte xByte;

            int i = 0;

            do
            {
                xByte = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

                i++;

                if (i >= WAIT_TIMEOUT)
                {
                    break;
                }
            }
            while (xByte == Ack);

            return xByte;
        }

        /// <summary>
        /// Reads the byte after acknowledgement.
        /// </summary>
        /// <param name="aByte">The byte read after acknowledgement.</param>
        /// <returns>Returns false if the timeout expired, true otherwise.</returns>
        public bool ReadByteAfterAckWithTimeout(ref byte aByte)
        {
            int i = 0;

            do
            {
                aByte = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

                i++;

                if (i >= WAIT_TIMEOUT)
                {
                    return false;
                }
            }
            while (aByte == Ack);

            return true;
        }

        /// <summary>
        /// Prepares the controller to write a command in the second PS/2 device port.
        /// </summary>
        public void PrepareSecondPortWrite()
        {
            SendCommand(Command.WriteNextByteToSecondPS2PortInputBuffer);
        }

        /// <summary>
        /// Waits for the PS/2 device reset.
        /// </summary>
        /// <returns>Returns true if the device resets successfully, false otherwise.</returns>
        public bool WaitForDeviceReset(string name)
        {
            //logs.WriteInfo("(PS/2 Controller) Waiting for device reset:");
            if (!WaitForAck())
            {
                // mDebugger.SendInternal("(PS/2 Controller) No Acknowledgement");
            }

            WaitToReadData();

            byte xByte = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

            if (xByte == 0xAA)
            {
                logs.WriteOk("(PS/2 Controller) Device reset successful");
                return true;
            }
            else
            {
                logs.WriteWarn($"(PS/2 Controller) Device {name} reset failed. Error: {xByte}");
                return false;
            }
        }

        /// <summary>
        /// Waits to read data.
        /// </summary>
        /// <returns>Returns false if the timeout expired, true otherwise.</returns>
        public bool WaitToReadData()
        {
            int i = 0;

            while ((IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Status) & 1) == 0)
            {
                i++;

                if (i >= WAIT_TIMEOUT)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Waits to write data.
        /// </summary>
        /// <returns>Returns false if the timeout expired, true otherwise.</returns>
        public bool WaitToWrite()
        {
            int i = 0;

            while ((IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Status) & (1 << 1)) != 0)
            {
                i++;

                if (i >= WAIT_TIMEOUT)
                {
                    return false;
                }
            }

            return true;
        }

        #region IO

        private byte ReadData()
        {
            WaitToReadData();

            var xByte = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

            //logs.WriteInfo("(PS/2 Controller) Returned byte: " + xByte.ToString());

            return xByte;
        }

        private bool ReadDataWithTimeout(ref byte aByte)
        {

            if (WaitToReadData())
            {
                aByte = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);


                return true;
            }

            return false;
        }

        private void WriteData(byte aByte)
        {
            WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aByte);
        }

        private void SendCommand(Command aCommand, byte? aByte = null)
        {
            //logs.WriteInfo("(PS/2 Controller) Sending command: " + ((byte)aCommand).ToString());

            WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Command, (byte)aCommand);
            //logs.WriteInfo("Command sent!");

            if (aByte.HasValue)
            {

                WaitToWrite();
                IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aByte.Value);
            }
        }

        private void SendDeviceCommand(DeviceCommand aDeviceCommand, bool aSecondPS2Port, byte? aByte = null)
        {
            //logs.WriteInfo("(PS/2 Controller) Sending device command: " + ((byte)aDeviceCommand).ToString());

            if (aSecondPS2Port)
            {
                SendCommand(Command.WriteNextByteToSecondPS2PortInputBuffer);
            }

            WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, (byte)aDeviceCommand);

            WaitForAck();

            //logs.WriteInfo("Command sent!");

            if (aByte.HasValue)
            {

                if (aSecondPS2Port)
                {
                    SendCommand(Command.WriteNextByteToSecondPS2PortInputBuffer);
                }

                WaitToWrite();
                IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aByte.Value);

                WaitForAck();
            }
        }

        #endregion
    }
}
