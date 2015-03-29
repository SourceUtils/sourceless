using BinarySerialization;

namespace Sourceless.GoldSource.Demo.Message
{
    public class SequenceInfoDemoMessage : BinaryEntity<SequenceInfoDemoMessage>
    {
        [FieldOrder(0)]
        public int IncomingSequence { get; private set; }

        [FieldOrder(1)]
        public int IncomingAcknowledged { get; private set; }

        [FieldOrder(2)]
        public int IncomingReliableAcknowledged { get; private set; }

        [FieldOrder(3)]
        public int IncomingReliableSequence { get; private set; }

        [FieldOrder(4)]
        public int OutgoingSequence { get; private set; }

        [FieldOrder(5)]
        public int ReliableSequence { get; private set; }

        [FieldOrder(6)]
        public int LastReliableSequence { get; private set; }
    }
}