using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.DatCoordinator.Status.Dialogs
{
    public partial class ChangeServerPortForm : Form
    {
        public ChangeServerPortForm()
        {
            InitializeComponent();
        }

        public int PortNr
        {
            get { return Decimal.ToInt32(numPortNr.Value); }
            set { numPortNr.Value = Math.Max(numPortNr.Minimum, Math.Min(numPortNr.Maximum, value)); }
        }

        private void OnOK(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
