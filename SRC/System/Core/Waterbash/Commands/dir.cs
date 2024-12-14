using System;
using System.Collections.Generic;
using System.IO;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class dir : WSHCommand
    {
        public override string HelpNote { get; set; } = "Lists the files and directories in the current directory";
        public dir()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            if (Params.Length == 0)
            {
                ExecuteDir(myShell.GetPath(), myShell);
                return;
            }

            string Path = Params[0];
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

            if (Path[Path.Length - 1] != '\\')
                Path += "\\";

            Path = Path.Replace('/', '\\');

            ExecuteDir(Path, myShell);
        }
        void ExecuteDir(string dir, Watershell myShell)
        {
            myShell.CWriteLine($"Directory of {dir}");
            string[] allDirectories = Directory.GetDirectories(dir);
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
            myShell.CWriteLine($"Directories ({allDirectories.Length})");
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            foreach (var directory in allDirectories)
            {
                myShell.CWriteLine($"{directory}");
            }

            string[] allFiles = Directory.GetFiles(dir);
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
            myShell.CWriteLine($"Files ({allFiles.Length})");
            myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
            for (int i = 0; i < allFiles.Length; i++)
            {
                myShell.CWriteLine($"{allFiles[i]}");
            }
        }
    }
}
