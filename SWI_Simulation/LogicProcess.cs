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
                    if (!CombinationBySize.ContainsKey(VarName.Count))
                    {
                        CombinationBySize[VarName.Count] = getCombination(KB.Atoms.Count, VarName.Count);
                    }
                    
                    var combianation = CombinationBySize[VarName.Count];
                }
            }
            while(newFact.Count > 0);
            return false;
        }
    }
}