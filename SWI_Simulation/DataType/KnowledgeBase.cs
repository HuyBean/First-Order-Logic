using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace SWI_Simulation.DataType
{
    public class KnowledgeBase
    {
        public bool isExplored {get;set;}
        private Dictionary<string, HashSet<Tern>> Facts;
        public List<Query> Queries { get; private set; }
        public List<Rule> Rules {get; private set;}
        public HashSet<String> Atoms{get;set;}
        public List<string> AtomsList => Atoms.ToList();
        public int FactsCount 
        {
            get
            {
                int result = 0;
                foreach(var item in Facts)
                {
                    result += item.Value.Count;
                }
                return result;
            }
        }

        public bool ContainsFact(Tern t)
        {
            if (!Facts.ContainsKey(t.Value))
                return false;
            return Facts[t.Value].Contains(t);
        }

        public bool isRule(String val)
        {
            var part = val.Split(":-");
            if (part.Length != 2) return false;
            return true;
        }
        public bool isFact(string val)
        {
            if (val[val.Length - 1] == '.')
                val = val.Remove(val.Length - 1);
            val = val.TrimStart().TrimEnd();
            return Regex.IsMatch(val, RegexPattern.FACT_PATTERN);
        }

        public KnowledgeBase()
        {
            Queries = new List<Query>();
            Facts = new Dictionary<string, HashSet<Tern>>();
            Rules = new List<Rule>();
            Atoms = new HashSet<string>();
        }

        public KnowledgeBase Clone()
        {
            KnowledgeBase Base = new KnowledgeBase();
            Base.Rules = Rules;
            return Base;
        }

        private void addTern(List<Tern> container, string raw)
        {
            isExplored = false;
            foreach (var val in Regex.Matches(raw, RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value))
            {
                if (val is null)
                    continue;
                Tern? item = Tern.StringToTern(val);

                if (item?.Arguments != null)
                {
                    foreach (var t in item.Arguments)
                    {
                        if (t.Type == TernType.Atom)
                            Atoms.Add(t.Value);
                    }
                }

                if (item is not null)
                    container.Add(item);
            }
        }

        private void addTern(HashSet<Tern> container, string raw)
        {
            foreach(var val in Regex.Matches(raw, RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value))
            {
                if (val is null)
                    continue;
                Tern? item = Tern.StringToTern(val);
                
                if (item?.Arguments != null)
                {
                    foreach (var t in item.Arguments)
                    {
                        if (t.Type == TernType.Atom)
                            Atoms.Add(t.Value);
                    }
                }

                if (item is not null)
                    container.Add(item);
            }
        }

        public void addFact(string raw)
        {
            foreach (var val in Regex.Matches(raw, RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value))
            {
                if (val is null)
                    continue;
                Tern? item = Tern.StringToTern(val);

                if (item?.Arguments != null)
                {
                    foreach (var t in item.Arguments)
                    {
                        if (t.Type == TernType.Atom)
                            Atoms.Add(t.Value);
                    }
                }

                if (item is not null)
                {
                    if (!Facts.ContainsKey(item.Value))
                        Facts[item.Value] = new HashSet<Tern>();
                    Facts[item.Value].Add(item);
                }
            }
        }

        public void addFact(Tern item)
        {
            if (!Facts.ContainsKey(item.Value))
                Facts[item.Value] = new HashSet<Tern>();
            Facts[item.Value].Add(item);
        }

        public void addRule (string val)
        {
            Rules.Add(new Rule(val));
        }

        public void addQuerries(string val)
        {
            Queries.Add(new Query(val));
        }

        public void addQuerries(Query val)
        {
            Queries.Add(val);
        }
    }
}