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
        public delegate void SyncTickMessageHandler(object sender, SyncTickEventArgs e);
        public delegate void SequenceInfoMessageHandler(object sender, SequenceInfoEventArgs e);
        public delegate void FrameCompleteMessageHandler(object sender, FrameCompleteEventArgs e);
        public delegate void ClientDataMessageHandler(object sender, ClientDataEventArgs e);
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
        public event SyncTickMessageHandler OnSyncTickMessage;
        public event SequenceInfoMessageHandler OnSequenceInfoMessage;
        public event FrameCompleteMessageHandler OnFrameCompleteMessage;
        public event ClientDataMessageHandler OnClientDataMessage;
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
            var args = new NetworkPacketEventArgs
            {
                Header = messageHeader,
                Message = demoMessage
            };
            OnNetworkPacketMessage.Invoke(this, args);
        }

        private void ProcessSyncTickMessage(DemoMessageHeader messageHeader)
        {
            var args = new SyncTickEventArgs
            {
                Header = messageHeader
            };
            OnSyncTickMessage.Invoke(this, args);
        }

        private void ProcessSequenceInfoMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = SequenceInfoDemoMessage.Read(segmentStream);
            var args = new SequenceInfoEventArgs
            {
                Header = messageHeader,
                Message = demoMessage
            };
            OnSequenceInfoMessage.Invoke(this, args);
        }

        private void ProcessFrameCompleteMessage(DemoMessageHeader messageHeader)
        {
            var args = new FrameCompleteEventArgs
            {
                Header = messageHeader
            };
            OnFrameCompleteMessage.Invoke(this, args);
        }
        
        private void ProcessClientDataMessage(Stream segmentStream, DemoMessageHeader messageHeader)
        {
            var demoMessage = ClientDataDemoMessage.Read(segmentStream);
            var args = new ClientDataEventArgs
            {
                Header = messageHeader,
                Message = demoMessage
            };
            OnClientDataMessage.Invoke(this, args);
        }

        private void ProcessSegmentEndMessage(DemoMessageHeader messageHeader)
        {
            var args = new SegmentEndEventArgs
            {
                Header = messageHeader
            };
            OnSegmentEndMessage.Invoke(this, args);
        }

        public class NetworkPacketEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
            public NetworkPacketDemoMessage Message { get; set; }
        }

        public class SyncTickEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
        }
        
        public class SequenceInfoEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
            public SequenceInfoDemoMessage Message { get; set; }
        }
        
        public class FrameCompleteEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
        } 
        
        public class ClientDataEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
            public ClientDataDemoMessage Message { get; set; }
        }

        public class SegmentEndEventArgs : EventArgs
        {
            public DemoMessageHeader Header { get; set; }
        }
    }
}