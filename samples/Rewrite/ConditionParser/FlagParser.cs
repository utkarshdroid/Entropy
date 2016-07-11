// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using static Rewrite.ConditionParser.Flags;

namespace Rewrite.ConditionParser
{
    public class FlagParser
    {
        public static Flags TokenizeAndParseFlags(string flags) {
            // Check that flags are contained within []
            if (!flags.StartsWith("[") || !flags.EndsWith("]")) {
                throw new FormatException();
            }
            // Lexing esque step to split all flags.
            // Illegal syntax to have any spaces.
            var tokens = flags.Substring(1, flags.Count() - 2).Split(',');
            // Go through tokens and verify they have meaning.
            // Flags can be KVPs, delimited by '='.
            var flagDict = new Dictionary<FlagType, string>(tokens.Count());
            foreach (string token in tokens)
            {
                if (token == null || token.Equals(String.Empty))
                {
                    continue;
                }
                string[] kvp = token.Split('=');
                if (kvp.Count() > 2)
                {
                    // not a kvp or statement, throw FormatException
                    throw new FormatException();
                }
                // Will throw format exception if illegal flag
                var flagType = Flags.LookupFlag(kvp[0]);
                if (kvp.Count() == 1)
                {
                    flagDict[flagType] = string.Empty;
                }
                else 
                {
                    flagDict[flagType] = kvp[1];
                }
            }
            return new Flags(flagDict);
        }
    }
}
