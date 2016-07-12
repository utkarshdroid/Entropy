// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Rewrite.ConditionParser
{
    public enum SegmentType
    {
        Literal,
        ServerParameter,
        ConditionParameter,
        RuleParameter
    }
    public class PatternSegment
    {
        public string Variable { get; }
        public SegmentType Type { get; }

        public PatternSegment(string variable, SegmentType type)
        {
            Variable = variable;
            Type = type;
        }
        public override bool Equals(object other)
        {
            if (!(other is PatternSegment))
            {
                return false;
            }
            PatternSegment ps = (PatternSegment)other;
            return Variable.Equals(ps.Variable) && Type.Equals(ps.Type);
        }
        public override int GetHashCode()
        {
            return Variable.GetHashCode() + Type.GetHashCode();
        }
    }
}
