using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Nodes.Expressions;

namespace TuchinC.AST.Nodes.Statements
{
    public record class Case(Literal Value,Block Body);
}
