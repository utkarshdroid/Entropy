using Rewrite.Flags;
using System;
using System.Text.RegularExpressions;

namespace Rewrite.ConditionParser
{
    public class ModRewriteExpressionCreator
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(1);
        public static ConditionExpression CreateConditionExpression(ParsedConditionExpression pce, ConditionFlags flags)
        {
            var condExp = new ConditionExpression();
            condExp.Invert = pce.Invert;
            if (pce.Type == ConditionType.Regex)
            {
                // todo make nullable
                if (flags != null && flags.HasFlag(ConditionFlagType.NoCase)) {
                    condExp.Operand = new RegexOperand { Regex = new Regex(pce.Operand, RegexOptions.IgnoreCase, RegexTimeout) };
                }
                else
                {
                    condExp.Operand = new RegexOperand { Regex = new Regex(pce.Operand, RegexOptions.None, RegexTimeout) };
                }
            }
            else if (pce.Type == ConditionType.IntComp)
            {
                switch(pce.Operation)
                {
                    case OperationType.Equal:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.Equal, Value = Int32.Parse(pce.Operand) };
                        break;
                    case OperationType.Greater:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.Greater, Value = Int32.Parse(pce.Operand) };
                        break;
                    case OperationType.GreaterEqual:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.GreaterEqual, Value = Int32.Parse(pce.Operand) };
                        break;
                    case OperationType.Less:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.Less, Value = Int32.Parse(pce.Operand) };
                        break;
                    case OperationType.LessEqual:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.LessEqual, Value = Int32.Parse(pce.Operand) };
                        break;
                    case OperationType.NotEqual:
                        condExp.Operand = new IntegerOperand { Operation = IntegerOperationType.NotEqual, Value = Int32.Parse(pce.Operand) };
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            else if (pce.Type == ConditionType.StringComp)
            {
                switch (pce.Operation)
                {
                    case OperationType.Equal:
                        condExp.Operand = new StringOperand { Operation = StringOperationType.Equal, Value = pce.Operand };
                        break;
                    case OperationType.Greater:
                        condExp.Operand = new StringOperand { Operation = StringOperationType.Greater, Value = pce.Operand };
                        break;
                    case OperationType.GreaterEqual:
                        condExp.Operand = new StringOperand { Operation = StringOperationType.GreaterEqual, Value = pce.Operand };
                        break;
                    case OperationType.Less:
                        condExp.Operand = new StringOperand { Operation = StringOperationType.Less, Value = pce.Operand };
                        break;
                    case OperationType.LessEqual:
                        condExp.Operand = new StringOperand { Operation = StringOperationType.LessEqual, Value = pce.Operand };
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            else
            {
                throw new NotImplementedException();
                //condExp.Operand = new PropertyOperand { Operation = PropertyOperationType.None };
            }
            return condExp;
        }
        public static RuleExpression CreateRuleExpression(ParsedConditionExpression pce, RuleFlags flags)
        {
            var ruleExp = new RuleExpression();
            ruleExp.Invert = pce.Invert;
            if (flags.HasFlag(RuleFlagType.NoCase))
            {
                ruleExp.Operand = new RegexOperand { Regex = new Regex(pce.Operand, RegexOptions.IgnoreCase, RegexTimeout) };
            }
            else
            {
                ruleExp.Operand = new RegexOperand { Regex = new Regex(pce.Operand, RegexOptions.None, RegexTimeout) };
            }
            return ruleExp;
        }
    }
}
