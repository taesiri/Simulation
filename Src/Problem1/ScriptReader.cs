using System;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Problem1
{
    public class ScriptReader
    {
        private readonly ScriptEngine _engine;
        private readonly ScriptScope _escope;
        private readonly Func<int, int, int> _function;
        private readonly Func<int, int> _userDefinedMethod;

        public ScriptReader()
        {
            //Setting up everything necessary!
            _engine = Python.CreateEngine();
            _escope = _engine.CreateScope();

            var reader = new StreamReader(new FileStream(@"Scripts\Script.py", FileMode.Open, FileAccess.Read));
            var scriptText = reader.ReadToEnd();

            _engine.Execute(scriptText, _escope);

            if (!_escope.TryGetVariable<Func<int, int, int>>("function", out _function))
            {
                throw new Exception("Error Occurred in Executing python script!");
            }
            if (!_escope.TryGetVariable<Func<int, int>>("myMethod", out _userDefinedMethod))
            {
                throw new Exception("Error Occurred in Executing python script!");
            }
        }

        public int ReturnValue(int t1, int t2)
        {
            return _function(t1, t2);
        }

        public int UseDefinedMethod(int t1)
        {
            return _userDefinedMethod(t1);
        }
    }
}