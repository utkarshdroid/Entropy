using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rewrite.Structure2
{
    public class FunctionalRule : Rule
    {
        public Func<HttpContext, RuleResult> OnApplyRule { get; set; }
        public Transformation OnCompletion { get; set; } = Transformation.Rewrite;
        public override RuleResult ApplyRule(HttpContext context) => OnApplyRule(context);
    }
}