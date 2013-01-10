using System;
using System.Collections.Generic;
using System.IO;
using Problem2.NameGenerator;
using Problem2.ScriptReaderEngine;
using Problem2.Solvers.Entities;

namespace Problem2.Solvers
{
    public class SolverEngine
    {
        #region ReaderEngine enum

        public enum ReaderEngine
        {
            Python,
            BuiltIn
        }

        #endregion

        #region SolvingMethod enum

        public enum SolvingMethod
        {
            Method1,
            Method2,
            MixedMethod
        }

        #endregion

        private readonly List<Carhops> _carhopses; // What a WORD! Carhopses

        private readonly int _caseLength;

        private readonly bool _isInitialized;
        private readonly Random _randomGenerator;
        private readonly ReaderEngine _readerEngine;

        private readonly IScriptReader _scriptReader;

        private readonly List<ICustomer> _solvedData;
        private readonly SolvingMethod _solverMethod;

        private readonly FutureEventList _globalList;

        private bool _isCaseSolved;

        public SolverEngine(ReaderEngine engine, SolvingMethod method, int length)
        {
            _randomGenerator = new Random(DateTime.Now.Millisecond);
            _solvedData = new List<ICustomer>();
            _carhopses = new List<Carhops>();
            _globalList = new FutureEventList();

            _isInitialized = true;
            _readerEngine = engine;
            _solverMethod = method;
            _caseLength = length;

            if (_readerEngine == ReaderEngine.Python)
            {
                _scriptReader = new PythonScriptReader();
            }
            else if (_readerEngine == ReaderEngine.BuiltIn)
            {
                _scriptReader = new BuiltInFunctions();
            }
        }

        public List<ICustomer> GetAnswer
        {
            get
            {
                if (_isCaseSolved)
                    return _solvedData;
                else
                    return new List<ICustomer>();
                //throw new Exception("Case not Solved yet!");
            }
        }

        public List<Carhops> GetCarhops
        {
            get
            {
                if (_isCaseSolved)
                    return _carhopses;
                else
                    return new List<Carhops>();
                //throw new Exception("Case not Solved yet!");
            }
        }
        public FutureEventList GetGlobalList
        {
            get
            {
                if (_isCaseSolved)
                    return _globalList;
                else
                    return new FutureEventList();
                //throw new Exception("Case not Solved yet!");
            }
        }

        public void SolveIt()
        {
            if (!_isInitialized)
                throw new Exception("Class is not Initialized yet");

            switch (_solverMethod)
            {
                case SolvingMethod.Method1:
                    Method1(_caseLength, _scriptReader);
                    _isCaseSolved = true;
                    break;
                case SolvingMethod.Method2:
                    Method2(_caseLength, _scriptReader);
                    _isCaseSolved = true;
                    break;
                case SolvingMethod.MixedMethod:
                    MixedMethod(_caseLength, _scriptReader);
                    _isCaseSolved = true;
                    break;
            }
        }


