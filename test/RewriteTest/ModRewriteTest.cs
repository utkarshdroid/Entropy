using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using Rewrite.Structure2;
using Xunit;

namespace RewriteTest
{
    public class ModRewriteTest
    {
        // Flag tests
        [Fact]
        public void ModRewriteRule_Check403OnForbiddenFlag()
        {
            var context = CreateRequest("/", "/hey/hello");
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.TokenizeAndParseFlags("[F]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.ResponseComplete);
            Assert.True(context.Response.StatusCode == 403);
        }

        [Fact]
        public void ModRewriteRule_Check410OnGoneFlag()
        {
            var context = CreateRequest("/", "/hey/hello");
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.TokenizeAndParseFlags("[G]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.ResponseComplete);
            Assert.True(context.Response.StatusCode == 410);
        }

        [Fact]
        public void ModRewriteRule_CheckLastFlag()
        {
            var context = CreateRequest("/", "/hey/hello");
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.TokenizeAndParseFlags("[L]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.StopRules);
            Assert.True(context.Request.Path.Equals(new PathString("/hello")));
        }

        [Theory]
        [InlineData("/Hey/Hello", "/Hello")]
        [InlineData("/heY/wHAT", "/wHAT")]
        public void ModRewriteRule_CheckIgnoreCases(string requestPath, string expected)
        {
            var context = CreateRequest("/", requestPath);
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.TokenizeAndParseFlags("[NC]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.Continue);
            Assert.True(context.Request.Path.Equals(new PathString(expected)));
        }
        [Theory]
        [InlineData("/Hey/Hello", "/Hello")]
        [InlineData("/heY/wHAT", "/wHAT")]
        public void ModRewriteRule_CheckFailureWhenNotIgnoringCases(string requestPath, string expected)
        {
            var context = CreateRequest("/", requestPath);
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.Continue);
            Assert.True(context.Request.Path.Equals(new PathString(requestPath)));
        }

        [Fact]
        public void ModRewriteRule_CheckRedirectFlag()
        {
            var context = CreateRequest("/", "/hey/hello");
            var rule = new ModRewriteRule
            {
                InitialRule = new GeneralExpression { Operand = "/hey/(.*)", Type = ConditionType.Regex },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
                Flags = FlagParser.TokenizeAndParseFlags("[G]")
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.ResponseComplete);
            Assert.True(context.Response.StatusCode == 410);
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
