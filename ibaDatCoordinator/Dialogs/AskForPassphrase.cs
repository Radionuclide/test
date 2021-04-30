using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class AskForPassphrase : Form
    {
        public AskForPassphrase()
        {
            InitializeComponent();
        }
        public string ShowDialog(string text, string caption)
        {
            Text = caption;
            textLabel.Text = text;
            confirmationBtn.Click += (sender, e) => { this.Close(); };
            this.AcceptButton = confirmationBtn;

            return this.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}