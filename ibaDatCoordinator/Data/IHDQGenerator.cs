using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Data
{
    interface IHDQGenerator
    {
        string HDServer { get; }
        int HDPort { get; }
        string[] HDStores { get; }

        TimeSpan StartRangeFromTrigger { get; }
        TimeSpan StopRangeFromTrigger { get; }
        bool UsePreviousTriggerAsStart { get; }
        TimeSpan PreferredTimeBase { get; }
        bool PreferredTimeBaseIsAuto { get; }
    }
}
