using System;

namespace Problem2.ScriptReaderEngine
{
    public class BuiltInFunctions : IScriptReader
    {
        private readonly Random _randomEngine;


        public BuiltInFunctions()
        {
            _randomEngine = new Random(DateTime.Now.Millisecond);
        }

        #region IScriptReader Members

        public int GenerateNumber(int t1, int t2)
        {
            int returnValue = _randomEngine.Next(t1, t2);
            return returnValue;
        }

        #endregion

        public int GetNormal(double mean, double standardDeviation)
        {
            var returnValue = -1;

            while (!(mean - standardDeviation <= returnValue && returnValue <= mean + standardDeviation))
            {
                if (standardDeviation <= 0.0)
                {
                    string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                    throw new ArgumentOutOfRangeException(msg);
                }
                var normal = SimpleRng.GetNormal();
                returnValue = (int) (mean + (standardDeviation*normal));
            }
            return returnValue;
        }
    }
}