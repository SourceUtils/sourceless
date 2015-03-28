using BinarySerialization;

namespace Sourceless.GoldSource.Demo
{
    public class SegmentDirectoryEntry
    {
        [FieldOrder(0)]
        public int Number { get; private set; }

        [FieldOrder(1)]
        [FieldLength(64)]
        public string Title { get; private set; }

        [FieldOrder(2)]
        public int Flags { get; private set; }

        [FieldOrder(3)]
        public int CdTrack { get; private set; }

        [FieldOrder(4)]
        public float Time { get; private set; }

        [FieldOrder(5)]
        public int Frames { get; private set; }

        [FieldOrder(6)]
        public int Offset { get; private set; }

        [FieldOrder(7)]
        public int Length { get; private set; }
    }
}