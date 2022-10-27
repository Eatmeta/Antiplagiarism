using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];

            for (var i = 0; i <= first.Count; ++i)
                opt[i, 0] = 0;
            for (var i = 0; i <= second.Count; ++i)
                opt[0, i] = 0;

            for (var i = 0; i < first.Count; i++)
            for (var j = 0; j < second.Count; j++)
                opt[i + 1, j + 1] = first[i] == second[j] ? opt[i, j] + 1 : Math.Max(opt[i, j + 1], opt[i + 1, j]);

            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var result = new List<string>();
            var j = first.Count;
            var i = second.Count;
            var current = opt[j, i];

            while (current != 0)
            {
                // если значение не меняется и при движении вверх и влево, то идем по диагонали
                if (opt[j, i - 1] == opt[j - 1, i])
                {
                    if (opt[j, i - 1] == 0 && opt[j - 1, i] == 0) 
                    {
                        result.Add(second[i-1]);
                        break;
                    }

                    if (opt[j, i - 1] != current)
                    {
                        result.Add(second[i-1]);
                        i--;
                        j--;
                        current--;
                        continue;
                    }
                    i--;
                    j--;
                    continue;
                }

                // если значение не поменялось или только при движении вверx - идем вверx
                if (opt[j - 1, i] > opt[j, i - 1])
                {
                    j--;
                    continue;
                }
                // если значение не поменялось или только при движении влево - идем влево
                i--;
            }
            result.Reverse();
            return result;
        }
    }
}