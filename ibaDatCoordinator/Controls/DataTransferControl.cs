using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class DataTransferControl : UserControl, IPropertyPane
    {
        public DataTransferControl()
        {
            InitializeComponent();
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            //throw new NotImplementedException();
        }

        public void LeaveCleanup()
        {
            //throw new NotImplementedException();
        }

        public void SaveData()
        {
            //throw new NotImplementedException();
        }
    }
}
