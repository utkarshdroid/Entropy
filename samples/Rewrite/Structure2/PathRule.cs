using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class PathRule : Rule
    {
        public Regex MatchPattern { get; set; }
        public string OnMatch { get; set; }
        public Transformation OnCompletion { get; set; } = Transformation.Rewrite;
        public override RuleResult ApplyRule(HttpContext context)
        {
            var matches = MatchPattern.Match(context.Request.Path);
            if (matches.Success)
            {
                // New method here to translate the outgoing format string to the correct value.
                try
                {
                    var path = matches.Result(OnMatch);
                    if (OnCompletion == Transformation.Redirect)
                    {
                        var req = context.Request;
                        var newUrl = string.Concat(
                            req.Scheme,
                            "://",
                            req.PathBase,
                            path,
                            req.QueryString);
                        context.Response.Redirect(newUrl);
                        return new RuleResult { Result = RuleTerminiation.ResponseComplete };
                    }
                    else
                    {
                        context.Request.Path = path;
                    }
                    if (OnCompletion == Transformation.TerminatingRewrite)
                    {
                        return new RuleResult { Result = RuleTerminiation.StopRules };
                    }
                    else
                    {
                        return new RuleResult { Result = RuleTerminiation.Continue };
                    }
                }
                catch (FormatException fe)
                {
                    // TODO handle format exception correctly
                }
                catch (ArgumentNullException ane)
                {
                    // TODO handle argment null exception correctly
                }
            }
            return new RuleResult { Result = RuleTerminiation.Continue };
        }
    }
    public enum Transformation
    {
        Rewrite,
        Redirect,
        TerminatingRewrite
    }
}
