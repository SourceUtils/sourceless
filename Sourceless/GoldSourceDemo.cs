using System.IO;
using BinarySerialization;

namespace Sourceless
{
    public class GoldSourceDemo
    {
        public GoldSourceDemo(DemoHeader header)
        {
            Header = header;
        }

        public DemoHeader Header { get; private set; }

        public static GoldSourceDemo FromFile(string filePath)
        {
            var serializer = new BinarySerializer();
            var stream = new FileStream(filePath, FileMode.Open);
            var demoHeader = serializer.Deserialize<DemoHeader>(stream);

            return new GoldSourceDemo(demoHeader);
        }
    }
}