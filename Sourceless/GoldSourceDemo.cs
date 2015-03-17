using System.IO;
using BinarySerialization;

namespace Sourceless
{
    public class GoldSourceDemo
    {
        public GoldSourceDemo(DemoHeader header, SegmentDirectoryEntry[] segmentDirs)
        {
            Header = header;
            Segments = segmentDirs;
        }

        public DemoHeader Header { get; private set; }
        public SegmentDirectoryEntry[] Segments { get; set; }

        public static GoldSourceDemo FromFile(string filePath)
        {
            var serializer = new BinarySerializer();
            var demoStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var demoHeader = serializer.Deserialize<DemoHeader>(demoStream);

            demoStream.Seek(demoHeader.SegmentDirectoryOffset, SeekOrigin.Begin);
            var segmentDir = serializer.Deserialize<SegmentDirectory>(demoStream);

            return new GoldSourceDemo(demoHeader, segmentDir.Entries);
        }
    }
}