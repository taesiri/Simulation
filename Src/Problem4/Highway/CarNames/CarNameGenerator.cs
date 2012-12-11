using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem4.Highway.CarNames
{
    public class CarNameGenerator
    {
        private readonly List<string> _cars;
        private readonly List<Tuple<string, string>> _company;
        private readonly List<Tuple<string, string>> _model;

        private readonly Random _randomEngine;

        public CarNameGenerator()
        {
            _randomEngine = new Random(DateTime.Now.Millisecond);
            _cars = new List<string>();

            _company = new List<Tuple<string, string>>();
            _model = new List<Tuple<string, string>>();
            try
            {
                var reader =
                    new StreamReader(new FileStream(@"Names\make.csv",
                                                    FileMode.Open, FileAccess.Read));

                string companyName = reader.ReadToEnd();


                reader =
                    new StreamReader(new FileStream(@"Names\models.csv",
                                                    FileMode.Open, FileAccess.Read));

                string modelName = reader.ReadToEnd();


                companyName = companyName.Replace("\r\n", ";");
                modelName = modelName.Replace("\r\n", ";");


                String[] values = companyName.Split(';');
                foreach (string val in values)
                {
                    string[] str = val.Split(',');
                    if (str.Length == 2)
                    {
                        _company.Add(new Tuple<string, string>(str[0], str[1]));
                    }
                }

                values = modelName.Split(';');
                foreach (string val in values)
                {
                    string[] str = val.Split(',');
                    if (str.Length == 2)
                    {
                        _model.Add(new Tuple<string, string>(str[0], str[1]));
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not Initialize the Class!");
            }
        }

        public string GenerateBusName()
        {
            return "اتوبوس شركت واحد ";
        }

        public string GenerateCarName()
        {
            int g = _randomEngine.Next(0, _company.Count - 1);

            string companyName = _company[g].Item2;
            string companyId = _company[g].Item1;


            List<string> mdList = (from tuple in _model where tuple.Item1 == companyId select tuple.Item2).ToList();

            if (mdList.Count == 0)
                throw new Exception("Something went wrong!");

            g = _randomEngine.Next(0, mdList.Count - 1);

            return companyName + " " + mdList[g];
        }
    }
}