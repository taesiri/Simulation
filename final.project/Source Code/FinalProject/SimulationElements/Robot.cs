using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements
{
    public class Robot
    {
        public string LastActivity = "None";


        public Robot()
        {
            GlobalTimeScale = 1;
            Status = RobotStatus.Idle;
        }

        public double GlobalTimeScale { get; set; }
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


        public void MoveIt(ServicePlatformElement source, ServicePlatformElement destination, TimeSpan duration)
        {
            duration = TimeSpan.FromSeconds(duration.TotalSeconds*GlobalTimeScale);

            // Place Holder
            LastActivity = source + "->" + destination;

            ServiceBoxElement box = source.ServiceBox;
            source.ServiceBox = null;
            source.Status = StationStatus.Empty;

            destination.ServiceBox = box;

            double src = box.Transformer.OffsetX;
            double desten = 0;

            switch (destination.Name)
            {
                case "A":
                    //box.Transform = new TranslateTransform3D(-7.5, 0, 2.1);
                    desten = -7.5;
                    break;
                case "B":
                    //box.Transform = new TranslateTransform3D(0, 0, 2.1);
                    desten = 0;
                    break;
                case "C":
                    //box.Transform = new TranslateTransform3D(7.5, 0, 2.1);
                    desten = 7.5;
                    break;
            }

            var da = new DoubleAnimation {From = src, To = desten, Duration = new Duration(duration)};


            var newTransformer = new TranslateTransform3D(box.Transformer.OffsetX, box.Transformer.OffsetY, 2.1);
            box.Transform = newTransformer;
            box.Transformer = newTransformer;
            newTransformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }

        public void MoveIt(ServiceEntranceStation source, ServicePlatformElement destination, TimeSpan duration)
        {
            duration = TimeSpan.FromSeconds(duration.TotalSeconds*GlobalTimeScale);


            // Move one Box from Entrance Station and put it on StationA
            LastActivity = "EQ->A"; // One Possibility!
            ServiceBoxElement box = source.Dequeue();
            destination.ServiceBox = box;

            double src = box.Transformer.OffsetX;
            const double desten = -7.5;

            var da = new DoubleAnimation {From = src, To = desten, Duration = new Duration(duration)};
            var newTransformer = new TranslateTransform3D(box.Transformer.OffsetX, box.Transformer.OffsetY, 2.1);
            box.Transform = newTransformer;
            box.Transformer = newTransformer;
            newTransformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }

        public void MoveIt(ServicePlatformElement source, ServiceInspectorStation destination, TimeSpan duration, int p)
        {
            duration = TimeSpan.FromSeconds(duration.TotalSeconds*GlobalTimeScale);

            LastActivity = "C->Ins";
            ServiceBoxElement box = source.ServiceBox;
            source.ServiceBox = null;
            source.Status = StationStatus.Empty;
            int qLen = destination.GetQueueLen;
            double srcX = box.Transformer.OffsetX;
            const double destenX = 16;
            double srcY = box.Transformer.OffsetY;

            double destenY = 0;

            if (p == 0)
            {
                destenY = 10 + (5*qLen);
                destination.PushBoxToQueue(box);
            }
            else if (p == 1)
            {
                destenY = 3;
                destination.Inspector1Box = box;
            }
            else if (p == 2)
            {
                destenY = -3;
                destination.Inspector2Box = box;
            }

            var daX = new DoubleAnimation {From = srcX, To = destenX, Duration = new Duration(duration)};
            var daY = new DoubleAnimation {From = srcY, To = destenY, Duration = new Duration(duration)};
            var newTransformer = new TranslateTransform3D(box.Transformer.OffsetX, box.Transformer.OffsetY, 2.1);
            box.Transform = newTransformer;
            box.Transformer = newTransformer;
            newTransformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, daX);
            newTransformer.BeginAnimation(TranslateTransform3D.OffsetYProperty, daY);
         
        }


        public void LoadBox(ServicePlatformElement platform)
        {
            // Place Holder
        }
    }
}