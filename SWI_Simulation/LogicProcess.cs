using System;
using System.Collections.Generic;
using System.Linq;
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    public static class LogicProcess
    {
        public static void Process(KnowledgeBase KB)
        {
            
        }

        private static void getCombinationRecursive (ref List<List<int>> result, int AtomsCount, int num, ref List<int> current)
        {
            if (current.Count == num)
            {
                result.Add(new List<int>(current));
                return;
            }
            for (int i = 0; i < AtomsCount; ++i)
            {
                current.Add(i);
                getCombinationRecursive (ref result, AtomsCount, num,ref current);
                current.RemoveAt(current.Count - 1);
            }
        }

        private static List<List <int>> getCombination (int AtomsCount, int num)
        {
            var result = new List<List<int>>();
            var current = new List<int>();
            getCombinationRecursive(ref result, AtomsCount, num, ref current);
            return result;
        }

        private static HashSet<string> getVarFromRule (Tuple <List<Tern> , List<Tern>> rule)
        {
            HashSet<string> VarName = new HashSet<string>();
            foreach (var t in rule.Item1)
            {
                foreach (var name in t.GetVarArgList())
                {
                    VarName.Add(name);
                }
            }
            foreach (var t in rule.Item2)
            {
                foreach (var name in t.GetVarArgList())
                {
                    VarName.Add(name);
                }
            }
            return VarName;
        }

        private static Tern ReplaceVar (Tern old, List<string> name, List<int> index, List<string> val)
        {
            Tern Result = old.Clone();
            if (Result.Arguments is null)
                return Result;
            for (int i = 0; i < Result.Arguments.Count; ++i)
            {
                if (Result.Arguments[i].Type == TernType.Variable)
                {
                    Result.Arguments[i] = val[index[name.IndexOf(Result.Arguments[i].Value)]];
                }
            }
            return Result;
        }

        private static bool CheckCondition (List<Tern> factQueries, List<Tern> newFacts, List<Tern> Facts, List<string> VarNames, List<string> AtomNames, List<int> combine )
        {
            bool allTrue = true;
            foreach (var t in factQueries)
            {
                var replacedTern = ReplaceVar(t, VarNames, combine, AtomNames);
                if (replacedTern.Arguments is null)
                {
                    continue;
                }

                switch (replacedTern.Value)
                {
                    case "\\=":
                        if (replacedTern.Arguments[0] == replacedTern.Arguments[1])
                        {
                            allTrue = false;
                        }
                        break;
                    default:
                        allTrue = !(!newFacts.Contains(replacedTern) && !Facts.Contains(replacedTern));
                        break;
                }
                if (!allTrue)
                    break;
            }
            return allTrue;
        }

        private static void AddConclusion(List<Tern> conclusionContainter, List<Tern> newFacts, List<string> VarNames, List<string> AtomNames, List<int> combine)
        {
            foreach (var t in conclusionContainter)
            {
                var replacedTern = ReplaceVar(t, VarNames, combine, AtomNames);
                if (replacedTern.Arguments is null)
                {
                    continue;
                }
                switch (replacedTern.Value)
                {
                    case "\\=":
                        continue;
                }
                if (!newFacts.Contains(replacedTern))
                {
                    newFacts.Add(replacedTern);
                }
            }
        }

        private static bool CheckQuery (HashSet<Tern> facts, Query q)
        {
            bool QueryExisted = true;
            foreach (var t in q.Condition)
            {
                if (!facts.Contains(t))
                {
                    QueryExisted = false;
                    break;
                }
            }
            return QueryExisted;
        }
        public static bool ForwardChaning (KnowledgeBase KB, Query q)
        {            

            if (CheckQuery(KB.Facts, q))
                return true;

            Dictionary <int, List<List<int>> > CombinationBySize = new Dictionary<int, List<List<int>>>();
            List<Tern> newFacts = new List<Tern>();
            do
            {
                newFacts.Clear();
                // get variable name
                foreach (var rule in KB.Rules)
                {
                    HashSet<string> VarName = getVarFromRule(rule);

                    if (!CombinationBySize.ContainsKey(VarName.Count))
                    {
                        CombinationBySize[VarName.Count] = getCombination(KB.Atoms.Count, VarName.Count);
                    }

                    var combianations = CombinationBySize[VarName.Count];

                    foreach (var combine in combianations)
                    {
                        //check right
                        bool rightTrue = CheckCondition(rule.Item2, newFacts, KB.Facts.ToList(), VarName.ToList(), KB.Atoms.ToList(), combine);
                        if (rightTrue)
                        {
                            AddConclusion(rule.Item1, newFacts, VarName.ToList(), KB.Atoms.ToList(), combine);
                        }
                    }
                }
                Console.WriteLine($"Endloop! newFacts {newFacts.Count}");   
                KB.Facts.UnionWith(newFacts);
                
                if (CheckQuery(KB.Facts, q))
                {
                    return true;
                }
            }
            while(newFacts.Count > 0);
            return CheckQuery(KB.Facts, q);
        }
    }
}