using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using iba.Processing;
using IbaSnmpLib;

namespace iba.Controls
{
    public partial class SnmpControl : UserControl, IPropertyPane
    {

        #region Construction, Destruction, Init

        public SnmpControl()
        {
            InitializeComponent();
            _tmp___instCounter++;
        }

        private void SnmpControl_Load(object sender, EventArgs e)
        {
            gbDebug.Init();
            gbConfiguration.Init();
            gbDiagnostics.Init();
            gbObjects.Init();

            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker != null)
            {
                snmpWorker.StatusChanged += SnmpWorker_StatusChanged;
            }

            // todo
            // fill combo boxes not by hand but from enums 
            // to ensure reliability
            //for (int i = (int)IbaSnmpAuthenticationAlgorithm.Md5; i < (int)IbaSnmpAuthenticationAlgorithm.Sha; i++)
            //{
            //    cmbAuthentication.Items;
            //}
        }

        #endregion


        #region Debug

        private static int _tmp___instCounter = 0;
        private int _tmp___cnt1;
        private int _tmp___cnt2;
        private int _tmp___cnt3;
        private int _tmp___cntTimer;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
                ibaSnmp?.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
                ibaSnmp?.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Get list of endpoints and configured traps. This function is not effective and can be use in debug-time only
        /// </summary>
        /// <returns></returns>
        public string _tmp___GetLibraryDescriptionString(IbaSnmp _lib)
        {
            if (_lib == null)
            {
                return "";
            }

            var str = "";

            // general
            str += $"Is started: {_lib.IsStarted}; ";
            str += $"Want to start: {_lib.WantToStart}; ";
            str += $"Product: {_lib.IbaProductId}\r\n";

            //            if (cbShowEndpoints.Checked)
            {
                str += $"\r\nEndpoints: {_lib.ActiveEndPoints.Count}\r\n";
                foreach (var endPoint in _lib.ActiveEndPoints)
                {
                    var af = "???";
                    if (endPoint.AddressFamily == AddressFamily.InterNetwork)
                    {
                        af = "IPv4";
                    }
                    if (endPoint.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        af = "IPv6";
                    }

                    str += $"{af,-5} {endPoint.Address,-28} :{endPoint.Port}\r\n";
                }
            }

            //if (cbShowTrapInform.Checked)
            //{
            //    str += $"\r\nTrap/Inform: {_lib.NotificationList.Count}\r\n";
            //    for (var i = 0; i < _lib.NotificationList.Count; i++)
            //    {
            //        var ntfBase = _lib.NotificationList[i];

            //        str += $"Trap[{i}] ep: {ntfBase.DestinationEndpoint}\r\n";
            //        str += $"Trap[{i}] v: {ntfBase.ProtocolVersion}\r\n";
            //        str += $"Trap[{i}] community: {ntfBase.CommunityString}\r\n";
            //        str +=
            //            $"Trap[{i}] user: {ntfBase.User.Username} {ntfBase.User.Password} {ntfBase.User.EncryptionKey} {ntfBase.User.AuthAlgorithm} {ntfBase.User.EncrAlgorithm}\r\n";
            //    }
            //}

            //if (cbShowVariables.Checked)
            {
                var oids = _lib.GetListOfAllOids();
                str += $"\r\nTotal objects: {oids.Count}";

                try
                {
                    foreach (var oid in oids)
                    {
                        string oidString = $"{oid}";
                        string eventString = "";
                        try
                        {
                            IbaSnmpOid suff = _lib.GetOidSuffixForFullOid(oid);
                            if (suff != null)
                            {
                                oidString += $" (suff {suff})";
                                if (_lib.IsEventHandlerRegistered(suff))
                                {
                                    eventString = " [Has EVENT handler]";
                                }
                            }
                        }
                        catch
                        {
                            //
                        }

                        IbaSnmpValueType type;
                        object val = _lib.GetValue(oid, out type);
                        string valString = val?.ToString() ?? "<null>";

                        string typeString = $"{type}";

                        if (IbaSnmp.IsInEnumRegion(type) && _lib.IsEnumDataTypeRegistered(type))
                        {
                            string typeName = _lib.GetEnumDataTypeName(type);
                            typeString = $"{(int)type}={typeName}";

                            if (val is int)
                            {
                                string enumValueString = _lib.GetEnumValueName(type, (int)val) ?? "<???>";
                                valString += $" ({enumValueString})";
                            }
                        }

                        str += $"\r\n{oidString} = ({typeString}) {valString}{eventString}";
                    }

                }
                catch
                {
                    str += " [Ex]";
                }
            }

            return str;
        }

