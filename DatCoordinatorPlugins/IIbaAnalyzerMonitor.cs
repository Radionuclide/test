using System;

namespace iba.Processing
{
    public enum MonitorStatus { OK, OUT_OF_MEMORY, OUT_OF_TIME };
    public delegate void IbaAnalyzerCall();

    public interface IIbaAnalyzerMonitor : IDisposable
    {
        void Execute(IbaAnalyzerCall ibaAnalyzerCall);
        MonitorStatus Status { get; }
    }
}
