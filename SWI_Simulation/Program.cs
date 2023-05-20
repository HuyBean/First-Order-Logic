
using System;
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            KnowledgeBase KB = new KnowledgeBase();
            KB.readFromFile(@"D:\Github\First-Order-Logic\SWI_Simulation\royal_family.pl");
            LogicProcess.AnswerQuestions(KB);
        }
    }
}