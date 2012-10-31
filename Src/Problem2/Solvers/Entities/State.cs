using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem2.Solvers.Entities
{
    public class State
    {
        private bool _c1, _c2, _c3;

        public State()
        {
            _c1 = false;
            _c2 = false;
            _c3 = false;
        }
        public State(State state)
        {
            _c1 = state.GetFirstStatus();
            _c2 = state.GetSecondStatus();
            _c3 = state.GetThirdStatus(); 
        }
        public State (bool c1, bool c2, bool c3 )
        {
            _c1 = c1;
            _c2 = c2;
            _c3 = c3;
        }

        public void SetState(bool c1, bool c2, bool c3)
        {
            _c1 = c1;
            _c2 = c2;
            _c3 = c3;
        }

        public bool GetFirstStatus()
        {
            return _c1;

        }

        public bool GetSecondStatus()
        {
            return _c2;

        }

        public bool GetThirdStatus()
        {
            return _c3;

        }

        public static bool IsAllBusy(State state)
        {
            return false;
        }
    }
}
