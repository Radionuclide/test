using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Crownwood.DotNetMagic.Controls;

using iba.Utility;
using iba.HD.Client.Interfaces;
using iba.HD.Common;
using iba.Data;
using iba.Controls;

namespace iba.Client.Archiver
{
    partial class HDEventWizard : Form, IHdEventWizard
    {
        public HDEventWizard(AnalyzerManager ioconfig, List<string> priorities)
        {
            InitializeComponent();

            foreach (string p in priorities)
                cbPriority.Items.Add(p);

            string[] items = new string[] {
                Properties.Resources.UseSignalName
            };

            cbEventName.Items.AddRange(items);
            cbEventText.Items.AddRange(items);
            
            cbTriggerMode.Items.AddRange(new string[] {
                Properties.Resources.TriggerPerFile,
                Properties.Resources.TriggerPerPulse
            });

            template = new HDEventTemplate();

            //Load template settings
            cbEventName.SelectedIndex = 0;
            cbEventText.SelectedIndex = 0;
            cbPriority.Text = priorities[0];

            cbTriggerMode.SelectedIndex = 1;

            selectedSignals = new List<IbaAnalyzer.ISignalTreeNode>();
            CreateSignalTree(ioconfig);
        }

        HDEventTemplate template;
        public HDEventTemplate Template
        {
            get { return template; }
        }

        List<IbaAnalyzer.ISignalTreeNode> selectedSignals;
        public List<IbaAnalyzer.ISignalTreeNode> SelectedSignals
        {
            get { return selectedSignals; }
        }

       private void btAdd_Click(object sender, EventArgs e)
        {
            template.TriggerMode = cbTriggerMode.SelectedIndex == 0 ? HDCreateEventTaskData.HDEventTriggerEnum.PerFile : HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse;
            template.Priority = cbPriority.Text;
            DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region Signal tree
        bool bIgnoreChecks;
        AnalyzerTreeControl signalTree;

        private void CreateSignalTree(AnalyzerManager analyzerManager)
        {
            var channelTree = new RepositoryItemChannelTreeEdit(analyzerManager, ChannelTreeFilter.Digital);
            signalTree = channelTree.ChannelTree;
            signalTree.AllowDrop = false;
            signalTree.MultiSelect = true;
            signalTree.CheckBoxes = true;
            signalTree.Location = dummyTree.Location;
            signalTree.Size = dummyTree.Size;
            signalTree.Anchor = dummyTree.Anchor;
            dummyTree.Parent.Controls.Add(signalTree);
            dummyTree.Parent.Controls.Remove(dummyTree);
            dummyTree.Dispose();

            signalTree.TreeAfterCheck += new NodeEventHandler(Tree_AfterCheck);
        }

        private void Tree_AfterCheck(TreeControl tc, NodeEventArgs e)
        {
            if (bIgnoreChecks)
                return;

            if (e.Node.Tag is IbaAnalyzer.ISignalTreeNode)
            {
                IbaAnalyzer.ISignalTreeNode signal = (IbaAnalyzer.ISignalTreeNode)e.Node.Tag;
                if (e.Node.Checked)
                    selectedSignals.Add(signal);
                else
                    selectedSignals.Remove(signal);
            }
            else
            {
                if (e.Node.Checked)
                    AddChildren(e.Node);
                else
                    RemoveChildren(e.Node);
            }
        }

        private void AddChildren(Node parent)
        {
            for (int i = 0; i < parent.Nodes.Count; i++)
            {
                Node node = parent.Nodes[i];
                IbaAnalyzer.ISignalTreeNode signal = node.Tag as IbaAnalyzer.ISignalTreeNode;
                if (signal != null)
                    selectedSignals.Add(signal);
                else
                    AddChildren(node);
            }
        }

        private void RemoveChildren(Node parent)
        {
            for (int i = 0; i < parent.Nodes.Count; i++)
            {
                Node node = parent.Nodes[i];
                IbaAnalyzer.ISignalTreeNode signal = node.Tag as IbaAnalyzer.ISignalTreeNode;
                if (signal != null)
                    selectedSignals.Remove(signal);
                else
                    RemoveChildren(node);
            }
        }

        public void ClearSignals()
        {
            for (int i = selectedSignals.Count - 1; i >= 0; i--)
            {
                Node node = signalTree.GetSignalNode(selectedSignals[i].channelId);
                if (node != null)
                    node.Checked = false;
            }
        }

        public void SetSignals(List<string> ids)
        {
            while (!signalTree.Load()) ;
            foreach (string id in ids)
            {
                Node node = signalTree.GetSignalNode(id);
                if (node != null)
                    node.Checked = true;
            }
        }

        public List<ControlEventTreeData> Apply()
        {
            List<ControlEventTreeData> result = new List<ControlEventTreeData>();
            foreach (IbaAnalyzer.ISignalTreeNode signal in selectedSignals)
            {
                result.Add(template.Apply(signal));
            }
            return result;
        }

        List<string> IHdEventWizard.SelectedSignals()
        {
            throw new NotImplementedException();
        }
        #endregion*/
    }

    class HDEventTemplate
    {
        public HDEventTemplate()
        {
            EventName = NameOption.Name;
            EventComment1 = NameOption.Comment1;
            EventComment2 = NameOption.Comment2;
            EventText = NameOption.Name;

            Priority = "";
        }

        public enum NameOption
        {
            Name = 0,
            Comment1 = 1,
            Comment2 = 2
        }

        public NameOption EventName;
        public NameOption EventComment1;
        public NameOption EventComment2;
        public NameOption EventText;
        public HDCreateEventTaskData.HDEventTriggerEnum TriggerMode;

        public string Priority;
        
        public ControlEventTreeData Apply(IbaAnalyzer.ISignalTreeNode signal)
        {
            EventWriterSignal eventServerConfig = new EventWriterSignal("", new HdSegmentText(GetTextFromSignal(signal, EventName), GetTextFromSignal(signal, EventComment1), GetTextFromSignal(signal, EventComment2)), Priority, GetTextFromSignal(signal, EventText));
            HDCreateEventTaskData.EventData eventConfig = new HDCreateEventTaskData.EventData();

            eventConfig.PulseSignal = signal.channelId;
            eventConfig.TriggerMode = TriggerMode;
            LocalEventData localEvent = new LocalEventData("", "");
            localEvent.Tag = eventConfig;
            ControlEventTreeData controlEventTreeData = new ControlEventTreeData(eventServerConfig);
            controlEventTreeData.localEventData = localEvent;
            return controlEventTreeData;
        }

        private string GetTextFromSignal(IbaAnalyzer.ISignalTreeNode signal, NameOption option)
        {
            switch (option)
            {
                case NameOption.Name:
                    return signal.Text;
                case NameOption.Comment1:
                    return "";
                case NameOption.Comment2:
                    return "";
            }
            return "";
        }
    }
}
