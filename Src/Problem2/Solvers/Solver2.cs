//Deprecated

using System.Collections.Generic;
using System.Linq;
using Problem2.ScriptReaderEngine;
using Problem2.TableRows;

namespace Problem2.Solvers
{
    public static class Solver2
    {
        public static ReportTableRowList SolveIt(int length)
        {
            var returnList = new ReportTableRowList();
            var scriptReader = new PythonScriptReader();
            int cumulativeLifeTime = 0;

            while (length > 0)
            {
                int randomDelayNumber = scriptReader.GenerateNumber(1, 100);
                int mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

                int life1 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));
                int life2 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));
                int life3 = scriptReader.MapLifeTime(scriptReader.GenerateNumber(1, 100));

                int firstFailure = new List<int> {life1, life2, life3}.Min();

                cumulativeLifeTime += firstFailure;
                returnList.PushRow(new ReportTableRowClass(life1, life2, life3, firstFailure, cumulativeLifeTime,
                                                           randomDelayNumber, mappedDelayTimeValue));
                length--;
            }
            return returnList;
        }
    }
}