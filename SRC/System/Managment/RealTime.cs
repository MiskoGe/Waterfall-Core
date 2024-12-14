using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using System.Collections.Generic;
using Waterfall.System.Processes;
using Waterfall.System.Security;

namespace Waterfall.System.Managment
{
    public static class RealTime
    {
        public static int _frames;
        public static int _ticks;
        public static int _finaleTicks;
        public static int _fps = 120;
        public static int _deltaT = 0;
        static int refresh;
        static int framesRefresh = 4;
        public static byte RTCSec;
        public static byte lastRefresh;
        public static ulong CPUUptime;
        public static ulong CPUSecond = 3200000000;
        public static Queue<ulong> FpsQueue = new Queue<ulong>();
        public static Queue<ulong> TicksQueue = new Queue<ulong>();
        public static ulong UsedKB;
        public static ulong AllMB;
        static int ticksBeofreTimeUpdate;
        public static void CountFPS()
        {
            FpsQueue.Enqueue(CPUUptime + CPUSecond);

            if (FpsQueue.Count > 0)
            {
                while (FpsQueue.Count > 0 && FpsQueue.Peek() < CPUUptime)
                {
                    FpsQueue.Dequeue();
                }
            }
            _fps = FpsQueue.Count;
        }
        public static void CountTicks()
        {
            ticksBeofreTimeUpdate++;
            TicksQueue.Enqueue(CPUUptime + CPUSecond);
            if (TicksQueue.Count > 0)
            {
                while (TicksQueue.Count > 0 && TicksQueue.Peek() < CPUUptime)
                {
                    TicksQueue.Dequeue();
                }
                _finaleTicks = TicksQueue.Count;
            }
            if (ticksBeofreTimeUpdate >= 12)
                RTCSec = RTC.Second;
        }
        public static void CallHeapCollect()
        {
            if (refresh >= framesRefresh)
            {
                Heap.Collect();
                refresh = 0;
                if (_fps < 80)
                {
                    framesRefresh = 4;
                }
                else
                {
                    framesRefresh = 9;
                }
                RTCSec = RTC.Second;
            }
            else
                refresh++;
        }
        public static void GetCPUUptime()
        {
            CPUUptime = CPU.GetCPUUptime();
        }
    }
}
