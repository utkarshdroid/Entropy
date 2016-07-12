using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class RuleResult
    {
        public RuleTerminiation Result { get; set; }
    }

    public enum RuleTerminiation
    {
        Continue,
        ResponseComplete,
        StopRules
    }
}
