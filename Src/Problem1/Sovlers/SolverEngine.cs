using System;
using System.Collections.Generic;
using System.Linq;
using Problem1.ScriptReaderEngine;
using Problem1.TableRows;

namespace Problem1.Sovlers
{
    public class SolverEngine
    {
        private readonly bool _isInitialized = false;
        private readonly ReaderEngine _readerEngine;
        private readonly SolvingMethod _solverMethod;
        private readonly int _caseLength;

        private readonly IScriptReader _scriptReader;
        public SolverEngine(ReaderEngine engine, SolvingMethod method, int length)
        {
            _isInitialized = true;
            _readerEngine = engine;
            _solverMethod = method;
            _caseLength = length;

            if (_readerEngine == ReaderEngine.Python)
            {
                _scriptReader = new PythonScriptReader();
            }
            else if (_readerEngine == ReaderEngine.CSharp)
            {

            }
            else if (_readerEngine == ReaderEngine.FSharp)
            {

            }
            else if (_readerEngine == ReaderEngine.Ruby)
            {

            }
        }

        public ReportTableRowList SolveIt()
        {
            if (!_isInitialized)
                throw new Exception("Class is not Initialized yet");

            var returnList = new ReportTableRowList();

            switch (_solverMethod)
            {
                case SolvingMethod.Method1:
                    returnList = Method1(_caseLength, _scriptReader);
                    break;
                case SolvingMethod.Method2:
                    returnList = Method2(_caseLength, _scriptReader);
                    break;
                default:
                    break;
            }
            return returnList;
        }


        private ReportTableRowList Method1(int length, IScriptReader scriptReader)
        {
            var list = new ReportTableRowList();
            var cumulativeLifeTime = 0;
            while (length > 0)
            {
                var randomLifeTimeNumber = scriptReader.GenerateNumber(0, 100);
                var mappedLifeTimeValue = scriptReader.MapLifeTime(randomLifeTimeNumber);

                var randomDelayNumber = scriptReader.GenerateNumber(0, 100);
                var mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

                cumulativeLifeTime += mappedLifeTimeValue;

                list.PushRow(new ReportTableRowClass(randomLifeTimeNumber, mappedLifeTimeValue,
                                                                  cumulativeLifeTime,
                                                                  randomDelayNumber, mappedDelayTimeValue));

                length--;
            }
            return list;
        }

        private ReportTableRowList Method2(int length, IScriptReader scriptReader)
        {
            var list = new ReportTableRowList();
            var cumulativeLifeTime = 0;

            while (length > 0)
            {
                var randomDelayNumber = scriptReader.GenerateNumber(1, 100);
                var mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

                var life1 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));
                var life2 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));
                var life3 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));

                var firstFailure = new List<int>() {life1, life2, life3}.Min();

                cumulativeLifeTime += firstFailure;

                list.PushRow(new ReportTableRowClass(life1, life2, life3, firstFailure, cumulativeLifeTime,
                                                     randomDelayNumber, mappedDelayTimeValue));

                length--;
            }
            return list;
        }

        public enum ReaderEngine
        {
            Python,
            CSharp,
            Ruby,
            FSharp
        }
        public enum SolvingMethod
        {
            Method1,
            Method2
        }
    }
}
