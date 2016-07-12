using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class IntegerOperand : Operand
    {
        public int Value { get; set; }
        public IntegerOperationType Operation { get; set; }

        public bool VisitIntegerOperand(string concatTestString)
        {
            int compValue;
            if (!Int32.TryParse(concatTestString, out compValue))
            {
                throw new FormatException();
            }
            switch (Operation)
            {
                case IntegerOperationType.Equal:
                    return compValue == Value;
                case IntegerOperationType.Greater:
                    return compValue > Value;
                case IntegerOperationType.GreaterEqual:
                    return compValue >= Value;
                case IntegerOperationType.Less:
                    return compValue < Value;
                case IntegerOperationType.LessEqual:
                    return compValue <= Value;
                case IntegerOperationType.NotEqual:
                    return compValue != Value;
                default:
                    throw new ArgumentException();
            }
        }
    }
    public enum IntegerOperationType
    {
        None,
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        NotEqual
    }

}
