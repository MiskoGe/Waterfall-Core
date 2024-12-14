using System;
using System.Collections.Generic;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class help : WSHCommand
    {
        public override string HelpNote { get; set; } = "Provides help information for commands";
        public help()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }

        public override void Execute(string[] Params, Watershell myBash)
        {
            myBash.CWriteLine("All WaterShell commands");
            foreach (var command in myBash.commands)
            {
                myBash.CWrite(command.Key + new string(' ', 20 - command.Key.Length));
                WSHCommand commandInstance = command.Value();
                myBash.CWriteLine(commandInstance.HelpNote);
            }
            myBash.CWriteLine("To get help for a command, use 'command' --help");
        }
    }
}
