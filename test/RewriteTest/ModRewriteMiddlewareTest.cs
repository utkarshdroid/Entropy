using Microsoft.AspNetCore.Http;
using Rewrite.ConditionParser;
using Rewrite.Structure2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
namespace RewriteTest
{
    public class ModRewriteMiddlewareTest
    {
        [Fact]
        public void Invoke_RewritePathWhenMatching()
        {
            var context = CreateRequest("/", "/hey/hello");

            var rewriteBuilder = new UrlRewriteBuilder();
            var ruleBuilder = new ModRewriteRuleBuilder("RewriteRule /hey/(.*) /$1 ");
            rewriteBuilder.AddModRule("RewriteRule /hey/(.*) /$1 [L]");
            RequestDelegate next = (c) =>
            {
                return Task.FromResult<object>(null);
            };

            var options = new UrlRewriteOptions { Rules = rewriteBuilder.Build()};
            var middleware = new UrlRewriteMiddleware(next, options);
            middleware.Invoke(context);
            Assert.True(context.Request.Path.Value == "/hello");
        }

        [Fact]
        public void Invoke_RewritePathTerminatesOnFirstSuccessOfRule()
        {
            var context = CreateRequest("/", "/hey/hello");

            var rewriteBuilder = new UrlRewriteBuilder();

            rewriteBuilder.AddModRule("RewriteRule /hey/(.*) /$1 [L]");
            rewriteBuilder.AddModRule("RewriteRule /hello /what");
            RequestDelegate next = (c) =>
            {
                return Task.FromResult<object>(null);
            };

            var options = new UrlRewriteOptions { Rules = rewriteBuilder.Build() };
            var middleware = new UrlRewriteMiddleware(next, options);
            middleware.Invoke(context);
            Assert.True(context.Request.Path.Value == "/hello");
        }

        [Fact]
        public void Invoke_RewritePathDoesNotTerminateOnFirstSuccessOfRule()
        {
            var context = CreateRequest("/", "/hey/hello");

            var rewriteBuilder = new UrlRewriteBuilder();
            rewriteBuilder.AddModRule("RewriteRule /hey/(.*) /$1");
            rewriteBuilder.AddModRule("RewriteRule /hello /what");
            RequestDelegate next = (c) =>
            {
                return Task.FromResult<object>(null);
            };

            var options = new UrlRewriteOptions { Rules = rewriteBuilder.Build() };
            var middleware = new UrlRewriteMiddleware(next, options);
            middleware.Invoke(context);
            Assert.True(context.Request.Path.Value == "/what");
        }
        [Theory]
        [InlineData("", true)]
        public void Invoke_StringComparisonTests(string input, bool expected)
        {

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
