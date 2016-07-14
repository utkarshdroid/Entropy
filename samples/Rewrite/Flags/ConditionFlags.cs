using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Flags
{
    public class ConditionFlags
    {
        private static readonly IDictionary<string, ConditionFlagType> ConditionFlagLookup = new Dictionary<string, ConditionFlagType>(StringComparer.OrdinalIgnoreCase) {
            { "nc", ConditionFlagType.NoCase},
            { "or", ConditionFlagType.Or},
            { "nv", ConditionFlagType.NoVary}
            };

        // TODO make private
        public IDictionary<ConditionFlagType, string> FlagDictionary { get; }

        public ConditionFlags(IDictionary<ConditionFlagType, string> flags)
        {
            // TODO use ref to check dictionary equality
            FlagDictionary = flags;
        }

        public ConditionFlags()
        {
            FlagDictionary = new Dictionary<ConditionFlagType, string>();
        }

        public void AddFlag(string flag, string value)
        {
            ConditionFlagType res;
            if (!ConditionFlagLookup.TryGetValue(flag, out res))
            {
                throw new FormatException("Invalid flag");
            }
            AddFlag(res, value);
        }
        public void AddFlag(ConditionFlagType flag, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            FlagDictionary[flag] = value;
        }

        public string CheckFlag(ConditionFlagType flag)
        {
            string res;
            if (!FlagDictionary.TryGetValue(flag, out res))
            {
                return null;
            }
            return res;
        }

        public bool HasFlag(ConditionFlagType flag)
        {
            string res;
            return FlagDictionary.TryGetValue(flag, out res);
        }
    }
    public enum ConditionFlagType
    {
        NoCase,
        Or,
        NoVary
    }
}
