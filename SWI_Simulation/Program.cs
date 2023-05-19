
using System;
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KnowledgeBase KB = new KnowledgeBase();
            KB.readFromFile(@"D:\Github\First-Order-Logic\SWI_Simulation\royal_family.pl");
            bool res = LogicProcess.ForwardChaning(KB, "grandfather(prince_charles,prince_george).");
            Console.WriteLine($"Answer is {res}");
        }
    }
}