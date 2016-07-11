
using Rewrite.ConditionParser;
using Rewrite.RuleParser;
using Rewrite.Structure2;
using System;
using System.Collections.Generic;
using System.IO;
namespace Rewrite.FileParser
{
    public static class RewriteConfigurationFileParser
    { 
        public static List<Rule> Parse(Stream input)
        {
            var reader = new StreamReader(input);
            var line = (string) null;
            var rules = new List<Rule>();
            var currentRule = new ModRewriteRule { Conditions = new List<Condition>() };
            while ((line = reader.ReadLine()) != null) {
                // TODO handle comments
                List<string> tokens = RewriteTokenizer.TokenizeRule(line);
                if (tokens.Count < 3)
                {
                    // This means the line didn't have an appropriate format, throw format exception
                    throw new FormatException();
                }
                // TODO make a new class called rule parser that does and either return an exception or return the rule.
                switch (tokens[0])
                {
                    case "RewriteCond":
                        {
                            Pattern matchesForCondition = ConditionTestStringParser.ParseConditionTestString(tokens[1]);
                            GeneralExpression ie = ConditionActionParser.ParseActionCondition(tokens[2]);
                            Flags flags = null;
                            if (tokens.Count == 4)
                            {
                                flags = FlagParser.TokenizeAndParseFlags(tokens[3]);
                            }
                            currentRule.Conditions.Add(new Condition(matchesForCondition, ie, flags ));
                            break;
                        }
                    case "RewriteRule":
                        {
                            // parse regex
                            // then do similar logic to the condition test string replacement
                            GeneralExpression ie = RuleRegexParser.ParseRuleRegex(tokens[1]);
                            Pattern matchesForRule = ConditionTestStringParser.ParseConditionTestString(tokens[2]);
                            if (tokens.Count == 4)
                            {
                                currentRule.Flags = FlagParser.TokenizeAndParseFlags(tokens[3]);
                            }
                            currentRule.InitialRule = ie;
                            currentRule.Transforms = matchesForRule;
                            rules.Add(currentRule);
                            currentRule = new ModRewriteRule { Conditions = new List<Condition>() };
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }
            return rules;
        }
    }
}
