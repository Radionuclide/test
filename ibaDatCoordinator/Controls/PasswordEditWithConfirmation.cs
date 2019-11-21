using System;
using System.Diagnostics;
using System.Windows.Forms;
using iba.Dialogs;

namespace iba.Controls
{
    public delegate bool ValidatePasswordDelegate(object sender, string password, out string errorMessage);

    /// <summary> Copied from ibaPDA.
    /// Is used in <see cref="OpcUaControl"/> and <see cref="SnmpControl"/>
    /// </summary>
    public partial class PasswordEditWithConfirmation : UserControl
    {
        public PasswordEditWithConfirmation()
        {
            InitializeComponent();

            Password = "";
        }

        //public static PasswordEditWithConfirmation ReplaceControl(Control ctrl)
        //{
        //    return ReplaceUtil.ReplaceControl(ctrl, new PasswordEditWithConfirmation());
        //}

        public override string Text
        {
            get => Password;
            set => Password = value;
        }

        string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value ?? "";
                tbPass.Text = _password.Length > 0 ? "      " : "";
            }
        }

        public bool ReadOnly
        {
            get => !btEdit.Enabled;
            set => btEdit.Enabled = !value;
        }

        public string Tooltip { get; set; }
                        
        private void btEdit_Click(object sender, EventArgs e)
        {
            using (PasswordEditForm form = new PasswordEditForm())
            {
                if(ValidatePasswordEvent != null)
                    form.ValidatePasswordEvent += Form_ValidatePasswordEvent;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (Password != form.Password)
                    {
                        Password = form.Password;
                        PasswordChanged?.Invoke(this, EventArgs.Empty);
                    }
                }

                if (ValidatePasswordEvent != null)
                    form.ValidatePasswordEvent -= Form_ValidatePasswordEvent;
            }
        }

        public event ValidatePasswordDelegate ValidatePasswordEvent;

        private bool Form_ValidatePasswordEvent(object sender, string password, out string errorMessage)
        {
            Debug.Assert(ValidatePasswordEvent != null);
            return ValidatePasswordEvent.Invoke(this, password, out errorMessage);
        }

        public event EventHandler PasswordChanged;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!String.IsNullOrEmpty(Tooltip))
                toolTip1.Show(Tooltip, tbPass, 0, tbPass.Height + 1, 10000);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            toolTip1.Hide(tbPass);
        }
    }
}
