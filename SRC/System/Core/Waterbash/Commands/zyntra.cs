using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class zyntra : WSHCommand
    {
        public override string HelpNote { get; set; } = "Special for Szymekk.";
        public zyntra()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {
                { "--help", ParamHelp }
            };
            paramHelp = new List<ParamInfo>
            {
                new ParamInfo{ longParam = "help", description = "hiiii szymekk" },
            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            
            myShell.CWriteLine("hi szymekk xd");
        }

        public void ParamHelp(Watershell myBash)
        {
            new HelpDisplayer(myBash, "special message for szymekk", paramHelp);
        }
    }
}