        private void Method1(int length, IScriptReader scriptReader)
        {
            var generator = new GenerateName();

            var able = new Able {IsIdle = true};
            var baker = new Baker {IsIdle = true};
            var charlie = new Charlie {IsIdle = true};

            _carhopses.Add(able);
            _carhopses.Add(baker);
            _carhopses.Add(charlie);

            var fel = new FutureEventList();
            var customersQueue = new Queue<ICustomer>(length);
            int totalTime = 0;

            for (int i = 0; i < length; i++)
            {
                int randomNumber = i == 0 ? 0 : Math.Abs(scriptReader.GetNormal(5, 5));
                totalTime += randomNumber;

                var customer = new Customer(totalTime, Math.Abs(scriptReader.GetNormal(10, 6)), generator)
                                   {ArrivalTimeNumber = randomNumber};

                var felItem = new FutureEventListRow
                                  {
                                      Time = totalTime,
                                      Customer = customer,
                                      EventType = EventType.Arrival
                                  };

                fel.PushEventRow(felItem);
                _globalList .PushEventRow(felItem);
            }

            while (fel.Length > 0)
            {
                FutureEventListRow nearestEvent = fel.GetNearestEvent();
                int currentTime = nearestEvent.Time;

                if (nearestEvent.EventType == EventType.Arrival)
                {
                    var nState = new State(!able.IsIdle, !baker.IsIdle, !charlie.IsIdle);
                    nearestEvent.Customer.OnArrivalSystemState = nState;
                    if (able.IsIdle)
                    {
                        nearestEvent.Customer.ServiceProvider = able;
                        able.CurrentCustomer = nearestEvent.Customer;

                        var felItem = new FutureEventListRow
                                          {
                                              Time = nearestEvent.Customer.ServiceTime + currentTime,
                                              Customer = nearestEvent.Customer,
                                              EventType = EventType.Departure,
                                          };

                        fel.PushEventRow(felItem);
                        _globalList.PushEventRow(felItem);

                        able.IsIdle = false;
                    }
                    else if (baker.IsIdle)
                    {
                        nearestEvent.Customer.ServiceProvider = baker;
                        baker.CurrentCustomer = nearestEvent.Customer;

                        var felItem = new FutureEventListRow
                                          {
                                              Time = nearestEvent.Customer.ServiceTime + currentTime,
                                              Customer = nearestEvent.Customer,
                                              EventType = EventType.Departure
                                          };

                        fel.PushEventRow(felItem);
                        _globalList.PushEventRow(felItem);
                        baker.IsIdle = false;
                    }
                    else if (charlie.IsIdle)
                    {
                        nearestEvent.Customer.ServiceProvider = charlie;
                        charlie.CurrentCustomer = nearestEvent.Customer;

                        var felItem = new FutureEventListRow
                                          {
                                              Time = nearestEvent.Customer.ServiceTime + currentTime,
                                              Customer = nearestEvent.Customer,
                                              EventType = EventType.Departure
                                          };
                        fel.PushEventRow(felItem);
                        _globalList.PushEventRow(felItem);
                        charlie.IsIdle = false;
                    }
                    else
                    {
                        customersQueue.Enqueue(nearestEvent.Customer);
                    }
                }
                else if (nearestEvent.EventType == EventType.Departure)
                {
                    Carhops currentCarhop = nearestEvent.Customer.ServiceProvider;
                    currentCarhop.IsIdle = true;

                    var enCustomer = new Customer(nearestEvent.Customer.ArrivalTime, nearestEvent.Customer.ServiceTime,
                                                  generator)
                                         {
                                             ServiceProvider = nearestEvent.Customer.ServiceProvider,
                                             DepartureTime = currentTime,
                                             ArrivalTimeNumber = nearestEvent.Customer.ArrivalTimeNumber,
                                             OnArrivalSystemState = nearestEvent.Customer.OnArrivalSystemState
                                         };
                    enCustomer.WaitTime = enCustomer.DepartureTime - (enCustomer.ArrivalTime + enCustomer.ServiceTime);

                    var nState = new State(!able.IsIdle, !baker.IsIdle, !charlie.IsIdle);
                    enCustomer.AfterDepartureState = nState;

                    if (enCustomer.WaitTime < 0)
                        throw new InvalidDataException();

                    _solvedData.Add(enCustomer);

                    if (customersQueue.Count > 0)
                    {
                        // his work is done!
                        ICustomer firstInQueue = customersQueue.Dequeue();

                        firstInQueue.ServiceProvider = currentCarhop;

                        currentCarhop.CurrentCustomer = firstInQueue;
                        currentCarhop.IsIdle = false;


                        var felItem = new FutureEventListRow
                                          {
                                              Time = firstInQueue.ServiceTime + currentTime,
                                              Customer = firstInQueue,
                                              EventType = EventType.Departure
                                          };
                        fel.PushEventRow(felItem);
                        _globalList.PushEventRow(felItem);
                    }
                    else
                    {
                        if (nearestEvent.Customer.ServiceProvider is Able)
                        {
                            able.IsIdle = true;
                            able.CurrentCustomer = null;
                        }
                        else if (nearestEvent.Customer.ServiceProvider is Baker)
                        {
                            baker.IsIdle = true;
                            baker.CurrentCustomer = null;
                        }
                        else if (nearestEvent.Customer.ServiceProvider is Charlie)
                        {
                            charlie.IsIdle = true;
                            charlie.CurrentCustomer = null;
                        }
                    }
                }
                fel.RemoveFirstElement();
            }
        }

