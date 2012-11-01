namespace Problem2.ScriptReaderEngine
{
    internal interface IScriptReader
    {
        int GenerateNumber(int t1, int t2);
        int GetNormal(double mean, double standardDeviation);
    }
}