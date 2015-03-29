using System;
using Sourceless.GoldSource.Demo.Message;

namespace Sourceless.GoldSource.Demo
{
    public class DemoMessageEventArgs<T> : EventArgs
    {
        public DemoMessageHeader Header { get; set; }
        public T Body { get; set; }
    }
}