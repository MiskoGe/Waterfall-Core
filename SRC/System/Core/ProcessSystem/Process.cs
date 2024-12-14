using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.ProcessManager
{
    public class Process
    {
        public virtual void LoadConfig() { }
        public virtual void Start() { }
        public virtual bool Stop() { return true; }
        public virtual void Run() { }
        public virtual void RunEverySecond() { }

        public string ProcessName { get; set; } = "";
        public int PID { get; } = RandomPID();
        public Type ProcessType { get; set; } = Type.Normal;
        public Priority Priority { get; set; } = Priority.Normal;
        public UserControl UserControl { get; } = new UserControl();
        public bool ShowOnTaskbar { get; set; }
        public Bitmap Icon { get; set; }
        private static int RandomPID()
        {
            Random random = new Random();
            return random.Next(10000, 100000);
        }
        public List<Process> ConnectedProcesses { get; set; } = new List<Process>();
    }
    public enum Type { Normal, SystemProtected };
    public enum Priority { Lowest, Low, Normal, High, VeryHigh, Critical }
    public class UserControl
    {
        public bool NativeApp = true;
        public string Publisher = "Unknown";
        public int PermissionLevel = 0;
    }
}
