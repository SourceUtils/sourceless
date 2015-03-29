using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    /// <summary>
    ///     A packet stored directly off the network stream.
    /// </summary>
    public class NetworkPacketDemoMessage : BinaryEntity<NetworkPacketDemoMessage>
    {
        [FieldOrder(0)]
        public float ViewAngleX { get; private set; }

        [FieldOrder(1)]
        public float OriginX { get; private set; }

        [FieldOrder(2)]
        public float ViewAngleY { get; private set; }

        [FieldOrder(3)]
        public float OriginY { get; private set; }

        [FieldOrder(4)]
        public float ViewAngleZ { get; private set; }

        [FieldOrder(5)]
        public float OriginZ { get; private set; }

        [FieldOrder(6)]
        public int Length { get; private set; }

        [FieldOrder(7)]
        [FieldLength("Length")]
        public byte[] Data { get; private set; }
    }
}