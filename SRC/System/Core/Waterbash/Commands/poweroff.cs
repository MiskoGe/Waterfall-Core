using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class poweroff : WSHCommand
    {
        public override string HelpNote { get; set; } = "Shuts down the system";
        public poweroff()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {
            };
        }

        public override void Execute(string[] Params, Watershell myBash)
        {
            if (Params.Length == 0)
            {
                Cosmos.System.Power.Shutdown();
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
        }

    }
}
