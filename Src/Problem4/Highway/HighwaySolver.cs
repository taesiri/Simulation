using System;
using System.Collections.Generic;
using System.Windows;
using Problem4.Generator;
using Problem4.Highway.CarNames;

namespace Problem4.Highway
{
    public class HighwaySolver
    {
        private readonly RandomGen _generator;
        private readonly CarNameGenerator _nameGenerator;
        private readonly List<CarDetailsRow> _solvedData;
        private readonly int _totalCars;
        private bool _isSolved;

        public HighwaySolver(int numberOfTotalCars)
        {
            _generator = new RandomGen();
            _totalCars = numberOfTotalCars;
            _solvedData = new List<CarDetailsRow>();

            _nameGenerator = new CarNameGenerator();
        }

        public List<CarDetailsRow> SolvedData
        {
            get
            {
                if (!_isSolved)
                    throw new Exception("The Case is not solved yet!");
                return _solvedData;
            }
        }


        public void SolveIt()
        {
            try
            {
                _isSolved = true;

                int remains = _totalCars;
                var clock = new DateTime(2012, 12, 19, 8, 0, 0);
                while (remains >= 0)
                { 
                    double iAt = _generator.Pick(19, 10);

                    double duration = _generator.Pick(60, 15);
                    clock = clock.AddSeconds(iAt);
                    
                    CarType carType = _generator.PickCarType();
                    string carName = carType == CarType.C40
                                         ? _nameGenerator.GenerateBusName()
                                         : _nameGenerator.GenerateCarName();
                    _solvedData.Add(new CarDetailsRow(carName, clock, (int)duration, (int) iAt, carType));

                    remains -= Helper.CarCapacity(carType);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Message);
            }
        }
    }
}