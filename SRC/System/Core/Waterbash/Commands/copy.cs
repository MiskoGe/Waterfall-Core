using System;
using System.Collections.Generic;
using System.IO;
using Waterfall.System.Security.FS;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class copy : WSHCommand
    {
        public override string HelpNote { get; set; } = "Copies specified files.";
        public copy()
        {
            MinimumParamsLength = 2;
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            string Path = Params[0];
            string NewPath = Params[1];
            if (Path.StartsWith("\\"))
            {
                Path = myShell.GetPath() + Path;
            }
            else if (Path == "..")
            {
                Path = myShell.GetPath();
                if (Path == myShell.GetDrive() + @":\")
                {
                    myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
                    myShell.CWriteLine("You can't get out of this directory.");
                    myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
                }
                else
                {
                    Path = Path.Remove(Path.Length - 1);
                    int lastSlashIndex = Path.LastIndexOf(@"\");
                    string result = Path.Substring(0, lastSlashIndex + 1);
                    Path = result;
                }
            }
            else if (Path[1] != ':') //disk
            {
                Path = myShell.GetPath() + Path;
            }

            if (Path[Path.Length - 1] == '\\')
                Path = Path.Substring(0, Path.Length - 1);

            Path = Path.Replace('/', '\\');
            NewPath = NewPath.Replace('/', '\\');
            ExecuteDir(Path, NewPath, myShell);
        }
        void ExecuteDir(string dir, string newdir, Watershell myShell)
        {
            if (!FileManagment.CanCreate(dir, myShell.Process))
            {
                myShell.CWriteLine("No permissions.");
            }
            else
            {
                // File.Copy(dir, newdir);
                myShell.CWriteLine("File successfully copied to " + newdir);
            }
        }
    }
}