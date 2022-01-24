using System;

namespace iba.Data
{
    [Serializable]
    public class SnmpWriterTaskData : ComputedValuesTaskData
    {
        public SnmpWriterTaskData(ConfigurationData parent) : base(parent) { }
        public SnmpWriterTaskData() : this(null) { }
    }
}
