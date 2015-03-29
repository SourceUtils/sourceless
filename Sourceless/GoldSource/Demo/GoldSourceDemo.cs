using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sourceless.GoldSource.Demo.Message;

namespace Sourceless.GoldSource.Demo
{
    public class GoldSourceDemo
    {
        public GoldSourceDemo(DemoHeader header, SegmentDirectoryEntry[] segmentDirEntries, List<byte[]> segments)
        {
            Header = header;
            SegmentDirectoryEntries = segmentDirEntries;
            Segments = segments;
        }

        public DemoHeader Header { get; private set; }
        public SegmentDirectoryEntry[] SegmentDirectoryEntries { get; set; }
        public List<byte[]> Segments { get; private set; }
        public event EventHandler<DemoMessageEventArgs<NetworkPacketDemoMessage>> OnNetworkPacketMessage;
        public event EventHandler<EmptyDemoMessageEventArgs> OnSyncTickMessage;
        public event EventHandler<DemoMessageEventArgs<SequenceInfoDemoMessage>> OnSequenceInfoMessage;
        public event EventHandler<EmptyDemoMessageEventArgs> OnFrameCompleteMessage;
        public event EventHandler<DemoMessageEventArgs<ClientCommandDemoMessage>> OnClientCommandMessage;
        public event EventHandler<DemoMessageEventArgs<ClientDataDemoMessage>> OnClientDataMessage;
        public event EventHandler<EmptyDemoMessageEventArgs> OnSegmentEndMessage;

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
                    case DemoMessage.NetworkPacket0:
                    case DemoMessage.NetworkPacket1:
                        ProcessNetworkPacketMessage(curSegment, messageHeader);
                        break;
                    case DemoMessage.SyncTick:
                        ProcessSyncTickMessage(messageHeader);
                        break;
                    case DemoMessage.SequenceInfo:
                        ProcessSequenceInfoMessage(curSegment, messageHeader);
                        break;
                    case DemoMessage.FrameComplete:
                        ProcessFrameCompleteMessage(messageHeader);
                        break;
                    case DemoMessage.ClientCommand:
                        ProcessClientCommandMessage(curSegment, messageHeader);
                        break;
                    case DemoMessage.ClientData:
                        ProcessClientDataMessage(curSegment, messageHeader);
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
            var args = new DemoMessageEventArgs<NetworkPacketDemoMessage>
            {
                Header = messageHeader,
                Body = demoMessage
            };
            OnNetworkPacketMessage.Invoke(this, args);
        }

        private void ProcessSyncTickMessage(DemoMessageHeader messageHeader)
        {
            var args = new EmptyDemoMessageEventArgs
            {
                Header = messageHeader
            };
            OnSyncTickMessage.Invoke(this, args);
        }

        private void ProcessSequenceInfoMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = SequenceInfoDemoMessage.Read(segmentStream);
            var args = new DemoMessageEventArgs<SequenceInfoDemoMessage>
            {
                Header = messageHeader,
                Body = demoMessage
            };
            OnSequenceInfoMessage.Invoke(this, args);
        }

        private void ProcessFrameCompleteMessage(DemoMessageHeader messageHeader)
        {
            var args = new EmptyDemoMessageEventArgs
            {
                Header = messageHeader
            };
            OnFrameCompleteMessage.Invoke(this, args);
        }

        private void ProcessClientCommandMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = ClientCommandDemoMessage.Read(segmentStream);
            var args = new DemoMessageEventArgs<ClientCommandDemoMessage>
            {
                Header = messageHeader,
                Body = demoMessage
            };
            OnClientCommandMessage.Invoke(this, args);
        }

        private void ProcessClientDataMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = ClientDataDemoMessage.Read(segmentStream);
            var args = new DemoMessageEventArgs<ClientDataDemoMessage>
            {
                Header = messageHeader,
                Body = demoMessage
            };
            OnClientDataMessage.Invoke(this, args);
        }

        private void ProcessSegmentEndMessage(DemoMessageHeader messageHeader)
        {
            var args = new EmptyDemoMessageEventArgs
            {
                Header = messageHeader
            };
            OnSegmentEndMessage.Invoke(this, args);
        }
    }
}