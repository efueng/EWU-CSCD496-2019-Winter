using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryLibrary
{
    public class Lottery
    {
        public IRandom Rand { get; set; }
        public Lottery(IRandom obj = null)
        {
            Rand = obj ?? new RealRandom();
        }


    }
}
