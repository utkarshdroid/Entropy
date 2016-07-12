// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rewrite.Structure2
{
    public class UrlRewriteBuilder
    {
        private List<Rule> _rules = new List<Rule>();

        public List<Rule> Build()
        {
            return new List<Rule>(_rules);
        }

        public void AddRules(List<Rule> rules)
        {
            _rules.AddRange(rules);
        }

        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }

        public void RewritePath(string regex, string newPath, bool stopRewriteOnSuccess = false)
        {
            _rules.Add(new PathRule { MatchPattern = new Regex(regex), OnMatch = newPath, OnCompletion = stopRewriteOnSuccess ? Transformation.TerminatingRewrite : Transformation.Rewrite });
        }

        public void RewriteScheme(bool stopRewriteOnSuccess = false)
        {
            _rules.Add(new SchemeRule {OnCompletion = stopRewriteOnSuccess ? Transformation.TerminatingRewrite : Transformation.Rewrite });
        }

        public void RedirectPath(string regex, string newPath, bool stopRewriteOnSuccess = false)
        {
            _rules.Add(new PathRule { MatchPattern = new Regex(regex), OnMatch = newPath, OnCompletion = Transformation.Redirect });
        }

        public void RedirectScheme(int? sslPort)
        {
            _rules.Add(new SchemeRule { SSLPort = sslPort, OnCompletion = Transformation.Redirect });
        }

        public void CustomRule(Func<HttpContext, RuleResult> onApplyRule, Transformation transform, string description = null)
        {
            _rules.Add(new FunctionalRule { OnApplyRule = onApplyRule, OnCompletion = transform});
        }


        public void RulesFromConfig(IConfiguration rulesFromConfig)
        {
            // TODO figure out naming
            var rules = rulesFromConfig.GetSection("Rewrite").GetChildren();
            // TODO eventually delegate this to another method.
            foreach (var item in rules)
            {
                switch (item["type"])
                {
                    case "RewritePath":
                        RewritePath(item["match"], item["action"], item["terminate"] == "true");
                        break;
                    case "RewriteScheme":
                        RewriteScheme(item["terminate"] == "true");
                        break;
                    case "RedirectPath":
                        RedirectPath(item["match"], item["action"], item["terminate"] == "true");
                        break;
                    case "RedirectScheme":
                        int i; // TODO is this the right style here?
                        RedirectScheme(Int32.TryParse(item["port"], out i) ? i : (int?)null);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
