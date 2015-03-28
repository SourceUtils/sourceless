using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    public class DemoMessageHeader
    {
        [FieldOrder(0)]
        public byte Type { get; private set; }

        [FieldOrder(1)]
        public float Time { get; private set; }

        [FieldOrder(2)]
        public int Frame { get; private set; }
    }
}