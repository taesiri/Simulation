using System;
using System.Collections;

namespace Problem3
{
    public static class ListExtension
    {
        public static void BubbleSort(this IList o)
        {
            for (int i = o.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    object o1 = o[j - 1];
                    object o2 = o[j];
                    if (((IComparable)o1).CompareTo(o2) > 0)
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);
                    }
                }
            }
        }

        public static string ConvertToString(this IList o)
        {
            string toStr = "";
            for (int i = 0; i < o.Count - 1; i++)
            {
                toStr += o[i] + ", ";
            }
            toStr += o[o.Count - 1].ToString();

            return toStr;
        }
        public static string ConvertToStringDetailed(this IList o)
        {
            string toStr = String.Format("Total length of List is {0}{1}", o.Count, Environment.NewLine);
            toStr += "{ ";
            for (int i = 0; i < o.Count - 1; i++)
            {
                toStr += o[i] + ", ";
            }
            toStr += o[o.Count - 1]+ " }.";

            return toStr;
        }
        public static string ConvertToStringDetailedSorted(this IList o)
        {
            o.BubbleSort();
            string toStr = String.Format("Total length of List is {0}{1}", o.Count, Environment.NewLine);
            toStr += "{ ";
            for (int i = 0; i < o.Count - 1; i++)
            {
                toStr += o[i] + ", ";
            }
            toStr += o[o.Count - 1] + " }.";

            return toStr;
        }
    }
}