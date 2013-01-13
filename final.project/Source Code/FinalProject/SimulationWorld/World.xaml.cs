using System.Windows;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.FutureEventList;

namespace FinalProject.SimulationWorld
{
    /// <summary>
    ///     Interaction logic for World.xaml
    /// </summary>
    public partial class World
    {
        public static World Instance;
        private readonly FutureEventList _fel = new FutureEventList();


        private ServiceEntranceStation _entranceStation;
        private ServiceInspectorStation _inspectorStation;

        private ServicePlatformElement _platformA;
        private ServicePlatformElement _platformB;
        private ServicePlatformElement _platformC;


        //private ServiceEntranceStation _platformElem;

        private Robot _robot;

        public World()
        {
            InitializeComponent();
            Instance = this;
            CreateScene();
        }

        private void BtnDeployOnClick(object sender, RoutedEventArgs e)
        {
            var platformElement = new ServiceEntranceStation();

            Mother.Children.Add(platformElement);

            //_platformElem = platformElement;
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
            //var da = new DoubleAnimation {From = -10, To = 10f, Duration = new Duration(TimeSpan.FromSeconds(2))};
            //_platformElem.Tranformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }


        public void CreateScene()
        {
            _entranceStation = new ServiceEntranceStation(new Point3D(-22, 0, 0.5f));

            _platformA = new ServicePlatformElement(new Point3D(-7.5, 0, -4.5));
            _platformB = new ServicePlatformElement(new Point3D(0, 0, -4.5));
            _platformC = new ServicePlatformElement(new Point3D(7.5, 0, -4.5));

            _inspectorStation = new ServiceInspectorStation(new Point3D(18, 0, 0.5f));

            _robot = new Robot();

            var sampleBox = new ServiceBoxElement(new Point3D(0, 0, 2.1), 2.5, 2.5, 2.5, @"Textures\postBox.jpg");

            Mother.Children.Add(_platformA);
            Mother.Children.Add(_platformB);
            Mother.Children.Add(_platformC);
            Mother.Children.Add(_entranceStation);
            Mother.Children.Add(_inspectorStation);

            Mother.Children.Add(sampleBox);
        }


        public void Simulator()
        {
            int counter = 0;
            bool running = true;
            while (running)
            {
                counter++;
                FutureEvent imminentEvent = _fel.GetImminentEvent();
                switch (imminentEvent.Event)
                {
                    case Events.Arrival:
                        OnArrival();
                        break;
                    case Events.Departure:
                        OnDeparture();
                        break;
                    case Events.ServiceEndedOnStationA:
                        OnServiceEndedAtStationA();
                        break;
                    case Events.ServiceEndedOnStationB:
                        OnServiceEndedAtStationB();
                        break;
                    case Events.ServiceEndedOnStationC:
                        OnServiceEndedAtStationC();
                        break;
                    case Events.RobotJobFinished:
                        OnRobotJobFinished();
                        break;
                    case Events.ServiceStartedOnStationA:
                        // Place Holder!
                        break;
                    case Events.ServiceStartedOnStationB:
                        // Place Holder!
                        break;
                    case Events.ServiceStartedOnStationC:
                        // Place Holder!
                        break;
                    case Events.MovingToStationA:
                        // Place Holder!
                        break;
                    case Events.MovingToStationB:
                        // Place Holder!
                        break;
                    case Events.MovingToStationC:
                        // Place Holder!
                        break;
                    case Events.EndOfSimulation:
                        running = false;
                        break;
                    case Events.BeginingOfSimulation:
                        // Place Holder!
                        break;

                    default:
                        break;
                }
                if (counter > 10)
                {
                    running = false;
                }
            }
            //END OF Simulation
        }

        public void OnArrival()
        {
            var newBox = new ServiceBoxElement();

            if (_robot.IsIdle)
            {
                _robot.MoveIt(newBox, _entranceStation);
                _robot.Status = RobotStatus.Busy;
            }
            else
            {
                _entranceStation.EntranceQueue.Enqueue(newBox);
                // Q++; //
            }

            // Scheduling the next Entrance!
            _fel.GenerateNextEntrance();
        }

        public void OnServiceEndedAtStationA()
        {
            if (_platformB.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformA.ServiceBox, _platformB);
            }
            else
            {
                _platformA.Status = StationStatus.BlockedOrAwaiting;
            }
        }

        public void OnServiceEndedAtStationB()
        {
            if (_platformC.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformB.ServiceBox, _platformC);
            }
            else
            {
                _platformB.Status = StationStatus.BlockedOrAwaiting;
            }
        }

        public void OnServiceEndedAtStationC()
        {
            if (_robot.IsIdle)
            {
                _robot.MoveIt(_platformB.ServiceBox, _inspectorStation);
            }
            else
            {
                _platformC.Status = StationStatus.BlockedOrAwaiting;
            }
        }

        public void OnRobotJobFinished()
        {
            if (_platformC.IsAwaiting)
            {
                _robot.MoveIt(_platformB.ServiceBox, _inspectorStation);
            }
            else if (_platformB.IsAwaiting)
            {
                _robot.MoveIt(_platformB.ServiceBox, _platformC);
            }
            else if (_platformA.IsAwaiting)
            {
                _robot.MoveIt(_platformA.ServiceBox, _platformB);
            }
            else if (_entranceStation.GetQueueLen > 0)
            {
                _robot.MoveIt(_entranceStation);
            }
            else
            {
                _robot.Status = RobotStatus.Idle;
            }
        }

        public void OnDeparture()
        {
        }
    }
}