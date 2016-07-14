using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class RuleFlags
    {
        private static readonly IDictionary<string, RuleFlagType> RuleFlagLookup = new Dictionary<string, RuleFlagType>(StringComparer.OrdinalIgnoreCase) {
            { "b", RuleFlagType.EscapeBackreference},
            { "c", RuleFlagType.Chain },
            { "co", RuleFlagType.Cookie },
            { "dpi", RuleFlagType.DiscardPath },
            { "e", RuleFlagType.Env},
            { "end", RuleFlagType.End },
            { "f", RuleFlagType.Forbidden },
            { "g", RuleFlagType.Gone },
            { "h", RuleFlagType.Handler },
            { "l", RuleFlagType.Last },
            { "n", RuleFlagType.Next },
            { "nc", RuleFlagType.NoCase },
            { "ne", RuleFlagType.NoEscape },
            { "ns", RuleFlagType.NoSubReq },
            { "p", RuleFlagType.Proxy },
            { "pt", RuleFlagType.PassThrough },
            { "qsa", RuleFlagType.QSAppend },
            { "qsd", RuleFlagType.QSDiscard },
            { "qsl", RuleFlagType.QSLast },
            { "r", RuleFlagType.Redirect },
            { "s", RuleFlagType.Skip },
            { "t", RuleFlagType.Type }
            };

        // TODO make private
        public IDictionary<RuleFlagType, string> FlagDictionary { get; }

        public RuleFlags(IDictionary<RuleFlagType, string> flags)
        {
            // TODO use ref to check dictionary equality
            FlagDictionary = flags;
        }

        public RuleFlags()
        {
            FlagDictionary = new Dictionary<RuleFlagType, string>();
        }

        public void AddFlag(string flag, string value)
        {
            RuleFlagType res;
            if (!RuleFlagLookup.TryGetValue(flag, out res))
            {
                throw new FormatException("Invalid flag");
            }
            AddFlag(res, value);
        }
        public void AddFlag(RuleFlagType flag, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            FlagDictionary[flag] = value;
        }

        public string CheckFlag(RuleFlagType flag)
        {
            string res;
            if (!FlagDictionary.TryGetValue(flag, out res)) 
            {
                return null;
            }
            return res;
        }

        public bool HasFlag(RuleFlagType flag)
        {
            string res;
            return FlagDictionary.TryGetValue(flag, out res);
        }
    }
    public enum RuleFlagType
    {
        EscapeBackreference,
        Chain,
        Cookie,
        DiscardPath,
        Env,
        End,
        Forbidden,
        Gone,
        Handler,
        Last,
        Next,
        NoCase,
        NoEscape,
        NoSubReq,
        NoVary,
        Or,
        Proxy,
        PassThrough,
        QSAppend,
        QSDiscard,
        QSLast,
        Redirect,
        Skip,
        Type
    }
}
