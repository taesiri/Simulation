namespace Problem1.TableRows
{
    public class ReportTableRowClass
    {
        public ReportTableRowClass(int c1, int c2, int c3, int c4, int c5)
        {
            Column1 = c1;
            Column2 = c2;
            Column3 = c3;
            Column4 = c4;
            Column5 = c5;
        }

        public ReportTableRowClass(int c1, int c2, int c3, int c4, int c5, int c6, int c7)
        {
            Column1 = c1;
            Column2 = c2;
            Column3 = c3;
            Column4 = c4;
            Column5 = c5;
            Column6 = c6;
            Column7 = c7;
        }

        public int Column1 { get; set; }
        public int Column2 { get; set; }
        public int Column3 { get; set; }
        public int Column4 { get; set; }
        public int Column5 { get; set; }
        public int Column6 { get; set; }
        public int Column7 { get; set; }
    }
}