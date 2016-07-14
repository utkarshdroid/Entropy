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
    public class ModRewriteInvokeTest
    {
        // Create rule, apply rule, check context modified
        [Fact]
        public void Invoke_CheckSimpleRule()
        {
            var context = CreateRequest("/", "/hey/hello");
            var rule = new ModRewriteRule
            {
                InitialRule = new RuleExpression { Operand = new RegexOperand { Regex = new Regex("/hey/(.*)") }, Invert = false },
                Transforms = ConditionTestStringParser.ParseConditionTestString("/$1"),
            };
            var res = rule.ApplyRule(context);
            Assert.True(res.Result == RuleTerminiation.Continue);
            Assert.True(context.Request.Path.Equals(new PathString("/hello")));
        }

        private HttpContext CreateRequest(string basePath, string requestPath)
        {
            HttpContext context = new DefaultHttpContext();
            context.Request.PathBase = new PathString(basePath);
            context.Request.Path = new PathString(requestPath);
            context.Request.Host = new HostString("example.com");
            return context;
        }
    }
}
