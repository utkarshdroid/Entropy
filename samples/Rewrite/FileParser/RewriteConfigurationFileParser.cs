
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
        public static List<Rule> Parse(TextReader input)
        {

            string line = null;
            var rules = new List<Rule>();
            var conditions = new List<Condition>();
            // TODO consider passing Itokenizer and Ifileparser
            while ((line = input.ReadLine()) != null) {
                // TODO handle comments
                var tokens = RewriteTokenizer.TokenizeRule(line);
                if (tokens.Count < 3 || tokens.Count > 4)
                {
                    // This means the line didn't have an appropriate format, throw format exception
                    throw new FormatException();
                }
                // TODO make a new class called rule parser that does and either return an exception or return the rule.
                switch (tokens[0])
                {
                    case "RewriteCond":
                        {
                            ModRewriteConditionBuilder builder = null;
                            if (tokens.Count == 3)
                            {
                                builder = new ModRewriteConditionBuilder(tokens[1], tokens[2]);
                            }
                            else if (tokens.Count == 4)
                            {
                                builder = new ModRewriteConditionBuilder(tokens[1], tokens[2], tokens[3]);
                            }
                            else
                            {
                                throw new FormatException();
                            }
                            conditions.Add(builder.Build());
                            break;
                        }
                    case "RewriteRule":
                        {
                            ModRewriteRuleBuilder builder = null;
                            if (tokens.Count == 3)
                            {
                                builder = new ModRewriteRuleBuilder(tokens[1], tokens[2]);
                            }
                            else if (tokens.Count == 4)
                            {
                                builder = new ModRewriteRuleBuilder(tokens[1], tokens[2], tokens[3]);
                            }
                            else
                            {
                                throw new FormatException();
                            }
                            builder.AddConditions(conditions);
                            rules.Add(builder.Build());
                            conditions = new List<Condition>();
                            break;
                        }
                    default:
                        throw new NotImplementedException(tokens[0]);
                }
            }
            return rules;
        }

    }
}
