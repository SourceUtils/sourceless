using System.IO;
using BinarySerialization;

namespace Sourceless.GoldSource
{
    public class GoldSourceDemo
    {
        public GoldSourceDemo(DemoHeader header, SegmentDirectoryEntry[] segmentDirEntries)
        {
            Header = header;
            SegmentDirectoryEntries = segmentDirEntries;
        }

        public DemoHeader Header { get; private set; }
        public SegmentDirectoryEntry[] SegmentDirectoryEntries { get; set; }

        public static GoldSourceDemo FromFile(string filePath)
        {
            var serializer = new BinarySerializer();
            var demoStream = new FileStream(filePath, FileMode.Open);

            var demoHeader = serializer.Deserialize<DemoHeader>(demoStream);

            demoStream.Seek(demoHeader.SegmentDirectoryOffset, SeekOrigin.Begin);
            var segmentDir = serializer.Deserialize<SegmentDirectory>(demoStream);

            return new GoldSourceDemo(demoHeader, segmentDir.Entries);
        }
    }
}