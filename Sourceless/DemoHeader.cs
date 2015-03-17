using BinarySerialization;

namespace Sourceless
{
    public class DemoHeader
    {
        [FieldOrder(0)]
        [FieldLength(8)]
        public string Magic { get; set; }

        [FieldOrder(1)]
        public int DemoProtocolVersion { get; set; }

        [FieldOrder(2)]
        public int ServerProtocolVersion { get; set; }

        [FieldOrder(3)]
        [FieldLength(128)]
        public string MapName { get; set; }

        [FieldOrder(4)]
        [FieldLength(128)]
        public string GameDllName { get; set; }

        [FieldOrder(5)]
        public uint MapCrc { get; set; }

        [FieldOrder(6)]
        public int DirectoryOffset { get; set; }
    }
}