using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using Rewrite.Structure2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
namespace RewriteTest
{
    public class ModRewriteCreatorTest
    {

        [Theory(Skip = "Need to seperately check that nc flags are checked")]
        [InlineData("/Hey/Hello", "/Hello")]
        [InlineData("/heY/wHAT", "/wHAT")]
        public void ModRewriteRule_CheckIgnoreCases(string requestPath, string expected)
        {
            var context = CreateRequest("/", requestPath);
            var rule = new ModRewriteRule
            {
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.ParseRuleFlags("[NC]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.Continue);
            Assert.True(context.Request.Path.Equals(new PathString(expected)));
        }
        [Theory(Skip = "Need to seperately check that regex flags are checked")]
        [InlineData("/Hey/Hello", "/Hello")]
        [InlineData("/heY/wHAT", "/wHAT")]
        public void ModRewriteRule_CheckFailureWhenNotIgnoringCases(string requestPath, string expected)
        {
            var context = CreateRequest("/", requestPath);
            var rule = new ModRewriteRule
            {
                InitialRule = new RuleExpression { Operand = new RegexOperand { Regex = new Regex("/hey/(.*)") }, Invert = false },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.Continue);
            Assert.True(context.Request.Path.Equals(new PathString(requestPath)));
        }
        private HttpContext CreateRequest(string basePath, string requestPath, string requestQuery = "", string hostName = "")
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.PathBase = new PathString(basePath);
            context.Request.Path = new PathString(requestPath);
            context.Request.QueryString = new QueryString(requestQuery);
            context.Request.Host = new HostString(hostName);
            return context;
        }
    }
}
