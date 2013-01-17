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
        private readonly List<ServiceBoxElement> _jobDoneList;
        private readonly Random _rnd;
        public ServiceRobotElement MotherRobot;
        private int _arrivalCounter;


        private ServiceEntranceStation _entranceStation;
        private ServiceInspectorStation _inspectorStation;

        private ServicePlatformElement _platformA;
        private ServicePlatformElement _platformB;
        private ServicePlatformElement _platformC;

        private Robot _robot;

        public World()
        {
            GlobalTimeScale = 1;
            InitializeComponent();
            Instance = this;
            CreateScene();
            _felViewer = new FutureEventListViewer();

            _startTime = DateTime.Today + TimeSpan.FromHours(8);

            _fel.EventList.Add(new FutureEvent(Events.Arrival, _startTime));
            _felViewer.SetData(_fel.EventList);
            _felViewer.Show();


            _rnd = new Random(DateTime.Now.Millisecond);
            int numb = _rnd.Next(5, 200);

            for (int i = 0; i < numb; i++)
            {
                RandomEngine.GetNormal();
            }
            _jobDoneList = new List<ServiceBoxElement>();
        }

        public double GlobalTimeScale { get; set; }

        private void BtnStartSimulationClick(object sender, RoutedEventArgs e)
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
            _inspectorStation.Inspector1StatusText.Position = new Point3D(22, 3, 0);
            _inspectorStation.Inspector2StatusText.Position = new Point3D(22, -3, 0);
            _inspectorStation.InspectorQStatusText.Position = new Point3D(22, 14, 0);
            _inspectorStation.Inspector1StatusText.Text = "Worker 1 : " + _inspectorStation.Inspector1Status.ToString();
            _inspectorStation.Inspector2StatusText.Text = "Worker 2 : " + _inspectorStation.Inspector2Status.ToString();
            _inspectorStation.InspectorQStatusText.Text = "Queue Len : 0";

            _robot = new Robot {GlobalTimeScale = 1/60f};

            Mother.Children.Add(_platformA);
            Mother.Children.Add(_platformB);
            Mother.Children.Add(_platformC);
            Mother.Children.Add(_entranceStation);
            Mother.Children.Add(_inspectorStation);

            MotherRobot = new ServiceRobotElement();
            MotherRobot.Transform = new TranslateTransform3D(-12, 8, -3.5);
            Mother.Children.Add(MotherRobot);

            //Timer
            GlobalTimeScale = 1/60f;
            _simulationClock = 0;

            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10f)};

            _timer.Tick += timer_Tick;
        }

        private void OnArrival(DateTime currentTime)
        {
            _arrivalCounter++;


            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            if (_robot.IsIdle && _platformA.IsEmpty)
            {
                _entranceStation.AddNewBox(new ServiceBoxElement(2.5, 2.5, 2.5)
                    {
                        Name = "Box-" + _arrivalCounter.ToString(CultureInfo.InvariantCulture),
                        ArrivalTime = currentTime,
                        Identifier = guidString
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
                        ArrivalTime = currentTime,
                        Identifier = guidString
                    });

                // Q++; //
            }

            // Scheduling the next Entrance!
            _fel.GenerateNextEntrance(currentTime);
        }

        private void OnServiceStartedAtStationA(DateTime currentTime)
        {
            ServiceBoxElement box = _platformA.ServiceBox;
            box.BoxDetails.StationAServiceStartTime = currentTime;


            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.1)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationA, currentTime.Add(boxServiceEndTime)));
            _platformA.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationB(DateTime currentTime)
        {
            ServiceBoxElement box = _platformB.ServiceBox;
            box.BoxDetails.StationBServiceStartTime = currentTime;

            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.3)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationB, currentTime.Add(boxServiceEndTime)));
            _platformB.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationC(DateTime currentTime)
        {
            ServiceBoxElement box = _platformC.ServiceBox;
            box.BoxDetails.StationCServiceStartTime = currentTime;

            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.2)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationC, currentTime.Add(boxServiceEndTime)));
            _platformC.Status = StationStatus.OnService;
        }


        private void OnServiceEndedAtStationA(DateTime currentTime)
        {
            ServiceBoxElement box = _platformA.ServiceBox;
            box.BoxDetails.StationAServiceEndTime = currentTime;

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
            ServiceBoxElement box = _platformB.ServiceBox;
            box.BoxDetails.StationBServiceEndTime = currentTime;

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
            ServiceBoxElement box = _platformC.ServiceBox;
            _platformC.Status = StationStatus.ServiceDone;

            if (_robot.IsIdle)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));


                if (_inspectorStation.Inspector1Status == WorkerStatus.Idle &&
                    _inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    int tos = _rnd.Next(1, 10);
                    if (tos < 5)
                    {
                        _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 1);
                        _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted,
                                                         currentTime.Add(boxMovementTime)));
                        box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);
                    }
                    else
                    {
                        _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 2);
                        _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted,
                                                         currentTime.Add(boxMovementTime)));
                        box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);
                    }
                }
                else if (_inspectorStation.Inspector1Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 1);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted, currentTime.Add(boxMovementTime)));
                    box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);
                }
                else if (_inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 2);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted, currentTime.Add(boxMovementTime)));
                    box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);
                }
                else
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 0);
                    _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspectorQ, currentTime.Add(boxMovementTime)));
                    box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);
                }

                _robot.Status = RobotStatus.Busy;
                _platformC.Status = StationStatus.Empty;
            }
            else
            {
                if (!_robot.IsIdle)
                    _platformC.Status = StationStatus.WaitingforRobot;
            }
            box.BoxDetails.StationCServiceEndTime = currentTime;
        }


        private void OnRobotJobFinished(DateTime currentTime)
        {
            if (_platformC.IsBlockedOrAwaiting)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                ServiceBoxElement box = _platformC.ServiceBox;
                box.BoxDetails.EnteredToInspector = currentTime.Add(boxMovementTime);


                if (_inspectorStation.Inspector1Status == WorkerStatus.Idle &&
                    _inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    int tos = _rnd.Next(1, 10);
                    if (tos < 5)
                    {
                        _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 1);
                        _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted,
                                                         currentTime.Add(boxMovementTime)));
                    }
                    else
                    {
                        _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 2);
                        _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted,
                                                         currentTime.Add(boxMovementTime)));
                    }
                }
                else if (_inspectorStation.Inspector1Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 1);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted, currentTime.Add(boxMovementTime)));
                }
                else if (_inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 2);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted, currentTime.Add(boxMovementTime)));
                }
                else
                {
                    _robot.MoveIt(_platformC, _inspectorStation, boxMovementTime, 0);
                    _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspectorQ, currentTime.Add(boxMovementTime)));
                }

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
                    _robot.ResetRobot();
                }
            }
            else
            {
                _robot.Status = RobotStatus.Idle;
                _robot.ResetRobot();
            }
        }

        private void OnLoadingBoxStartedOnStationA(DateTime currentTime)
        {
            ServiceBoxElement box = _platformA.ServiceBox;
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformA);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationA, currentTime.Add(boxLoadingTime)));
            _platformA.Status = StationStatus.Loading;

            box.BoxDetails.StationALoadDuration = boxLoadingTime;
        }

        private void OnLoadingBoxEndedOnStationA(DateTime currentTime)
        {
            _platformA.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationA, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }

        private void OnLoadingBoxStartedOnStationB(DateTime currentTime)
        {
            ServiceBoxElement box = _platformB.ServiceBox;
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformB);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationB, currentTime.Add(boxLoadingTime)));
            _platformB.Status = StationStatus.Loading;

            box.BoxDetails.StationBLoadDuration = boxLoadingTime;
        }

        private void OnLoadingBoxEndedOnStationB(DateTime currentTime)
        {
            _platformB.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationB, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }


        private void OnLoadingBoxStartedOnStationC(DateTime currentTime)
        {
            ServiceBoxElement box = _platformC.ServiceBox;
            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformC);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationC, currentTime.Add(boxLoadingTime)));
            _platformC.Status = StationStatus.Loading;
            box.BoxDetails.StationCLoadDuration = boxLoadingTime;
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

        private void OnInspector1JobStarted(DateTime currentTime)
        {
            ServiceBoxElement box = _inspectorStation.Inspector1Box;
            _inspectorStation.Inspector1Status = WorkerStatus.Busy;
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));

            //When the Job going to finish? 
            TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(RandomEngine.GetNormal(2, 1))));
            _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobDone, currentTime.Add(workTime)));

            box.BoxDetails.InspectorServiceStartTime = currentTime;
        }


        private void OnInspector2JobStarted(DateTime currentTime)
        {
            ServiceBoxElement box = _inspectorStation.Inspector2Box;
            _inspectorStation.Inspector2Status = WorkerStatus.Busy;
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));

            //When the Job going to finish? 

            TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(RandomEngine.GetNormal(2, 1))));
            _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobDone, currentTime.Add(workTime)));
            box.BoxDetails.InspectorServiceStartTime = currentTime;
        }


        private void OnInspector1JobFinished(DateTime currentTime)
        {
            _inspectorStation.Inspector1Status = WorkerStatus.Idle;
            ServiceBoxElement box = _inspectorStation.Inspector1Box;
            box.BoxDetails.InspectorServiceDoneTime = currentTime;
            box.BoxDetails.DepartureTime = currentTime;

            _jobDoneList.Add(box);
            ArrangeBoxes();
            // Check the Q


            if (_inspectorStation.GetQueueLen > 0)
            {
                _inspectorStation.Inspector1Box = _inspectorStation.InspectorQueue.Dequeue();
                _inspectorStation.Inspector1Box.Transform = new TranslateTransform3D(16, 3, 0);
                _inspectorStation.Inspector1Box.Transformer = new TranslateTransform3D(16, 3, 0);
                //When the Job going to finish? 
                TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(RandomEngine.GetNormal(2, 1))));
                _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobDone, currentTime.Add(workTime)));
                _inspectorStation.Inspector1Status = WorkerStatus.Busy;
                _inspectorStation.Inspector1Box.BoxDetails.InspectorServiceStartTime = currentTime;
            }
            else
            {
                _inspectorStation.Inspector1Box = null;
            }
        }


        private void OnInspector2JobFinished(DateTime currentTime)
        {
            _inspectorStation.Inspector2Status = WorkerStatus.Idle;
            ServiceBoxElement box = _inspectorStation.Inspector2Box;
            box.BoxDetails.InspectorServiceDoneTime = currentTime;
            box.BoxDetails.DepartureTime = currentTime;

            _jobDoneList.Add(box);
            ArrangeBoxes();
            // Check the Q

            if (_inspectorStation.GetQueueLen > 0)
            {
                _inspectorStation.Inspector2Box = _inspectorStation.InspectorQueue.Dequeue();
                _inspectorStation.Inspector2Box.Transform = new TranslateTransform3D(16, -3, 0);
                _inspectorStation.Inspector2Box.Transformer = new TranslateTransform3D(16, -3, 0);
                //When the Job going to finish? 
                TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(RandomEngine.GetNormal(2, 1))));
                _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobDone, currentTime.Add(workTime)));
                _inspectorStation.Inspector2Status = WorkerStatus.Busy;
                _inspectorStation.Inspector2Box.BoxDetails.InspectorServiceStartTime = currentTime;
            }
            else
            {
                _inspectorStation.Inspector2Box = null;
            }
        }


        public void OnDeparture()
        {
        }

        private void ArrangeBoxes()
        {
            int counter = 0;
            foreach (ServiceBoxElement box  in _jobDoneList)
            {
                counter++;
                box.Transform = new TranslateTransform3D(30 + (counter*5), 0, 0);
                box.Transformer = new TranslateTransform3D(30 + (counter*5), 0, 0);
            }
        }

        private void BtnStopSimulationClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void MnuItemExitClick(object sender, RoutedEventArgs e)
        {
            _felViewer.Close();
            Close();
        }

        #region TimerSimulation

        private readonly DateTime _startTime;
        private int _simulationClock;

        private DispatcherTimer _timer;

        private void StartSimulation()
        {
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = _startTime;
            currentTime = currentTime.Add(TimeSpan.FromSeconds(_simulationClock));


            _felViewer.Title = currentTime.ToString("h:mm:ss");
            LblSimulationClock.Content = currentTime.ToString("h:mm:ss");

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
                            case Events.BoxArrivedToInspectorQ:
                                OnBoxArrivedToInspector(futureEvent.Time);
                                break;
                            case Events.InspectorWorker1JobDone:
                                OnInspector1JobFinished(futureEvent.Time);
                                break;
                            case Events.InspectorWorker2JobDone:
                                OnInspector2JobFinished(futureEvent.Time);
                                break;
                            case Events.InspectorWorker1JobStarted:
                                OnInspector1JobStarted(futureEvent.Time);
                                break;
                            case Events.InspectorWorker2JobStarted:
                                OnInspector2JobStarted(futureEvent.Time);
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
            LblRobotStatus.Content = "Robot Status : " + _robot.Status;
            _simulationClock++;


            SysImageCtrl.SetState(new SystemImage
                {
                    EntranceQueueLen = _entranceStation.GetQueueLen,
                    StationAStatus = _platformA.Status,
                    StationBStatus = _platformB.Status,
                    StationCStatus = _platformC.Status,
                    Inspector1Status = _inspectorStation.Inspector1Status,
                    Inspector2Status = _inspectorStation.Inspector2Status
                });
        }

        #endregion
    }
}