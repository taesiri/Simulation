using System;
using System.Collections.Generic;
using System.IO;

namespace Problem2.NameGenerator
{
    public class GenerateName
    {
        private readonly List<string> _firstNames;
        private readonly List<string> _lastNames;

        private readonly Random _randomEngine;

        public GenerateName()
        {
            _randomEngine = new Random(DateTime.Now.Millisecond);

            _firstNames = new List<string>();
            _lastNames = new List<string>();
            try
            {
                var reader =
                    new StreamReader(new FileStream(@"Names\nameDataBase.csv",
                        FileMode.Open,FileAccess.Read));

                string fileSt = reader.ReadToEnd();
                fileSt = fileSt.Replace("\r\n", ";");
                String[] values = fileSt.Split(';');
                foreach (string val in values)
                {
                    string[] str = val.Split(',');
                    if (str.Length == 2)
                    {
                        _firstNames.Add(str[0]);
                        _lastNames.Add(str[1]);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not Initialize the Calss!");
            }
        }

        public string GenerateFirstName()
        {
            return _firstNames[GenerateNumber()];
        }

        public string GenerateLastName()
        {
            return _lastNames[GenerateNumber()];
        }

        public int GenerateNumber()
        {
            int minimum = _lastNames.Count;
            if (_firstNames.Count < minimum)
                minimum = _firstNames.Count;

            int returnValue = _randomEngine.Next(0, minimum);
            return returnValue;
        }
    }
}