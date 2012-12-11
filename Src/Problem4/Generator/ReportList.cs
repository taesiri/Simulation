using System;
using System.Collections.Generic;
using System.Globalization;

namespace Problem4.Generator
{
    public sealed class ReportList
    {
        public ReportList(List<float> data, int modoluse, int seed, int multiplier, int increment)
        {
            InnerData = data;
            Modoluse = modoluse;
            Seed = seed;
            Multiplier = multiplier;
            Increment = increment;
        }

        public ReportList(List<float> data)
        {
            InnerData = data;
        }

        public ReportList(ReportList data)
        {
            InnerData = new List<float>(data.InnerData);
            Modoluse = data.Modoluse;
            Seed = data.Seed;
            Multiplier = data.Multiplier;
            Increment = data.Increment;
        }

        public ReportList()
        {
            InnerData = new List<float>();
        }

        public List<float> InnerData { get; set; }


        public int GetLength
        {
            get { return InnerData.Count; }
        }

        public string GetInformation
        {
            get
            {
                return
                    String.Format(
                        "Modoluse \t:   {0}\nStart  Value \t:   {1}\nMultiplier \t:   {2}\nIncerment \t:   {3}",
                        Modoluse.ToString(CultureInfo.InvariantCulture),
                        Seed.ToString(CultureInfo.InvariantCulture),
                        Multiplier.ToString(CultureInfo.InvariantCulture),
                        Increment.ToString(CultureInfo.InvariantCulture));
            }
        }

        public int Modoluse { get; set; }
        public int Seed { get; set; }
        public int Multiplier { get; set; }
        public int Increment { get; set; }
    }
}