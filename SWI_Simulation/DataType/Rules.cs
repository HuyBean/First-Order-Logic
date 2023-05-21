using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SWI_Simulation.DataType
{
    public class Rule
    {
        public List<Tern> Left {get; private set;}
        public List<Tern> Right {get; private set;}
        public HashSet<string> Variables {get; private set;}
        public List<string> VariablesList => Variables.ToList();
        public List<bool>? isFacts;
        public Rule()
        {
            Left = new List<Tern>();
            Right = new List<Tern>();
            Variables = new HashSet<string>();
        }

        public Rule(string val) : this()
        {
            if (val[val.Length - 1] == '.')
                val = val.Remove(val.Length - 1);
            var Parts = val.Split(":-");
            Parts[0] = Parts[0].TrimStart().TrimEnd();
            Parts[1] = Parts[1].TrimStart().TrimEnd();

            addTern(Left, Parts[0]);

            var RightRaw = new List<Tern>();
            addTern(Right, Parts[1]);
        }

        private void addTern(List<Tern> containter, string raw)
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
                            if (arg.Type == TernType.Variable)
                            {
                                Variables ??= new HashSet<string>();
                                Variables?.Add(arg.Value);
                            }
                        }
                    }
                    containter.Add(item);
                }
            }
        }
    }
}