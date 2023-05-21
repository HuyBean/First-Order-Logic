using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SWI_Simulation.DataType
{
    public class Query
    {
        public HashSet<Tern> Condition {get; private set;}
        public List<Tern> ConditionList => Condition.ToList();
        public HashSet<string>? Variables {get; private set;}
        public List<string>? VariablesList => Variables?.ToList();
        public HashSet<string>? Atoms{get; private set;}
        public Query()
        {
            Condition = new HashSet<Tern>();
        }

        public Query(string val) : this()
        {            
            if (!isQuery(val))
                return;
            val = val.Remove(0, 2);
            addTern(val);
        }

        private void addTern(string raw)
        {
            foreach (var val in Regex.Matches(raw, RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value))
            {
                if (val is null)
                    continue;
                Tern? item = Tern.StringToTern(val);
                if (item is not null)
                {
                    if (item.Arguments is not null)
                    {
                        foreach (var arg in item.Arguments)
                        {
                            if (arg.Type == TernType.Atom)
                            {
                                Atoms ??= new HashSet<string>();
                                Atoms?.Add(arg.Value);
                            }
                            if (arg.Type == TernType.Variable)
                            {
                                Variables ??= new HashSet<string>();
                                Variables?.Add(arg.Value);
                            }
                        }                    
                    }
                    Condition.Add(item);
                }
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

        public override string ToString()
        {
            string value = "";
            if (Condition is null)
                return value;
            var temp = Condition.Cast<Tern>().Select(t => t.ToString());
            return "?-" + string.Join(", ", temp);
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