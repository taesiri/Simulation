using System;
using System.IO;
using Microsoft.Scripting.Hosting;

namespace Problem1.ScriptReaderEngine
{
    public class RubyScriptReader : IScriptReader
    {
        private readonly Func<int, int> _delayTimeMapper;
        private readonly ScriptEngine _engine;
        private readonly ScriptScope _escope;
        private readonly Func<int, int> _lifeTimeMapper;
        private readonly Func<int, int, int> _randomGenerator;
        private readonly Func<int, int> _userDefinedMethod;


        public RubyScriptReader()
        {
            //_engine = IronRuby.Ruby.CreateEngine();
            //_escope = _engine.CreateScope();

            //var reader = new StreamReader(new FileStream(@"Scripts\Script.rb", FileMode.Open, FileAccess.Read));
            //string scriptText = reader.ReadToEnd();
            //reader.Close();

            //_engine.Execute(scriptText, _escope);

        }

        #region IScriptReader Members

        public int GenerateNumber(int t1, int t2)
        {
            throw new NotImplementedException();
        }

        public int UseDefinedMethod(int t1)
        {
            throw new NotImplementedException();
        }

        public int MapLifeTime(int t1)
        {
            throw new NotImplementedException();
        }

        public int MapDelayTime(int t1)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}