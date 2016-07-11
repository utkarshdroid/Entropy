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
        public GeneralExpression InitialRule { get; set; }
        public Pattern Transforms { get; set; }
        public Flags Flags { get; set; } = new Flags(new Dictionary<FlagType, string>());
        public override RuleResult ApplyRule(HttpContext context)
        {
            // 1. Figure out which section of the string to match for the initial rule.
            RegexOptions ignoreCase = RegexOptions.None;
            if (Flags.CheckFlag(FlagType.NoCase) != null)
            {
                ignoreCase = RegexOptions.IgnoreCase;
            }
            var results = Regex.Match(context.Request.Path.ToString(), InitialRule.Operand, ignoreCase, TimeSpan.FromMilliseconds(1)); 
            if (CheckResult(results.Success))
            {
                return new RuleResult { Result = RuleTerminiation.Continue };
            }

            if (Flags.CheckFlag(FlagType.EscapeBackreference) != null)
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

            if (Flags.CheckFlag(FlagType.Forbidden) != null)
            {
                context.Response.StatusCode = 403;
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            else if (Flags.CheckFlag(FlagType.Gone) != null)
            {
                context.Response.StatusCode = 410;
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            context.Request.Path = new PathString(result);
            string statusCode;
            if ((statusCode = Flags.CheckFlag(FlagType.Redirect)) != null)
            {
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            else if (Flags.CheckFlag(FlagType.Last) != null || Flags.CheckFlag(FlagType.End) != null)
            {
                return new RuleResult { Result = RuleTerminiation.StopRules };
            }
            else
            {
                return new RuleResult { Result = RuleTerminiation.Continue };
            }
        }

        private bool CheckResult(bool result)
        {
            return !(result ^ InitialRule.Invert);
        }

        private bool CheckCondition(ModRewriteRule rule, HttpContext context, Match results, Match previous)
        {
            foreach (var condition in rule.Conditions)
            {
                var concatTestString = condition.TestStringSegments.ApplyPattern(context, results, previous);
                var pass = false;
                switch (condition.ConditionRegex.Type)
                {
                    case ConditionType.PropertyTest:
                        pass = CheckFileCondition(concatTestString, condition, context);
                        break;
                    case ConditionType.IntComp:
                        pass = CheckIntCondition(concatTestString, condition, context);
                        break;
                    case ConditionType.StringComp:
                        pass = CheckStringCondition(concatTestString, condition, context);
                        break;
                    case ConditionType.Regex:
                        previous = Regex.Match(concatTestString, condition.ConditionRegex.Operand);
                        pass = previous.Success;
                        break;
                }
                if (CheckResult(pass) && !(condition.Flags.CheckFlag(FlagType.Or) != null))
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
