using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class ram : WSHCommand
    {
        public override string HelpNote { get; set; } = "Displays information about random access memory";
        public ram()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {
                { "-a", ParamA },
                { "--all", ParamA },
                { "-u", ParamU },
                { "--usage", ParamU },
                { "-p", ParamP },
                { "--pages", ParamP },
                { "--help", ParamHelp }
            };
            paramHelp = new List<ParamInfo>
            {
                new ParamInfo{ longParam = "all", shortParam = 'a', description = "print all information, in the following order" },
                new ParamInfo{ longParam = "usage", shortParam = 'u', description = "print ram usage" },
                new ParamInfo{ longParam = "pages", shortParam = 'p', description = "print information about RAT pages", },
                new ParamInfo{ longParam = "help", description = "display this help", },
            };
        }
        public override void Execute(string[] Params, Watershell myBash)
        {
            if (Params.Length == 0)
            {
                ParamU(myBash);
            }
            else
            {
                foreach (var item in Params)
                {
                    if (paramActions.ContainsKey(item))
                        paramActions[item](myBash);
                    else
                    {
                        myBash.CWriteLine($"Unknown parameter: {item}");
                    }
                }
            }
        }
        public void ParamA(Watershell myBash)
        {
            ParamU(myBash);
            ParamP(myBash);
        }
        public void ParamP(Watershell myBash)
        {
            double usedRamPercent = (double)(GCImplementation.GetUsedRAM() / (double)(GCImplementation.GetAvailableRAM() * (1024 * 1024))) * 100;
            string roundedUsedRamPercent = string.Format("{0:0.0}", usedRamPercent);
            myBash.CWriteLine($"Free RAT Pages: {Cosmos.Core.Memory.RAT.FreePageCount} Total page count: {Cosmos.Core.Memory.RAT.TotalPageCount} Used pages: {Cosmos.Core.Memory.RAT.TotalPageCount - Cosmos.Core.Memory.RAT.FreePageCount} Page size: {Cosmos.Core.Memory.RAT.PageSize} ");
        }

        public void ParamU(Watershell myBash)
        {
            double usedRamPercent = (double)(GCImplementation.GetUsedRAM() / (double)(GCImplementation.GetAvailableRAM() * (1024 * 1024))) * 100;
            string roundedUsedRamPercent = string.Format("{0:0.0}", usedRamPercent);
            myBash.CWriteLine("Ram usage: " + (GCImplementation.GetUsedRAM() / (1024 * 1024)) + "/" + GCImplementation.GetAvailableRAM() + " MB (" + roundedUsedRamPercent + "% used) ");
        }
        public void ParamHelp(Watershell myBash)
        {
            new HelpDisplayer(myBash, "ram [OPTION]...", paramHelp);
        }
    }
}
