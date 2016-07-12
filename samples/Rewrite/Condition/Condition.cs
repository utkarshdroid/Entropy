// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rewrite.ConditionParser
{
    public class Condition
    {
        public Pattern TestStringSegments { get; }
        public ConditionExpression ConditionExpression { get; }
        public Flags Flags { get; }
        public Condition(Pattern testStringSegments, ConditionExpression conditionRegex, Flags flags)
        {
            TestStringSegments = testStringSegments;
            ConditionExpression = conditionRegex;
            Flags = flags;
        }
    }
}
