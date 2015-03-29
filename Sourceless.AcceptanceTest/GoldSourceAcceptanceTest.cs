using System.Collections.Generic;
using Sourceless.GoldSource.Demo;
using Sourceless.GoldSource.Demo.Message;
using Xunit;

namespace Sourceless.AcceptanceTest
{
    public class GoldSourceAcceptanceTest
    {
        [Fact]
        public void ReadHL1005Demo()
        {
            var demo = GoldSourceDemo.FromFile("Demos\\HL1005.dem");

            AssertDemoHeaderIsReadCorrectly(demo);
            AssertSegmentDirectoryIsReadCorrectly(demo);
            AssertSegmentsAreReadCorrectly(demo);

            var networkPacketMessages = new List<GoldSourceDemo.NetworkPacketEventArgs>();
            demo.OnNetworkPacketMessage += (sender, msg) => { networkPacketMessages.Add(msg); };

            var segmentEndMessages = new List<GoldSourceDemo.SegmentEndEventArgs>();
            demo.OnSegmentEndMessage += (sender, msg) => { segmentEndMessages.Add(msg); };

            var syncTickMessages = new List<GoldSourceDemo.SyncTickEventArgs>();
            demo.OnSyncTickMessage += (sender, msg) => { syncTickMessages.Add(msg); };

            var clientDataMessages = new List<GoldSourceDemo.ClientDataEventArgs>();
            demo.OnClientDataMessage += (sender, msg) => { clientDataMessages.Add(msg); };

            var sequenceInfoMessages = new List<GoldSourceDemo.SequenceInfoEventArgs>();
            demo.OnSequenceInfoMessage += (sender, msg) => { sequenceInfoMessages.Add(msg); };

            var frameCompleteMessages = new List<GoldSourceDemo.FrameCompleteEventArgs>();
            demo.OnFrameCompleteMessage += (sender, msg) => { frameCompleteMessages.Add(msg); };

            var clientCommandMessages = new List<GoldSourceDemo.ClientCommandEventArgs>();
            demo.OnClientCommandMessage += (sender, msg) => { clientCommandMessages.Add(msg); };

            demo.Read();

            AssertNetworkPacketMessagesAreReadCorrectly(networkPacketMessages);
            AssertSegmentEndMessagesAreReadCorrectly(segmentEndMessages);
            AssertSyncTickMessagesAreReadCorrectly(syncTickMessages);
            AssertClientDataMessagesAreReadCorrectly(clientDataMessages);
            AssertSequenceInfoMessagesAreReadCorrectly(sequenceInfoMessages);
            AssertFrameCompleteMessagesAreReadCorrectly(frameCompleteMessages);
            AssertClientCommandMessagesAreReadCorrectly(clientCommandMessages);
        }

        private static void AssertDemoHeaderIsReadCorrectly(GoldSourceDemo demo)
        {
            var header = demo.Header;
            Assert.Equal("HLDEMO", header.Magic);
            Assert.Equal(4, header.DemoProtocolVersion);
            Assert.Equal(35, header.ServerProtocolVersion);
            Assert.Equal("boot_camp", header.MapName);
            Assert.Equal("valve", header.GameDllName);
            Assert.Equal(0x84369235, header.MapCrc);
            Assert.Equal(0x87DFA, header.SegmentDirectoryOffset);
        }

        private static void AssertSegmentDirectoryIsReadCorrectly(GoldSourceDemo demo)
        {
            var segmentDirEntries = demo.SegmentDirectoryEntries;
            Assert.Equal(2, segmentDirEntries.Length);

            Assert.Equal(0, segmentDirEntries[0].Number);
            Assert.Equal("LOADING", segmentDirEntries[0].Title);
            Assert.Equal(0, segmentDirEntries[0].Flags);
            Assert.Equal(-1, segmentDirEntries[0].CdTrack);
            Assert.Equal(0.0, segmentDirEntries[0].Time);
            Assert.Equal(0, segmentDirEntries[0].Frames);
            Assert.Equal(0x118, segmentDirEntries[0].Offset);
            Assert.Equal(0x60EC, segmentDirEntries[0].Length);

            Assert.Equal(1, segmentDirEntries[1].Number);
            Assert.Equal("Playback", segmentDirEntries[1].Title);
            Assert.Equal(0, segmentDirEntries[1].Flags);
            Assert.Equal(-1, segmentDirEntries[1].CdTrack);
            Assert.InRange(segmentDirEntries[1].Time, 25.464447, 25.464448);
            Assert.Equal(1317, segmentDirEntries[1].Frames);
            Assert.Equal(0x6204, segmentDirEntries[1].Offset);
            Assert.Equal(0x81BF6, segmentDirEntries[1].Length);
        }

