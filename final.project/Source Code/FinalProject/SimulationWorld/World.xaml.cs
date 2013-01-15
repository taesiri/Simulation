using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using FinalProject.SimulationElements;
using FinalProject.SimulationElements.Dialog;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.FutureEventList;
using FinalProject.SimulationElements.RandomGenerator;

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
            GlobalTimeScale = 1;
            InitializeComponent();
            Instance = this;
            CreateScene();
            _felViewer = new FutureEventListViewer();


            _startTime = DateTime.Now;

            _fel.EventList.Add(new FutureEvent(Events.Arrival, _startTime));
            _felViewer.SetData(_fel.EventList);
            _felViewer.Show();


            var rnd = new Random();
            int numb = rnd.Next(1, 200);


            for (int i = 0; i < numb; i++)
            {
                RandomEngine.GetNormal();
            }
        }

        public double GlobalTimeScale { get; set; }

        private void BtnDeployOnClick(object sender, RoutedEventArgs e)
        {
            Simulator();
        }

        private void BtnMoveIt_OnClick(object sender, RoutedEventArgs e)
        {
            StartSimulation();
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

            _robot = new Robot {GlobalTimeScale = 1/60f};

            Mother.Children.Add(_platformA);
            Mother.Children.Add(_platformB);
            Mother.Children.Add(_platformC);
            Mother.Children.Add(_entranceStation);
            Mother.Children.Add(_inspectorStation);
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




                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationA, currentTime.Add(boxMovementTime)));
                _robot.MoveIt(_entranceStation, _platformA, boxMovementTime);
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
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.1)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationA, currentTime.Add(boxServiceEndTime)));
            _platformA.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationB(DateTime currentTime)
        {
            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.3)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationB, currentTime.Add(boxServiceEndTime)));
            _platformB.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationC(DateTime currentTime)
        {
            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.2)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationC, currentTime.Add(boxServiceEndTime)));
            _platformC.Status = StationStatus.OnService;
        }


        private void OnServiceEndedAtStationA(DateTime currentTime)
        {
            _platformA.Status = StationStatus.ServiceDone;

            if (_platformB.IsEmpty && _robot.IsIdle)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformA, _platformB, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationB, currentTime.Add(boxMovementTime)));
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
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformB, _platformC, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationC, currentTime.Add(boxMovementTime)));
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
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspector, currentTime.Add(boxMovementTime)));
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
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspector, currentTime.Add(boxMovementTime)));
                _platformC.Status = StationStatus.Empty;
            }
            else if (_platformB.IsBlockedOrAwaiting && _platformC.IsEmpty)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformB, _platformC, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationC, currentTime.Add(boxMovementTime)));
                _platformB.Status = StationStatus.Empty;
            }
            else if (_platformA.IsBlockedOrAwaiting && _platformB.IsEmpty)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformA, _platformB, boxMovementTime);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationB, currentTime.Add(boxMovementTime)));
                _platformA.Status = StationStatus.Empty;
            }
            else if (_entranceStation.GetQueueLen > 0)
            {
                if (_platformA.IsEmpty)
                {
                    TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                    _robot.MoveIt(_entranceStation, _platformA, boxMovementTime);
                    _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationA,
                                                     currentTime.Add(boxMovementTime)));
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
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformA);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationA, currentTime.Add(boxLoadingTime)));
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
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
            
            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformB);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationB, currentTime.Add(boxLoadingTime)));
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
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
            
            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformC);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationC, currentTime.Add(boxLoadingTime)));
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

        private void OnInspector2JobFinished(DateTime currentTime)
        {
            throw new NotImplementedException();
        }

        private void OnInspector1JobFinished(DateTime currentTime)
        {
            throw new NotImplementedException();
        }
        
        public void OnDeparture()
        {
        }

        #region AutomaticSimulator

        private readonly DateTime _startTime;
        private int _simulationClock;

        private DispatcherTimer _timer;

        private void StartSimulation()
        {
            GlobalTimeScale = 1/60f;
            _simulationClock = 0;

            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10f)};

            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = _startTime;
            currentTime = currentTime.Add(TimeSpan.FromSeconds(_simulationClock));


            _felViewer.Title = currentTime.ToString("h:mm:ss");
            _felViewer.SetData(_fel.EventList);

            List<FutureEvent> events = _fel.GetEventsAt(currentTime);
            if (events.Count > 0)
            {
                do
                {
                 
                    foreach (FutureEvent futureEvent in events)
                    {
                        switch (futureEvent.Event)
                        {
                            case Events.Arrival:
                                OnArrival(futureEvent.Time);
                                break;
                            case Events.Departure:
                                OnDeparture();
                                break;
                            case Events.ServiceEndedOnStationA:
                                OnServiceEndedAtStationA(futureEvent.Time);
                                break;
                            case Events.ServiceEndedOnStationB:
                                OnServiceEndedAtStationB(futureEvent.Time);
                                break;
                            case Events.ServiceEndedOnStationC:
                                OnServiceEndedAtStationC(futureEvent.Time);
                                break;
                            case Events.RobotJobFinished:
                                OnRobotJobFinished(futureEvent.Time);
                                break;
                            case Events.ServiceStartedOnStationA: // BOX arrived to the Station A!
                                OnServiceStartedAtStationA(futureEvent.Time);
                                break;
                            case Events.ServiceStartedOnStationB: // BOX arrived to the Station B!
                                OnServiceStartedAtStationB(futureEvent.Time);
                                break;
                            case Events.ServiceStartedOnStationC: // BOX arrived to the Station C!
                                OnServiceStartedAtStationC(futureEvent.Time);
                                break;
                            case Events.LoadingBoxStartedOnStationA:
                                OnLoadingBoxStartedOnStationA(futureEvent.Time);
                                break;
                            case Events.LoadingBoxEndedOnStationA:
                                OnLoadingBoxEndedOnStationA(futureEvent.Time);
                                break;
                            case Events.LoadingBoxStartedOnStationB:
                                OnLoadingBoxStartedOnStationB(futureEvent.Time);
                                break;
                            case Events.LoadingBoxEndedOnStationB:
                                OnLoadingBoxEndedOnStationB(futureEvent.Time);
                                break;
                            case Events.LoadingBoxStartedOnStationC:
                                OnLoadingBoxStartedOnStationC(futureEvent.Time);
                                break;
                            case Events.LoadingBoxEndedOnStationC:
                                OnLoadingBoxEndedOnStationC(futureEvent.Time);
                                break;
                            case Events.BoxArrivedToInspector:
                                OnBoxArrivedToInspector(futureEvent.Time);
                                break;
                            case Events.InspectorWorker1JobDone:
                                OnInspector1JobFinished(futureEvent.Time);
                                break;
                            case Events.InspectorWorker2JobDone:
                                OnInspector2JobFinished(futureEvent.Time);
                                break;
                            case Events.EndOfSimulation:
                                _timer.Stop();
                                break;
                            case Events.BeginingOfSimulation:
                                break;
                        }
                    }


                    events = new List<FutureEvent>(_fel.GetEventsAt(currentTime));
                } while (events.Count != 0);
            }
            LblRobotStatus.Content = _robot.Status;
            _simulationClock++;
        }

      

        #endregion
    }
}