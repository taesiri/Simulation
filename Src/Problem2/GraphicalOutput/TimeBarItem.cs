namespace Problem2.GraphicalOutput
{
    public class TimeBarItem
    {
        public TimeBarItem()
        {

        }

        public TimeBarItem(string fullname, int len, int left, int right, int numb)
        {
            FullCustomerName = fullname;
            Length = len;
            Left = left;
            Right = right;
            Number = numb;
        }

        public string FullCustomerName { get; set; }
        public int Number { get; set; }
        public int Length { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

    }
}