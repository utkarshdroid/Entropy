using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class Flags
    {
        private static readonly IDictionary<string, FlagType> FlagLookup = new Dictionary<string, FlagType>(StringComparer.OrdinalIgnoreCase) {
            { "b", FlagType.EscapeBackreference},
            { "c", FlagType.Chain },
            { "co", FlagType.Cookie },
            { "dpi", FlagType.DiscardPath },
            { "e", FlagType.Env},
            { "end", FlagType.End },
            { "f", FlagType.Forbidden },
            { "g", FlagType.Gone },
            { "h", FlagType.Handler },
            { "l", FlagType.Last },
            { "n", FlagType.Next },
            { "nc", FlagType.NoCase },
            { "ne", FlagType.NoEscape },
            { "ns", FlagType.NoSubReq },
            { "p", FlagType.Proxy },
            { "pt", FlagType.PassThrough },
            { "qsa", FlagType.QSAppend },
            { "qsd", FlagType.QSDiscard },
            { "qsl", FlagType.QSLast },
            { "r", FlagType.Redirect },
            { "s", FlagType.Skip },
            { "t", FlagType.Type }
            };

        public IDictionary<FlagType, string> FlagDictionary { get; }
        public Flags(IDictionary<FlagType, string> flags)
        {
            FlagDictionary = flags;
        }

        public static FlagType LookupFlag(string flag)
        {
            FlagType res;
            if (!FlagLookup.TryGetValue(flag, out res))
            {
                throw new FormatException("Invalid flag");
            }
            return res;
        }
        public string CheckFlag(FlagType flag)
        {
            string res;
            if (!FlagDictionary.TryGetValue(flag, out res)) 
            {
                return null;
            }
            return res;
        }
    }
    public enum FlagType
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
