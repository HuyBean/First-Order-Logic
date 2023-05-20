using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SWI_Simulation.DataType
{
    public class Query
    {
        public HashSet<Tern> Condition {get; private set;}
        public HashSet<string>? Variables {get; private set;}
        public Query()
        {
            Condition = new HashSet<Tern>();
        }

        public static implicit operator Query(string val)
        {
            Query q = new Query();

            if (!isQuery(val))
                return q;
            val = val.Remove(0, 2);
            addTern(q.Condition, val);
            return q;
        }

        private static void addTern(HashSet<Tern> container, string raw)
        {
            foreach (var val in Regex.Matches(raw, RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value))
            {
                if (val is null)
                    continue;
                Tern? item = Tern.StringToTern(val);

                if (item is not null)
                    container.Add(item);
            }
        }

        public void AddCondition (Tern? t)
        {
            if (t is null) return;

            Condition.Add(t);
            if (t.Arguments is null)
                return;
            foreach (var arg in t.Arguments)
            {
                if (arg.Type == TernType.Variable)
                {
                    Variables ??= new HashSet<string>();
                    Variables?.Add(arg.Value);
                }
            }
        }

        public bool hasVariable ()
        {
            return (Variables is null);
        }

        public static bool isQuery(string val)
        {
            if (!Regex.IsMatch(val, RegexPattern.QUERY_SIMPLE_PARTTERN))
                return false;
            string temp = val.Remove(0, 2);
            if (!(Regex.Matches(val, RegexPattern.QUERIES_COMPONENT_PATTERN).Count > 0))
                return false;
            return true;
        }
    }
}