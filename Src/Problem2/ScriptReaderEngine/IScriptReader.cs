namespace Problem2.ScriptReaderEngine
{
    internal interface IScriptReader
    {
        int GenerateNumber(int t1, int t2);
        int UseDefinedMethod(int t1);
        int MapLifeTime(int t1);
        int MapDelayTime(int t1);
    }
}