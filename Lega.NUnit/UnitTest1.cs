using Lega.Core.Memory;

namespace Lega.NUnit
{
    public class Tests
    {

        VirtualMemory memory;

        [SetUp]
        public void Setup()
        {
            memory = new VirtualMemory(512);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}