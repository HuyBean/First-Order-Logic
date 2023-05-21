using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    public static class LogicProcess
    {
        public static void AnswerQuestions(KnowledgeBase KB, StreamWriter? file = null)
        {
            foreach (var q in KB.Queries)
            {
                file?.WriteLine(q);
                Console.WriteLine(q);
                var Result = ForwardChaning(KB, q, file).ToString() + ".";
                file?.WriteLine(Result);
                Console.WriteLine(Result);
                file?.WriteLine();
                Console.WriteLine();
            }
        }

        private static HashSet<string> getVarFromRule(Tuple<List<Tern>, List<Tern>> rule)
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

        private static Tern ReplaceVar(Tern old, List<string> name, List<int> index, List<string> val)
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

        private static bool CheckCondition(List<Tern> factQueries, HashSet<Tern>? newFacts, List<Tern> Facts, List<string> VarNames, List<string> AtomNames, List<int> combine)
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
                    case "==":
                        if (replacedTern.Arguments[0] != replacedTern.Arguments[1])
                        {
                            allTrue = false;
                        }
                        break;
                    case "\\=":
                        if (replacedTern.Arguments[0] == replacedTern.Arguments[1])
                        {
                            allTrue = false;
                        }
                        break;
                    default:
                        allTrue = (newFacts?.Contains(replacedTern) ?? false);
                        allTrue = allTrue || Facts.Contains(replacedTern);
                        break;
                }
                if (!allTrue)
                    break;
            }
            return allTrue;
        }

        private static void AddConclusion(List<Tern> conclusionContainter, HashSet<Tern>? newFacts, HashSet<Tern> Facts, List<string> VarNames, List<string> AtomNames, List<int> combine)
        {
            foreach (var t in conclusionContainter)
            {
                bool isTruth = true;
                var replacedTern = ReplaceVar(t, VarNames, combine, AtomNames);
                if (replacedTern.Arguments is null)
                {
                    continue;
                }
                switch (replacedTern.Value)
                {
                    case "==":
                    case "\\=":
                        continue;
                    default:
                        isTruth = (newFacts?.Contains(replacedTern) ?? false);
                        isTruth = isTruth || Facts.Contains(replacedTern);
                        break;
                }
                if (!isTruth)
                {
                    newFacts?.Add(replacedTern);
                }
            }
        }

        private static bool CheckQuery(ref List<bool>? wasAppeared, HashSet<string> atoms, HashSet<Tern> facts, Query q, StreamWriter? file)
        {
            if (q.Variables is null)
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

            var combine = CombinationSet.getBySize(atoms.Count, q.Variables.Count);
            if (wasAppeared is null) 
                wasAppeared =  Enumerable.Repeat(false, combine.Count).ToList();
            for (int i = 0; i < combine.Count; ++i)
            {
                if (wasAppeared[i])
                    continue;
                bool isExist = CheckCondition(q.Condition.ToList(), null, facts.ToList(), q.Variables.ToList(), atoms.ToList(), combine[i]);
                if (isExist)
                {
                    wasAppeared[i] = true;
                    var VarNames = q.Variables.ToList();
                    var AtomNames = atoms.ToList();
                    for (int j = 0; j < VarNames.Count; ++j)
                    {
                        var Result = $"{VarNames[j]} = {AtomNames[combine[i][j]]}\t" + ((j != VarNames.Count - 1) ? "," : ";");
                        Console.WriteLine(Result);
                        file?.WriteLine(Result);
                    }
                }
            }
            return false;
        }
        public static bool ForwardChaning(KnowledgeBase KB, Query q, StreamWriter? file)
        {            
            List<bool>? wasAppeardCombine = null;

            if (CheckQuery(ref wasAppeardCombine, KB.Atoms, KB.Facts, q, file))
                return true;
            if (KB.isExplored)
                return false;
            HashSet<Tern> newFacts = new HashSet<Tern>();
            do
            {
                newFacts.Clear();
                // get variable name
                foreach (var rule in KB.Rules)
                {
                    HashSet<string> VarName = getVarFromRule(rule);

                    var combianations = CombinationSet.getBySize(KB.Atoms.Count, VarName.Count);

                    foreach (var combine in combianations)
                    {
                        //check right
                        var test = rule.Item1[0].Value;

                        bool rightTrue = CheckCondition(rule.Item2, newFacts, KB.Facts.ToList(), VarName.ToList(), KB.Atoms.ToList(), combine);
                        if (rightTrue)
                        {
                            AddConclusion(rule.Item1, newFacts, KB.Facts, VarName.ToList(), KB.Atoms.ToList(), combine);
                        }
                    }
                }
                KB.Facts.UnionWith(newFacts);

                if (CheckQuery(ref wasAppeardCombine, KB.Atoms, KB.Facts, q, file))
                {
                    KB.isExplored = newFacts.Count == 0;
                    return true;
                }
            }
            while (newFacts.Count > 0);
            KB.isExplored = true;
            return false;
        }
    }
}