using System;
using System.Windows;
using System.Windows.Media.Animation;
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
        private readonly FutureEventList _fel = new FutureEventList();


        private ServiceEntranceStation _entranceStation;
        private ServiceInspectorStation _exitStationA;

        private ServicePlatformElement _platformA;
        private ServicePlatformElement _platformB;
        private ServicePlatformElement _platformC;
        private ServiceEntranceStation _platformElem;

        private Robot _robot;

        public World()
        {
            InitializeComponent();
            CreateScene();
        }

        private void BtnDeployOnClick(object sender, RoutedEventArgs e)
        {
            var platformElement = new ServiceEntranceStation();

            Mother.Children.Add(platformElement);

            _platformElem = platformElement;
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
            var da = new DoubleAnimation {From = -10, To = 10f, Duration = new Duration(TimeSpan.FromSeconds(2))};
            _platformElem.Tranformer.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);
        }


        public void CreateScene()
        {
            _entranceStation = new ServiceEntranceStation(new Point3D(-22, 0, 0.5f));

            _platformA = new ServicePlatformElement(new Point3D(-7.5, 0, -4.5));
            _platformB = new ServicePlatformElement(new Point3D(0, 0, -4.5));
            _platformC = new ServicePlatformElement(new Point3D(7.5, 0, -4.5));

            _exitStationA = new ServiceInspectorStation(new Point3D(18, 0, 0.5f));

            _robot = new Robot();

            var sampleBox = new ServiceBoxElement(new Point3D(0, 0, 2.1), 2.5, 2.5, 2.5, @"Textures\postBox.jpg");

            Mother.Children.Add(_platformA);
            Mother.Children.Add(_platformB);
            Mother.Children.Add(_platformC);
            Mother.Children.Add(_entranceStation);
            Mother.Children.Add(_exitStationA);

            Mother.Children.Add(sampleBox);
        }


        public void Simulator()
        {
            bool running = true;
            while (running)
            {
                Events imminentEvent = _fel.GetImminentEvent();
                switch (imminentEvent)
                {
                    case Events.Arrival:
                        if (_robot.IsIdle)
                        {
                            _robot.MoveIt();
                            _robot.Status = RobotStatus.Busy;
                        }
                        else
                        {
                            // Q++;
                        }
                        break;
                    case Events.Departure:
                        // Place Holder
                        break;

                    case Events.ServiceEndedOnStationA:
                        if (_platformB.IsEmpty && _robot.IsIdle)
                        {
                        }
                        else
                        {
                            _platformA.Status = StationStatus.BlockedOrAwaiting;
                        }

                        break;
                    case Events.ServiceEndedOnStationB:
                        if (_platformC.IsEmpty && _robot.IsIdle)
                        {
                        }
                        else
                        {
                            _platformB.Status = StationStatus.BlockedOrAwaiting;
                        }

                        break;
                    case Events.ServiceEndedOnStationC:
                        if (_robot.IsIdle)
                        {
                        }
                        else
                        {
                            _platformC.Status = StationStatus.BlockedOrAwaiting;
                        }

                        break;


                    case Events.RobotJobFinished:

                        if (_platformC.IsAwaiting)
                        {
                        }
                        else if (_platformB.IsAwaiting)
                        {
                        }
                        else if (_platformA.IsAwaiting)
                        {
                        }
                        else if (_entranceStation.GetQueueLen > 0 )
                        {
                        }
                        else
                        {
                            _robot.Status = RobotStatus.Idle;
                        }


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
            }


            //END OF Simulation
        }

        public void OnArrival()
        {
        }

        public void OnDeparture()
        {
        }
    }
}