using System;
using Sourceless.GoldSource.Demo.Message;

namespace Sourceless.GoldSource.Demo
{
    public class EmptyDemoMessageEventArgs : EventArgs
    {
        public DemoMessageHeader Header { get; set; }
    }
}