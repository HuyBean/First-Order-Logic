using System.Collections.Generic;

namespace SWI_Simulation.DataType
{
    public static class CombinationSet
    {
        public static Dictionary<int, List<List<int>>>  CombinationBySize = new Dictionary<int, List<List<int>>>();

        private static bool InitStatus = false;

        public static int atomSize {get; private set;}
        private static void Init(int atomCounter)
        {
            InitStatus = true;
            atomSize = atomCounter;
        }

        private static void generateCombinationRecursive(ref List<List<int>> result, int AtomsCount, int num, ref List<int> current)
        {
            if (current.Count == num)
            {
                result.Add(new List<int>(current));
                return;
            }
            for (int i = 0; i < AtomsCount; ++i)
            {
                current.Add(i);
                generateCombinationRecursive(ref result, AtomsCount, num, ref current);
                current.RemoveAt(current.Count - 1);
            }
        }
        private static List<List<int>> getCombination(int AtomsCount, int num)
        {
            var result = new List<List<int>>();
            var current = new List<int>();
            generateCombinationRecursive(ref result, AtomsCount, num, ref current);
            return result;
        }
        public static List<List<int>> getBySize (int n, int k)
        {
            if (!InitStatus)
            {
                Init(n);
            }
            if (n < atomSize)
            {
                return new List<List<int>>();
            }

            if (!CombinationBySize.ContainsKey(k))
            {
                CombinationBySize[k] = getCombination(n, k);
            }

            return CombinationBySize[k];
        }
    }
}