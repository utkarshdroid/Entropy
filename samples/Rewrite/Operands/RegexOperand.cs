using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class RegexOperand : Operand
    {
        public Regex Regex { get; set; }
        public bool VisitRegexOperand(Match previous, string concatTestString)
        {
            previous = Regex.Match(concatTestString);
            return previous.Success;
        }
    }
}
