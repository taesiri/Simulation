using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements
{
    public class Robot
    {
        public RobotStatus Status { get; set; }

        public bool IsIdle
        {
            get
            {
                if (Status == RobotStatus.Idle) return true;
                else
                    return false;
            }
        }

        public void MoveIt()
        {
            // Place Holder
        }
    }
}