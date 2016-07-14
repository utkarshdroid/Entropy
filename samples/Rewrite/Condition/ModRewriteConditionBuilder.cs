using Rewrite.ConditionParser;
using Rewrite.FileParser;
using Rewrite.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ModRewriteConditionBuilder
    {
        private Pattern _testString;
        private ParsedConditionExpression _pce;
        private ConditionFlags _flags;

        public ModRewriteConditionBuilder(string conditionString)
        {
            // TODO remove the first RewriteCond token?
            var tokens = RewriteTokenizer.TokenizeRule(conditionString);
            if (tokens.Count() == 3)
            {
                CreateCondition(tokens[1], tokens[2], null);
            }
            else if (tokens.Count() == 4)
            {
                CreateCondition(tokens[1], tokens[2], tokens[3]);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public ModRewriteConditionBuilder(string testString, string condition)
        {
            CreateCondition(testString, condition, null);
        }

        public ModRewriteConditionBuilder(string testString, string condition, string flags)
        {
            CreateCondition(testString, condition, flags);
        }
        public Condition Build()
        {
            var expression = ModRewriteExpressionCreator.CreateConditionExpression(_pce, _flags);
            return new Condition(_testString, expression, _flags);
        }
        private void CreateCondition(string testString, string condition, string flagsString)
        {
            _testString = ConditionTestStringParser.ParseConditionTestString(testString);
            _pce = ConditionActionParser.ParseActionCondition(condition);
            _flags = FlagParser.ParseConditionFlags(flagsString);
        }

        public void AddFlag(string flag)
        {
            AddFlag(flag, null);
        }

        public void AddFlag(ConditionFlagType flag)
        {
            AddFlag(flag, null);
        }

        public void AddFlag(string flag, string value)
        {
            if (_flags == null)
            {
                _flags = new ConditionFlags();
            }
            _flags.AddFlag(flag, value);
        }
        public void AddFlag(ConditionFlagType flag, string value)
        {
            if (_flags == null)
            {
                _flags = new ConditionFlags();
            }
            _flags.AddFlag(flag, value);
        }

        public void AddFlags(string flags)
        {
            // TODO ignore [].
            if (_flags == null)
            {
                _flags = FlagParser.ParseConditionFlags(flags);
            }
            else
            {
                FlagParser.ParseConditionFlags(flags, _flags);
            }
        }

    }
}
