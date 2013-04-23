

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class ChannelData
    {
        public double Interval { get; set; }
        public double XOffset { get; set; }
        public List<float> Data { get; set; }
    }
}
