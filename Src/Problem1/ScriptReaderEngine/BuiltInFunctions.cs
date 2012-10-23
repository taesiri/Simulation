using System;
namespace Problem1.ScriptReaderEngine
{
    public class BuiltInFunctions : IScriptReader
    {
        #region IScriptReader Members

        private readonly Random _randomEngine ;

        public BuiltInFunctions()
        {
            _randomEngine = new Random(DateTime.Now.Millisecond);
        }
        public int GenerateNumber(int t1, int t2)
        {
            var returnValue = _randomEngine.Next(t1, t2);
            return returnValue;
        }

        public int UseDefinedMethod(int t1)
        {
            var returnValue = _randomEngine.Next(t1);
            return returnValue;
        }

        public int MapLifeTime(int t1)
        {
            if (t1 >= 0 && t1 < 10)
                return 1000;
            else if (t1 >= 10 && t1 < 20)
                return 1100;
            else if (t1 >= 20 && t1 < 30)
                return 1200;
            else if (t1 >= 30 && t1 < 40)
                return 1300;
            else if (t1 >= 40 && t1 < 50)
                return 1400;
            else if (t1 >= 50 && t1 < 60)
                return 1500;
            else if (t1 >= 60 && t1 < 70)
                return 1600;
            else if (t1 >= 70 && t1 < 80)
                return 1700;
            else if (t1 >= 80 && t1 < 90)
                return 1800;
            else if (t1 >= 90 && t1 < 100)
                return 1900;
            else
                return 1500;
        }

        public int MapDelayTime(int t1)
        {
            if (t1 >= 0 && t1 < 10)
                return 1;
            else if (t1 >= 10 && t1 < 20)
                return 2;
            else if (t1 >= 20 && t1 < 30)
                return 3;
            else if (t1 >= 30 && t1 < 40)
                return 4;
            else if (t1 >= 40 && t1 < 50)
                return 5;
            else if (t1 >= 50 && t1 < 60)
                return 6;
            else if (t1 >= 60 && t1 < 70)
                return 7;
            else if (t1 >= 70 && t1 < 80)
                return 8;
            else if (t1 >= 80 && t1 < 90)
                return 9;
            else
                return 4;
        }

        #endregion
    }
}