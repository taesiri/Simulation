using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem1
{
    public static class Solver1
    {
        public static EventList SolveIt(int length)
        {
            var returnList = new EventList();
            var scriptReader = new ScriptReader();

            while (length > 0)
            {
                returnList.PushEvent(new Tuple<int, int, int, int, int>(scriptReader.ReturnValue(0,100), 0, 0,
                                                                        scriptReader.ReturnValue(0,10), 0));
                length--;
            }
            return returnList;
        }
    }
}
