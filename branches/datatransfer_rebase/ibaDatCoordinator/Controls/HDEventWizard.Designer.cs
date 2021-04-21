namespace iba.Client.Archiver
{
    partial class HDEventWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HDEventWizard));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbOutgoing = new System.Windows.Forms.ComboBox();
            this.lbOutgoing = new System.Windows.Forms.Label();
            this.cbIncoming = new System.Windows.Forms.ComboBox();
            this.lbIncoming = new System.Windows.Forms.Label();
            this.chkOutgoing = new System.Windows.Forms.CheckBox();
            this.chkInverted = new System.Windows.Forms.CheckBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.cbTriggerMode = new System.Windows.Forms.ComboBox();
            this.cbEventText = new System.Windows.Forms.ComboBox();
            this.cbEventName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dummyTree = new System.Windows.Forms.TreeView();
            this.btAdd = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cbOutgoing);
            this.groupBox1.Controls.Add(this.lbOutgoing);
            this.groupBox1.Controls.Add(this.cbIncoming);
            this.groupBox1.Controls.Add(this.lbIncoming);
            this.groupBox1.Controls.Add(this.chkOutgoing);
            this.groupBox1.Controls.Add(this.chkInverted);
            this.groupBox1.Controls.Add(this.cbPriority);
            this.groupBox1.Controls.Add(this.cbTriggerMode);
            this.groupBox1.Controls.Add(this.cbEventText);
            this.groupBox1.Controls.Add(this.cbEventName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbOutgoing
            // 
            this.cbOutgoing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutgoing.FormattingEnabled = true;
            resources.ApplyResources(this.cbOutgoing, "cbOutgoing");
            this.cbOutgoing.Name = "cbOutgoing";
            // 
            // lbOutgoing
            // 
            resources.ApplyResources(this.lbOutgoing, "lbOutgoing");
            this.lbOutgoing.Name = "lbOutgoing";
            // 
            // cbIncoming
            // 
            this.cbIncoming.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIncoming.FormattingEnabled = true;
            resources.ApplyResources(this.cbIncoming, "cbIncoming");
            this.cbIncoming.Name = "cbIncoming";
            // 
            // lbIncoming
            // 
            resources.ApplyResources(this.lbIncoming, "lbIncoming");
            this.lbIncoming.Name = "lbIncoming";
            // 
            // chkOutgoing
            // 
            resources.ApplyResources(this.chkOutgoing, "chkOutgoing");
            this.chkOutgoing.Name = "chkOutgoing";
            this.chkOutgoing.UseVisualStyleBackColor = true;
            // 
            // chkInverted
            // 
            resources.ApplyResources(this.chkInverted, "chkInverted");
            this.chkInverted.Name = "chkInverted";
            this.chkInverted.UseVisualStyleBackColor = true;
            // 
            // cbPriority
            // 
            this.cbPriority.FormattingEnabled = true;
            resources.ApplyResources(this.cbPriority, "cbPriority");
            this.cbPriority.Name = "cbPriority";
            // 
            // cbTriggerMode
            // 
            this.cbTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTriggerMode.FormattingEnabled = true;
            resources.ApplyResources(this.cbTriggerMode, "cbTriggerMode");
            this.cbTriggerMode.Name = "cbTriggerMode";
            this.cbTriggerMode.SelectedIndexChanged += new System.EventHandler(this.cbTriggerMode_SelectedIndexChanged);
            // 
            // cbEventText
            // 
            this.cbEventText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEventText.FormattingEnabled = true;
            resources.ApplyResources(this.cbEventText, "cbEventText");
            this.cbEventText.Name = "cbEventText";
            // 
            // cbEventName
            // 
            this.cbEventName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEventName.FormattingEnabled = true;
            resources.ApplyResources(this.cbEventName, "cbEventName");
            this.cbEventName.Name = "cbEventName";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.dummyTree);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // dummyTree
            // 
            resources.ApplyResources(this.dummyTree, "dummyTree");
            this.dummyTree.Name = "dummyTree";
            // 
            // btAdd
            // 
            resources.ApplyResources(this.btAdd, "btAdd");
            this.btAdd.Name = "btAdd";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // HDEventWizard
            // 
            this.AcceptButton = this.btAdd;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "HDEventWizard";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTriggerMode;
        private System.Windows.Forms.ComboBox cbEventText;
        private System.Windows.Forms.ComboBox cbEventName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TreeView dummyTree;
        private System.Windows.Forms.ComboBox cbPriority;
        private System.Windows.Forms.CheckBox chkOutgoing;
        private System.Windows.Forms.CheckBox chkInverted;
        private System.Windows.Forms.ComboBox cbOutgoing;
        private System.Windows.Forms.Label lbOutgoing;
        private System.Windows.Forms.ComboBox cbIncoming;
        private System.Windows.Forms.Label lbIncoming;
    }
}