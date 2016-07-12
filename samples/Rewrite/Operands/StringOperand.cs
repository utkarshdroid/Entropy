using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class StringOperand : Operand
    {
        public string Value { get; set; }
        public StringOperationType Operation { get; set; } 
        public bool VisitStringOperand(string concatTestString)
        {
            if (Operation == StringOperationType.Equal)
            {
                return concatTestString.CompareTo(Value) == 0;
            }
            else if (Operation == StringOperationType.Greater)
            {
                return concatTestString.CompareTo(Value) > 0;
            }
            else if (Operation == StringOperationType.GreaterEqual)
            {
                return concatTestString.CompareTo(Value) >= 0;
            }
            else if (Operation == StringOperationType.Less)
            {
                return concatTestString.CompareTo(Value) < 0;
            }
            else if (Operation == StringOperationType.LessEqual)
            {
                return concatTestString.CompareTo(Value) <= 0;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
    public enum StringOperationType
    {
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual
    }
}
