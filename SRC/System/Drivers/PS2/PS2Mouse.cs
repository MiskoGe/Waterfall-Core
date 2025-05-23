﻿using Cosmos.Core;
using Cosmos.HAL;
using Waterfall.System.Core.CLI;
using Waterfall.System.Core.SystemInput;

namespace Waterfall.System.Core.Drivers.PS2
{
    public class PS2Mouse : Device
    {
        CLILogs log;


        private readonly PS2Controller ps2Controller;

        private readonly byte[] mouseByte = new byte[4];
        private static byte mouseCycle = 0;
        private byte mouseID = 0;
        internal PS2Mouse(byte aPort, byte aMouseID, PS2Controller aPS2Controller, CLILogs aLog)
        {
            PS2Port = aPort;
            log = aLog;
            mouseID = aMouseID;
            ps2Controller = aPS2Controller;
        }

        enum Command : byte
        {
            SetScaling1_1 = 0xE6,
            SetScaling2_1 = 0xE7,
            SetResolution = 0xE8,
            StatusRequest = 0xE9,
            SetStreamMode = 0xEA,
            RequestSinglePacket = 0xEB,
            ResetWrapMode = 0xEC,
            SetWrapMode = 0xEE,
            SetRemoteMode = 0xF0,
            GetMouseID = 0xF2,
            SetSampleRate = 0xF3,
            EnablePacketStreaming = 0xF4,
            DisablePacketStreaming = 0xF5,
            SetDefaults = 0xF6,
            Resend = 0xFE,
            Reset = 0xFF
        }

        #region Properties

        public bool HasScrollWheel => mouseID == 3 || mouseID == 4;

        public byte PS2Port { get; }

        #endregion

        #region Methods

        /// <summary>
        /// This is the required call to start
        /// the mouse receiving interrupts.
        /// </summary>
        public void Initialize()
        {
            log.WriteInfo("Initializing PS2 Mouse!");
            SendCommand(0xFF);
            ps2Controller.WaitForDeviceReset("PS2 Mouse");

            if (mouseID == 0)
            {
                mouseID = TryToEnableScrollWheel();

                log.WriteInfo("(PS/2 Mouse) Mouse ID: " + mouseID);

                if (mouseID == 3)
                {
                    mouseID = TryToEnableAdditionalButtons();
                }

            }

            //SendCommand(Command.SetDefaults);
            //mPS2Controller.WaitForAck();

            INTs.SetIrqHandler(12, HandleMouse);

            SendCommand(0xF4);
            ps2Controller.WaitForAck();
        }

        /// <summary>
        /// Tries to enable the scroll wheel.
        /// </summary>
        /// <returns>Returns the mouse id.</returns>
        private byte TryToEnableScrollWheel()
        {
            SendCommand(0xF3, 200);
            SendCommand(0xF3, 100);
            SendCommand(0xF3, 80);

            SendCommand(0xF2);

            return ps2Controller.ReadByteAfterAck();
        }

        /// <summary>
        /// Tries to enable additional buttons (buttons 4 and 5).
        /// </summary>
        /// <returns>Returns the mouse id.</returns>
        private byte TryToEnableAdditionalButtons()
        {
            SendCommand(0xF3, 200);
            SendCommand(0xF3, 200);
            SendCommand(0xF3, 80);

            SendCommand(0xF2);

            return ps2Controller.ReadByteAfterAck();
        }

        public void HandleMouse(ref INTs.IRQContext context)
        {
            if (mouseCycle == 0)
            {
                mouseByte[0] = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

                //Bit 3 of byte 0 is 1, then we have a good package
                if ((mouseByte[0] & (1 << 3)) == 1 << 3)
                {
                    mouseCycle++;
                }
            }
            else if (mouseCycle == 1)
            {
                mouseByte[1] = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);
                mouseCycle++;
            }
            else if (mouseCycle == 2)
            {
                mouseByte[2] = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);

                if (HasScrollWheel)
                {
                    mouseCycle++;
                }
            }
            else if (mouseCycle == 3)
            {
                mouseByte[3] = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);
                mouseCycle++;
            }

            if ((mouseCycle == 2 && !HasScrollWheel) || (mouseCycle == 4 && HasScrollWheel))
            {
                int xDeltaX = 0;
                int xDeltaY = 0;
                int xScrollWheel = 0;

                if ((mouseByte[0] & (1 << 4)) == 1 << 4)
                {
                    xDeltaX = mouseByte[1] | ~0xFF;
                }
                else
                {
                    xDeltaX = mouseByte[1];
                }

                if ((mouseByte[0] & (1 << 5)) == 1 << 5)
                {
                    xDeltaY = -(mouseByte[2] | ~0xFF);
                }
                else
                {
                    xDeltaY = -mouseByte[2];
                }

                var xMouseState = mouseByte[0] & 0b0000_0111;

                if (HasScrollWheel)
                {
                    var xScrollWheelByte = mouseByte[3];

                    xScrollWheel = (xScrollWheelByte & 0b1000) == 0 ? xScrollWheelByte : xScrollWheelByte | ~0x0F;

                    if (mouseID == 4)
                    {
                        var xAdditionalButtonsByte = mouseByte[3] & 0b0011_0000;
                        xMouseState |= xAdditionalButtonsByte >> 1;
                    }
                }

                Mouse.OnMouseChanged(xDeltaX, xDeltaY, xMouseState, xScrollWheel);

                mouseCycle = 0;
            }
        }
        private void SendCommand(byte aCommand, byte? aByte = null)
        {
            //log.WriteInfo("(PS/2 Mouse) Sending command: " + (aCommand).ToString());

            if (PS2Port == 2)
            {
                ps2Controller.PrepareSecondPortWrite();
            }

            ps2Controller.WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aCommand);

            ps2Controller.WaitForAck();

            //log.WriteInfo("Command sent!");
            if (aByte.HasValue)
            {

                if (PS2Port == 2)
                {
                    ps2Controller.PrepareSecondPortWrite();
                }

                ps2Controller.WaitToWrite();
                IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aByte.Value);

                ps2Controller.WaitForAck();
            }
        }

        #endregion

    }
}
