//Deprecated

using Problem2.ScriptReaderEngine;
using Problem2.TableRows;

namespace Problem2.Solvers
{
    public static class Solver1
    {
        public static ReportTableRowList SolveIt(int length)
        {
            var returnList = new ReportTableRowList();
            var scriptReader = new PythonScriptReader();
            int cumulativeLifeTime = 0;

            while (length > 0)
            {
                int randomLifeTimeNumber = scriptReader.GenerateNumber(1, 100);
                int mappedLifeTimeValue = scriptReader.MapLifeTime(randomLifeTimeNumber);

                int randomDelayNumber = scriptReader.GenerateNumber(1, 100);
                int mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

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