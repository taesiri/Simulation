using System;
using System.Collections.Generic;

namespace Problem3.Helper
{
    public static class NumberPicker
    {
        public static List<int> RandomPick(int max, int number)
        {
            var rl = new List<int>();
            var rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < number; i++)
            {
                rl.Add(rnd.Next(1, max));
            }

            rl.BubbleSort();
            return rl;
        }
        public static List<int> NormalPicker(int max,int middle, int number)
        {
            var rl = new List<int>();
            if (middle <= (number / 2))
            {
                for (int i = 1; i <= number; i++)
                {
                    rl.Add(i);
                }
            }
            else
            {
                for (int i = (middle - (number/2)); i <= (middle + (number/2)); i++)
                {
                    rl.Add(i);
                }
            }
            return rl;
        }

    }
}