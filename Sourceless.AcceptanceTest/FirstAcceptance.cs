﻿using Xunit;

namespace Sourceless.AcceptanceTest
{
    public class FirstAcceptance
    {
        [Fact]
        public void ReadDemoHeader()
        {
            var demo = GoldSourceDemo.FromFile("Demos\\HL1005.dem");
            var header = demo.Header;

            Assert.Equal("HLDEMO", header.Magic);
            Assert.Equal(4, header.DemoProtocolVersion);
            Assert.Equal(35, header.ServerProtocolVersion);
            Assert.Equal("boot_camp", header.MapName);
            Assert.Equal("valve", header.GameDllName);
            Assert.Equal(0x84369235, header.MapCrc);
            Assert.Equal(0x4E007, header.DirectoryOffset);
        }
    }
}