// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;

namespace Rewrite.ConditionParser
{
    public class ConditionTestStringParser
    {
        private const char Percent = '%';
        private const char Dollar = '$';
        private const char Space = ' ';
        private const char Colon = ':';
        private const char OpenBrace = '{';
        private const char CloseBrace = '}';
        // TODO comments about what the test string is.
        // TODO comments about what went wrong in exceptions
        public static Pattern ParseConditionTestString(string testString)
        {
            // Example inputs:
            // %{REMOTE_ADDR}
            // /var/www/%{REQUEST_URI}
            // %1
            // $1
            if (testString == null)
            {
                testString = string.Empty;
            }
            var context = new ParserContext(testString);
            var results = new List<PatternSegment>();
            while (context.Next())
            {
                if (context.Current == Percent)
                {
                    // This is a server parameter, parse for a condition variable
                    if (!context.Next())
                    {
                        throw new ArgumentException();
                    }
                    if (!ParseConditionParameter(context, results))
                    {
                        throw new ArgumentException();
                    }
                }
                else if (context.Current == Dollar)
                {
                    // This is a parameter from the rule, verify that it is a number from 0 to 9 directly after it
                    // and create a new Pattern Segment.
                    if (!context.Next())
                    {
                        throw new ArgumentException();
                    }
                    context.Mark();
                    if (context.Current >= '0' && context.Current <= '9')
                    {
                        context.Next();
                        var ruleVariable = context.Capture();
                        context.Back();
                        results.Add(new PatternSegment(ruleVariable, SegmentType.RuleParameter));
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    // Parse for literals, which will return on either the end of the test string 
                    // or when it hits a special character
                    if (!ParseLiteral(context, results))
                    {
                        throw new ArgumentException();
                    }
                }
            }
            return new Pattern(results);
        }

        // pre: will only be called if context.Current is an unescaped '%' 
        // post: adds an application to the teststring results that will be concatinated on success
        // TODO make this return the condition test string segment and a bool
        private static bool ParseConditionParameter(ParserContext context, List<PatternSegment> results)
        {
            // Parse { }
            if (context.Current == OpenBrace)
            {
                // Start of a server variable
                if (!context.Next())
                {
                    // Dangling {
                    throw new ArgumentException();
                }
                context.Mark();
                while (context.Current != CloseBrace)
                {
                    if (!context.Next())
                    {
                        // No closing } for the server variable
                        throw new ArgumentException();
                    }
                    else if (context.Current == Colon)
                    {
                        // Have a segmented look up Ex: HTTP:xxxx 
                        // TODO handle this case
                    }
                }

                // Need to verify server variable captured exists
                var rawServerVariable = context.Capture();
                if (IsValidServerVariable(rawServerVariable))
                {
                    results.Add(new PatternSegment(rawServerVariable, SegmentType.ServerParameter));
                }
                else
                {
                    return false;
                }
            }
            else if (context.Current >= '0' && context.Current <= '9')
            {
                // means we have a segmented lookup
                // store information in the testString result to know what to look up.
                context.Mark();
                context.Next();
                var rawConditionParameter = context.Capture();
                // Once we leave this method, the while loop will call next again. Because
                // capture is exclusive, we need to go one past the end index, capture, and then go back.
                context.Back();
                results.Add(new PatternSegment(rawConditionParameter, SegmentType.ConditionParameter));
            }
            else
            {
                // illegal escape of a character
                throw new ArgumentException();
            }
            return true;
        }

        private static bool ParseLiteral(ParserContext context, List<PatternSegment> results)
        {
            context.Mark();
            string literal;
            while (true)
            {
                if (context.Current == Percent || context.Current == Dollar)
                {
                    literal = context.Capture();
                    context.Back();
                    break;
                }
                if (!context.Next())
                {
                    literal = context.Capture();
                    break;
                }
            }
            if (IsValidLiteral(context, literal))
            {
                // add results
                results.Add(new PatternSegment(literal, SegmentType.Literal));
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool IsValidLiteral(ParserContext context, string literal)
        {
            // TODO
            return true;
        }
        private static bool IsValidServerVariable(string variable)
        {
            // TODO
            return ServerVariables.GetServerVariable(variable) != ServerVariable.NONE;
        }
    }
}
