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
        public HashSet<Tern> Facts {get; private set;}
        public List<Query> Queries { get; private set; }
        public List< Tuple< List<Tern>, List<Tern>>> Rules {get; private set;}
        public HashSet<String> Atoms{get;set;}

        private bool isRule(String val)
        {
            var part = val.Split(":-");
            if (part.Length != 2) return false;
            return true;
        }
        bool isFact(string val)
        {
            if (val[val.Length - 1] == '.')
                val = val.Remove(val.Length - 1);
            val = val.TrimStart().TrimEnd();
            return Regex.IsMatch(val, RegexPattern.FACT_PATTERN);
        }

        public KnowledgeBase()
        {
            Queries = new List<Query>();
            Facts = new HashSet<Tern>();
            Rules = new List<Tuple<List<Tern>, List<Tern>>> ();
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

        public void addFact(string val)
        {
            addTern(Facts, val);
        }

        public void addFact(Tern val)
        {
            Facts.Add(val);
        }

        public void addRule (string val)
        {
            if (val[val.Length - 1] == '.')
                val = val.Remove(val.Length - 1);
            var Parts = val.Split(":-");
            Parts[0] = Parts[0].TrimStart().TrimEnd();
            Parts[1] = Parts[1].TrimStart().TrimEnd();

            var LeftRaw = new List<Tern>();
            addTern(LeftRaw, Parts[0]);

            var RightRaw = new List<Tern>();
            addTern(RightRaw, Parts[1]);

            Rules.Add(new Tuple<List<Tern>, List<Tern>>(LeftRaw, RightRaw));
        }

        public void addQuerries(string val)
        {
            Queries.Add(new Query(val));
        }

        public void addQuerries(Query val)
        {
            Queries.Add(val);
        }

        public void readFromFile (string path)
        {
            var lines = Regex.Replace
                        (
                            Regex.Replace(
                                File.ReadAllText(path),
                                RegexPattern.COMMENT_PATTERN,
                                "").Replace("\r", "\n")
                            ,
                            RegexPattern.MULTI_NEWLINE,
                            "\n"
                        )
                        .TrimStart('\n')
                        .TrimEnd('\n')
                        .Split("\n");
            foreach(var line in lines)
            {
                if (Query.isQuery(line))
                    addQuerries(line);
                else if (isRule(line))
                    addRule(line);
                else if (isFact(line))
                    addFact(line);
            }
        }
    }
}