// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;

namespace Rewrite.ConditionParser
{
    // TODO give reasons for all format exceptions
    public static class ConditionActionParser
    {
        private const char Not = '!';
        private const char Dash = '-';
        private const char Less = '<';
        private const char Greater = '>';
        private const char EqualSign = '=';

        // Given a Condition Regex Expression, obtain a ConditionRegexStatement
        public static ParsedConditionExpression ParseActionCondition(string condition)
        {
            if (condition == null)
            {
                condition = String.Empty;
            }
            var context = new ModRewriteParserContext(condition);
            var results = new ParsedConditionExpression();
            if (!context.Next())
            {
                return null;
            }
            // If we hit a !, make sure the condition is inverted when resolving the string
            if (context.Current == Not)
            {
                results.Invert = true;
                if (!context.Next())
                {
                    return null;
                }
            }
            // Control Block for strings. Set the operation and type fields based on the sign
            if (context.Current == Greater)
            {
                if (!context.Next())
                {
                    // Dangling ">"
                    throw new FormatException(context.Error()); 
                }
                if (context.Current == EqualSign)
                {
                    if (!context.Next())
                    {
                        // Dangling ">="
                        throw new FormatException(context.Error());
                    }
                    results.Operation = OperationType.GreaterEqual;
                    results.Type = ConditionType.StringComp;
                } 
                else
                {
                    results.Operation = OperationType.Greater;
                    results.Type = ConditionType.StringComp;
                }
            }
            else if (context.Current == Less)
            {
                if (!context.Next())
                {
                    // Dangling "<"
                    throw new FormatException(context.Error());
                }
                if (context.Current == EqualSign)
                {
                    if (!context.Next())
                    {
                        // Dangling "<="
                        throw new FormatException(context.Error());
                    }
                    results.Operation = OperationType.LessEqual;
                    results.Type = ConditionType.StringComp;
                }
                else
                {
                    results.Operation = OperationType.Less;
                    results.Type = ConditionType.StringComp;
                }
            }
            else if (context.Current == EqualSign)
            {
                if (!context.Next())
                {
                    // Dangling "="
                    throw new FormatException(context.Error());
                }
                results.Operation = OperationType.Equal;
                results.Type = ConditionType.StringComp;
            }
            else if (context.Current == Dash)
            {
                // Look up Property or integer comparison portion.
                results = ParseAttributeTest(context, results.Invert);
                if (results.Type == ConditionType.PropertyTest)
                {
                    return results;
                }
                context.Next();
            }
            else
            {
                // Otherwise we assume it is regex.
                results.Type = ConditionType.Regex; 
            }
            // Capture the rest of the string guarantee validity.
            results.Operand = (condition.Substring(context.GetIndex()));
            if (IsValidActionCondition(results))
            {
                return results;
            }
            else
            {
                return null;
            }
        }

        public static ParsedConditionExpression ParseAttributeTest(ModRewriteParserContext context, bool invert)
        {
            if (!context.Next())
            {
                throw new FormatException(context.Error());
            }
            if (context.Current == 'd')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Directory, Invert = invert };
            }
            else if (context.Current == 'f')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.RegularFile, Invert = invert };
            }
            else if (context.Current == 'F')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.ExistingFile, Invert = invert };
            }
            else if (context.Current == 'h' || context.Current == 'L')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.SymbolicLink, Invert = invert };
            }
            else if (context.Current == 's')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Size, Invert = invert };
            }
            else if (context.Current == 'U')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.ExistingUrl, Invert = invert };
            }
            else if (context.Current == 'x')
            {
                return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.Executable, Invert = invert };
            }
            else if (context.Current == 'e')
            {
                if (!context.Next() || context.Current != 'q')
                {
                    // Illegal statement.
                    throw new FormatException(context.Error());
                }
                return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.Equal, Invert = invert };
            }
            else if (context.Current == 'g')
            {
                if (!context.Next())
                {
                    throw new FormatException(context.Error());
                }
                if (context.Current == 't')
                {
                    return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.Greater, Invert = invert };
                }
                else if (context.Current == 'e')
                {
                    return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.GreaterEqual, Invert = invert };
                }
                else
                {
                    throw new FormatException(context.Error());
                }
            }
            else if (context.Current == 'l')
            {
                if (!context.Next())
                {
                    return new ParsedConditionExpression { Type = ConditionType.PropertyTest, Operation = OperationType.SymbolicLink, Invert = invert };
                }
                if (context.Current == 't')
                {
                    return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.Less, Invert = invert };
                }
                else if (context.Current == 'e')
                {
                    return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.LessEqual, Invert = invert };
                }
                else
                {
                    throw new FormatException(context.Error());
                }
            }
            else if (context.Current == 'n')
            {
                if (!context.Next() || context.Current != 'e')
                {
                    throw new FormatException(context.Error());
                }
                return new ParsedConditionExpression { Type = ConditionType.IntComp, Operation = OperationType.NotEqual, Invert = invert };
            }
            else
            {
                throw new FormatException(context.Error());
            }
        }

        private static bool IsValidActionCondition(ParsedConditionExpression results)
        {
            if (results.Type == ConditionType.IntComp)
            {
                // If the type is an integer, verify operand is actually an int
                int res;
                if (!Int32.TryParse(results.Operand, out res))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
