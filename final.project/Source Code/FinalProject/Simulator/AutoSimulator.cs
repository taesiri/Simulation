using System;
using System.Collections.Generic;
using System.Globalization;
using FinalProject.SimulationElements.Enums;
using FinalProject.SimulationElements.FutureEventList;
using FinalProject.SimulationElements.RandomGenerator;

namespace FinalProject.Simulator
{
    // Doing simulation fast the report the result back!
    public class AutoSimulator
    {
        private readonly Entrance _entranceStation;
        private readonly FutureEventList _fel;
        private readonly Inspector _inspectorStation;

        private readonly Platform _platformA;
        private readonly Platform _platformB;
        private readonly Platform _platformC;

        private readonly ARobot _robot;
        private int _arrivalCounter;
        private SimulationResult _result;


        public AutoSimulator(DateTime startTime, DateTime endTime)
        {
            _fel = new FutureEventList {EndTime = endTime};
            _fel.AddNewEvent(new FutureEvent(Events.Arrival, startTime));

            StartTime = startTime; // 8:00 AM
            EndTime = endTime; // 4:00 PM

            _entranceStation = new Entrance {Name = "Entrance"};

            _platformA = new Platform {Name = "A"};
            _platformB = new Platform {Name = "B"};
            _platformC = new Platform {Name = "C"};
            _inspectorStation = new Inspector();
            _robot = new ARobot();


            var rnd = new Random();
            int numb = rnd.Next(1, 200);

            for (int i = 0; i < numb; i++)
            {
                RandomEngine.GetNormal();
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public void StartSimulation()
        {
            _result = new SimulationResult();

            var futureEvent = new FutureEvent(Events.Arrival, new DateTime()); //Dummy!
            while (futureEvent.Event != Events.EndOfSimulation)
            {
                futureEvent = _fel.GetImminentEvent();
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
                        break;
                }
            }
        }

        public SimulationResult GetResult()
        {
            if (_result != null)
                return _result;
            throw new Exception("The Case isn't solved yet!");
        }

        #region Simulation

        private void OnArrival(DateTime currentTime)
        {
            _arrivalCounter++;

            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");


            if (_robot.IsIdle && _platformA.IsEmpty)
            {
                _entranceStation.AddNewBox(new Box
                    {
                        Name = "Box-" + _arrivalCounter.ToString(CultureInfo.InvariantCulture),
                        ArrivalTime = currentTime,
                        Identifier = guidString
                    });


                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationA, currentTime.Add(boxMovementTime)));
                _robot.MoveIt(_entranceStation, _platformA);
                _robot.Status = RobotStatus.Busy;
            }
            else
            {
                _entranceStation.AddNewBox(new Box
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
            Box box = _platformA.ServiceBox;
            box.StationAServiceStartTime = currentTime;

            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.1)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationA, currentTime.Add(boxServiceEndTime)));
            _platformA.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationB(DateTime currentTime)
        {
            Box box = _platformB.ServiceBox;
            box.StationBServiceStartTime = currentTime;

            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.3)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationB, currentTime.Add(boxServiceEndTime)));
            _platformB.Status = StationStatus.OnService;
        }

        private void OnServiceStartedAtStationC(DateTime currentTime)
        {
            Box box = _platformC.ServiceBox;
            box.StationCServiceStartTime = currentTime;

            // Scheduling the Service End Time
            TimeSpan boxServiceEndTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(1.2)));
            _fel.AddNewEvent(new FutureEvent(Events.ServiceEndedOnStationC, currentTime.Add(boxServiceEndTime)));
            _platformC.Status = StationStatus.OnService;
        }


        private void OnServiceEndedAtStationA(DateTime currentTime)
        {
            Box box = _platformA.ServiceBox;
            box.StationAServiceEndTime = currentTime;

            _platformA.Status = StationStatus.ServiceDone;

            if (_platformB.IsEmpty && _robot.IsIdle)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformA, _platformB);
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
            Box box = _platformB.ServiceBox;
            box.StationBServiceEndTime = currentTime;

            _platformB.Status = StationStatus.ServiceDone;

            if (_platformC.IsEmpty && _robot.IsIdle)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformB, _platformC);
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
            Box box = _platformC.ServiceBox;

            _platformC.Status = StationStatus.ServiceDone;

            if (_robot.IsIdle)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));


                if (_inspectorStation.Inspector1Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 1);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted, currentTime.Add(boxMovementTime)));
                    box.EnteredToInspector = currentTime.Add(boxMovementTime);
                }
                else if (_inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 2);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted, currentTime.Add(boxMovementTime)));
                    box.EnteredToInspector = currentTime.Add(boxMovementTime);
                }
                else
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 0);
                    _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspectorQ, currentTime.Add(boxMovementTime)));
                    box.EnteredToInspector = currentTime.Add(boxMovementTime);
                }

                _robot.Status = RobotStatus.Busy;
                _platformC.Status = StationStatus.Empty;
            }
            else
            {
                if (!_robot.IsIdle)
                    _platformC.Status = StationStatus.WaitingforRobot;
            }
            box.StationCServiceEndTime = currentTime;
        }


        private void OnRobotJobFinished(DateTime currentTime)
        {
            if (_platformC.IsBlockedOrAwaiting)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                Box box = _platformC.ServiceBox;
                box.EnteredToInspector = currentTime.Add(boxMovementTime);

                if (_inspectorStation.Inspector1Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 1);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobStarted, currentTime.Add(boxMovementTime)));
                }
                else if (_inspectorStation.Inspector2Status == WorkerStatus.Idle)
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 2);
                    _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobStarted, currentTime.Add(boxMovementTime)));
                }
                else
                {
                    _robot.MoveIt(_platformC, _inspectorStation, 0);
                    _fel.AddNewEvent(new FutureEvent(Events.BoxArrivedToInspectorQ, currentTime.Add(boxMovementTime)));
                }

                _platformC.Status = StationStatus.Empty;
            }
            else if (_platformB.IsBlockedOrAwaiting && _platformC.IsEmpty)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformB, _platformC);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationC, currentTime.Add(boxMovementTime)));
                _platformB.Status = StationStatus.Empty;
            }
            else if (_platformA.IsBlockedOrAwaiting && _platformB.IsEmpty)
            {
                TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                _robot.MoveIt(_platformA, _platformB);
                _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxStartedOnStationB, currentTime.Add(boxMovementTime)));
                _platformA.Status = StationStatus.Empty;
            }
            else if (_entranceStation.GetQueueLen > 0)
            {
                if (_platformA.IsEmpty)
                {
                    TimeSpan boxMovementTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));
                    _robot.MoveIt(_entranceStation, _platformA);
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
            Box box = _platformA.ServiceBox;


            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformA);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationA, currentTime.Add(boxLoadingTime)));
            _platformA.Status = StationStatus.Loading;

            box.StationALoadDuration = boxLoadingTime;
        }

        private void OnLoadingBoxEndedOnStationA(DateTime currentTime)
        {
            _platformA.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationA, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }

        private void OnLoadingBoxStartedOnStationB(DateTime currentTime)
        {
            Box box = _platformB.ServiceBox;

            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformB);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationB, currentTime.Add(boxLoadingTime)));
            _platformB.Status = StationStatus.Loading;

            box.StationBLoadDuration = boxLoadingTime;
        }

        private void OnLoadingBoxEndedOnStationB(DateTime currentTime)
        {
            _platformB.Status = StationStatus.Loaded;
            _fel.AddNewEvent(new FutureEvent(Events.ServiceStartedOnStationB, currentTime));
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));
        }


        private void OnLoadingBoxStartedOnStationC(DateTime currentTime)
        {
            Box box = _platformC.ServiceBox;

            TimeSpan boxLoadingTime = TimeSpan.FromMinutes(Math.Round(RandomEngine.GetExpo(0.7)));

            _robot.Status = RobotStatus.Busy;
            _robot.LoadBox(_platformC);
            _fel.AddNewEvent(new FutureEvent(Events.LoadingBoxEndedOnStationC, currentTime.Add(boxLoadingTime)));
            _platformC.Status = StationStatus.Loading;

            box.StationCLoadDuration = boxLoadingTime;
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
            Box box = _inspectorStation.Inspector1Box;

            _inspectorStation.Inspector1Status = WorkerStatus.Busy;
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));

            //When the Job going to finish? 
            TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(2 + RandomEngine.GetNormal())));
            _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobDone, currentTime.Add(workTime)));


            box.InspectorServiceStartTime = currentTime;
        }


        private void OnInspector2JobStarted(DateTime currentTime)
        {
            Box box = _inspectorStation.Inspector2Box;

            _inspectorStation.Inspector2Status = WorkerStatus.Busy;
            _fel.AddNewEvent(new FutureEvent(Events.RobotJobFinished, currentTime));

            //When the Job going to finish? 

            TimeSpan workTime = TimeSpan.FromMinutes(2 + RandomEngine.GetNormal());
            _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobDone, currentTime.Add(workTime)));


            box.InspectorServiceStartTime = currentTime;
        }


        private void OnInspector1JobFinished(DateTime currentTime)
        {
            _inspectorStation.Inspector1Status = WorkerStatus.Idle;
            Box box = _inspectorStation.Inspector1Box;

            box.InspectorServiceDoneTime = currentTime;
            box.DepartureTime = currentTime;
            _result.BoxResult.Add(box);


            if (_inspectorStation.GetQueueLen > 0)
            {
                _inspectorStation.Inspector1Box = _inspectorStation.InspectorQueue.Dequeue();
                //When the Job going to finish? 
                TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(2 + RandomEngine.GetNormal())));
                _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker1JobDone, currentTime.Add(workTime)));
                _inspectorStation.Inspector1Status = WorkerStatus.Busy;


            }
            else
            {
                _inspectorStation.Inspector1Box = null;
            }
        }

        private void OnInspector2JobFinished(DateTime currentTime)
        {
            _inspectorStation.Inspector2Status = WorkerStatus.Idle;
            Box box = _inspectorStation.Inspector2Box;

            box.InspectorServiceDoneTime = currentTime;
            box.DepartureTime = currentTime;
            _result.BoxResult.Add(box);

            if (_inspectorStation.GetQueueLen > 0)
            {
                _inspectorStation.Inspector2Box = _inspectorStation.InspectorQueue.Dequeue();
                //When the Job going to finish? 
                TimeSpan workTime = TimeSpan.FromSeconds(Math.Round(60*(2 + RandomEngine.GetNormal())));
                _fel.AddNewEvent(new FutureEvent(Events.InspectorWorker2JobDone, currentTime.Add(workTime)));
                _inspectorStation.Inspector2Status = WorkerStatus.Busy;
            }
            else
            {
                _inspectorStation.Inspector2Box = null;
            }
        }

        public void OnDeparture()
        {
        }

        #endregion
    }

    public class SimulationResult
    {
        public List<Box> BoxResult;
        public List<ResultRow> Result;

        public SimulationResult()
        {
            BoxResult = new List<Box>();
            Result = new List<ResultRow>();
        }

        public void PushRow(ResultRow row)
        {
            Result.Add(row);
        }
    }

    public class ResultRow
    {
        public Events CurrentEvent { get; set; }
        public SystemImage SystemImage { get; set; }
        public DateTime Time { get; set; }
    }
}