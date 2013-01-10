using System;
using System.IO;
using System.Windows;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Problem1.ScriptReaderEngine
{
    public class PythonScriptReader : IScriptReader
    {
        private readonly Func<int, int> _delayTimeMapper;
        private readonly ScriptEngine _engine;
        private readonly ScriptScope _escope;
        private readonly Func<int, int> _lifeTimeMapper;
        private readonly Func<int, int, int> _randomGenerator;
        private readonly Func<int, int> _userDefinedMethod;

        public PythonScriptReader()
        {
            _engine = Python.CreateEngine();
            _escope = _engine.CreateScope();

            var reader = new StreamReader(new FileStream(@"Scripts\Script.py", FileMode.Open, FileAccess.Read));
            string scriptText = reader.ReadToEnd();
            reader.Close();

            _engine.Execute(scriptText, _escope);

            if (!_escope.TryGetVariable("GeneratorFunc", out _randomGenerator))
            {
                MessageBox.Show("Error Occurred in Executing python script! - GeneratorFunc", "Error");
                throw new Exception("Error Occurred in Executing python script!");
            }
            if (!_escope.TryGetVariable("userDefinedFunc", out _userDefinedMethod))
            {
                MessageBox.Show("Error Occurred in Executing python script! - userDefinedFunc", "Error");
                throw new Exception("Error Occurred in Executing python script!");
            }
            if (!_escope.TryGetVariable("LifeSpanMapper", out _lifeTimeMapper))
            {
                MessageBox.Show("Error Occurred in Executing python script! - LifeSpanMapper", "Error");
                throw new Exception("Error Occurred in Executing python script!");
            }
            if (!_escope.TryGetVariable("DelayMapper", out _delayTimeMapper))
            {
                MessageBox.Show("Error Occurred in Executing python script! - DelayMapper", "Error");
                throw new Exception("Error Occurred in Executing python script!");
            }
        }

        #region IScriptReader Members

        public int GenerateNumber(int t1, int t2)
        {
            return _randomGenerator(t1, t2);
        }

        public int UseDefinedMethod(int t1)
        {
            return _userDefinedMethod(t1);
        }

        public int MapLifeTime(int t1)
        {
            return _lifeTimeMapper(t1);
        }

        public int MapDelayTime(int t1)
        {
            return _delayTimeMapper(t1);
        }

        #endregion
    }
}