        #endregion


        #region IPropertyPane Members

        private SnmpData _data;

        private bool _isUserModeActive;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            _data = datasource as SnmpData; // clone of current Manager's data

            // if data is wrong, disable all controls, and cancel load
            Enabled = _data != null;
            if (_data == null)
            {
                return;
            }


            // let the manager know that GUI is visible
            // so GUI-specific things can be started suspended
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }
            //snmpWorker.IsGuiVisible = true;

            // todo
            label1.Text = $@"Data loaded {_tmp___cnt1++}";

            _isUserModeActive = false;

            // read from data to controls
            try
            {
                ConfigurationFromDataToControls();
                InitializeObjectsTree();
                snmpWorker.ApplyStatusToTextBox(tbStatus);
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }

            timerStatus.Enabled = true;
            _isUserModeActive = true;
        }

        public void SaveData()
        {
            // todo ask Michael. Save = Load * 2. why?
            label3.Text = $@"Data Saved {_tmp___cnt3++}";

            TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;
        }

        public void LeaveCleanup()
        {
            label2.Text = $@"Data cleaned {_tmp___cnt2++}";

            // let the manager know that GUI is not visible
            // so GUI-specific things can be suspended
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }
            //snmpWorker.IsGuiVisible = false;
            timerStatus.Enabled = false;
        }

        #endregion


        #region Configuration
        
        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                ApplyConfigurationToManager();
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonConfigurationReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                    "Are you sure you want to reset configuration to default?",
                    "Reset configuration?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            SnmpData defData = new SnmpData();

            // copy default data to current data
            // but not all data, just configuration data
            _data.Port = defData.Port;
            _data.V1V2Security = defData.V1V2Security;
            _data.V3Security = defData.V3Security;
            // do not reset enabled/disabled : _data.Enabled = ...

            try
            {
                ConfigurationFromDataToControls();
                ApplyConfigurationToManager();
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;
            _data.Port = (int)numPort.Value;
            // security v1 v2
            _data.V1V2Security = tbCommunity.Text;
            // security v3
            IbaSnmpUserAccount v3S = new IbaSnmpUserAccount();

            int indAuth = cmbAuthentication.SelectedIndex;
            v3S.AuthAlgorithm = indAuth == -1
                ? IbaSnmpAuthenticationAlgorithm.Md5 // default
                : (IbaSnmpAuthenticationAlgorithm)indAuth; // just cast position in the list to enum

            int indEncr = cmbEncryption.SelectedIndex;
            v3S.EncrAlgorithm = indEncr == -1
                ? IbaSnmpEncryptionAlgorithm.None // default
                : (IbaSnmpEncryptionAlgorithm)indEncr; // just cast position in the list to enum

            v3S.Username = tbUserName.Text;
            v3S.Password = tbPassword.Text;
            v3S.EncryptionKey = tbEncryptionKey.Text;

            _data.V3Security = v3S;
        }

        private void ConfigurationFromDataToControls()
        {
            // general
            cbEnabled.Checked = _data.Enabled;
            numPort.Value = _data.Port;
            // security v1 v2
            tbCommunity.Text = _data?.V1V2Security;
            // security v3
            var v3S = _data.V3Security;
            tbUserName.Text = v3S.Username;
            tbPassword.Text = v3S.Password;
            tbEncryptionKey.Text = v3S.EncryptionKey;
            cmbAuthentication.SelectedIndex = (int)v3S.AuthAlgorithm;
            cmbEncryption.SelectedIndex = (int)v3S.EncrAlgorithm;
        }

        // todo rename and move if it will concern objects also
        private void ApplyConfigurationToManager()
        {
            try
            {
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region Diagnostics

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            _tmp___cntTimer++;

            label4.Text = $@"Instance {_tmp___instCounter} " + (_tmp___cntTimer % 2 == 0 ? "|" : "-");

            IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;

            string str = "";
            str += ibaSnmp == null
                ? @"ibaSnmp == null"
                : _tmp___GetLibraryDescriptionString(ibaSnmp);

            tbDebug.Text = str;
        }
        
        private void SnmpWorker_StatusChanged(object sender, SnmpWorkerStatusChangedEventArgs e)
        {
            if (tbStatus.InvokeRequired)
            {
                Invoke(new EventHandler<SnmpWorkerStatusChangedEventArgs>(SnmpWorker_StatusChanged), sender, e);
            }
            else
            {
                tbStatus.BackColor = e.Color;
                tbStatus.Text = e.Message;
            }
        }

        #endregion


        #region Objects

        public void InitializeObjectsTree()
        {
            TreeNodeCollection nodes = tvObjects.Nodes;
            nodes.Clear();

            var worker = TaskManager.Manager?.SnmpWorker;
            if (worker == null)
            {
                return;
            }

            // todo get copy and work with copy
            lock (worker.LockObject)
            {
                var od = worker?.ObjectsData;
                IbaSnmp ibaSnmp = worker?.IbaSnmp;
                if (od == null || ibaSnmp == null) return;

                var nodeRoot = nodes.Add(ibaSnmp.OidIbaRoot.ToString());

                // root.0=Library
                var nodeLib = nodeRoot.Nodes.Add($"0. Library");
                //IbaSnmpOid oidLib = ibaSnmp.OidIbaSnmpLibInfo;

                nodeLib.Nodes.Add("1. Name");
                nodeLib.Nodes.Add("2. Version");
                nodeLib.Nodes.Add("3. Host name");
                nodeLib.Nodes.Add("4. System time");

                // root.2=DatCo
                var nodeCoord = nodeRoot.Nodes.Add($"{(int)IbaSnmpProductId.IbaDatCoordinator}. ibaDatCoordinator");

                // root.2=DatCo.0=General
                var nodeGen = nodeCoord.Nodes.Add("0. General");
                nodeGen.Nodes.Add("1. Title");
                nodeGen.Nodes.Add("2. Version");
                var nodeLic = nodeGen.Nodes.Add("3. Licensing");
                // todo lic


                // root.2=DatCo.1=Product
                var nodeProduct = nodeCoord.Nodes.Add("1. Product");

                // root.2=DatCo.1=Product.1=Cleanup
                var nodeGlobalCleanup = nodeProduct.Nodes.Add("1. GlobalCleanup");
                for (int i = 0; i < od.GlobalCleanup.Count; i++)
                {
                    var cleanupInfo = od.GlobalCleanup[i];
                    var driveNode = nodeGlobalCleanup.Nodes.Add($"{i+1}. Drive {cleanupInfo.DriveName}");

                    driveNode.Nodes.Add("0. DriveName: " + cleanupInfo.DriveName);
                    driveNode.Nodes.Add("1. Active: " + cleanupInfo.Active);
                    driveNode.Nodes.Add("2. Size: " + cleanupInfo.Size);
                    driveNode.Nodes.Add("3. CurrentFreeSpace: " + cleanupInfo.CurrentFreeSpace);
                    driveNode.Nodes.Add("4. MinFreeSpace: " + cleanupInfo.MinFreeSpace);
                    driveNode.Nodes.Add("5. RescanTime: " + cleanupInfo.RescanTime);
                }

                // root.2=DatCo.1=Product.2=StandardJobs
                var nodeStdJobs = nodeProduct.Nodes.Add($"2. Standard Jobs (total {od.StandardJobs.Count})");
                for (int i = 0; i < od.StandardJobs.Count; i++)
                {
                    var jobInfo = od.StandardJobs[i];
                    var jobNode = nodeStdJobs.Nodes.Add($"{i + 1}. {jobInfo.JobName}");

                    var jobGenNode = jobNode.Nodes.Add("0. General");
                    jobGenNode.Nodes.Add("0. Name: " + jobInfo.JobName);
                    jobGenNode.Nodes.Add("1. Status: " + jobInfo.Status);
                    jobGenNode.Nodes.Add("2. Todo #:" + jobInfo.TodoCount);
                    jobGenNode.Nodes.Add("3. Done #:" + jobInfo.DoneCount);

                    jobGenNode.Nodes.Add("4. Failed #:" + jobInfo.FailedCount);
                    jobGenNode.Nodes.Add("5. Perm.Failed #:" + jobInfo.PermFailedCount);

                    jobGenNode.Nodes.Add("6. Perm.Failed #:" + jobInfo.TimestampJobStarted);
                    jobGenNode.Nodes.Add("7. Perm.Failed #:" + jobInfo.LastCycleScanningTime);

                    jobGenNode.Nodes.Add("8.0. LastProcessingLastDatFileProcessed:" + jobInfo.LastProcessingLastDatFileProcessed);
                    jobGenNode.Nodes.Add("8.1. LastProcessingStartTimeStamp:" + jobInfo.LastProcessingStartTimeStamp);
                    jobGenNode.Nodes.Add("8.2. LastProcessingFinishTimeStamp:" + jobInfo.LastProcessingFinishTimeStamp);

                    AddTasksToTree(jobNode, jobInfo.Tasks);
                }

                // root.2=DatCo.1=Product.3=ScheduledJobs
                var nodeSchJobs = nodeProduct.Nodes.Add($"3. Scheduled Jobs (total {od.ScheduledJobs.Count})");
                for (int i = 0; i < od.ScheduledJobs.Count; i++)
                {
                    var jobInfo = od.ScheduledJobs[i];
                    var jobNode = nodeSchJobs.Nodes.Add($"{i + 1}. {jobInfo.JobName}");

                    var jobGenNode = jobNode.Nodes.Add("0. General");
                    jobGenNode.Nodes.Add("0. Name: " + jobInfo.JobName);

                    AddTasksToTree(jobNode, jobInfo.Tasks);
                }

                // root.2=DatCo.1=Product.4=OneTimeJobs
                var nodeOtJobs = nodeProduct.Nodes.Add($"4. One Time Jobs (total {od.OneTimeJobs.Count}):");
                for (int i = 0; i < od.OneTimeJobs.Count; i++)
                {
                    var jobInfo = od.OneTimeJobs[i];
                    var jobNode = nodeOtJobs.Nodes.Add($"{i + 1}. {jobInfo.JobName}");

                    var jobGenNode = jobNode.Nodes.Add("0. General");
                    jobGenNode.Nodes.Add("0. Name: " + jobInfo.JobName);

                    AddTasksToTree(jobNode, jobInfo.Tasks);
                }

                nodeRoot.Expand();
                nodeCoord.Expand();
                nodeProduct.Expand();
                nodeGlobalCleanup.Expand();
                nodeStdJobs.Expand();
                nodeSchJobs.Expand();
                nodeOtJobs.Expand();
                //tvObjects.ExpandAll();
            }
        }

        private void AddTasksToTree(TreeNode parent, List<SnmpObjectsData.TaskInfo> tasks)
        {
            var jobTasksNode = parent.Nodes.Add($"1. Tasks (total {tasks?.Count})");

            if (tasks == null)
            {
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                SnmpObjectsData.TaskInfo taskInfo = tasks[i];
                AddTaskToTree(i + 1, jobTasksNode, taskInfo);
            }
        }

        private void AddTaskToTree(int index, TreeNode parent, SnmpObjectsData.TaskInfo taskInfo)
        {
            TreeNode taskNode = parent.Nodes.Add($"{index}. {taskInfo.TaskName}");
            taskNode.Nodes.Add("0. TaskName: " + taskInfo.TaskName);
            taskNode.Nodes.Add("1. Tasktype : " + taskInfo.TaskType);
            taskNode.Nodes.Add("2. Success: " + taskInfo.Success);
            taskNode.Nodes.Add("3. DurationOfLastExecution: " + taskInfo.DurationOfLastExecution);
            taskNode.Nodes.Add("4. CurMemoryUsed: " + taskInfo.CurrentMemoryUsed);

            var ci = taskInfo.CleanupInfo;
            if (ci == null)
            {
                return;
            }

            TreeNode cleanupNode = taskNode.Nodes.Add("5. Cleanup");
            cleanupNode.Nodes.Add("0. LimitChoice:" + ci.LimitChoice);
            cleanupNode.Nodes.Add("1. Subdirectories:" + ci.Subdirectories);
            cleanupNode.Nodes.Add("2. FreeDiskSpace:" + ci.FreeDiskSpace);
            cleanupNode.Nodes.Add("3. UsedDiskSpace:" + ci.UsedDiskSpace);
        }

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tbObjValue.Text = tbObjOid.Text = e.Node.Text;
            return;
            // todo add oid to tag
            var worker = TaskManager.Manager?.SnmpWorker;
            if (worker == null)
            {
                return;
            }
            var od = worker?.ObjectsData;
            IbaSnmp ibaSnmp = worker?.IbaSnmp;
            if (od == null || ibaSnmp == null) return;

            IbaSnmpOid oid = e.Node.Tag as IbaSnmpOid;

            tbObjOid.Text = "";
            tbObjValue.Text = "";

            if (oid == null) return;

            tbObjOid.Text = oid.ToString();
            var val = ibaSnmp.GetUserValue(oid, false);
            tbObjValue.Text = val?.ToString();
        }

        private void buttonObjectsRefresh_Click(object sender, EventArgs e)
        {
            InitializeObjectsTree();
        }

        #endregion


        private void buttonDebugRefresh_Click(object sender, EventArgs e)
        {
            var man = TaskManager.Manager;
            SnmpWorker snmpWorker = man?.SnmpWorker;

            snmpWorker?.RefreshObjectData();
        }


    }
}
