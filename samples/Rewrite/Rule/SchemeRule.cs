using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class SchemeRule : Rule
    {
        public int? SSLPort { get; set; }
        public Transformation OnCompletion { get; set; } = Transformation.Rewrite;
        public override RuleResult ApplyRule(HttpContext context)
        {

            // TODO this only does http to https, add more features in the future. 
            if (!context.Request.IsHttps)
            {
                var host = context.Request.Host;
                if (SSLPort.HasValue && SSLPort.Value > 0)
                {
                    // a specific SSL port is specified
                    host = new HostString(host.Host, SSLPort.Value);
                }
                else
                {
                    // clear the port
                    host = new HostString(host.Host);
                }

                if ((OnCompletion != Transformation.Redirect))
                {
                    context.Request.Scheme = "https";
                    context.Request.Host = host;
                    if (OnCompletion == Transformation.TerminatingRewrite)
                    {
                        return new RuleResult { Result = RuleTerminiation.StopRules };
                    }
                    else
                    {
                        return new RuleResult { Result = RuleTerminiation.Continue };
                    }
                }

                var req = context.Request;
                var newUrl = string.Concat(
                    "https://",
                    host,
                    req.PathBase,
                    req.Path,
                    req.QueryString);
                context.Response.Redirect(newUrl);
                return new RuleResult { Result = RuleTerminiation.ResponseComplete };
            }
            return new RuleResult { Result = RuleTerminiation.Continue }; ;
        }
    }
}
