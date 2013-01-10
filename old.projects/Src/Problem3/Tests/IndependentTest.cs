using System;
using System.Collections.Generic;
using Problem3.Helper;

namespace Problem3.Tests
{
    public static class IndependentTest
    {
        //public static List<> 

        public static List<Tuple<int, int, int, float>> PokerTest(ReportList list)
        {
            var rL = new List<Tuple<int, int, int, float>>();
            int n = list.GetLength;
            int len = list.GetNumbersMaxLen;
            List<float> plist = PmList(len);

            int counter = 1;
            foreach (float item in plist)
            {
                double nei = Math.Round(n*item);
                int observation = list.GetDuplicationOccurrences(len - (counter - 1));
                rL.Add(new Tuple<int, int, int, float>(counter, observation, (int) nei,
                                                       (float) (Math.Pow(observation - nei, 2)/(float) nei)));
                counter++;
            }

            return rL;
        }

        public static List<float> PmList(int number)
        {
            var rL = new List<float>();
            for (int i = 1; i <= number; i++)
            {
                rL.Add(Pm(number, i, 10));
            }

            return rL;
        }

        public static float Pm(int k, int m, int d)
        {
            float answer = 1f;
            for (int i = 0; i <= (m - 1); i++)
            {
                answer *= (d - i);
            }
            answer *= Striling(k, m);
            answer /= (float) Math.Pow(d, k);

            return answer;
        }

        public static int Striling(int n, int k)
        {
            int sum = 0;
            for (int i = 0; i <= k; i++)
            {
                sum += ((int) Math.Pow(-1.0f, i))*(Combination(k, i))*((int) Math.Pow(k - i, n));
            }
            return (int) ((1.0f/Fact(k))*sum);
        }

        public static int Combination(int a, int b)
        {
            return (Fact(a)/(Fact(b)*Fact(a - b)));
        }

        public static int Fact(int i)
        {
            if (i == 0)
                return 1;
            return i*Fact(i - 1);
        }
    }
}