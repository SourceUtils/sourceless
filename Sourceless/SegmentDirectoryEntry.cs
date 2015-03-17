using System.Collections.Generic;
using BinarySerialization;

namespace Sourceless
{
    public class SegmentDirectoryEntry
    {
        [FieldOrder(0)]
        public int Number { get; set; }

        [FieldOrder(1)]
        [FieldLength(64)]
        public string Title { get; set; }

        [FieldOrder(2)]
        public int Flags { get; set; }

        [FieldOrder(3)]
        public int CdTrack { get; set; }

        [FieldOrder(4)]
        public float Time { get; set; }

        [FieldOrder(5)]
        public int Frames { get; set; }

        [FieldOrder(6)]
        public int Offset { get; set; }

        [FieldOrder(7)]
        public int Length { get; set; }
    }
}