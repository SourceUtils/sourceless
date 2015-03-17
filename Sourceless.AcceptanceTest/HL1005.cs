using Xunit;

namespace Sourceless.AcceptanceTest
{
    public class HL1005
    {
        [Fact]
        public void ReadsDemoHeader()
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
        }

        [Fact]
        public void ReadsDemoSegments()
        {
            var demo = GoldSourceDemo.FromFile("Demos\\HL1005.dem");
            var segments = demo.Segments;

            Assert.Equal(2, segments.Length);

            Assert.Equal(0, segments[0].Number);
            Assert.Equal("LOADING", segments[0].Title);
            Assert.Equal(0, segments[0].Flags);
            Assert.Equal(-1, segments[0].CdTrack);
            Assert.Equal(0.0, segments[0].Time);
            Assert.Equal(0, segments[0].Frames);
            Assert.Equal(0x118, segments[0].Offset);
            Assert.Equal(0x60EC, segments[0].Length);

            Assert.Equal(1, segments[1].Number);
            Assert.Equal("Playback", segments[1].Title);
            Assert.Equal(0, segments[1].Flags);
            Assert.Equal(-1, segments[1].CdTrack);
            Assert.InRange(segments[1].Time, 11.378293, 11.378294);
            Assert.Equal(814, segments[1].Frames);
            Assert.Equal(0x6204, segments[1].Offset);
            Assert.Equal(0x47E03, segments[1].Length);
        }
    }
}