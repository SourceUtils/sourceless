using BinarySerialization;

namespace Sourceless.Common
{
    public class Vector
    {
        [FieldOrder(0)]
        public float X { get; private set; }

        [FieldOrder(1)]
        public float Y { get; private set; }

        [FieldOrder(2)]
        public float Z { get; private set; }
    }
}