using System.IO;
using BinarySerialization;

namespace Sourceless.GoldSource
{
    public class BinaryEntity<T>
    {
        public static T Read(Stream stream)
        {
            var serializer = new BinarySerializer();
            return serializer.Deserialize<T>(stream);
        }
    }
}