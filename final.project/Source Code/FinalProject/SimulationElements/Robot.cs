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

        public void MoveIt(ServiceBoxElement box, ServicePlatformElement platformElement)
        {
            // Place Holder
        }

        public void MoveIt(ServiceBoxElement box, ServiceEntranceStation entranceStation)
        {
            // Handle the Rendering!
        }

        public void MoveIt(ServiceBoxElement box, ServiceInspectorStation inspectorStation)
        {
            // 
        }

        internal void MoveIt(ServiceEntranceStation entranceStation)
        {
            // Move the box - Queue
        }
    }
}