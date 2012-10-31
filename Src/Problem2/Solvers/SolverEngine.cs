using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly int _caseLength;

        private readonly bool _isInitialized;
        private readonly ReaderEngine _readerEngine;

        private readonly IScriptReader _scriptReader;
        private readonly SolvingMethod _solverMethod;

        private readonly List<ICustomer> _solvedData;
        private bool _isCaseSolved = false;

        public SolverEngine(ReaderEngine engine, SolvingMethod method, int length)
        {
            _solvedData = new List<ICustomer>();

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

            var able = new Able {IsIdle = true};
            var baker = new Baker { IsIdle = true };
            var charlie = new Charlie { IsIdle = true };

            var arrivalTimeList = new List<int> {0};
            var fel = new FutureEventList();

            var customersQueue = new Queue<ICustomer>(length);

            for (var i = 1; i < length; i++)
            {
                arrivalTimeList.Add(arrivalTimeList[i - 1] + scriptReader.GenerateNumber(0, 10));
            }

            foreach (var item in arrivalTimeList)
            {
                var felItem = new FutureEventListRow
                                  {
                                      Time = item,
                                      Customer = new Customer(item, scriptReader.GenerateNumber(10, 20)),
                                      EventType = EventType.Arrival
                                  };

                fel.PushEventRow(felItem);
            }

            Debug.WriteLine("Done! Generating!");

            while (fel.Length > 0)
            {
                var nearestEvent = fel.GetNearestEvent();
                var currentTime = nearestEvent.Time;
                if (nearestEvent.EventType == EventType.Arrival)
                {
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

                        charlie.IsIdle = false;
                    }
                    else
                    {
                        customersQueue.Enqueue(nearestEvent.Customer);
                    }
                }
                else if (nearestEvent.EventType == EventType.Departure)
                {
                    var enCustomer = new Customer(nearestEvent.Customer.ArrivalTime, nearestEvent.Customer.ServiceTime)
                                         {
                                             ServiceProvider = nearestEvent.Customer.ServiceProvider,
                                             DepartureTime = currentTime
                                         };
                    enCustomer.WaitTime = enCustomer.DepartureTime - (enCustomer.ArrivalTime + enCustomer.ServiceTime);

                    _solvedData.Add(enCustomer);
                    if (customersQueue.Count > 0)
                    {
                        var currentCarhop = nearestEvent.Customer.ServiceProvider as ICarhops; // his work is done!
                        var firstInQueue = customersQueue.Dequeue();

                        firstInQueue.ServiceProvider = currentCarhop;

                        currentCarhop.CurrentCustomer = firstInQueue;
                        currentCarhop.IsIdle = false;


                        var felItem = new FutureEventListRow
                        {
                            Time = nearestEvent.Customer.ServiceTime + currentTime,
                            Customer = firstInQueue,
                            EventType = EventType.Departure
                        };
                        fel.PushEventRow(felItem);
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
            _solvedData.ToString();
        }

        private void Method2(int length, IScriptReader scriptReader)
        {

        }
        private void MixedMethod(int length, IScriptReader scriptReader)
        {

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
    }
}