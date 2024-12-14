using System;
using System.Collections.Generic;
using Waterfall.System.Security;

namespace Waterfall.System.Core.Waterbash.Commands
{
	public class uname : WSHCommand
	{
        public override string HelpNote { get; set; } = "Shows information about the system";
        public uname()
		{
			paramActions = new Dictionary<string, Action<Watershell>>
			{
				{ "-a", ParamA },
				{ "--all", ParamA },
				{ "-s", ParamS },
				{ "--kernel-name", ParamS },
				{ "-n", ParamN },
				{ "--nodename", ParamN },
				{ "-r", ParamR },
				{ "--kernel-release", ParamR },
				{ "-v", ParamV },
				{ "--kernel-version", ParamV },
				{ "-o", ParamO },
				{ "--operating-system", ParamO },
				{ "--help", ParamHelp },
				{ "--version", ParamVersion }
			};
		}
		public override void Execute(string[] Params, Watershell myBash)
		{
			if (Params.Length == 0)
			{
				ParamS(myBash);
			}
			else
			{
				foreach (var item in Params)
				{
					if (paramActions.ContainsKey(item))
						paramActions[item](myBash);
					else
					{
						myBash.CWrite($"Unknown parameter: {item}");
					}
				}
			}
			myBash.CWriteLine("");
		}
		public void ParamA(Watershell myBash)
		{
			ParamS(myBash);
			ParamN(myBash);
			ParamR(myBash);
			ParamV(myBash);
			ParamO(myBash);
		}
		public void ParamS(Watershell myBash)
		{
			myBash.CWrite("Waterfall ");
		}
		public void ParamN(Watershell myBash)
		{
			myBash.CWrite("none ");
		}
		public void ParamR(Watershell myBash)
		{
			myBash.CWrite(BuildConfig.Version + " ");
		}
		public void ParamV(Watershell myBash)
		{
			myBash.CWrite(BuildConfig.SubVersion + " ");
		}
		public void ParamO(Watershell myBash)
		{
			myBash.CWrite("Waterfall Operating System ");
		}
		public void ParamHelp(Watershell myBash)
		{
			myBash.CWriteLine("Usage: uname [OPTION]...");
			myBash.CWriteLine("  -a, --all                print all information, in the following order");
			myBash.CWriteLine("  -s, --kernel-name        print the kernel name");
			myBash.CWriteLine("  -n, --nodename           print the network node hostname");
			myBash.CWriteLine("  -r, --kernel-release     print the kernel release");
			myBash.CWriteLine("  -v, --kernel-version     print the kernel version");
			myBash.CWriteLine("  -o, --operating-system   print the operating system");
			myBash.CWriteLine("      --help               display this help");
			myBash.CWrite("      --version                    display version information ");
		}
		public void ParamVersion(Watershell myBash)
		{
			myBash.CWriteLine("uname 1.0");
			myBash.CWriteLine("Watershell");
			myBash.CWriteLine("");
			myBash.CWriteLine("Written by Szymekk ");
		}
	}
}
