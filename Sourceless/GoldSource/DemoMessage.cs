using BinarySerialization;

namespace Sourceless.GoldSource
{
    public class DemoMessage
    {
        [FieldOrder(0)]
        public DemoMessageHeader Header { get; private set; }
    }
}