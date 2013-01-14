using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Media3D;
using FinalProject.SimulationElements;
using FinalProject.SimulationElements.Dialog;
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
        private readonly FutureEventListViewer _felViewer;
        private int _arrivalCounter;


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
            _felViewer = new FutureEventListViewer();


            _fel.EventList.Add(new FutureEvent(Events.Arrival, DateTime.Now));
            _felViewer.SetData(_fel.EventList);
            _felViewer.Show();
        }

        private void BtnDeployOnClick(object sender, RoutedEventArgs e)
        {
            Simulator();
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
        }

        public void CreateScene()
        {
            _entranceStation = new ServiceEntranceStation(new Point3D(-22, 0, 0.5f)) {Name = "Entrance"};

            _platformA = new ServicePlatformElement(new Point3D(-7.5, 0, -4.5)) {Name = "A"};
            _platformA.PlatformStatusText.Position = new Point3D(-8, 2, -1);
            _platformA.PlatformStatusText.Text = _platformA.Status.ToString();
            _platformB = new ServicePlatformElement(new Point3D(0, 0, -4.5)) {Name = "B"};
            _platformB.PlatformStatusText.Position = new Point3D(0, 2, -1);
            _platformB.PlatformStatusText.Text = _platformB.Status.ToString();
            _platformC = new ServicePlatformElement(new Point3D(7.5, 0, -4.5)) {Name = "C"};
            _platformC.PlatformStatusText.Position = new Point3D(8, 2, -1);
            _platformC.PlatformStatusText.Text = _platformC.Status.ToString();

            _inspectorStation = new ServiceInspectorStation(new Point3D(18, 0, 0.5f));

            _robot = new Robot();

            //var sampleBox = new ServiceBoxElement(new Point3D(16, 10, 2.1), 2.5, 2.5, 2.5, @"Textures\postBox.jpg");

            Mother.Children.Add(_platformA);
            Mother.Children.Add(_platformB);
            Mother.Children.Add(_platformC);
            Mother.Children.Add(_entranceStation);
            Mother.Children.Add(_inspectorStation);

            //Mother.Children.Add(sampleBox);
        }


        public void Simulator()
        {
            var allEvents = new List<FutureEvent>();
            var allImage = new List<SystemImage>();
            int counter = 0;
            bool running = true;
            while (running)
            {
                _felViewer.SetData(_fel.EventList);

                counter++;
                FutureEvent imminentEvent = _fel.GetImminentEvent();
                allEvents.Add(imminentEvent);

                Title = string.Format("Event : {0} Time : {1}", imminentEvent.Event.ToString(),
                                      imminentEvent.Time.ToString("h:mm:ss"));

                switch (imminentEvent.Event)
                {
                    case Events.Arrival:
                        OnArrival(imminentEvent.Time);
                        break;
                    case Events.Departure:
                        OnDeparture();
                        break;
                    case Events.ServiceEndedOnStationA:
                        OnServiceEndedAtStationA(imminentEvent.Time);
                        break;
                    case Events.ServiceEndedOnStationB:
                        OnServiceEndedAtStationB(imminentEvent.Time);
                        break;
                    case Events.ServiceEndedOnStationC:
                        OnServiceEndedAtStationC(imminentEvent.Time);
                        break;
                    case Events.RobotJobFinished:
                        OnRobotJobFinished(imminentEvent.Time);
                        break;
                    case Events.ServiceStartedOnStationA: // BOX arrived to the Station A!
                        OnServiceStartedAtStationA(imminentEvent.Time);
                        break;
                    case Events.ServiceStartedOnStationB: // BOX arrived to the Station B!
                        OnServiceStartedAtStationB(imminentEvent.Time);
                        break;
                    case Events.ServiceStartedOnStationC: // BOX arrived to the Station C!
                        OnServiceStartedAtStationC(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxStartedOnStationA:
                        OnLoadingBoxStartedOnStationA(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxEndedOnStationA:
                        OnLoadingBoxEndedOnStationA(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxStartedOnStationB:
                        OnLoadingBoxStartedOnStationB(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxEndedOnStationB:
                        OnLoadingBoxEndedOnStationB(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxStartedOnStationC:
                        OnLoadingBoxStartedOnStationC(imminentEvent.Time);
                        break;
                    case Events.LoadingBoxEndedOnStationC:
                        OnLoadingBoxEndedOnStationC(imminentEvent.Time);
                        break;
                    case Events.BoxArrivedToInspector:
                        OnBoxArrivedToInspector(imminentEvent.Time);
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

                LblRobotStatus.Content = _robot.Status;

                if (counter >= 1)
                {
                    running = false;
                    counter = 0;
                }

                //Create System Image
                var image = new SystemImage
                    {
                        EntranceQueueLen = _entranceStation.GetQueueLen,
                        StationAStatus = _platformA.Status,
                        StationBStatus = _platformB.Status,
                        StationCStatus = _platformC.Status,
                        Inspector1Status = WorkerStatus.Idle,
                        Inspector2Status = WorkerStatus.Idle,
                        CurrentEvent = imminentEvent,
                        RobotStatus = _robot.Status,
                        RobotLastActivity = _robot.LastActivity
                    };

                allImage.Add(image);
            }
            //END OF Simulation

            //var sysViewer = new SystemViewer();
            //sysViewer.SetData(allImage);
            //sysViewer.Show();
        }


        private void OnArrival(DateTime currentTime)
        {
            _arrivalCounter++;


            if (_robot.IsIdle && _platformA.IsEmpty)
            {
                _entranceStation.AddNewBox(new ServiceBoxElement(2.5, 2.5, 2.5)
                    {
                        Name = "Box-" + _arrivalCounter.ToString(CultureInfo.InvariantCulture),
                        ArrivalTime = currentTime
                    });

                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationA,
                                                 currentTime.Add(TimeSpan.FromSeconds(2))));
                _robot.MoveIt(_entranceStation, _platformA, TimeSpan.FromSeconds(2));
                _robot.Status = RobotStatus.Busy;
            }
            else
            {
                _entranceStation.AddNewBox(new ServiceBoxElement(2.5, 2.5, 2.5)
                    {
                        Name = "Box-" + _arrivalCounter.ToString(CultureInfo.InvariantCulture),
                        ArrivalTime = currentTime
                    });

                // Q++; //
            }

            // Scheduling the next Entrance!
            _fel.GenerateNextEntrance(currentTime);
        }

        private void OnServiceStartedAtStationA(DateTime currentTime)
        {
            // Scheduling the Service End Time
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationA, currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformA.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationB(DateTime currentTime)
        {
            // Scheduling the Service End Time
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationB, currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformB.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationC(DateTime currentTime)
        {
            // Scheduling the Service End Time
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationC, currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformC.Status = StationStatus.OnService;
        }


        private void OnServiceEndedAtStationA(DateTime currentTime)
        {
            _platformA.Status = StationStatus.ServiceDone;

            if (_platformB.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformA, _platformB, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationB,
                                                 currentTime.Add(TimeSpan.FromSeconds(2))));
                _robot.Status = RobotStatus.Busy;
                _platformA.Status = StationStatus.Empty;
            }
            else
            {
                if (!_platformB.IsEmpty && !_robot.IsIdle)
                    _platformA.Status = StationStatus.BlockedAndWaitingforRobot;
                else if (!_robot.IsIdle)
                    _platformA.Status = StationStatus.WaitingforRobot;
                else if (!_platformB.IsEmpty)
                    _platformA.Status = StationStatus.Blocked;
            }
        }

        private void OnServiceEndedAtStationB(DateTime currentTime)
        {
            _platformB.Status = StationStatus.ServiceDone;

            if (_platformC.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformB, _platformC, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationC,
                                                 currentTime.Add(TimeSpan.FromSeconds(2))));

                _robot.Status = RobotStatus.Busy;
                _platformB.Status = StationStatus.Empty;
            }
            else
            {
                if (!_platformC.IsEmpty && !_robot.IsIdle)
                    _platformB.Status = StationStatus.BlockedAndWaitingforRobot;
                else if (!_robot.IsIdle)
                    _platformB.Status = StationStatus.WaitingforRobot;
                else if (!_platformC.IsEmpty)
                    _platformB.Status = StationStatus.Blocked;
            }
        }

        private void OnServiceEndedAtStationC(DateTime currentTime)
        {
            _platformC.Status = StationStatus.ServiceDone;

            if (_robot.IsIdle)
            {
                _robot.MoveIt(_platformC, _inspectorStation, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspector, currentTime.Add(TimeSpan.FromSeconds(3))));
                _robot.Status = RobotStatus.Busy;
                _platformC.Status = StationStatus.Empty;
            }
            else
            {
                if (!_robot.IsIdle)
                    _platformC.Status = StationStatus.WaitingforRobot;
            }
        }


        private void OnRobotJobFinished(DateTime currentTime)
        {
            if (_platformC.IsBlockedOrAwaiting)
            {
                _robot.MoveIt(_platformC, _inspectorStation, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspector, currentTime.Add(TimeSpan.FromSeconds(3))));
                _platformC.Status = StationStatus.Empty;
            }
            else if (_platformB.IsBlockedOrAwaiting && _platformC.IsEmpty)
            {
                _robot.MoveIt(_platformB, _platformC, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationC,
                                                 currentTime.Add(TimeSpan.FromSeconds(3))));
                _platformB.Status = StationStatus.Empty;
            }
            else if (_platformA.IsBlockedOrAwaiting && _platformB.IsEmpty)
            {
                _robot.MoveIt(_platformA, _platformB, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationB,
                                                 currentTime.Add(TimeSpan.FromSeconds(3))));
                _platformA.Status = StationStatus.Empty;
            }
            else if (_entranceStation.GetQueueLen > 0)
            {
                if (_platformA.IsEmpty)
                {
                    _robot.MoveIt(_entranceStation, _platformA, TimeSpan.FromSeconds(3));
                    _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationA,
                                                     currentTime.Add(TimeSpan.FromSeconds(2))));
                }
                else
                {
                    _robot.Status = RobotStatus.Idle;
                }
            }
            else
            {
                _robot.Status = RobotStatus.Idle;
            }
        }

        private void OnLoadingBoxStartedOnStationA(DateTime currentTime)
        {
            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformA);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationA, currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformA.Status = StationStatus.Loading;
        }

        private void OnLoadingBoxEndedOnStationA(DateTime currentTime)
        {
            _platformA.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationA, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }

        private void OnLoadingBoxStartedOnStationB(DateTime currentTime)
        {
            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformB);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationB,
                                             currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformB.Status = StationStatus.Loading;
        }

        private void OnLoadingBoxEndedOnStationB(DateTime currentTime)
        {
            _platformB.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationB, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }


        private void OnLoadingBoxStartedOnStationC(DateTime currentTime)
        {
            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformC);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationC,
                                             currentTime.Add(TimeSpan.FromSeconds(2))));
            _platformC.Status = StationStatus.Loading;
        }

        private void OnLoadingBoxEndedOnStationC(DateTime currentTime)
        {
            _platformC.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationC, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }

        private void OnBoxArrivedToInspector(DateTime currentTime)
        {
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }

        public void OnDeparture()
        {
        }
    }
}