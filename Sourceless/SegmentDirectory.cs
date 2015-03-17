using BinarySerialization;

namespace Sourceless
{
    public class SegmentDirectory
    {
        [FieldOrder(0)]
        public int SegmentCount { get; set; }

        [FieldOrder(1)]
        [FieldCount("SegmentCount")]
        public SegmentDirectoryEntry[] Entries { get; set; }
    }
}