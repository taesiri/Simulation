using System;
using System.Collections.Generic;
using System.Linq;
using Problem3.Helper;

namespace Problem3.Tests
{
    public static class UniformityTests
    {
        public static List<Tuple<int, float, int, float>> ChiSquare(int n, ReportList list)
        {
            var rL = new List<Tuple<int, float, int, float>>();

            for (int i = 0; i < n; i++)
            {
                var range = new Range(i*(1/(float) n), ((i + 1)*(1/(float) n)));
                int oi = list.GetNumberOfObservationInRange(range);
                int ei = list.InnerData.Count/n;
                float oiei = ((oi - ei)*(oi - ei)/(float) ei);
                rL.Add(new Tuple<int, float, int, float>(i + 1, oi, ei, oiei));
            }

            return rL;
        }

        public static bool IsUniformWithChiSquare(int n, ReportList list)
        {
            List<Tuple<int, float, int, float>> data = ChiSquare(n, list);
            float sum = data.Sum(tuple => tuple.Item4);

            return !(sum > 16.919f);
        }

        public static bool IsUniformWithChiSquare(List<Tuple<int, float, int, float>> list)
        {
            float sum = list.Sum(tuple => tuple.Item4);

            return !(sum > 16.919f);
        }


        public static List<Tuple<int, float, float, float, float, float>> KSTest(ReportList list)
        {
            var rL = new List<Tuple<int, float, float, float, float, float>>();
            var cloneList = new List<float>(list.InnerData);
            cloneList.BubbleSort();
            int n = cloneList.Count;

            for (int i = 0; i < n; i++)
            {
                float ri = cloneList[i];
                float iDivn = (i + 1)/(float) n;
                float i1Divn = i/(float) n;
                rL.Add(new Tuple<int, float, float, float, float, float>(i + 1, ri, iDivn, i1Divn, Math.Abs(iDivn - ri),
                                                                         Math.Abs(ri - i1Divn)));
            }

            return rL;
        }

        public static bool IsUniformWithKS(List<Tuple<int, float, float, float, float, float>> list)
        {
            float maxDPlus = 0f;
            float maxDNegetive = 0f;

            foreach (var tuple in list)
            {
                if (tuple.Item5 > maxDPlus)
                    maxDPlus = tuple.Item5;
                if (tuple.Item6 > maxDNegetive)
                    maxDNegetive = tuple.Item6;
            }

            float result = Math.Max(maxDNegetive, maxDPlus);

            return result < ((1.36f)/Math.Sqrt(list.Count));
        }

        public static bool IsUniformWithKS(ReportList list)
        {
            return IsUniformWithKS(KSTest(list));
        }
    }
}