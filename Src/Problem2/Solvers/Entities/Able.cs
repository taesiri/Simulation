using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem2.Solvers.Entities
{
    class Able : ICarhops
    {

        public bool IsIdle { get; set; }
        public ICustomer CurrentCustomer { get; set; }

     
    }
}
