using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ModRewriteRule : Rule
    {
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public string Description { get; set; } = string.Empty;
        public RuleExpression InitialRule { get; set; }
        public Pattern Transforms { get; set; }
        public RuleFlags Flags { get; set; } = new RuleFlags();
        public ModRewriteRule() { }
        public ModRewriteRule(List<Condition> conditions, RuleExpression initialRule, Pattern transforms, RuleFlags flags, string description = "")
        {
            Conditions = conditions;
            InitialRule = initialRule;
            Transforms = transforms;
            Flags = flags;
            Description = description;
        }
        public override RuleResult ApplyRule(HttpContext context)
        {
            // 1. Figure out which section of the string to match for the initial rule.
            var results = InitialRule.Operand.Regex.Match(context.Request.Path.ToString());
            //var results = Regex.Match(context.Request.Path.ToString(), InitialRule.Operand, ignoreCase, TimeSpan.FromMilliseconds(1)); 
            if (CheckMatchResult(results.Success))
            {
                return new RuleResult { Result = RuleTerminiation.Continue };
            }

            if (Flags.CheckFlag(RuleFlagType.EscapeBackreference) != null)
            {
                // TODO Escape Backreferences here.
            }
            // 2. Go through all conditions and compare them to the created string
            var previous = Match.Empty;
            if (!CheckCondition(this, context, results, previous))
            {
                return new RuleResult { Result = RuleTerminiation.Continue };
            }
            // at this point, our rule passed, we can now apply the on match function
            var result = Transforms.ApplyPattern(context, results, previous);

            if (Flags.CheckFlag(RuleFlagType.Forbidden) != null)
            {
                context.Response.StatusCode = 403;
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            else if (Flags.CheckFlag(RuleFlagType.Gone) != null)
            {
                context.Response.StatusCode = 410;
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            context.Request.Path = new PathString(result);
            string statusCode;
            if ((statusCode = Flags.CheckFlag(RuleFlagType.Redirect)) != null)
            {
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            else if (Flags.CheckFlag(RuleFlagType.Last) != null || Flags.CheckFlag(RuleFlagType.End) != null)
            {
                return new RuleResult { Result = RuleTerminiation.StopRules };
            }
            else
            {
                return new RuleResult { Result = RuleTerminiation.Continue };
            }
        }

        private bool CheckMatchResult(bool result)
        {
            return !(result ^ InitialRule.Invert);
        }

        private bool CheckCondition(ModRewriteRule rule, HttpContext context, Match results, Match previous)
        {
            if (rule.Conditions == null)
            {
                return true;
            }
            // TODO Visitor pattern here
            foreach (var condition in rule.Conditions)
            {
                var concatTestString = condition.TestStringSegments.ApplyPattern(context, results, previous);
                var match = condition.ConditionExpression.VisitConditionExpression(context, results, previous, concatTestString);

                if (CheckMatchResult(match) && !(Flags.HasFlag(RuleFlagType.Or)))
                {
                    return false;
                }
            }
            return true;
        }

        // TODO
        private bool CheckFileCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }

        // TODO
        private bool CheckIntCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }

        // TODO
        private bool CheckStringCondition(string testString, Condition condition, HttpContext context)
        {
            return true;
        }
    }
}
