using System.Collections.Generic;

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

        public void PushRow(ResultRow row)
        {
            Result.Add(row);
        }
    }
}