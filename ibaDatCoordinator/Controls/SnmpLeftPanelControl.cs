using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Processing;

namespace iba.Controls
{
    public partial class SnmpLeftPanelControl : UserControl
    {
        public SnmpLeftPanelControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
        }

        private readonly MainForm _mainForm;

        private void buttonShowDebug_Click(object sender, EventArgs e)
        {
        }
    }
}