        private static void AssertSegmentsAreReadCorrectly(GoldSourceDemo demo)
        {
            Assert.Equal(2, demo.Segments.Count);
            Assert.Equal(0x60EC, demo.Segments[0].Length);
            Assert.Equal(0x81BF6, demo.Segments[1].Length);
        }

        private static void AssertNetworkPacketMessagesAreReadCorrectly(
            List<GoldSourceDemo.NetworkPacketEventArgs> networkPacketMessages)
        {
            Assert.Equal(1325, networkPacketMessages.Count);

            Assert.Equal(DemoMessage.NetworkPacket0, networkPacketMessages[0].Header.Type);
            Assert.InRange(networkPacketMessages[0].Header.Time, 0.309847, 0.309848);
            Assert.Equal(6, networkPacketMessages[0].Header.Frame);
            Assert.Equal(0, networkPacketMessages[0].Message.ViewAngleX);
            Assert.Equal(0, networkPacketMessages[0].Message.OriginX);
            Assert.Equal(0, networkPacketMessages[0].Message.ViewAngleY);
            Assert.Equal(0, networkPacketMessages[0].Message.OriginY);
            Assert.Equal(0, networkPacketMessages[0].Message.ViewAngleZ);
            Assert.Equal(0, networkPacketMessages[0].Message.OriginZ);
            Assert.Equal(175, networkPacketMessages[0].Message.Length);
            Assert.Equal(175, networkPacketMessages[0].Message.Data.Length);
            Assert.Equal(0x03, networkPacketMessages[0].Message.Data[0]);
            Assert.Equal(0x00, networkPacketMessages[0].Message.Data[1]);
            Assert.Equal(0x00, networkPacketMessages[0].Message.Data[2]);
            Assert.Equal(0x80, networkPacketMessages[0].Message.Data[3]);

            // Special packet with id = 1 (don't know exactly what it means yet)
            Assert.Equal(DemoMessage.NetworkPacket1, networkPacketMessages[8].Header.Type);
            Assert.InRange(networkPacketMessages[8].Header.Time, -0.000001350, -0.000001349);
            Assert.Equal(0, networkPacketMessages[8].Header.Frame);
            Assert.InRange(networkPacketMessages[8].Message.ViewAngleX, -31.81200, -31.81199);
            Assert.Equal(843.125, networkPacketMessages[8].Message.OriginX);
            Assert.InRange(networkPacketMessages[8].Message.ViewAngleY, 186.86781, 186.86782);
            Assert.Equal(-16, networkPacketMessages[8].Message.OriginY);
            Assert.Equal(0, networkPacketMessages[8].Message.ViewAngleZ);
            Assert.Equal(-29.5, networkPacketMessages[8].Message.OriginZ);
            Assert.Equal(704, networkPacketMessages[8].Message.Length);
            Assert.Equal(704, networkPacketMessages[8].Message.Data.Length);
            Assert.Equal(0xDB, networkPacketMessages[8].Message.Data[0]);
            Assert.Equal(0x16, networkPacketMessages[8].Message.Data[1]);
            Assert.Equal(0x00, networkPacketMessages[8].Message.Data[2]);
            Assert.Equal(0x80, networkPacketMessages[8].Message.Data[3]);
        }

        private static void AssertSegmentEndMessagesAreReadCorrectly(
            List<GoldSourceDemo.SegmentEndEventArgs> segmentEndMessages)
        {
            Assert.Equal(2, segmentEndMessages.Count);

            Assert.Equal(DemoMessage.SegmentEnd, segmentEndMessages[0].Header.Type);
            Assert.InRange(segmentEndMessages[0].Header.Time, 81.791069, 81.791070);
            Assert.Equal(5854, segmentEndMessages[0].Header.Frame);
        }

