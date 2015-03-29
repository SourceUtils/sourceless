using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sourceless.GoldSource.Demo.Message;

namespace Sourceless.GoldSource.Demo
{
    public class GoldSourceDemo
    {
        public delegate void NetworkPacketMessageHandler(object sender, NetworkPacketEventArgs e);
        public delegate void SegmentEndMessageHandler(object sender, SegmentEndEventArgs e);

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
        public event SegmentEndMessageHandler OnSegmentEndMessage;

        public static GoldSourceDemo FromFile(string filePath)
        {
            var demoStream = new FileStream(filePath, FileMode.Open);

            var demoHeader = DemoHeader.Read(demoStream);
            demoStream.Seek(demoHeader.SegmentDirectoryOffset, SeekOrigin.Begin);
            var segmentDir = SegmentDirectory.Read(demoStream);
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
            var curSegmentNr = 0;
            var curSegment = new MemoryStream(Segments[curSegmentNr]);
            while (true)
            {
                var messageHeader = DemoMessageHeader.Read(curSegment);
                switch (messageHeader.Type)
                {
                    case DemoMessage.NetworkPacket:
                        ProcessNetworkPacketMessage(curSegment, messageHeader);
                        break;
                    case DemoMessage.SegmentEnd:
                        ProcessSegmentEndMessage(messageHeader);
                        curSegment.Close();

                        if (++curSegmentNr == Segments.Count)
                            return;

                        curSegment = new MemoryStream(Segments[curSegmentNr]);
                        break;
                    default:
                        curSegment.Close();
                        return;
                }
            }
        }

        private void ProcessNetworkPacketMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = NetworkPacketDemoMessage.Read(segmentStream);

            var packetArgs = new NetworkPacketEventArgs
            {
                Header = messageHeader,
                Message = demoMessage
            };
            OnNetworkPacketMessage.Invoke(this, packetArgs);
        }

        private void ProcessSegmentEndMessage(DemoMessageHeader messageHeader)
        {
            var endArgs = new SegmentEndEventArgs
            {
                Header = messageHeader
            };
            OnSegmentEndMessage.Invoke(this, endArgs);
        }

        public class NetworkPacketEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
            public NetworkPacketDemoMessage Message { get; set; }
        }

        public class SegmentEndEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
        }
    }
}