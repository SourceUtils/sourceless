using Sourceless.GoldSource;
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
        }
    }
}