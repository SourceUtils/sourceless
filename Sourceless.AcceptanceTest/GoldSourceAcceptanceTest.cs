using System;
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
            Assert.Equal(0x4E007, header.SegmentDirectoryOffset);

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
            Assert.InRange(segmentDirEntries[1].Time, 11.378293, 11.378294);
            Assert.Equal(814, segmentDirEntries[1].Frames);
            Assert.Equal(0x6204, segmentDirEntries[1].Offset);
            Assert.Equal(0x47E03, segmentDirEntries[1].Length);

            Assert.Equal(2, demo.Segments.Count);
            Assert.Equal(0x60EC, demo.Segments[0].Length);
            Assert.Equal(0x47E03, demo.Segments[1].Length);

            var networkPacketCount = 0;
            demo.OnNetworkPacketMessage += (sender, msg) =>
            {
                networkPacketCount++;

                if (networkPacketCount == 1)
                {
                    Assert.Equal(DemoMessage.NetworkPacket, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 0.360358, 0.360359);
                    Assert.Equal(7, msg.Header.Frame);
                    Assert.Equal(0, msg.Message.Unk1);
                    Assert.Equal(0, msg.Message.Unk2);
                    Assert.Equal(0, msg.Message.Unk3);
                    Assert.Equal(0, msg.Message.Unk4);
                    Assert.Equal(0, msg.Message.Unk5);
                    Assert.Equal(0, msg.Message.Unk6);
                    Assert.Equal(175, msg.Message.Length);
                    Assert.Equal(175, msg.Message.Data.Length);
                    Assert.Equal(0x03, msg.Message.Data[0]);
                    Assert.Equal(0x00, msg.Message.Data[1]);
                    Assert.Equal(0x00, msg.Message.Data[2]);
                    Assert.Equal(0x80, msg.Message.Data[3]);
                }
                else if (networkPacketCount == 2)
                {
                    Assert.Equal(DemoMessage.NetworkPacket, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, 0.374247, 0.374248);
                    Assert.Equal(8, msg.Header.Frame);
                    Assert.Equal(0, msg.Message.Unk1);
                    Assert.Equal(0, msg.Message.Unk2);
                    Assert.Equal(0, msg.Message.Unk3);
                    Assert.Equal(0, msg.Message.Unk4);
                    Assert.Equal(0, msg.Message.Unk5);
                    Assert.Equal(0, msg.Message.Unk6);
                    Assert.Equal(7046, msg.Message.Length);
                    Assert.Equal(7046, msg.Message.Data.Length);
                    Assert.Equal(0x04, msg.Message.Data[0]);
                    Assert.Equal(0x00, msg.Message.Data[1]);
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
                    Assert.InRange(msg.Header.Time, 12.879183, 12.879184);
                    Assert.Equal(894, msg.Header.Frame);
                }
            };

            var syncTickCount = 0;
            demo.OnSyncTickMessage += (sender, msg) =>
            {
                syncTickCount++;

                if (syncTickCount == 1)
                {
                    Assert.Equal(DemoMessage.SyncTick, msg.Header.Type);
                    Assert.InRange(msg.Header.Time, -0.000000282, -0.000000281);
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
                    Assert.InRange(msg.Header.Time, -0.000000282, -0.000000281);
                    Assert.Equal(0, msg.Header.Frame);

                    Assert.Equal(1777.375, msg.Message.Origin.X);
                    Assert.Equal(428.25, msg.Message.Origin.Y);
                    Assert.Equal(36, msg.Message.Origin.Z);
                    Assert.Equal(28, msg.Message.ViewHeight);
                    Assert.Equal(0, msg.Message.MaxSpeed);
                    Assert.InRange(msg.Message.ViewAngles.X, -13.13123, -13.13122);
                    Assert.InRange(msg.Message.ViewAngles.Y, 154.8907, 154.8908);
                    Assert.Equal(0, msg.Message.ViewAngles.Z);
                    Assert.Equal(0, msg.Message.PunchAngles.X);
                    Assert.Equal(0, msg.Message.PunchAngles.Y);
                    Assert.Equal(0, msg.Message.PunchAngles.Z);
                    Assert.Equal(0, msg.Message.KeyBits);
                    Assert.Equal(-2147483626, msg.Message.WeaponBits);
                    Assert.Equal(90, msg.Message.Fov);
                }
            };

            demo.Read();
            Assert.True(networkPacketCount >= 2);
            Assert.True(segmentEndCount >= 1);
            Assert.True(syncTickCount >= 1);
            Assert.True(clientDataCount >= 1);
        }
    }
}