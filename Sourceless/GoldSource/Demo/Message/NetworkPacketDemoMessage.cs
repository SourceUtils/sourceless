using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    /// <summary>
    ///     A packet stored directly off the network stream.
    /// </summary>
    public class NetworkPacketDemoMessage
    {
        [FieldOrder(0)]
        public float Unk1 { get; private set; }

        [FieldOrder(1)]
        public float Unk2 { get; private set; }

        [FieldOrder(2)]
        public float Unk3 { get; private set; }

        [FieldOrder(3)]
        public float Unk4 { get; private set; }

        [FieldOrder(4)]
        public float Unk5 { get; private set; }

        [FieldOrder(5)]
        public float Unk6 { get; private set; }

        [FieldOrder(6)]
        public int Length { get; private set; }

        [FieldOrder(7)]
        [FieldLength("Length")]
        public byte[] Data { get; private set; }
    }
}