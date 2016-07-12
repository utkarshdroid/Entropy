using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class Pattern
    {
        private IEnumerable<PatternSegment> Segments { get; }
        public Pattern (IEnumerable<PatternSegment> segments)
        {
            Segments = segments;
        }
        public string ApplyPattern(HttpContext context, Match ruleMatch, Match prevCondition)
        {
            var res = new StringBuilder();
            foreach (PatternSegment segment in Segments)
            {
                // TODO handle case when segment.Variable is 0.
                switch (segment.Type)
                {
                    case SegmentType.Literal:
                        res.Append(segment.Variable);
                        break;
                    case SegmentType.ServerParameter:
                        var serverParam = ServerVariables.ApplyServerVariable(context, segment.Variable);
                        // TODO fix ApplyServerVariable to actually modify context.
                        if (serverParam != ServerVariable.NONE)
                        {
                            res.Append(serverParam);
                        }
                        break;
                    case SegmentType.RuleParameter:
                        var ruleParam = ruleMatch.Groups[segment.Variable];
                        if (ruleParam != null)
                        {
                            res.Append(ruleParam);
                        }
                        break;
                    case SegmentType.ConditionParameter:
                        var condParam = prevCondition.Groups[segment.Variable];
                        if (condParam != null)
                        {
                            res.Append(condParam);
                        }
                        break;
                }
            }
            return res.ToString();
        }
        public override bool Equals(Object other)
        {
            if (!(other is Pattern))
            {
                return false;
            }
            var pattern = (Pattern)other;
            if (Segments.Count() != pattern.Segments.Count())
            {
                return false;
            }
            for (var i = 0; i < Segments.Count(); i++)
            {
                if (!pattern.Segments.ElementAt(i).Equals(pattern.Segments.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            var i = 31;
            foreach (var pattern in Segments)
            {
                i = i * pattern.GetHashCode();
            }
            return i;
        }
    }
}
