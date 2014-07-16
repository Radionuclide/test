namespace iba.Controls
{
    partial class CopyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyControl));
            this.m_gbTarget = new Utility.CollapsibleGroupBox();
            this.panelOut = new System.Windows.Forms.Panel();
            this.groupBox6 = new Utility.CollapsibleGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new Utility.CollapsibleGroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbMove = new System.Windows.Forms.RadioButton();
            this.m_rbDelete = new System.Windows.Forms.RadioButton();
            this.m_rbCopy = new System.Windows.Forms.RadioButton();
            this.m_gbTarget.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
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
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.tableLayoutPanel4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbPrevOutput, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbDatFile, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // m_rbPrevOutput
            // 
            resources.ApplyResources(this.m_rbPrevOutput, "m_rbPrevOutput");
            this.m_rbPrevOutput.Name = "m_rbPrevOutput";
            this.m_rbPrevOutput.TabStop = true;
            this.m_rbPrevOutput.UseVisualStyleBackColor = true;
            // 
            // m_rbDatFile
            // 
            resources.ApplyResources(this.m_rbDatFile, "m_rbDatFile");
            this.m_rbDatFile.Name = "m_rbDatFile";
            this.m_rbDatFile.TabStop = true;
            this.m_rbDatFile.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.tableLayoutPanel6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.m_rbMove, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbDelete, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbCopy, 0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // m_rbMove
            // 
            resources.ApplyResources(this.m_rbMove, "m_rbMove");
            this.m_rbMove.Name = "m_rbMove";
            this.m_rbMove.TabStop = true;
            this.m_rbMove.UseVisualStyleBackColor = true;
            // 
            // m_rbDelete
            // 
            resources.ApplyResources(this.m_rbDelete, "m_rbDelete");
            this.m_rbDelete.Name = "m_rbDelete";
            this.m_rbDelete.TabStop = true;
            this.m_rbDelete.UseVisualStyleBackColor = true;
            this.m_rbDelete.CheckedChanged += new System.EventHandler(this.m_rbDelete_CheckedChanged);
            // 
            // m_rbCopy
            // 
            resources.ApplyResources(this.m_rbCopy, "m_rbCopy");
            this.m_rbCopy.Name = "m_rbCopy";
            this.m_rbCopy.TabStop = true;
            this.m_rbCopy.UseVisualStyleBackColor = true;
            // 
            // CopyControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);           
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.m_gbTarget);
            this.MinimumSize = new System.Drawing.Size(0, 580);
            this.Name = "CopyControl";
            this.m_gbTarget.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Utility.CollapsibleGroupBox m_gbTarget;
        private Utility.CollapsibleGroupBox groupBox6;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.RadioButton m_rbPrevOutput;
        private System.Windows.Forms.RadioButton m_rbDatFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private Utility.CollapsibleGroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbMove;
        private System.Windows.Forms.RadioButton m_rbDelete;
        private System.Windows.Forms.RadioButton m_rbCopy;
        private System.Windows.Forms.Panel panelOut;
    }
}
