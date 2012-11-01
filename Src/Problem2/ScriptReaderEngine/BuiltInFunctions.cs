using System;
using System.IO;

namespace Problem2.ScriptReaderEngine
{
    /// <summary>
    /// Part of Codes are Written by John D. Cook 
    /// Article Home Page : http://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation
    /// </summary>
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
            if (standardDeviation <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                throw new ArgumentOutOfRangeException(msg);
            }
            var normal = SimpleRng.GetNormal();
            var returnValue = (int)(mean + (standardDeviation * normal));            
            //if(returnValue<0)
                
                //throw new InvalidDataException();
            return returnValue;
        }
    }
}