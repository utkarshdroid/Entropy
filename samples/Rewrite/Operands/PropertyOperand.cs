using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class PropertyOperand : Operand
    {
        public PropertyOperationType Operation { get; set; }
        public bool VisitPropertyOperand(string concatTestString)
        {
            return true;
        }
    }

    public enum PropertyOperationType
    {
        None,
        Directory,
        RegularFile,
        ExistingFile,
        SymbolicLink,
        Size,
        ExistingUrl,
        Executable
    }
}
