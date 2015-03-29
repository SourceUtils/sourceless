using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinarySerialization;
using Sourceless.GoldSource.Demo.Message;

namespace Sourceless.GoldSource.Demo
{
    public class GoldSourceDemo
    {
        public delegate void NetworkPacketMessageHandler(object sender, NetworkPacketEventArgs e);

        public GoldSourceDemo(DemoHeader header, SegmentDirectoryEntry[] segmentDirEntries, List<byte[]> segments)
        {
            Header = header;
            SegmentDirectoryEntries = segmentDirEntries;
            Segments = segments;
        }

        public DemoHeader Header { get; private set; }
        public SegmentDirectoryEntry[] SegmentDirectoryEntries { get; set; }
        public List<byte[]> Segments { get; private set; }
        public event NetworkPacketMessageHandler OnNetworkPacketMessage;

        public static GoldSourceDemo FromFile(string filePath)
        {
            var serializer = new BinarySerializer();
            var demoStream = new FileStream(filePath, FileMode.Open);

            var demoHeader = serializer.Deserialize<DemoHeader>(demoStream);

            demoStream.Seek(demoHeader.SegmentDirectoryOffset, SeekOrigin.Begin);
            var segmentDir = serializer.Deserialize<SegmentDirectory>(demoStream);

            var segments = segmentDir.Entries.Select(dir => ReadSegment(dir.Offset, dir.Length, demoStream)).ToList();

            demoStream.Close();
            return new GoldSourceDemo(demoHeader, segmentDir.Entries, segments);
        }

        private static byte[] ReadSegment(int offset, int length, Stream source)
        {
            var segment = new byte[length];
            source.Seek(offset, SeekOrigin.Begin);
            source.Read(segment, 0, length);
            return segment;
        }

        public void Read()
        {
            var segmentStream = new MemoryStream(Segments[0]);
            var serializer = new BinarySerializer();

            while (true)
            {
                var messageHeader = serializer.Deserialize<DemoMessageHeader>(segmentStream);

                if (messageHeader.Type == DemoMessage.NetworkPacket)
                {
                    var demoMessage = serializer.Deserialize<NetworkPacketDemoMessage>(segmentStream);

                    var eventArgs = new NetworkPacketEventArgs
                    {
                        Header = messageHeader,
                        Message = demoMessage
                    };
                    OnNetworkPacketMessage.Invoke(this, eventArgs);
                }
                else
                {
                    segmentStream.Close();
                    break;
                }
            }
        }

        public class NetworkPacketEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
            public NetworkPacketDemoMessage Message { get; set; }
        }
    }
}