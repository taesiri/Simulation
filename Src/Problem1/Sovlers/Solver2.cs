//Deprecated
using System;
using System.Collections.Generic;
using Problem1.ScriptReaderEngine;
using Problem1.TableRows;
using System.Linq;
namespace Problem1.Sovlers
{
    public static class Solver2
    {
        public static ReportTableRowList SolveIt(int length)
        {
            var returnList = new ReportTableRowList();
            var scriptReader = new PythonScriptReader();
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
                returnList.PushRow(new ReportTableRowClass(life1, life2, life3, firstFailure, cumulativeLifeTime,
                                                           randomDelayNumber, mappedDelayTimeValue));
                length--;
            }
            return returnList;
        }
    }
}