using System.Linq;

namespace Problem3.Helper
{
    public static class StringHelper
    {
        public static int MaximumNumberOfDuplication(string str)
        {
            return str.Select(chr => CountStringOccurrences(str, chr)).Concat(new[] { 0 }).Max();
        }

        public static int CountStringOccurrences(string text, char pattern)
        {
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += 1;
                count++;
            }
            return count;
        }
    }
}