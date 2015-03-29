using BinarySerialization;
using Sourceless.Common;

namespace Sourceless.GoldSource.Demo.Message
{
    public class ClientDataDemoMessage : BinaryEntity<ClientDataDemoMessage>
    {
        [FieldOrder(0)]
        public Vector Origin { get; private set; }

        [FieldOrder(1)]
        public float ViewHeight { get; private set; }
        
        [FieldOrder(2)]
        public float MaxSpeed { get; private set; }

        [FieldOrder(3)]
        public Vector ViewAngles { get; private set; }
        
        [FieldOrder(4)]
        public Vector PunchAngles { get; private set; }

        [FieldOrder(5)]
        public int KeyBits { get; private set; }

        [FieldOrder(6)]
        public int WeaponBits { get; private set; }

        [FieldOrder(7)]
        public float Fov { get; private set; }
    }
}