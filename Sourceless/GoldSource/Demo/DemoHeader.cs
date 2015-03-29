using System.IO;
using BinarySerialization;

namespace Sourceless.GoldSource.Demo
{
    public class DemoHeader : BinaryEntity<DemoHeader>
    {
        [FieldOrder(0)]
        [FieldLength(8)]
        public string Magic { get; private set; }

        [FieldOrder(1)]
        public int DemoProtocolVersion { get; private set; }

        [FieldOrder(2)]
        public int ServerProtocolVersion { get; private set; }

        [FieldOrder(3)]
        [FieldLength(128)]
        public string MapName { get; private set; }

        [FieldOrder(4)]
        [FieldLength(128)]
        public string GameDllName { get; private set; }

        [FieldOrder(5)]
        public uint MapCrc { get; private set; }

        [FieldOrder(6)]
        public int SegmentDirectoryOffset { get; private set; }
    }
}