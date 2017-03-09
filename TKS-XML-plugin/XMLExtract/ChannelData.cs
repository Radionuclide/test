

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class ChannelData
    {
        public float Interval { get; set; }
        public float XOffset { get; set; }
        public List<float> Data { get; set; }
    }
}
