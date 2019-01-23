using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryLibrary.Tests
{
    class TestableRandom : IRandom
    {
        public List<int> NumbersList { get; set; }
        public int Index { get; set; }
        public TestableRandom(List<int> numbers)
        {
            NumbersList = numbers;
        }
        public int Next()
        {
            return NumbersList[Index++];
        }
    }
}
