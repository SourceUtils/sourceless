using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    public class DemoMessage
    {
        [FieldOrder(0)]
        public DemoMessageHeader Header { get; private set; }
    }
}