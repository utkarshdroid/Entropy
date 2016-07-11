// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace RewriteTest
{
    public class ConditionTestStringParserTest
    {
        [Fact]
        public void ConditionParser_SingleServerVariable()
        {
            var serverVar = "%{HTTPS}";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("HTTPS", SegmentType.ServerParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_MultipleServerVariables()
        {
            var serverVar = "%{HTTPS}%{REQUEST_URI}";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("HTTPS", SegmentType.ServerParameter));
            list.Add(new PatternSegment("REQUEST_URL", SegmentType.ServerParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_ParseLiteral()
        {
            var serverVar = "Hello!";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("Hello!", SegmentType.Literal));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_ParseConditionParameters()
        {
            var serverVar = "%1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("1", SegmentType.ConditionParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_ParseMultipleConditionParameters()
        {
            var serverVar = "%1%2";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("1", SegmentType.ConditionParameter));
            list.Add(new PatternSegment("2", SegmentType.ConditionParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_ParseRuleVariable()
        {
            var serverVar = "$1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("1", SegmentType.RuleParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }
        [Fact]
        public void ConditionParser_ParseMultipleRuleVariables()
        {
            var serverVar = "$1$2";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("1", SegmentType.RuleParameter));
            list.Add(new PatternSegment("2", SegmentType.RuleParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Fact]
        public void ConditionParser_ParserComplexRequest()
        {
            var serverVar = "%{HTTPS}/$1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var list = new List<PatternSegment>();
            list.Add(new PatternSegment("HTTPS", SegmentType.ServerParameter));
            list.Add(new PatternSegment("/", SegmentType.Literal));
            list.Add(new PatternSegment("1", SegmentType.RuleParameter));
            var expected = new Pattern(list);
            Assert.True(result.Equals(expected));
        }

        [Theory]
        [InlineData(@"%}")] // no } at end
        [InlineData(@"%{")] // no closing }
        [InlineData(@"%a")] // invalid character after %
        [InlineData(@"$a")] // invalid character after $
        [InlineData(@"%{asdf")] // no closing } with characters
        public void ConditionParser_Bad(string testString)
        {
            Assert.Throws<ArgumentException>(() => ConditionTestStringParser.ParseConditionTestString(testString));
        }
    }
}
