
using SWI_Simulation.DataType;

namespace SWI_Simulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KnowledgeBase KB = new KnowledgeBase();
            KB.readFromFile(@"D:\Github\First-Order-Logic\SWI_Simulation\royal_family.pl");
            LogicProcess.ForwardChaning(KB, "sibling('Princess Anne', madam).");
        }
    }
}