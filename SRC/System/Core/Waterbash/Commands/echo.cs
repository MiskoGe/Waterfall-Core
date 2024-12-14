using System;
using System.Collections.Generic;
using System.IO;
using Waterfall.System.Security.FS;

namespace Waterfall.System.Core.Waterbash
{
    public class echo : WSHCommand
    {
        public override string HelpNote { get; set; } = "Displays the specified text to the output";
        public echo()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            if (Params.Length > 1)
            {
                string finale = "";
                for (int i = 0; i < Params.Length; i++)
                {
                    finale += Params[i] + " ";
                }
                if (finale.Contains(">"))
                {
                    string[] parts = finale.Split('>');
                    string fileName = parts[parts.Length - 1];
                    fileName = fileName.Substring(0, fileName.Length - 1);
                    if (FileManagment.CanEdit(myShell.GetPath() + fileName, myShell.Process))
                    {
                        if (!FileManagment.CanCreate(myShell.GetPath() + fileName, myShell.Process))
                        {
                            myShell.CWriteLine("No permissions.");
                            return;
                        }
                        var file_stream = File.Create(myShell.GetPath() + fileName);

                        file_stream.Close();
                        string content = "";
                        for (int i = 0; i < parts.Length - 1; i++)
                        {
                            content += parts[i];
                        }

                        File.WriteAllText(myShell.GetPath() + fileName, content);
                        myShell.CWriteLine($"File created in {myShell.GetPath()}");
                        //desktophost.checkIfOnDesktop(myShell.GetPath() + fileName, true);
                    }
                    else
                    {
                        myShell.CWriteLine("No permissions.");
                        return;
                    }
                }
                else
                {
                    myShell.CWriteLine(finale);
                }
            }
            else
            {
                myShell.CWriteLine("");
            }
        }
    }
}
