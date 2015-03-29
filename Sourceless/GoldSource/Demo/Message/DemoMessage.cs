namespace Sourceless.GoldSource.Demo.Message
{
    public enum DemoMessage : byte
    {
        NetworkPacket = 0,
        SyncTick = 2,
        SequenceInfo = 3,
        ClientData = 6,
        SegmentEnd = 8
    }
}