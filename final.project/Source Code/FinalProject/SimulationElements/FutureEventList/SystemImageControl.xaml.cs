using System.Windows.Media;
using System.Windows.Shapes;
using FinalProject.SimulationElements.Enums;

namespace FinalProject.SimulationElements.FutureEventList
{
    /// <summary>
    ///     Interaction logic for SystemImageControl.xaml
    /// </summary>
    public partial class SystemImageControl
    {
        public SystemImageControl()
        {
            InitializeComponent();
        }

        public void SetState(SystemImage image)
        {
            Colorlize(RecStationA, image.StationAStatus);
            Colorlize(RecStationB, image.StationBStatus);
            Colorlize(RecStationC, image.StationCStatus);
            Colorlize(RecEntrance, image.EntranceQueueLen);
            Colorlize(RecInspector1, image.Inspector1Status);
            Colorlize(RecInspector2, image.Inspector2Status);
        }

        public void Colorlize(Rectangle rec, StationStatus status)
        {
            switch (status)
            {
                case StationStatus.Empty:
                    rec.Fill = Brushes.Silver;
                    break;
                case StationStatus.Loading:
                    rec.Fill = Brushes.Gold;
                    break;

                case StationStatus.OnService:
                    rec.Fill = Brushes.Orange;
                    break;

                case StationStatus.Blocked:
                    rec.Fill = Brushes.Red;
                    break;
                case StationStatus.BlockedAndWaitingforRobot:
                    rec.Fill = Brushes.DarkRed;
                    break;
                case StationStatus.WaitingforRobot:
                    rec.Fill = Brushes.OrangeRed;
                    break;
            }
        }

        public void Colorlize(Rectangle rec, WorkerStatus status)
        {
            switch (status)
            {
                case WorkerStatus.Idle:
                    rec.Fill = Brushes.Silver;
                    break;
                case WorkerStatus.Busy:
                    rec.Fill = Brushes.YellowGreen;
                    break;
            }
        }

        public void Colorlize(Rectangle rec, int qLen)
        {
            if (qLen == 0)
                rec.Fill = Brushes.Silver;
            else
            {
                rec.Fill = Brushes.SkyBlue;
            }
        }
    }
}