        private static void AssertSyncTickMessagesAreReadCorrectly(
            List<GoldSourceDemo.SyncTickEventArgs> syncTickMessages)
        {
            Assert.Equal(1, syncTickMessages.Count);

            Assert.Equal(DemoMessage.SyncTick, syncTickMessages[0].Header.Type);
            Assert.InRange(syncTickMessages[0].Header.Time, -0.000001350, -0.000001349);
            Assert.Equal(0, syncTickMessages[0].Header.Frame);
        }

        private static void AssertClientDataMessagesAreReadCorrectly(
            List<GoldSourceDemo.ClientDataEventArgs> clientDataMessages)
        {
            Assert.Equal(1317, clientDataMessages.Count);

            Assert.Equal(DemoMessage.ClientData, clientDataMessages[0].Header.Type);
            Assert.InRange(clientDataMessages[0].Header.Time, -0.000001350, -0.000001349);
            Assert.Equal(0, clientDataMessages[0].Header.Frame);
            Assert.Equal(843.125, clientDataMessages[0].Message.Origin.X);
            Assert.Equal(-16, clientDataMessages[0].Message.Origin.Y);
            Assert.Equal(-29.5, clientDataMessages[0].Message.Origin.Z);
            Assert.Equal(28, clientDataMessages[0].Message.ViewHeight);
            Assert.Equal(0, clientDataMessages[0].Message.MaxSpeed);
            Assert.InRange(clientDataMessages[0].Message.ViewAngles.X, 8.514001, 8.514002);
            Assert.InRange(clientDataMessages[0].Message.ViewAngles.Y, 106.215820, 106.215821);
            Assert.Equal(0, clientDataMessages[0].Message.ViewAngles.Z);
            Assert.Equal(0, clientDataMessages[0].Message.PunchAngles.X);
            Assert.Equal(0, clientDataMessages[0].Message.PunchAngles.Y);
            Assert.Equal(0, clientDataMessages[0].Message.PunchAngles.Z);
            Assert.Equal(0, clientDataMessages[0].Message.KeyBits);
            Assert.Equal(-2147483626, clientDataMessages[0].Message.WeaponBits);
            Assert.Equal(90, clientDataMessages[0].Message.Fov);
        }

        private static void AssertSequenceInfoMessagesAreReadCorrectly(
            List<GoldSourceDemo.SequenceInfoEventArgs> sequenceInfoMessages)
        {
            Assert.Equal(1317, sequenceInfoMessages.Count);

            Assert.Equal(DemoMessage.SequenceInfo, sequenceInfoMessages[0].Header.Type);
            Assert.InRange(sequenceInfoMessages[0].Header.Time, -0.000001350, -0.000001349);
            Assert.Equal(0, sequenceInfoMessages[0].Header.Frame);
            Assert.Equal(5851, sequenceInfoMessages[0].Message.IncomingSequence);
            Assert.Equal(5851, sequenceInfoMessages[0].Message.IncomingAcknowledged);
            Assert.Equal(0, sequenceInfoMessages[0].Message.IncomingReliableAcknowledged);
            Assert.Equal(1, sequenceInfoMessages[0].Message.IncomingReliableSequence);
            Assert.Equal(5852, sequenceInfoMessages[0].Message.OutgoingSequence);
            Assert.Equal(0, sequenceInfoMessages[0].Message.ReliableSequence);
            Assert.Equal(5324, sequenceInfoMessages[0].Message.LastReliableSequence);
        }

        private static void AssertFrameCompleteMessagesAreReadCorrectly(
            List<GoldSourceDemo.FrameCompleteEventArgs> frameCompleteMessages)
        {
            Assert.Equal(1317, frameCompleteMessages.Count);

            Assert.Equal(DemoMessage.FrameComplete, frameCompleteMessages[0].Header.Type);
            Assert.InRange(frameCompleteMessages[0].Header.Time, -0.000001350, -0.000001349);
            Assert.Equal(0, frameCompleteMessages[0].Header.Frame);
        }

        private static void AssertClientCommandMessagesAreReadCorrectly(
            List<GoldSourceDemo.ClientCommandEventArgs> clientCommandMessages)
        {
            Assert.Equal(2, clientCommandMessages.Count);

            Assert.Equal(DemoMessage.ClientCommand, clientCommandMessages[0].Header.Type);
            Assert.InRange(clientCommandMessages[0].Header.Time, 6.690042, 6.690043);
            Assert.Equal(1, clientCommandMessages[0].Header.Frame);
            Assert.Equal("-showscores", clientCommandMessages[0].Message.Command);
        }
    }
}