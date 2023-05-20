
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
            KB.addQuerries("?-aunt('Autumn Kelly', 'Mia Grace Tindall').");
            bool res = LogicProcess.ForwardChaning(KB, KB.Queries[KB.Queries.Count - 1]);
            Console.WriteLine($"Answer is {res}");
        }
    }
}