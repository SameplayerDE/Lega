namespace Lega.Core.Memory
{
    public interface IVirtualComponent
    {
        public VirtualMemoryRegion MemoryRegion
        { 
            get; 
        }

        public int MemoryOffset
        {
            get;
        }

        public int MemoryLength
        {
            get;
        }

        public void Map(VirtualMemory memory, int offset, int bytes);

    }
}
