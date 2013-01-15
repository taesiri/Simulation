using System;

namespace FinalProject.SimulationElements.RandomGenerator
{
    public class RandomEngine
    {
        private static uint _mW;
        private static uint _mZ;

        static RandomEngine()
        {
            _mW = 521288629;
            _mZ = 362436069;
        }


        public static void SetSeed(uint u, uint v)
        {
            if (u != 0) _mW = u;
            if (v != 0) _mZ = v;
        }

        public static void SetSeed(uint u)
        {
            _mW = u;
        }

        public static void SetSeedFromSystemTime()
        {
            DateTime dt = DateTime.Now;
            long x = dt.ToFileTime();
            SetSeed((uint) (x >> 16), (uint) (x%4294967296));
        }

        public static double GetUniform()
        {
            uint u = GetUint();
            return (u + 1.0)*2.328306435454494e-10;
        }

        private static uint GetUint()
        {
            _mZ = 36969*(_mZ & 65535) + (_mZ >> 16);
            _mW = 18000*(_mW & 65535) + (_mW >> 16);
            return (_mZ << 16) + _mW;
        }

        public static double GetNormal()
        {
            double u1 = GetUniform();
            double u2 = GetUniform();
            double r = Math.Sqrt(-2.0*Math.Log(u1));
            double theta = 2.0*Math.PI*u2;
            return r*Math.Sin(theta);
        }

        public static double GetNormal(double mean, double standardDeviation)
        {
            if (standardDeviation <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                throw new ArgumentOutOfRangeException(msg);
            }
            return mean + standardDeviation*GetNormal();
        }


        public static double GetExpo(double mean)
        {
            double norm = Math.Abs(GetNormal());

            if (norm >= 1)
                norm = 0.999;

            norm = 1 - norm;
            double number = Math.Log(norm, Math.E)*(-1*mean);

            number = Math.Round(number);
            if (number == 0)
                number = 1;

            return number;
        }
    }
}