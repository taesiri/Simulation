using System;
using Problem1.ScriptReaderEngine;

namespace Problem1
{
    public static class Solver1
    {
        public static EventList SolveIt(int length)
        {
            var returnList = new EventList();
            var scriptReader = new PythonScriptReader();
            var cumulativeLifeTime = 0;

            while (length > 0)
            {
                var randomLifeTimeNumber = scriptReader.GenerateNumber(0, 100);
                var mappedLifeTimeValue = scriptReader.MapLifeTime(randomLifeTimeNumber);

                var randomDelayNumber = scriptReader.GenerateNumber(0, 100);
                var mappedDelayTimeValue = scriptReader.MapDelayTime(randomDelayNumber);

                cumulativeLifeTime += mappedLifeTimeValue;

                returnList.PushEvent(new Tuple<int, int, int, int, int>(randomLifeTimeNumber, mappedLifeTimeValue,
                                                                        cumulativeLifeTime,
                                                                        randomDelayNumber, mappedDelayTimeValue));

                length--;
            }
            return returnList;


            
        }
    }
}