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

        public static bool ForwardChaning (KnowledgeBase KB, Tern Query)
        {            
            if (KB.Facts.Contains(Query))
                return true;
            Dictionary <int, List<List<int>> > CombinationBySize = new Dictionary<int, List<List<int>>>();
            List<Tern> newFact = new List<Tern>();
            do
            {
                newFact.Clear();
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
                        //check left
                        bool allTrue = true;
                        foreach (var t in rule.Item1)
                        {
                            var replacedTern = ReplaceVar(t, VarName.ToList(), combine, KB.Atoms.ToList());
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
                                    allTrue = !(!newFact.Contains(replacedTern) && !KB.Facts.Contains(replacedTern));
                                    break;
                            }
                            if (!allTrue)
                                break;
                        }
                        if (allTrue)
                        {
                            foreach (var t in rule.Item2)
                            {
                                var replacedTern = ReplaceVar(t, VarName.ToList(), combine, KB.Atoms.ToList());
                                if (replacedTern.Arguments is null)
                                {
                                    continue;
                                }
                                switch (replacedTern.Value)
                                {
                                    case "\\=":
                                        continue;
                                }
                                if (!newFact.Contains(replacedTern) && !KB.Facts.Contains(replacedTern))
                                {
                                    newFact.Add(replacedTern);
                                }
                            }
                            continue;
                        }


                        //check right
                        allTrue = true;
                        foreach (var t in rule.Item2)
                        {
                            var replacedTern = ReplaceVar(t, VarName.ToList(), combine, KB.Atoms.ToList());
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
                                    allTrue = !(!newFact.Contains(replacedTern) && !KB.Facts.Contains(replacedTern));
                                    break;
                            }
                            if (!allTrue)
                                break;
                        }
                        if (allTrue)
                        {
                            foreach (var t in rule.Item1)
                            {
                                var replacedTern = ReplaceVar(t, VarName.ToList(), combine, KB.Atoms.ToList());
                                if (replacedTern.Arguments is null)
                                {
                                    continue;
                                }
                                switch (replacedTern.Value)
                                {
                                    case "\\=":
                                        continue;
                                }
                                if (!newFact.Contains(replacedTern) && !KB.Facts.Contains(replacedTern))
                                {
                                    newFact.Add(replacedTern);
                                }
                            }
                        }
                    }
                }

                foreach (var f in newFact)
                {
                    KB.Facts.Add(f);
                }
            }
            while(newFact.Count > 0);
            return false;
        }
    }
}