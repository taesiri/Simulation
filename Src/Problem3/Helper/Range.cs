namespace Problem3.Helper
{
    public sealed class Range
    {
        public Range(float l, float u)
        {
            Upper = u;
            Lower = l;
        }

        public Range()
        {
            Upper = 100;
            Lower = 1;
        }

        public float Upper { get; set; }
        public float Lower { get; set; }
    }
}