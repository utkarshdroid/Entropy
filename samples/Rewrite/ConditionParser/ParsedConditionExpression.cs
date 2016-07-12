using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class ParsedConditionExpression : Expression
    {
        public bool Invert { get; set; }
        public ConditionType Type { get; set; }
        public OperationType Operation { get; set; }
        public string Operand { get; set; }
    }
    public enum ConditionType
    {
        Regex,
        PropertyTest,
        StringComp,
        IntComp
    }
    public enum OperationType
    {
        None,
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        NotEqual,
        Directory,
        RegularFile,
        ExistingFile,
        SymbolicLink,
        Size,
        ExistingUrl,
        Executable
    }
}
