using System;
using System.Collections.Generic;
using System.Text;

namespace Heart.Commands
{
    internal class BuildCommand : ICommand
    {
        public string CommandName => "build";

        public void Execute(in string Path, in string[] modifiers)
        {
            throw new NotImplementedException();
        }
    }
}
