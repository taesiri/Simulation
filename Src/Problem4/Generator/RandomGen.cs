using System;
using System.Collections.Generic;
using Problem4.Highway;

namespace Problem4.Generator
{
    public class RandomGen
    {
        private readonly Random _rnd;
        public List<float> Numbers;
        private int _pickCarPtr = 1;

        public RandomGen()
        {
            ReportList data = CongruentialMethod.MixedCongruential(1024, 1, 13, 11);
            Numbers = new List<float>(data.InnerData);
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        public double Pick()
        {
            return Numbers[_rnd.Next(0, Numbers.Count - 1)];
        }

        public float SmartPick()
        {
            float rValue = Numbers[_rnd.Next(0, Numbers.Count - 1)];
            int temp = _rnd.Next(0, 10);
            if (temp < 5)
                rValue -= 1;

            return rValue;
        }


        public double Pick(int mean, int deviation)
        {
            if (_pickCarPtr > Numbers.Count)
                _pickCarPtr = 0;

            double rValue = DirectTransformation(Numbers[_pickCarPtr], Numbers[_pickCarPtr + 1]).Item2;

            rValue = Math.Abs((rValue*deviation) + mean);

            if (rValue < mean - deviation)
                return mean - deviation;
            else if (rValue > mean + deviation)
                return mean + deviation;
            return rValue;
        }


        public CarType PickCarType()
        {
            var temp = (int) (Numbers[_pickCarPtr]*100);
            _pickCarPtr++;

            if (_pickCarPtr >= Numbers.Count)
                _pickCarPtr = 0;

            if (temp >= 0 && temp < 20)
            {
                return CarType.C1;
            }
            else if (temp >= 20 && temp < 40)
            {
                return CarType.C2;
            }
            else if (temp >= 40 && temp < 55)
            {
                return CarType.C3;
            }
            else if (temp >= 55 && temp < 65)
            {
                return CarType.C4;
            }


            return CarType.C40;
        }


        public Tuple<double, double> DirectTransformation(float r1, float r2)
        {
            double z1 = Math.Sqrt(-2*Math.Log(r1, Math.E))*Math.Cos(2*Math.PI*r2);
            double z2 = Math.Sqrt(-2*Math.Log(r1, Math.E))*Math.Sin(2*Math.PI*r2);

            return new Tuple<double, double>(z1, z2);
        }
    }
}