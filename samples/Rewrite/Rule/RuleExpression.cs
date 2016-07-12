using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class RuleExpression : Expression
    {
        public RegexOperand Operand { get; set; }
        public bool Invert { get; set; }
    }
}
