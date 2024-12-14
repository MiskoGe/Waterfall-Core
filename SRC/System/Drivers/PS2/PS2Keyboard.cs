using Cosmos.Core;
using Cosmos.HAL;
using Waterfall.System.Core.CLI;
using Waterfall.System.Core.SystemInput.Keys;
using static Cosmos.Core.INTs;

namespace Waterfall.System.Core.Drivers.PS2
{
    public class PS2Keyboard : Device
    {
        CLILogs log;
        public byte PS2Port { get; }
        private PS2Controller mPS2Controller;
        enum Command : byte
        {
            SetLEDs = 0xED,
            GetOrSetScanCodeSet = 0xF0,
            EnableScanning = 0xF4,
            DisableScanning = 0xF5,
            Reset = 0xFF
        }

        internal PS2Keyboard(byte aPort, CLILogs aLog, PS2Controller aPS2Controller)
        {
            PS2Port = aPort;
            log = aLog;
            mPS2Controller = aPS2Controller;
        }
        private void HandleIRQ(ref IRQContext aContext)
        {
            byte xScanCode = IOPort.Read8(Cosmos.Core.IOGroup.PS2Controller.Data);
            bool xReleased = (xScanCode & 0x80) == 0x80;

            if (xReleased)
            {
                xScanCode = (byte)(xScanCode ^ 0x80);
            }
            KeyHandler.KeyFromPS2(xScanCode, xReleased);
        }
        public void Initialize()
        {
            log.WriteInfo("Initializing PS2 Keyboard!");
            SendCommand(0xFF);
            mPS2Controller.WaitForDeviceReset("PS2 Keyboard");

            SetIrqHandler(1, HandleIRQ);

            SendCommand(0xF4);
            log.WriteOk("Started PS2 Keyboard!");
        }
        private void SendCommand(byte aCommand, byte? aByte = null)
        {
            if (PS2Port == 2)
            {
                mPS2Controller.PrepareSecondPortWrite();
            }
            mPS2Controller.WaitToWrite();
            IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aCommand);
            mPS2Controller.WaitForAck();
            if (aByte.HasValue)
            {

                if (PS2Port == 2)
                {
                    mPS2Controller.PrepareSecondPortWrite();
                }

                mPS2Controller.WaitToWrite();
                IOPort.Write8(Cosmos.Core.IOGroup.PS2Controller.Data, aByte.Value);

                mPS2Controller.WaitForAck();
            }
        }
        private void KeyboardInterrupt(ref IRQContext aContext)
        {
            byte action = IOPort.Read8(0x60);
        }

        public const uint WAIT_TIMEOUT = 100000;
        public const byte Ack = 0xFA;
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
        public void PrepareSecondPortWrite()
        {
            SendCommand(0xD4);
        }
    }
}
