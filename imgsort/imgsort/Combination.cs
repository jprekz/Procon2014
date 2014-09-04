using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramingContestImageSort
{
    public class Combination
    {

        private int combinationCount = -1;
        private int[][] combinationValue;
        private void combinationGet(int[] all, int select, int[] num)
        {
            if (select == 0)
            {
                combinationCount++;
                combinationValue[combinationCount] = num;
                return;
            }

            foreach (var value in all)
            {
                if (num.Length > 0 && value <= num[num.Length - 1])
                {
                    continue;
                }
                int[] newNum = new int[num.Length + 1];
                Array.Copy(num, newNum, num.Length);
                newNum[num.Length] = value;
                if (newNum.Count() != newNum.Distinct().Count())
                {
                    continue;
                }

                combinationGet(all, select - 1, newNum);
            }
        }
        private void combinationSet(int[] all, int select)
        {
            combinationValue = new int[factorial(all.Length) / (factorial(all.Length - select) * factorial(select))][];
        }
        private int factorial(int num)
        {
            int result = 1;
            for (int i = 1; i <= num; i++)
            {
                result *= i;
            }
            return result;
        }
        public int[][] combination(int all, int select)
        {
            int[] allArray = new int[all];
            for (int i = 0; i < all; i++)
            {
                allArray[i] = i;
            }
            combinationSet(allArray, select);
            int[] num = new int[0];
            combinationGet(allArray, select, num);
            return combinationValue;
        }
    }
}
