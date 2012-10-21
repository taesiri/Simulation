//Deprecated
using System;
using Problem1.ScriptReaderEngine;
using Problem1.TableRows;

namespace Problem1.Sovlers
{
    public static class Solver1
    {
        public static ReportTableRowList SolveIt(int length)
        {
            var returnList = new ReportTableRowList();
            var scriptReader = new PythonScriptReader();
            var cumulativeLifeTime = 0;

            while (length > 0)
            {
                var randomLifeTimeNumber = scriptReader.GenerateNumber(1, 100);
                var mappedLifeTimeValue = scriptReader.MapLifeTime(randomLifeTimeNumber);

                var randomDelayNumber = scriptReader.GenerateNumber(1, 100);
                var mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

                cumulativeLifeTime += mappedLifeTimeValue;

                returnList.PushRow(new ReportTableRowClass(randomLifeTimeNumber, mappedLifeTimeValue,
                                                           cumulativeLifeTime,
                                                           randomDelayNumber, mappedDelayTimeValue));

                length--;
            }
            return returnList;
        }
    }
}