using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class reboot : WSHCommand
    {
        public override string HelpNote { get; set; } = "Restarts the system";
        public reboot()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {
            };
        }

        public override void Execute(string[] Params, Watershell myBash)
        {
            if (Params.Length == 0)
            {
                Cosmos.System.Power.Reboot();
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
