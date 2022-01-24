using System;

namespace iba.Data
{
    [Serializable]
    public class OpcUaWriterTaskData : ComputedValuesTaskData
    {
        public OpcUaWriterTaskData(ConfigurationData parent) : base(parent) { }
        public OpcUaWriterTaskData() : this(null) { }

    }
}
