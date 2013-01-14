using System;
using System.Collections.Generic;
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
        public static World Instance;
        private readonly FutureEventList _fel = new FutureEventList();
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
            _fel.EventList.Add(new FutureEvent(Events.Arrival, DateTime.Now));

            int counter = 0;
            bool running = true;
            while (running)
            {
                counter++;
                FutureEvent imminentEvent = _fel.GetImminentEvent();
                allEvents.Add(imminentEvent);

                Title = string.Format("Event : {0} Time : {1}", imminentEvent.Event.ToString(), imminentEvent.Time.ToString("h:mm:ss"));

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
                    case Events.MovingToStationA: //Start Moving to The Station A (From EntranceStation)
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

        public void OnArrival(DateTime currentTime)
        {
            _arrivalCounter++;


            if (_robot.IsIdle && _platformA.IsEmpty)
            {
                _entranceStation.AddNewBox(new ServiceBoxElement(2.5, 2.5, 2.5)
                    {
                        Name = "Box-" + _arrivalCounter.ToString(),
                        ArrivalTime = currentTime
                    });

                // Scheduling the Arrival Time to the StationA
                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationA,
                                                 currentTime.Add(TimeSpan.FromSeconds(2))));

                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(2))));

                _robot.MoveIt(_entranceStation, _platformA, TimeSpan.FromSeconds(2));
                _robot.Status = RobotStatus.Busy;
            }
            else
            {
                _entranceStation.AddNewBox(new ServiceBoxElement(2.5, 2.5, 2.5)
                    {
                        Name = "Box-" + _arrivalCounter.ToString(),
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


        public void OnServiceEndedAtStationA(DateTime currentTime)
        {
            if (_platformB.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformA, _platformB, TimeSpan.FromSeconds(3));


                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationB,
                                                 currentTime.Add(TimeSpan.FromSeconds(3))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(3))));
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

        public void OnServiceEndedAtStationB(DateTime currentTime)
        {
            if (_platformC.IsEmpty && _robot.IsIdle)
            {
                _robot.MoveIt(_platformB, _platformC, TimeSpan.FromSeconds(3));

                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationC,
                                                 currentTime.Add(TimeSpan.FromSeconds(3))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(3))));
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

        public void OnServiceEndedAtStationC(DateTime currentTime)
        {
            if (_robot.IsIdle)
            {
                _robot.MoveIt(_platformC, _inspectorStation, TimeSpan.FromSeconds(3));
                _fel.AddNewEvent(new FutureEvent(Events.BoxMovedToInspector, currentTime.Add(TimeSpan.FromSeconds(3))));
                _robot.Status = RobotStatus.Busy;
                _platformC.Status = StationStatus.Empty;
            }
            else
            {
                if (!_robot.IsIdle)
                    _platformC.Status = StationStatus.WaitingforRobot;
            }
        }


        public void OnRobotJobFinished(DateTime currentTime)
        {
            if (_platformC.IsBlockedOrAwaiting)
            {
                _robot.MoveIt(_platformC, _inspectorStation, TimeSpan.FromSeconds(3));

                _fel.AddNewEvent(new FutureEvent(Events.BoxMovedToInspector, currentTime.Add(TimeSpan.FromSeconds(3))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(3))));

                _platformC.Status = StationStatus.Empty;
            }
            else if (_platformB.IsBlockedOrAwaiting && _platformC.IsEmpty)
            {
                _robot.MoveIt(_platformB, _platformC, TimeSpan.FromSeconds(3));

                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationC,currentTime.Add(TimeSpan.FromSeconds(3))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(3))));

                _platformB.Status = StationStatus.Empty;
            }
            else if (_platformA.IsBlockedOrAwaiting && _platformB.IsEmpty)
            {
                _robot.MoveIt(_platformA, _platformB, TimeSpan.FromSeconds(3));

                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationB, currentTime.Add(TimeSpan.FromSeconds(3))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(3))));
                
                _platformA.Status = StationStatus.Empty;
            }
            else if (_entranceStation.GetQueueLen > 0)
            {
                _robot.MoveIt(_entranceStation, _platformA, TimeSpan.FromSeconds(3));

                _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationA, currentTime.Add(TimeSpan.FromSeconds(2))));
                _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime.Add(TimeSpan.FromSeconds(2))));
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