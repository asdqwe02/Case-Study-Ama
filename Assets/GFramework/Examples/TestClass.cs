using MemoryPack;

namespace GFramework.Examples
{
    [MemoryPackable]
    public partial class TestClass
    {
        [MemoryPackInclude]
        private int _a = 10;
        [MemoryPackInclude]
        private int _b = 20;

        public int A => _a;
        public int B => _b;
    }
}