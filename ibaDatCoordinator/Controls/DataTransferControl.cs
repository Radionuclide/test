using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Properties;


namespace iba.Controls
{
    public partial class DataTransferControl : UserControl, IPropertyPane
    {


        public DataTransferControl()
        {
            InitializeComponent();

        }

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
           
        }

        public void SaveData()
        {
          
        }

        public void LeaveCleanup()
        {
   
        }

    }
}