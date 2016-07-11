using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public abstract class Rule
    {
        public abstract RuleResult ApplyRule(HttpContext context);
    }
}

