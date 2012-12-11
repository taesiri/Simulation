using System;
using System.Collections.Generic;
using Problem4.Highway;

namespace Problem4.Generator
{
    public class RandomGen
    {
        private readonly Random _rnd;
        public List<float> Numbers;

        public RandomGen()
        {
            ReportList data = CongruentialMethod.MixedCongruential(1024, 1, 13, 11);
            Numbers = new List<float>(data.InnerData);
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        public float Pick()
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

        public int Pick(int mean, int deviation)
        {
            int rValue = Convert.ToInt32(mean + (SmartPick()*deviation));

            if (rValue < mean - deviation)
                return mean - deviation;
            else if (rValue > mean + deviation)
                return mean + deviation;
            return rValue;
        }

        public CarType PickCarType()
        {
            int temp = _rnd.Next(0, 99);


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
    }
}