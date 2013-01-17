using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalProject.Simulator
{
    public class SimulationResult
    {
        public List<Box> BoxResult;
        public List<ResultRow> Result;

        public SimulationResult()
        {
            BoxResult = new List<Box>();
            Result = new List<ResultRow>();
        }

        public TimeSpan SimulationDuration
        {
            get { return SimulationEndDate - SimulationStartDate; }
        }

        public DateTime SimulationStartDate { get; set; }
        public DateTime SimulationEndDate { get; set; }

        public TimeSpan StationATotalService { get; set; }
        public TimeSpan StationBTotalService { get; set; }
        public TimeSpan StationCTotalService { get; set; }
        public TimeSpan Inspector1TotalService { get; set; }
        public TimeSpan Inspector2TotalService { get; set; }


        public TimeSpan GetAverageBoxInSystemTime
        {
            get
            {
                var average = new TimeSpan(0);

                average = BoxResult.Aggregate(average, (current, box) => current + box.GetInSystemTime);

                return TimeSpan.FromMinutes(60*average.TotalHours/BoxResult.Count);
            }
        }

        public DateTime LastBoxDeparture
        {
            get { return BoxResult[BoxResult.Count - 1].DepartureTime; }
        }

        public TimeSpan TotalService
        {
            get
            {
                return StationATotalService +
                       StationBTotalService +
                       StationCTotalService +
                       Inspector1TotalService +
                       Inspector2TotalService;
            }
        }

        public void PushRow(ResultRow row)
        {
            Result.Add(row);
        }
    }
}