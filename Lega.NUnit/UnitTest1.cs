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
            memory.Poke(0x0F, 0x01);
            Assert.Equals(memory.Peek(0x0F), 0x01);
        }
    }
}