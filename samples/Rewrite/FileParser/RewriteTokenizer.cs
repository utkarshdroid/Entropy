// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;

namespace Rewrite.FileParser
{
    public static class RewriteTokenizer
    {
        private const char Space = ' ';
        private const char Escape = '\\';
        private const char Tab = '\t';
        // TODO handle "s
        public static List<string> TokenizeRule(string rule)
        {
            if (string.IsNullOrEmpty(rule))
            {
                return null;
            }
            var context = new ParserContext(rule);
            if (!context.Next())
            {
                return null;
            }

            var tokens = new List<string>();
            context.Mark();
            while (true)
            {
                if (!context.Next())
                {
                    // End of string. Capture.
                    break;
                }
                else if (context.Current == Escape)
                {
                    // Need to progress such that the next character is not evaluated.
                    if (!context.Next())
                    {
                        // Means that a character was not escaped appropriately Ex: "foo\"
                        throw new ArgumentException();
                    }
                }
                else if (context.Current == Space || context.Current == Tab)
                {
                    // time to capture!
                    // TODO: This kinda sucks, set state and skip 
                    var token = context.Capture();
                    if (!string.IsNullOrEmpty(token))
                    {
                        tokens.Add(token);
                        while (context.Current == Space || context.Current == Tab)
                        {
                            if (!context.Next())
                            {
                                // At end of string, we can return at this point.
                                return tokens;
                            }
                        }
                        context.Mark();
                    }
                }
            }
            var done = context.Capture();
            if (!string.IsNullOrEmpty(done))
            {
                tokens.Add(done);
            }
            return tokens;
        }
    }
}
