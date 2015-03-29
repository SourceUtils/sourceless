namespace Sourceless.GoldSource.Demo.Message
{
    public enum DemoMessage : byte
    {
        NetworkPacket0 = 0,
        NetworkPacket1 = 1,
        SyncTick = 2,
        SequenceInfo = 3,
        FrameComplete = 4,
        ClientData = 6,
        SegmentEnd = 8
    }
}