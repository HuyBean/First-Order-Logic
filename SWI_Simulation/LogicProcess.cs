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
                file?.WriteLine($"Current fact: {KB.FactsCount}");
                Console.WriteLine($"Current fact: {KB.FactsCount}");
                file?.WriteLine();
                Console.WriteLine();
            }
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

        private static bool CheckCondition(List<Tern> factQueries, KnowledgeBase KB, List<string> VarNames, List<string> AtomNames, List<int> combine)
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
                        allTrue = KB.ContainsFact(replacedTern);
                        break;
                }
                if (!allTrue)
                    break;
            }
            return allTrue;
        }

        // return the real add to containter.
        private static int AddConclusion(List<Tern> conclusionContainter, KnowledgeBase KB, List<string> VarNames, List<string> AtomNames, List<int> combine)
        {
            int counter = 0;
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
                        isTruth = KB.ContainsFact(replacedTern);
                        break;
                }
                if (!isTruth)
                {
                    KB.addFact(replacedTern);
                    counter++;
                }
            }
            return counter;
        }

        private static bool CheckQuery(ref List<bool>? wasAppeared, List<string> atoms, KnowledgeBase KB, Query q, StreamWriter? file)
        {
            if (q.VariablesList is null || q.Variables is null)
            {
                bool QueryExisted = true;
                foreach (var t in q.Condition)
                {
                    if (!KB.ContainsFact(t))
                    {
                        QueryExisted = false;
                        break;
                    }
                }
                return QueryExisted;
            }
            if (q.Atoms is not null)
            {
                foreach (var val in q.Atoms)
                {
                    if (!atoms.Contains(val))
                        return false;
                }
            }

            var combine = CombinationSet.getBySize(atoms.Count, q.Variables.Count);
            if (wasAppeared is null)
                wasAppeared = Enumerable.Repeat(false, combine.Count).ToList();
            for (int i = 0; i < combine.Count; ++i)
            {
                if (wasAppeared[i])
                    continue;
                bool isExist = CheckCondition(q.ConditionList, KB, q.VariablesList, atoms, combine[i]);
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

            if (CheckQuery(ref wasAppeardCombine, KB.AtomsList, KB, q, file))
                return true;
            if (KB.isExplored)
                return false;
            int newFacts = 0;
            do
            {
                newFacts = 0;
                // get variable name
                foreach (var r in KB.Rules)
                {
                    var combine = CombinationSet.getBySize(KB.Atoms.Count, r.Variables.Count);

                    if (r.isFacts is null)
                    {
                        r.isFacts = Enumerable.Repeat(false, combine.Count).ToList();
                    }

                    // Console.WriteLine($"combine size {combine.Count}");
                    // Console.WriteLine($"Rule left:");
                    // foreach (var val in r.Left)
                    // {
                    //     Console.WriteLine($"\t {val}");
                    // }
                    // Console.WriteLine("Rule right:");

                    // foreach (var val in r.Right)
                    // {
                    //     Console.WriteLine($"\t {val}");
                    // }

                    for (int i = 0; i < combine.Count; ++i)
                    {
                        if (r.isFacts is not null && r.isFacts[i])
                            continue;
                        bool rightTrue = CheckCondition(r.Right, KB, r.VariablesList, KB.AtomsList, combine[i]);
                        if (rightTrue)
                        {
                            if (r.isFacts is not null)
                                r.isFacts[i] = true;
                            newFacts += AddConclusion(r.Left, KB, r.VariablesList, KB.AtomsList, combine[i]);
                        }
                    }
                };

                if (CheckQuery(ref wasAppeardCombine, KB.AtomsList, KB, q, file))
                {
                    KB.isExplored = (newFacts == 0);
                    return true;
                }
            }
            while (newFacts > 0);
            KB.isExplored = true;
            return false;
        }
    }
}