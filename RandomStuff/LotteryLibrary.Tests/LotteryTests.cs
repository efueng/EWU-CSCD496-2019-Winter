using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LotteryLibrary.Tests
{
    [TestClass]
    public class LotteryTests
    {
        [TestMethod]
        public void TestRandom()
        {
            var rand = new TestableRandom(new System.Collections.Generic.List<int> { 1, 3, 5, 7, 9 });
            var lottery = new Lottery(rand);
        }
    }
}
