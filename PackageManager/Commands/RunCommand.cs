using System;
using System.Collections.Generic;
using System.Text;
using TuchinC;

namespace Heart.Commands
{
    internal class RunCommand() : ICommand
    {
        public string CommandName => "run";



        public void Execute(in string path, in string[] modifiers)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();

            string main = $"{path}/main.tn";
            if (!File.Exists(main))
                throw new FileNotFoundException();

            var source = File.ReadAllText(main);
            if (modifiers.Length > 0)
            {
                if (modifiers[0] == "dis")
                {
                    var disassmbler = Tuchin.RunWithDisassembler(path, source);
                    Console.WriteLine("DISASSEMBLER");
                    Console.WriteLine(new String('=', 100));
                    Console.WriteLine(disassmbler);
                    Console.WriteLine(new String('=', 100));
                    return;
                }
            }
            Tuchin.Run(path, source);

        }
    }
}
