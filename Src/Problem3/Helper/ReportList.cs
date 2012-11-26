using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Problem3.Tests;

namespace Problem3.Helper
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

        public string GetReportSummery
        {
            get
            {
                string r = GetInformation;
                var data = new List<float>(InnerData);
                r += "\n\n" + data.ConvertToStringDetailedSorted() + "\n\n";

                r += "\nUniformity Test Result :";
                r += "\n\nChi-Square test : " + ChiSuquareResult().ToString();
                r += "\nKolmogorov–Smirnov test : " + KSResult().ToString();

                r += "\n\nIndependence Test Result :";
                r += "\n\nK2 : " + false.ToString();
                r += "\nKom : " + false.ToString();
                return r;
            }
        }


        public int Modoluse { get; set; }
        public int Seed { get; set; }
        public int Multiplier { get; set; }
        public int Increment { get; set; }

        public int GetNumberOfObservationInRange(Range range)
        {
            return InnerData.Count(item => item < range.Upper && item >= range.Lower);
        }

        public bool ChiSuquareResult()
        {
            return UniformityTests.IsUniformWithChiSquare(10, this);
        }

        public bool KSResult()
        {
            return UniformityTests.IsUniformWithKS(this);
        }

        public int GetNumbersMaxLen
        {
            get
            {
                return
                    InnerData.Select(f => (f.ToString(CultureInfo.InvariantCulture).Length - 2)).Concat(new[] {0}).Max();
            }
        }

        public int GetDuplicationOccurrences(int numberOfOccurrences)
        {
            int number = 0;
            foreach (var item in InnerData)
            {
                try
                {
                    var maxND = StringHelper.MaximumNumberOfDuplication(item.ToString().Replace("0.", string.Empty));
                    if (maxND == numberOfOccurrences)
                    {
                        number++;
                    }
                }
                catch (Exception exp)
                {
                    
                    throw new Exception(exp.Message);
                }
               
            }
            return number;
        }
    }
}