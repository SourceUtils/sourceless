using BinarySerialization;

namespace Sourceless.GoldSource.Demo
{
    public class SegmentDirectory
    {
        [FieldOrder(0)]
        public int SegmentCount { get; private set; }

        [FieldOrder(1)]
        [FieldCount("SegmentCount")]
        public SegmentDirectoryEntry[] Entries { get; private set; }
    }
}