using System;

namespace LotteryLibrary
{
    public class RealRandom : IRandom
    {
        public int Next()
        {
            return new Random().Next();
        }
    }
}
