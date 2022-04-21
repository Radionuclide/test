namespace iba.Controls
{
    partial class UpdateDataTaskControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDataTaskControl));
			this.m_gbTarget = new System.Windows.Forms.GroupBox();
			this.panelOut = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.m_panelAuth = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			this.m_tbDbPass = new System.Windows.Forms.TextBox();
			this.m_rbNT = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.m_rbOtherAuth = new System.Windows.Forms.RadioButton();
			this.m_tbDbUsername = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.m_btTestConnection = new System.Windows.Forms.Button();
			this.m_tbTableName = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.m_computer = new System.Windows.Forms.Panel();
			this.m_btBrowseServer = new System.Windows.Forms.Button();
			this.m_tbServer = new System.Windows.Forms.TextBox();
			this.m_rbServer = new System.Windows.Forms.RadioButton();
			this.m_rbLocal = new System.Windows.Forms.RadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.m_tbDatabaseName = new System.Windows.Forms.TextBox();
			this.m_cbProvider = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.m_gbTarget.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.m_panelAuth.SuspendLayout();
			this.m_computer.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_gbTarget
			// 
			resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
			this.m_gbTarget.Controls.Add(this.panelOut);
			this.m_gbTarget.Name = "m_gbTarget";
			this.m_gbTarget.TabStop = false;
			// 
			// panelOut
			// 
			resources.ApplyResources(this.panelOut, "panelOut");
			this.panelOut.Name = "panelOut";
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.m_panelAuth);
			this.groupBox1.Controls.Add(this.m_btTestConnection);
			this.groupBox1.Controls.Add(this.m_tbTableName);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.m_computer);
			this.groupBox1.Controls.Add(this.m_tbDatabaseName);
			this.groupBox1.Controls.Add(this.m_cbProvider);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// m_panelAuth
			// 
			resources.ApplyResources(this.m_panelAuth, "m_panelAuth");
			this.m_panelAuth.Controls.Add(this.label8);
			this.m_panelAuth.Controls.Add(this.m_tbDbPass);
			this.m_panelAuth.Controls.Add(this.m_rbNT);
			this.m_panelAuth.Controls.Add(this.label6);
			this.m_panelAuth.Controls.Add(this.m_rbOtherAuth);
			this.m_panelAuth.Controls.Add(this.m_tbDbUsername);
			this.m_panelAuth.Controls.Add(this.label7);
			this.m_panelAuth.Name = "m_panelAuth";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// m_tbDbPass
			// 
			resources.ApplyResources(this.m_tbDbPass, "m_tbDbPass");
			this.m_tbDbPass.Name = "m_tbDbPass";
			this.m_tbDbPass.UseSystemPasswordChar = true;
			this.m_tbDbPass.TextChanged += new System.EventHandler(this.m_tbDbUsername_TextChanged);
			// 
			// m_rbNT
			// 
			resources.ApplyResources(this.m_rbNT, "m_rbNT");
			this.m_rbNT.Name = "m_rbNT";
			this.m_rbNT.TabStop = true;
			this.m_rbNT.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// m_rbOtherAuth
			// 
			resources.ApplyResources(this.m_rbOtherAuth, "m_rbOtherAuth");
			this.m_rbOtherAuth.Name = "m_rbOtherAuth";
			this.m_rbOtherAuth.TabStop = true;
			this.m_rbOtherAuth.UseVisualStyleBackColor = true;
			// 
			// m_tbDbUsername
			// 
			resources.ApplyResources(this.m_tbDbUsername, "m_tbDbUsername");
			this.m_tbDbUsername.Name = "m_tbDbUsername";
			this.m_tbDbUsername.TextChanged += new System.EventHandler(this.m_tbDbUsername_TextChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// m_btTestConnection
			// 
			resources.ApplyResources(this.m_btTestConnection, "m_btTestConnection");
			this.m_btTestConnection.Name = "m_btTestConnection";
			this.m_btTestConnection.UseVisualStyleBackColor = true;
			this.m_btTestConnection.Click += new System.EventHandler(this.m_btTestConnection_Click);
			// 
			// m_tbTableName
			// 
			resources.ApplyResources(this.m_tbTableName, "m_tbTableName");
			this.m_tbTableName.Name = "m_tbTableName";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// m_computer
			// 
			resources.ApplyResources(this.m_computer, "m_computer");
			this.m_computer.Controls.Add(this.m_btBrowseServer);
			this.m_computer.Controls.Add(this.m_tbServer);
			this.m_computer.Controls.Add(this.m_rbServer);
			this.m_computer.Controls.Add(this.m_rbLocal);
			this.m_computer.Controls.Add(this.label10);
			this.m_computer.Name = "m_computer";
			// 
			// m_btBrowseServer
			// 
			resources.ApplyResources(this.m_btBrowseServer, "m_btBrowseServer");
			this.m_btBrowseServer.Image = Icons.Gui.All.Images.FolderOpen();
			this.m_btBrowseServer.Name = "m_btBrowseServer";
			this.m_btBrowseServer.UseVisualStyleBackColor = true;
			this.m_btBrowseServer.Click += new System.EventHandler(this.m_btBrowseServer_Click);
			// 
			// m_tbServer
			// 
			resources.ApplyResources(this.m_tbServer, "m_tbServer");
			this.m_tbServer.Name = "m_tbServer";
			// 
			// m_rbServer
			// 
			resources.ApplyResources(this.m_rbServer, "m_rbServer");
			this.m_rbServer.Name = "m_rbServer";
			this.m_rbServer.TabStop = true;
			this.m_rbServer.UseVisualStyleBackColor = true;
			// 
			// m_rbLocal
			// 
			resources.ApplyResources(this.m_rbLocal, "m_rbLocal");
			this.m_rbLocal.Name = "m_rbLocal";
			this.m_rbLocal.TabStop = true;
			this.m_rbLocal.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// m_tbDatabaseName
			// 
			resources.ApplyResources(this.m_tbDatabaseName, "m_tbDatabaseName");
			this.m_tbDatabaseName.Name = "m_tbDatabaseName";
			// 
			// m_cbProvider
			// 
			this.m_cbProvider.FormattingEnabled = true;
			this.m_cbProvider.Items.AddRange(new object[] {
            resources.GetString("m_cbProvider.Items"),
            resources.GetString("m_cbProvider.Items1"),
            resources.GetString("m_cbProvider.Items2"),
            resources.GetString("m_cbProvider.Items3")});
			resources.ApplyResources(this.m_cbProvider, "m_cbProvider");
			this.m_cbProvider.Name = "m_cbProvider";
			this.m_cbProvider.SelectedIndexChanged += new System.EventHandler(this.m_cbProvider_SelectedIndexChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// UpdateDataTaskControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.m_gbTarget);
			this.Name = "UpdateDataTaskControl";
			this.m_gbTarget.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.m_panelAuth.ResumeLayout(false);
			this.m_panelAuth.PerformLayout();
			this.m_computer.ResumeLayout(false);
			this.m_computer.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_gbTarget;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbDatabaseName;
        private System.Windows.Forms.ComboBox m_cbProvider;
        private System.Windows.Forms.Panel m_panelAuth;
        private System.Windows.Forms.TextBox m_tbDbPass;
        private System.Windows.Forms.RadioButton m_rbNT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton m_rbOtherAuth;
        private System.Windows.Forms.TextBox m_tbDbUsername;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel m_computer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_tbTableName;
        private System.Windows.Forms.Button m_btTestConnection;
        private System.Windows.Forms.RadioButton m_rbServer;
        private System.Windows.Forms.RadioButton m_rbLocal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button m_btBrowseServer;
        private System.Windows.Forms.TextBox m_tbServer;
        private System.Windows.Forms.Panel panelOut;
    }
}
