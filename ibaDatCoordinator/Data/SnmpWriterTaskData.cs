using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Data
{
    public class SnmpWriterTaskData : ComputedValuesTaskData
    {
        public SnmpWriterTaskData(ConfigurationData parent) : base(parent) { }
        public SnmpWriterTaskData() : this(null) { }
    }
}
