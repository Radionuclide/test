using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using iba.Utility;
using iba.Processing;
using iba.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;

namespace iba.Utility
{
    internal class TaskControl
    {
        public Task Task { get; set; }
        public CancellationTokenSource Cts { get; set; }

        public TaskControl(Task task, CancellationTokenSource cts)
        {
            Task = task;
            Cts = cts;
        }
    }
}
