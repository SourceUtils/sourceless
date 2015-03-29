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

            var header = demo.Header;
            Assert.Equal("HLDEMO", header.Magic);
            Assert.Equal(4, header.DemoProtocolVersion);
            Assert.Equal(35, header.ServerProtocolVersion);
            Assert.Equal("boot_camp", header.MapName);
            Assert.Equal("valve", header.GameDllName);
            Assert.Equal(0x84369235, header.MapCrc);
            Assert.Equal(0x87DFA, header.SegmentDirectoryOffset);

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

            Assert.Equal(2, demo.Segments.Count);
            Assert.Equal(0x60EC, demo.Segments[0].Length);
            Assert.Equal(0x81BF6, demo.Segments[1].Length);

            var networkPacketCount = 0;
            demo.OnNetworkPacketMessage += (sender, msg) =>
            {
                networkPacketCount++;

                if (networkPacketCount == 1)
                {
                    Assert.Equal(DemoMessage.NetworkPacket0, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 0.309847, 0.309848);
                    Assert.Equal(6, msg.Header.Frame);
                    Assert.Equal(0, msg.Message.ViewAngleX);
                    Assert.Equal(0, msg.Message.OriginX);
                    Assert.Equal(0, msg.Message.ViewAngleY);
                    Assert.Equal(0, msg.Message.OriginY);
                    Assert.Equal(0, msg.Message.ViewAngleZ);
                    Assert.Equal(0, msg.Message.OriginZ);
                    Assert.Equal(175, msg.Message.Length);
                    Assert.Equal(175, msg.Message.Data.Length);
                    Assert.Equal(0x03, msg.Message.Data[0]);
                    Assert.Equal(0x00, msg.Message.Data[1]);
                    Assert.Equal(0x00, msg.Message.Data[2]);
                    Assert.Equal(0x80, msg.Message.Data[3]);
                }
                else if (networkPacketCount == 2)
                {
                    Assert.Equal(DemoMessage.NetworkPacket0, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 0.323738, 0.323739);
                    Assert.Equal(7, msg.Header.Frame);
                    Assert.Equal(0, msg.Message.ViewAngleX);
                    Assert.Equal(0, msg.Message.OriginX);
                    Assert.Equal(0, msg.Message.ViewAngleY);
                    Assert.Equal(0, msg.Message.OriginY);
                    Assert.Equal(0, msg.Message.ViewAngleZ);
                    Assert.Equal(0, msg.Message.OriginZ);
                    Assert.Equal(7046, msg.Message.Length);
                    Assert.Equal(7046, msg.Message.Data.Length);
                    Assert.Equal(0x04, msg.Message.Data[0]);
                    Assert.Equal(0x00, msg.Message.Data[1]);
                    Assert.Equal(0x00, msg.Message.Data[2]);
                    Assert.Equal(0x80, msg.Message.Data[3]);
                }
                else if (networkPacketCount == 9) // Special packet with id = 1 (don't know exactly what it means yet)
                {
                    Assert.Equal(DemoMessage.NetworkPacket1, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000001350, -0.000001349);
                    Assert.Equal(0, msg.Header.Frame);
                    Assert.InRange(msg.Message.ViewAngleX, -31.81200, -31.81199);
                    Assert.Equal(843.125, msg.Message.OriginX);
                    Assert.InRange(msg.Message.ViewAngleY, 186.86781, 186.86782);
                    Assert.Equal(-16, msg.Message.OriginY);
                    Assert.Equal(0, msg.Message.ViewAngleZ);
                    Assert.Equal(-29.5, msg.Message.OriginZ);
                    Assert.Equal(704, msg.Message.Length);
                    Assert.Equal(704, msg.Message.Data.Length);
                    Assert.Equal(0xDB, msg.Message.Data[0]);
                    Assert.Equal(0x16, msg.Message.Data[1]);
                    Assert.Equal(0x00, msg.Message.Data[2]);
                    Assert.Equal(0x80, msg.Message.Data[3]);
                }
            };

            var segmentEndCount = 0;
            demo.OnSegmentEndMessage += (sender, msg) =>
            {
                segmentEndCount++;

                if (segmentEndCount == 1)
                {
                    Assert.Equal(DemoMessage.SegmentEnd, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 81.791069, 81.791070);
                    Assert.Equal(5854, msg.Header.Frame);
                }
            };

            var syncTickCount = 0;
            demo.OnSyncTickMessage += (sender, msg) =>
            {
                syncTickCount++;

                if (syncTickCount == 1)
                {
                    Assert.Equal(DemoMessage.SyncTick, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000001350, -0.000001349);
                    Assert.Equal(0, msg.Header.Frame);
                }
            };

            var clientDataCount = 0;
            demo.OnClientDataMessage += (sender, msg) =>
            {
                clientDataCount++;

                if (clientDataCount == 1)
                {
                    Assert.Equal(DemoMessage.ClientData, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000001350, -0.000001349);
                    Assert.Equal(0, msg.Header.Frame);

                    Assert.Equal(843.125, msg.Message.Origin.X);
                    Assert.Equal(-16, msg.Message.Origin.Y);
                    Assert.Equal(-29.5, msg.Message.Origin.Z);
                    Assert.Equal(28, msg.Message.ViewHeight);
                    Assert.Equal(0, msg.Message.MaxSpeed);
                    Assert.InRange(msg.Message.ViewAngles.X, 8.514001, 8.514002);
                    Assert.InRange(msg.Message.ViewAngles.Y, 106.215820, 106.215821);
                    Assert.Equal(0, msg.Message.ViewAngles.Z);
                    Assert.Equal(0, msg.Message.PunchAngles.X);
                    Assert.Equal(0, msg.Message.PunchAngles.Y);
                    Assert.Equal(0, msg.Message.PunchAngles.Z);
                    Assert.Equal(0, msg.Message.KeyBits);
                    Assert.Equal(-2147483626, msg.Message.WeaponBits);
                    Assert.Equal(90, msg.Message.Fov);
                }
            };

            var sequenceInfoCount = 0;
            demo.OnSequenceInfoMessage += (sender, msg) =>
            {
                sequenceInfoCount++;

                if (sequenceInfoCount == 1)
                {
                    Assert.Equal(DemoMessage.SequenceInfo, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000001350, -0.000001349);
                    Assert.Equal(0, msg.Header.Frame);

                    Assert.Equal(5851, msg.Message.IncomingSequence);
                    Assert.Equal(5851, msg.Message.IncomingAcknowledged);
                    Assert.Equal(0, msg.Message.IncomingReliableAcknowledged);
                    Assert.Equal(1, msg.Message.IncomingReliableSequence);
                    Assert.Equal(5852, msg.Message.OutgoingSequence);
                    Assert.Equal(0, msg.Message.ReliableSequence);
                    Assert.Equal(5324, msg.Message.LastReliableSequence);
                }
            };

            var frameCompleteCount = 0;
            demo.OnFrameCompleteMessage += (sender, msg) =>
            {
                frameCompleteCount++;

                if (frameCompleteCount == 1)
                {
                    Assert.Equal(DemoMessage.FrameComplete, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000001350, -0.000001349);
                    Assert.Equal(0, msg.Header.Frame);
                }
            };

            var clientCommandCount = 0;
            demo.OnClientCommandMessage += (sender, msg) =>
            {
                clientCommandCount++;

                if (clientCommandCount == 1)
                {
                    Assert.Equal(DemoMessage.ClientCommand, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 6.690042, 6.690043);
                    Assert.Equal(1, msg.Header.Frame);
                    Assert.Equal("-showscores", msg.Message.Command);
                }
                if (clientCommandCount == 2)
                {
                    Assert.Equal(DemoMessage.ClientCommand, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 7.758872, 7.758873);
                    Assert.Equal(49, msg.Header.Frame);
                    Assert.Equal("-showscores", msg.Message.Command);
                }
            };

            demo.Read();
            Assert.Equal(1325, networkPacketCount);
            Assert.Equal(2, segmentEndCount);
            Assert.Equal(1, syncTickCount);
            Assert.Equal(1317, clientDataCount);
            Assert.Equal(1317, sequenceInfoCount);
            Assert.Equal(1317, frameCompleteCount);
            Assert.Equal(2, clientCommandCount);
        }
    }
}