        private void Method2(int length, IScriptReader scriptReader)
        {
            var generator = new GenerateName();

            var able = new Able {IsIdle = true};
            var baker = new Baker {IsIdle = true};
            var charlie = new Charlie {IsIdle = true};

            _carhopses.Add(able);
            _carhopses.Add(baker);
            _carhopses.Add(charlie);

            var fel = new FutureEventList();
            var customersQueue = new Queue<ICustomer>(length);
            int totalTime = 0;

            for (int i = 0; i < length; i++)
            {
                int randomNumber = i == 0 ? 0 : Math.Abs(scriptReader.GetNormal(5, 5));
                totalTime += randomNumber;

                var customer = new Customer(totalTime, Math.Abs(scriptReader.GetNormal(10, 6)), generator)
                                   {ArrivalTimeNumber = randomNumber};

                var felItem = new FutureEventListRow
                                  {
                                      Time = totalTime,
                                      Customer = customer,
                                      EventType = EventType.Arrival
                                  };

                fel.PushEventRow(felItem);
                _globalList.PushEventRow(felItem);
            }

            while (fel.Length > 0)
            {
                FutureEventListRow nearestEvent = fel.GetNearestEvent();
                int currentTime = nearestEvent.Time;

                if (nearestEvent.EventType == EventType.Arrival)
                {
                    var nState = new State(!able.IsIdle, !baker.IsIdle, !charlie.IsIdle);
                    nearestEvent.Customer.OnArrivalSystemState = nState;

                    if (nState.IsAnyAvailable())
                    {
                        var availables = new List<Carhops>();
                        if (!nState.GetFirstStatus())
                            availables.Add(able);
                        if (!nState.GetSecondStatus())
                            availables.Add(baker);
                        if (!nState.GetThirdStatus())
                            availables.Add(charlie);

                        Carhops selectedCarhops = null;

                        int numb = 0;
                        switch (availables.Count)
                        {
                            case 1:
                                selectedCarhops = availables[0];
                                break;
                            case 2:
                                numb = _randomGenerator.Next(0, 20)/10;
                                if (numb >= 0 && numb < 1)
                                    selectedCarhops = availables[0];
                                else if (numb >= 1 && numb < 2)
                                    selectedCarhops = availables[1];
                                break;
                            case 3:
                                numb = _randomGenerator.Next(0, 30)/10;
                                if (numb >= 0 && numb < 1)
                                    selectedCarhops = availables[0];
                                else if (numb >= 1 && numb < 2)
                                    selectedCarhops = availables[1];
                                else if (numb >= 2 && numb < 3)
                                    selectedCarhops = availables[2];
                                break;
                            default:
                                break;
                        }
                        if (selectedCarhops != null)
                        {
                            nearestEvent.Customer.ServiceProvider = selectedCarhops;
                            selectedCarhops.CurrentCustomer = nearestEvent.Customer;

                            var felItem = new FutureEventListRow
                                              {
                                                  Time = nearestEvent.Customer.ServiceTime + currentTime,
                                                  Customer = nearestEvent.Customer,
                                                  EventType = EventType.Departure,
                                              };

                            fel.PushEventRow(felItem);
                            _globalList.PushEventRow(felItem);
                            selectedCarhops.IsIdle = false;
                        }
                        else
                        {
                            customersQueue.Enqueue(nearestEvent.Customer);
                        }
                    }
                    else
                    {
                        customersQueue.Enqueue(nearestEvent.Customer);
                    }
                }
                else if (nearestEvent.EventType == EventType.Departure)
                {
                    Carhops currentCarhop = nearestEvent.Customer.ServiceProvider;
                    currentCarhop.IsIdle = true;

                    var enCustomer = new Customer(nearestEvent.Customer.ArrivalTime, nearestEvent.Customer.ServiceTime,
                                                  generator)
                                         {
                                             ServiceProvider = nearestEvent.Customer.ServiceProvider,
                                             DepartureTime = currentTime,
                                             ArrivalTimeNumber = nearestEvent.Customer.ArrivalTimeNumber,
                                             OnArrivalSystemState = nearestEvent.Customer.OnArrivalSystemState
                                         };
                    enCustomer.WaitTime = enCustomer.DepartureTime - (enCustomer.ArrivalTime + enCustomer.ServiceTime);

                    var nState = new State(!able.IsIdle, !baker.IsIdle, !charlie.IsIdle);
                    enCustomer.AfterDepartureState = nState;

                    if (enCustomer.WaitTime < 0)
                        throw new InvalidDataException();

                    _solvedData.Add(enCustomer);

                    if (customersQueue.Count > 0)
                    {
                        // his work is done!
                        ICustomer firstInQueue = customersQueue.Dequeue();

                        firstInQueue.ServiceProvider = currentCarhop;

                        currentCarhop.CurrentCustomer = firstInQueue;
                        currentCarhop.IsIdle = false;


                        var felItem = new FutureEventListRow
                                          {
                                              Time = firstInQueue.ServiceTime + currentTime,
                                              Customer = firstInQueue,
                                              EventType = EventType.Departure
                                          };
                        fel.PushEventRow(felItem);
                        _globalList.PushEventRow(felItem);
                    }
                    else
                    {
                        if (nearestEvent.Customer.ServiceProvider is Able)
                        {
                            able.IsIdle = true;
                            able.CurrentCustomer = null;
                        }
                        else if (nearestEvent.Customer.ServiceProvider is Baker)
                        {
                            baker.IsIdle = true;
                            baker.CurrentCustomer = null;
                        }
                        else if (nearestEvent.Customer.ServiceProvider is Charlie)
                        {
                            charlie.IsIdle = true;
                            charlie.CurrentCustomer = null;
                        }
                    }
                }
                fel.RemoveFirstElement();
            }
        }

        private void MixedMethod(int length, IScriptReader scriptReader)
        {
            throw new NotImplementedException();
        }
    }
}