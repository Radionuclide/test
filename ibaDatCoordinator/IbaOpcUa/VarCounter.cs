using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ibaOpcServer.IbaOpcUa
{
    public class VarCounter
    {
        public VarCounter()
            : this(0) {}
        public VarCounter(int initialValue)
        {
            Count = initialValue;
        }

        public void AlterBy(int delta)
        {
            Count += delta;
        }
        public void Increment()
        {
            Count++;
        }

        public void Decrement()
        {
            Count--;
        }

        public void Reset()
        {
            Count = 0;
        }

        public int Count { get; private set; }

        public override string ToString()
        {
            return Count.ToString();
        }
    }
}
