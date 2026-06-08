using System;
using System.Collections.Generic;
using System.Text;

namespace Heart.Commands
{
    internal interface ICommand
    {
        string CommandName { get; }
        void Execute(in string Path, in string[] modifiers);
    }
}
