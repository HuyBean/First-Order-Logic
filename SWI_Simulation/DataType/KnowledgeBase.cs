using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace SWI_Simulation.DataType
{
    public class KnowledgeBase
    {
        
        public HashSet<Tern> Facts {get; private set;}
        public List<Tern> Queries { get; private set; }
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
        bool isQuery(string val)
        {
            if (!Regex.IsMatch(val, RegexPattern.QUERY_SIMPLE_PARTTERN))
                return false;
            string temp = val.Remove(0, 2);
            return isFact(temp);
        }

        public KnowledgeBase()
        {
            Queries = new List<Tern>();
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

        private static Tern? StringToTern(string? val)
        {
            if (val == null) return null;
            val = val.TrimStart().TrimEnd();
            Tern? newTern = null;
            if (Regex.IsMatch(val, RegexPattern.COMPOUND_TERN_PATTERN))
            {
                if (val[val.Length - 1] == '.')
                    val = val.Remove(val.Length - 1);
                var args = val.Replace(")", "").Split("(")[1].Split(", ").ToList();
                newTern = new Tern(TernType.CompoundTerm, val.Split("(")[0], args);
            }
            else if (Regex.Matches(val, RegexPattern.COMPARISION_OPERATION_PATTERN).Count == 1) 
            {
                string op = Regex.Match(val, RegexPattern.COMPARISION_OPERATION_PATTERN).Value;
                var args = Regex.Matches(val, RegexPattern.COMPARISION_ARGS_PATTERN).Cast<Match>().Select(match => match.Value).ToList();
                newTern = new Tern(TernType.Comparision, op, args);
            }
            return newTern;
        }

        public void addFact(string val)
        {
            Tern? item = StringToTern(val);
            if (item?.Type == TernType.Atom)
            {
                Atoms.Add(item.Value);
            }
            else if (item?.Arguments != null)
            {
                foreach (var t in item.Arguments)
                {
                    if (t.Type == TernType.Atom)
                        Atoms.Add(t.Value);
                }
            }
            if (item is not null)
                Facts.Add(item);
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
            var LeftRaw = Regex.Matches(Parts[0], RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value).ToList();
            var RightRaw = Regex.Matches(Parts[1], RegexPattern.FACT_PATTERN).Cast<Match>().Select(match => match.Value).ToList(); 

            Rules.Add(new Tuple<List<Tern>, List<Tern>>(new List<Tern>(), new List<Tern>()));
            foreach (var item in LeftRaw)
            {
                Tern? itemTern = StringToTern(item);
                if (itemTern != null)
                    Rules[Rules.Count - 1].Item1.Add(itemTern);
            }
            foreach (var item in RightRaw)
            {
                Tern? itemTern = StringToTern(item);
                if (itemTern != null)
                    Rules[Rules.Count - 1].Item2.Add(itemTern);            
            }
        }

        public void addQuerries(string val)
        {
            Tern? item = StringToTern(val.Remove(0, 2));
            if (item is not null)
                Queries.Add(item);
        }

        public void addQuerries(Tern val)
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
                if (isQuery(line))
                    addQuerries(line);
                else if (isRule(line))
                    addRule(line);
                else if (isFact(line))
                    addFact(line);
            }
        }
    }
}