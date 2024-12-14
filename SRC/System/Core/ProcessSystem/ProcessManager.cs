using System.Collections.Generic;
using Waterfall.System.Managment;

namespace Waterfall.System.Core.ProcessManager
{
	public static class ProcessManager
	{
		public static List<Process> Processes = new List<Process>();
		public static byte LastSecondUpdate;
		public static void Update()
		{
			foreach (Process process in Processes)
			{
				process.Run();
			}
			if (LastSecondUpdate != RealTime.RTCSec)
			{
				foreach (Process process in Processes)
				{
					process.RunEverySecond();
				}
				LastSecondUpdate = RealTime.RTCSec;
			}
		}
		public static void Start(Process NewProcess)
		{
			NewProcess.LoadConfig();
			if (Processes.Count == 0)
			{
				Processes.Add(NewProcess);
			}
			else
			{
				bool inserted = false;
				for (int i = Processes.Count - 1; i >= 0; i--)
				{
					if (Processes[i].Priority < NewProcess.Priority)
					{
						Processes.Insert(i + 1, NewProcess);
						inserted = true;
						break;
					}
				}
				if (!inserted)
				{
					Processes.Insert(0, NewProcess);
				}
			}
			NewProcess.Start();
		}
		public static void Stop(Process Process)
		{
			if (Process.Stop())
			{
				Processes.Remove(Process);
			}
		}
		public static void Terminate(Process Process)
		{
			Processes.Remove(Process);
		}
	}
}
