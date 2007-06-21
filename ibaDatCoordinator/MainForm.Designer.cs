namespace iba
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_statusBarStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_rightPane = new TD.Eyefinder.HeaderControl();
            this.m_navBar = new TD.Eyefinder.NavigationBar();
            this.m_watchdogPane = new TD.Eyefinder.NavigationPane();
            this.m_loggingPane = new TD.Eyefinder.NavigationPane();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.m_EntriesNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.m_statusPane = new TD.Eyefinder.NavigationPane();
            this.m_statusTreeView = new System.Windows.Forms.TreeView();
            this.m_configPane = new TD.Eyefinder.NavigationPane();
            this.m_configTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_stopButton = new System.Windows.Forms.Button();
            this.m_startButton = new System.Windows.Forms.Button();
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchdogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.m_statusStrip.SuspendLayout();
            this.m_navBar.SuspendLayout();
            this.m_loggingPane.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_EntriesNumericUpDown1)).BeginInit();
            this.m_statusPane.SuspendLayout();
            this.m_configPane.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.AccessibleDescription = null;
            this.toolStripContainer1.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.BottomToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.BottomToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.BottomToolStripPanel, "toolStripContainer1.BottomToolStripPanel");
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.m_statusStrip);
            this.toolStripContainer1.BottomToolStripPanel.Font = null;
            this.m_toolTip.SetToolTip(this.toolStripContainer1.BottomToolStripPanel, resources.GetString("toolStripContainer1.BottomToolStripPanel.ToolTip"));
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AccessibleDescription = null;
            this.toolStripContainer1.ContentPanel.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            this.toolStripContainer1.ContentPanel.BackgroundImage = null;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.m_rightPane);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.m_navBar);
            this.toolStripContainer1.ContentPanel.Font = null;
            this.m_toolTip.SetToolTip(this.toolStripContainer1.ContentPanel, resources.GetString("toolStripContainer1.ContentPanel.ToolTip"));
            this.toolStripContainer1.Font = null;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.LeftToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.LeftToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.LeftToolStripPanel, "toolStripContainer1.LeftToolStripPanel");
            this.toolStripContainer1.LeftToolStripPanel.Font = null;
            this.m_toolTip.SetToolTip(this.toolStripContainer1.LeftToolStripPanel, resources.GetString("toolStripContainer1.LeftToolStripPanel.ToolTip"));
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.RightToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.RightToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.RightToolStripPanel, "toolStripContainer1.RightToolStripPanel");
            this.toolStripContainer1.RightToolStripPanel.Font = null;
            this.m_toolTip.SetToolTip(this.toolStripContainer1.RightToolStripPanel, resources.GetString("toolStripContainer1.RightToolStripPanel.ToolTip"));
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.m_toolTip.SetToolTip(this.toolStripContainer1, resources.GetString("toolStripContainer1.ToolTip"));
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer1.TopToolStripPanel.AccessibleName = null;
            this.toolStripContainer1.TopToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer1.TopToolStripPanel, "toolStripContainer1.TopToolStripPanel");
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.m_menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Font = null;
            this.m_toolTip.SetToolTip(this.toolStripContainer1.TopToolStripPanel, resources.GetString("toolStripContainer1.TopToolStripPanel.ToolTip"));
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.AccessibleDescription = null;
            this.m_statusStrip.AccessibleName = null;
            resources.ApplyResources(this.m_statusStrip, "m_statusStrip");
            this.m_statusStrip.BackgroundImage = null;
            this.m_statusStrip.Font = null;
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_statusBarStripLabel});
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_toolTip.SetToolTip(this.m_statusStrip, resources.GetString("m_statusStrip.ToolTip"));
            // 
            // m_statusBarStripLabel
            // 
            this.m_statusBarStripLabel.AccessibleDescription = null;
            this.m_statusBarStripLabel.AccessibleName = null;
            resources.ApplyResources(this.m_statusBarStripLabel, "m_statusBarStripLabel");
            this.m_statusBarStripLabel.BackgroundImage = null;
            this.m_statusBarStripLabel.Name = "m_statusBarStripLabel";
            this.m_statusBarStripLabel.Spring = true;
            // 
            // m_rightPane
            // 
            this.m_rightPane.AccessibleDescription = null;
            this.m_rightPane.AccessibleName = null;
            resources.ApplyResources(this.m_rightPane, "m_rightPane");
            this.m_rightPane.BackgroundImage = null;
            this.m_rightPane.Font = null;
            this.m_rightPane.HeaderFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.m_rightPane.Name = "m_rightPane";
            this.m_toolTip.SetToolTip(this.m_rightPane, resources.GetString("m_rightPane.ToolTip"));
            this.m_rightPane.Enter += new System.EventHandler(this.m_rightPane_Enter);
            this.m_rightPane.Leave += new System.EventHandler(this.m_rightPane_Leave);
            // 
            // m_navBar
            // 
            this.m_navBar.AccessibleDescription = null;
            this.m_navBar.AccessibleName = null;
            resources.ApplyResources(this.m_navBar, "m_navBar");
            this.m_navBar.BackgroundImage = null;
            this.m_navBar.Controls.Add(this.m_watchdogPane);
            this.m_navBar.Controls.Add(this.m_loggingPane);
            this.m_navBar.Controls.Add(this.m_statusPane);
            this.m_navBar.Controls.Add(this.m_configPane);
            this.m_navBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_navBar.Font = null;
            this.m_navBar.HeaderFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.m_navBar.Name = "m_navBar";
            this.m_navBar.PaneFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.m_navBar.SelectedPane = this.m_loggingPane;
            this.m_navBar.ShowPanes = 4;
            this.m_toolTip.SetToolTip(this.m_navBar, resources.GetString("m_navBar.ToolTip"));
            this.m_navBar.SelectedPaneChanged += new System.EventHandler(this.navbar_SelectedPaneChanged);
            // 
            // m_watchdogPane
            // 
            this.m_watchdogPane.AccessibleDescription = null;
            this.m_watchdogPane.AccessibleName = null;
            resources.ApplyResources(this.m_watchdogPane, "m_watchdogPane");
            this.m_watchdogPane.BackgroundImage = null;
            this.m_watchdogPane.Font = null;
            this.m_watchdogPane.Name = "m_watchdogPane";
            this.m_toolTip.SetToolTip(this.m_watchdogPane, resources.GetString("m_watchdogPane.ToolTip"));
            // 
            // m_loggingPane
            // 
            this.m_loggingPane.AccessibleDescription = null;
            this.m_loggingPane.AccessibleName = null;
            resources.ApplyResources(this.m_loggingPane, "m_loggingPane");
            this.m_loggingPane.BackgroundImage = null;
            this.m_loggingPane.Controls.Add(this.splitContainer1);
            this.m_loggingPane.Font = null;
            this.m_loggingPane.Name = "m_loggingPane";
            this.m_toolTip.SetToolTip(this.m_loggingPane, resources.GetString("m_loggingPane.ToolTip"));
            // 
            // splitContainer1
            // 
            this.splitContainer1.AccessibleDescription = null;
            this.splitContainer1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackgroundImage = null;
            this.splitContainer1.Font = null;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AccessibleDescription = null;
            this.splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackgroundImage = null;
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Font = null;
            this.m_toolTip.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleDescription = null;
            this.splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackgroundImage = null;
            this.splitContainer1.Panel2.Controls.Add(this.m_EntriesNumericUpDown1);
            this.splitContainer1.Panel2.Font = null;
            this.m_toolTip.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
            this.m_toolTip.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            this.m_toolTip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // m_EntriesNumericUpDown1
            // 
            this.m_EntriesNumericUpDown1.AccessibleDescription = null;
            this.m_EntriesNumericUpDown1.AccessibleName = null;
            resources.ApplyResources(this.m_EntriesNumericUpDown1, "m_EntriesNumericUpDown1");
            this.m_EntriesNumericUpDown1.Font = null;
            this.m_EntriesNumericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.Name = "m_EntriesNumericUpDown1";
            this.m_toolTip.SetToolTip(this.m_EntriesNumericUpDown1, resources.GetString("m_EntriesNumericUpDown1.ToolTip"));
            this.m_EntriesNumericUpDown1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.ValueChanged += new System.EventHandler(this.m_EntriesNumericUpDown1_ValueChanged);
            // 
            // m_statusPane
            // 
            this.m_statusPane.AccessibleDescription = null;
            this.m_statusPane.AccessibleName = null;
            resources.ApplyResources(this.m_statusPane, "m_statusPane");
            this.m_statusPane.BackgroundImage = null;
            this.m_statusPane.Controls.Add(this.m_statusTreeView);
            this.m_statusPane.Font = null;
            this.m_statusPane.Name = "m_statusPane";
            this.m_toolTip.SetToolTip(this.m_statusPane, resources.GetString("m_statusPane.ToolTip"));
            // 
            // m_statusTreeView
            // 
            this.m_statusTreeView.AccessibleDescription = null;
            this.m_statusTreeView.AccessibleName = null;
            resources.ApplyResources(this.m_statusTreeView, "m_statusTreeView");
            this.m_statusTreeView.BackgroundImage = null;
            this.m_statusTreeView.Font = null;
            this.m_statusTreeView.HideSelection = false;
            this.m_statusTreeView.Name = "m_statusTreeView";
            this.m_toolTip.SetToolTip(this.m_statusTreeView, resources.GetString("m_statusTreeView.ToolTip"));
            this.m_statusTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnStatusTreeViewAfterSelect);
            // 
            // m_configPane
            // 
            this.m_configPane.AccessibleDescription = null;
            this.m_configPane.AccessibleName = null;
            resources.ApplyResources(this.m_configPane, "m_configPane");
            this.m_configPane.BackgroundImage = null;
            this.m_configPane.Controls.Add(this.m_configTreeView);
            this.m_configPane.Controls.Add(this.panel1);
            this.m_configPane.Font = null;
            this.m_configPane.Name = "m_configPane";
            this.m_toolTip.SetToolTip(this.m_configPane, resources.GetString("m_configPane.ToolTip"));
            // 
            // m_configTreeView
            // 
            this.m_configTreeView.AccessibleDescription = null;
            this.m_configTreeView.AccessibleName = null;
            this.m_configTreeView.AllowDrop = true;
            resources.ApplyResources(this.m_configTreeView, "m_configTreeView");
            this.m_configTreeView.BackgroundImage = null;
            this.m_configTreeView.Font = null;
            this.m_configTreeView.HideSelection = false;
            this.m_configTreeView.Name = "m_configTreeView";
            this.m_toolTip.SetToolTip(this.m_configTreeView, resources.GetString("m_configTreeView.ToolTip"));
            this.m_configTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_configTreeView_DragDrop);
            this.m_configTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.m_configTreeView_DragOver);
            this.m_configTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnConfigurationTreeViewAfterSelect);
            this.m_configTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_configTreeView_KeyDown);
            this.m_configTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.m_configTreeView_ItemDrag);
            this.m_configTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_configTreeView_MouseDown);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.m_stopButton);
            this.panel1.Controls.Add(this.m_startButton);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            this.m_toolTip.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // m_stopButton
            // 
            this.m_stopButton.AccessibleDescription = null;
            this.m_stopButton.AccessibleName = null;
            resources.ApplyResources(this.m_stopButton, "m_stopButton");
            this.m_stopButton.BackgroundImage = null;
            this.m_stopButton.Font = null;
            this.m_stopButton.Image = global::iba.Properties.Resources.Stop;
            this.m_stopButton.Name = "m_stopButton";
            this.m_toolTip.SetToolTip(this.m_stopButton, resources.GetString("m_stopButton.ToolTip"));
            this.m_stopButton.UseVisualStyleBackColor = true;
            this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
            // 
            // m_startButton
            // 
            this.m_startButton.AccessibleDescription = null;
            this.m_startButton.AccessibleName = null;
            resources.ApplyResources(this.m_startButton, "m_startButton");
            this.m_startButton.BackgroundImage = null;
            this.m_startButton.Font = null;
            this.m_startButton.Image = global::iba.Properties.Resources.Start;
            this.m_startButton.Name = "m_startButton";
            this.m_toolTip.SetToolTip(this.m_startButton, resources.GetString("m_startButton.ToolTip"));
            this.m_startButton.UseVisualStyleBackColor = true;
            this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.AccessibleDescription = null;
            this.m_menuStrip.AccessibleName = null;
            resources.ApplyResources(this.m_menuStrip, "m_menuStrip");
            this.m_menuStrip.BackgroundImage = null;
            this.m_menuStrip.Font = null;
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.serviceToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.m_toolTip.SetToolTip(this.m_menuStrip, resources.GetString("m_menuStrip.ToolTip"));
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.AccessibleDescription = null;
            this.fileToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.BackgroundImage = null;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.AccessibleDescription = null;
            this.newToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.BackgroundImage = null;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.AccessibleDescription = null;
            this.openToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.BackgroundImage = null;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.AccessibleDescription = null;
            this.toolStripSeparator.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            this.toolStripSeparator.Name = "toolStripSeparator";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.AccessibleDescription = null;
            this.saveToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.BackgroundImage = null;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.AccessibleDescription = null;
            this.saveAsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.BackgroundImage = null;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.AccessibleDescription = null;
            this.exitToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.BackgroundImage = null;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.AccessibleDescription = null;
            this.editToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.BackgroundImage = null;
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.AccessibleDescription = null;
            this.cutToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.BackgroundImage = null;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.AccessibleDescription = null;
            this.copyToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.BackgroundImage = null;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.AccessibleDescription = null;
            this.pasteToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.BackgroundImage = null;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.AccessibleDescription = null;
            this.deleteToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.BackgroundImage = null;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.AccessibleDescription = null;
            this.viewToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            this.viewToolStripMenuItem.BackgroundImage = null;
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.watchdogToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.AccessibleDescription = null;
            this.configurationToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.configurationToolStripMenuItem, "configurationToolStripMenuItem");
            this.configurationToolStripMenuItem.BackgroundImage = null;
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.AccessibleDescription = null;
            this.statusToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.BackgroundImage = null;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.AccessibleDescription = null;
            this.loggingToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.loggingToolStripMenuItem, "loggingToolStripMenuItem");
            this.loggingToolStripMenuItem.BackgroundImage = null;
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.loggingToolStripMenuItem.Click += new System.EventHandler(this.loggingToolStripMenuItem_Click);
            // 
            // watchdogToolStripMenuItem
            // 
            this.watchdogToolStripMenuItem.AccessibleDescription = null;
            this.watchdogToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.watchdogToolStripMenuItem, "watchdogToolStripMenuItem");
            this.watchdogToolStripMenuItem.BackgroundImage = null;
            this.watchdogToolStripMenuItem.Name = "watchdogToolStripMenuItem";
            this.watchdogToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.watchdogToolStripMenuItem.Click += new System.EventHandler(this.watchdogToolStripMenuItem_Click);
            // 
            // serviceToolStripMenuItem
            // 
            this.serviceToolStripMenuItem.AccessibleDescription = null;
            this.serviceToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.serviceToolStripMenuItem, "serviceToolStripMenuItem");
            this.serviceToolStripMenuItem.BackgroundImage = null;
            this.serviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServiceToolStripMenuItem,
            this.stopServiceToolStripMenuItem});
            this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            this.serviceToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.serviceToolStripMenuItem.DropDownOpening += new System.EventHandler(this.serviceToolStripMenuItem_DropDownOpening);
            // 
            // startServiceToolStripMenuItem
            // 
            this.startServiceToolStripMenuItem.AccessibleDescription = null;
            this.startServiceToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.startServiceToolStripMenuItem, "startServiceToolStripMenuItem");
            this.startServiceToolStripMenuItem.BackgroundImage = null;
            this.startServiceToolStripMenuItem.Name = "startServiceToolStripMenuItem";
            this.startServiceToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.startServiceToolStripMenuItem.Click += new System.EventHandler(this.miStartService_Click);
            // 
            // stopServiceToolStripMenuItem
            // 
            this.stopServiceToolStripMenuItem.AccessibleDescription = null;
            this.stopServiceToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.stopServiceToolStripMenuItem, "stopServiceToolStripMenuItem");
            this.stopServiceToolStripMenuItem.BackgroundImage = null;
            this.stopServiceToolStripMenuItem.Name = "stopServiceToolStripMenuItem";
            this.stopServiceToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.stopServiceToolStripMenuItem.Click += new System.EventHandler(this.miStopService_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.AccessibleDescription = null;
            this.helpToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.BackgroundImage = null;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.AccessibleDescription = null;
            this.aboutToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.BackgroundImage = null;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // m_saveFileDialog
            // 
            resources.ApplyResources(this.m_saveFileDialog, "m_saveFileDialog");
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.FileName = "openFileDialog1";
            resources.ApplyResources(this.m_openFileDialog, "m_openFileDialog");
            // 
            // MainForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.toolStripContainer1);
            this.Font = null;
            this.Name = "MainForm";
            this.m_toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.m_navBar.ResumeLayout(false);
            this.m_loggingPane.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_EntriesNumericUpDown1)).EndInit();
            this.m_statusPane.ResumeLayout(false);
            this.m_configPane.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TD.Eyefinder.NavigationBar m_navBar;
        private TD.Eyefinder.NavigationPane m_statusPane;
        private TD.Eyefinder.NavigationPane m_configPane;
        private TD.Eyefinder.HeaderControl m_rightPane;
        private System.Windows.Forms.TreeView m_configTreeView;
        private System.Windows.Forms.TreeView m_statusTreeView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private TD.Eyefinder.NavigationPane m_loggingPane;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_EntriesNumericUpDown1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button m_startButton;
        private System.Windows.Forms.Button m_stopButton;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel m_statusBarStripLabel;
        private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TD.Eyefinder.NavigationPane m_watchdogPane;
        private System.Windows.Forms.ToolStripMenuItem watchdogToolStripMenuItem;
    }
}

