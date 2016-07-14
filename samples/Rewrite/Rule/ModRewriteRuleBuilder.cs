using Rewrite.ConditionParser;
using Rewrite.FileParser;
using Rewrite.RuleParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ModRewriteRuleBuilder
    {
        private ParsedConditionExpression _pce;
        private List<Condition> _conditions;
        private RuleFlags _flags;
        private Pattern _patterns;
        public ModRewriteRule Build()
        {
            var ruleExpression = ModRewriteExpressionCreator.CreateRuleExpression(_pce, _flags);
            return new ModRewriteRule(_conditions, ruleExpression, _patterns, _flags);
        }

        public ModRewriteRuleBuilder(string initialRule, string transformation) : 
            this(initialRule, transformation, flags:null)
        {
        }
        public ModRewriteRuleBuilder(string rule)
        {
            // TODO
            var tokens = RewriteTokenizer.TokenizeRule(rule);
            if (tokens.Count() == 3)
            {
                CreateRule(tokens[1], tokens[2], null);
            }
            else if (tokens.Count() == 4)
            {
                CreateRule(tokens[1], tokens[2], tokens[3]);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public ModRewriteRuleBuilder(string initialRule, string transformation, string flags)
        {
            CreateRule(initialRule, transformation, flags);
        }
        public void CreateRule(string initialRule, string transformation, string flags)
        {
            _pce = RuleRegexParser.ParseRuleRegex(initialRule);
            _patterns = ConditionTestStringParser.ParseConditionTestString(transformation);
            _flags = FlagParser.ParseRuleFlags(flags);
        }

        public void AddCondition(string condition)
        {
            if (_conditions == null)
            {
                _conditions = new List<Condition>();
            }
            var condBuilder = new ModRewriteConditionBuilder(condition);
            _conditions.Add(condBuilder.Build());
        }

        public void AddCondition(Condition condition)
        {
            if (_conditions == null)
            {
                _conditions = new List<Condition>();
            }
            _conditions.Add(condition);
        }
        
        public void AddConditions(List<Condition> conditions)
        {
            if (_conditions == null)
            {
                _conditions = new List<Condition>();
            }
            _conditions.AddRange(conditions);
        }

        public void AddFlag(string flag)
        {
            AddFlag(flag, null);
        }
        
        public void AddFlag(RuleFlagType flag)
        {
            AddFlag(flag, null);
        }

        public void AddFlag(string flag, string value)
        {
            if (_flags == null)
            {
                _flags = new RuleFlags();
            }
            _flags.AddFlag(flag, value);
        }

        public void AddFlag(RuleFlagType flag, string value)
        {
            if (_flags == null)
            {
                _flags = new RuleFlags();
            }
            _flags.AddFlag(flag, value);
        }

        public void AddFlags(string flags)
        {
            // TODO ignore [].
            if (_flags == null)
            {
                _flags = FlagParser.ParseRuleFlags(flags);
            }
            else
            {
                FlagParser.ParseRuleFlags(flags, _flags);
            }
        }
    }
}
