using System;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.FutureEventList;

namespace FinalProject.Simulator
{
    public class ResultRow
    {
        public Events CurrentEvent { get; set; }
        public SystemImage SystemImage { get; set; }
        public DateTime Time { get; set; }
    }
}