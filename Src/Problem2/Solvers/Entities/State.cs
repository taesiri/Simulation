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
            _c1 = false; //True : Busy | False = Idle
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

        public bool IsAllAvailable()
        {
            if (_c1 == false && _c2 == false && _c2 == false)
                return true;
            return false;
        }
        public bool IsAnyAvailable()
        {
            return !(_c1 && _c2 && _c3);
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
        private string BoolToString(bool val)
        {
            if (val == true)
                return "1";
            else
            {
                return "0";
            }
        }

        public static bool IsAllBusy(State state)
        {
            return false;
        }
        
        public override string ToString()
        {

            return string.Format("({0},{1},{2})", BoolToString(_c1), BoolToString(_c2), BoolToString(_c3));
        }

       
    }
}
