using FinalProject.SimulationElements.Enums;

namespace FinalProject.Simulator
{
    public class ARobot
    {
        public string LastActivity = "None";
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

        public void MoveIt(Platform source, Platform destination)
        {
            // Place Holder
            LastActivity = source + "->" + destination;
            Box box = source.ServiceBox;
            source.ServiceBox = null;
            source.Status = StationStatus.Empty;
            destination.ServiceBox = box;
        }

        public void MoveIt(Entrance source, Platform destination)
        {
            // Move one Box from Entrance Station and put it on StationA
            LastActivity = "EQ->A"; // One Possibility!
            Box box = source.Dequeue();
            destination.ServiceBox = box;
        }

        public void MoveIt(Platform source, Inspector destination, int p)
        {
            LastActivity = "C->Ins";
            Box box = source.ServiceBox;
            source.ServiceBox = null;
            source.Status = StationStatus.Empty;
            if (p == 0)
            {
                destination.PushBoxToQueue(box);
            }
            else if (p == 1)
            {
                destination.Inspector1Box = box;
            }
            else if (p == 2)
            {
                destination.Inspector2Box = box;
            }
        }


        public void LoadBox(Platform platform)
        {
            // Place Holder
        }
    }
}