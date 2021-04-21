namespace Alunorf_roh_plugin
{
    partial class SelectInfoOrChannels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectInfoOrChannels));
            this.m_lbIba = new System.Windows.Forms.ListBox();
            this.m_lbRoh = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_btRemove = new System.Windows.Forms.Button();
            this.m_btAdd = new System.Windows.Forms.Button();
            this.m_cbMultiValuedFields = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lbIba
            // 
            this.m_lbIba.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lbIba.FormattingEnabled = true;
            this.m_lbIba.Location = new System.Drawing.Point(3, 3);
            this.m_lbIba.Name = "m_lbIba";
            this.m_lbIba.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.m_lbIba.Size = new System.Drawing.Size(171, 244);
            this.m_lbIba.Sorted = true;
            this.m_lbIba.TabIndex = 0;
            // 
            // m_lbRoh
            // 
            this.m_lbRoh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lbRoh.FormattingEnabled = true;
            this.m_lbRoh.Location = new System.Drawing.Point(230, 3);
            this.m_lbRoh.Name = "m_lbRoh";
            this.m_lbRoh.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.m_lbRoh.Size = new System.Drawing.Size(171, 244);
            this.m_lbRoh.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(260, 268);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(341, 268);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_lbRoh, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_lbIba, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(404, 250);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_btRemove);
            this.panel1.Controls.Add(this.m_btAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(180, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(44, 244);
            this.panel1.TabIndex = 2;
            // 
            // m_btRemove
            // 
            this.m_btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btRemove.Image = global::Alunorf_roh_plugin.Properties.Resources.left;
            this.m_btRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btRemove.Location = new System.Drawing.Point(2, 67);
            this.m_btRemove.Name = "m_btRemove";
            this.m_btRemove.Size = new System.Drawing.Size(40, 40);
            this.m_btRemove.TabIndex = 15;
            this.m_btRemove.UseVisualStyleBackColor = true;
            this.m_btRemove.Click += new System.EventHandler(this.m_btRemove_Click);
            // 
            // m_btAdd
            // 
            this.m_btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btAdd.Image = global::Alunorf_roh_plugin.Properties.Resources.right;
            this.m_btAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btAdd.Location = new System.Drawing.Point(2, 21);
            this.m_btAdd.Name = "m_btAdd";
            this.m_btAdd.Size = new System.Drawing.Size(40, 40);
            this.m_btAdd.TabIndex = 14;
            this.m_btAdd.UseVisualStyleBackColor = true;
            this.m_btAdd.Click += new System.EventHandler(this.m_btAdd_Click);
            // 
            // m_cbMultiValuedFields
            // 
            this.m_cbMultiValuedFields.AutoSize = true;
            this.m_cbMultiValuedFields.Location = new System.Drawing.Point(12, 272);
            this.m_cbMultiValuedFields.Name = "m_cbMultiValuedFields";
            this.m_cbMultiValuedFields.Size = new System.Drawing.Size(242, 17);
            this.m_cbMultiValuedFields.TabIndex = 5;
            this.m_cbMultiValuedFields.Text = "Mehrfachauswahl zulassen (Kanäle anzeigen)";
            this.m_cbMultiValuedFields.UseVisualStyleBackColor = true;
            this.m_cbMultiValuedFields.CheckedChanged += new System.EventHandler(this.m_cbMultiValuedFields_CheckedChanged);
            // 
            // SelectInfoOrChannels
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 303);
            this.Controls.Add(this.m_cbMultiValuedFields);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "SelectInfoOrChannels";
            this.Text = "select Infofields and channels";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox m_lbIba;
        private System.Windows.Forms.ListBox m_lbRoh;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button m_btRemove;
        private System.Windows.Forms.Button m_btAdd;
        private System.Windows.Forms.CheckBox m_cbMultiValuedFields;
    }
}