using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    public class ClientCommandDemoMessage : BinaryEntity<ClientCommandDemoMessage>
    {
        [FieldOrder(0)]
        [FieldLength(64)]
        public string Command { get; private set; }
    }
}