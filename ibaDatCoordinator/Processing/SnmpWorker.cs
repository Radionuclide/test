﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using IbaSnmpLib;

namespace iba.Processing
{
    public enum SnmpWorkerStatus
    {
        Started,
        Stopped,
        Errored,
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public SnmpWorkerStatus Status { get; }
        public Color Color { get; }
        public string Message { get; }
        public StatusChangedEventArgs(SnmpWorkerStatus status, Color color, string message)
        {
            Status = status;
            Color = color;
            Message = message;
        }
    }


    public class SnmpWorker
    {

        #region Construction, Destruction, Init

        public SnmpWorker()
        {
            Status = SnmpWorkerStatus.Stopped;
            // todo move to reeource?
            StatusString = "Waiting for delayed initialisation...";

            new Task(DelayedInitialisation).Start();
        }

        private void DelayedInitialisation()
        {
            int delay = 2;
            for (int i = delay - 1; i >= 0; i--)
            {
                Thread.Sleep(1000);
                // todo move to reeource?
                StatusString = $"Waiting for delayed initialisation, {i} second(s)...";
                StatusChanged?.Invoke(this,
                    new StatusChangedEventArgs(Status, StatusToColor(Status), StatusString));
            }

            IbaSnmp = new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);
            IbaSnmp.DosProtectionInternal.Enabled = false;
            RestartAgent();

            RegisterGeneralObjectHandlers();
            RegisterProductObjects();
        }

        #endregion

        public IbaSnmp IbaSnmp { get; private set; }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private SnmpData _snmpData;
        public SnmpData SnmpData
        {
            get { return _snmpData; }
            set
            {
                // todo probabaly override Equals, because def implementation does not work good enough for this
                //  if (_snmpData != null && _snmpData.Equals(value))
                if (_snmpData != null && _snmpData.Equals(value))
                {
                    return;
                }
                _snmpData = value;

                if (IbaSnmp != null)
                {
                    RestartAgent();
                }
            }
        }

        public SnmpWorkerStatus Status { get; private set; }
        public string StatusString { get; private set; }

        public void ApplyStatusToTextBox(TextBox tb)
        {
            tb.Text = StatusString;
            tb.BackColor = StatusToColor(Status);
        }

        public static Color StatusToColor(SnmpWorkerStatus status)
        {
            return status == SnmpWorkerStatus.Started
                ? Color.LimeGreen // running
                : (status == SnmpWorkerStatus.Stopped
                    ? Color.LightGray // stopped
                    : Color.Red); // error
        }

        /// <summary> Whether Snmp Control is currently displayed in the main form window </summary>
        //public bool IsGuiVisible { get; set; }

        public void RestartAgent()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaSnmp.Stop();
                Status = SnmpWorkerStatus.Stopped;
                // todo to resource
                StatusString = "SNMP server is disabled";

                ApplyConfigurationToIbaSnmp();

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                    Status = SnmpWorkerStatus.Started;
                    // todo to resource
                    StatusString = $"SNMP server running on port {_snmpData.Port}";
                    //SNMP server is disabled
                }
            }
            catch (Exception ex)
            {
                Status = SnmpWorkerStatus.Errored;
                // todo to resource
                StatusString = $"Starting the SNMP server failed with error: {ex.Message}";
            }

            // trigger status event
            StatusChanged?.Invoke(this,
                new StatusChangedEventArgs(Status, StatusToColor(Status), StatusString));

        }

        private void ApplyConfigurationToIbaSnmp()
        {
            if (IbaSnmp == null || SnmpData == null)
            {
                return;
            }

            // apply port, do not change ip addresses
            List<IPEndPoint> eps = IbaSnmp.EndPointsToListen;
            foreach (IPEndPoint ipe in eps)
            {
                ipe.Port = SnmpData.Port;
            }
            IbaSnmp.EndPointsToListen = eps;

            // security
            IbaSnmp.SetSecurityForV1AndV2(new List<string> { SnmpData.V1V2Security });
            IbaSnmp.SetSecurityForV3(new List<IbaSnmpUserAccount> { SnmpData.V3Security });

            // todo apply objects
            //SnmpData.
        }

        #region Objects

        #region General objects

        private void RegisterGeneralObjectHandlers()
        {
            // static
            IbaSnmp.ValueIbaProductGeneralTitle = "ibaDatCoordinator";
            var ver = GetType().Assembly.GetName().Version;
            IbaSnmp.SetValueIbaProductGeneralVersion(ver.Major, ver.Minor, ver.Build, null);

            // dynamic
            IbaSnmp.UpTimeRequested += IbaSnmp_UpTimeRequested;
            IbaSnmp.LicensingCustomerRequested += IbaSnmp_LicensingCustomerRequested;
            IbaSnmp.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingDemoTimeLimitRequested;
            IbaSnmp.LicensingHwIdRequested += IbaSnmp_LicensingHwIdRequested;
            IbaSnmp.LicensingIsValidRequested += IbaSnmp_LicensingIsValidRequested;
            IbaSnmp.LicensingSnRequested += IbaSnmp_LicensingSnRequested;
            IbaSnmp.LicensingTimeLimitRequested += IbaSnmp_LicensingTimeLimitRequested;
            IbaSnmp.LicensingTypeRequested += IbaSnmp_LicensingTypeRequested;
        }

        private void IbaSnmp_UpTimeRequested(object sender, IbaSnmpValueRequestedEventArgs<uint> e)
        {
            // todo override?
        }

        private void IbaSnmp_LicensingCustomerRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                e.Value = info.DongleFound ? info.Customer : "";
            }
            catch { /**/ }
        }
        private void IbaSnmp_LicensingDemoTimeLimitRequested(object sender, IbaSnmpValueRequestedEventArgs<int> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingHwIdRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingIsValidRequested(object sender, IbaSnmpValueRequestedEventArgs<bool> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                // todo is it reaaly this?
                e.Value = info.DongleFound;
            }
            catch { /**/ }
        }

        private void IbaSnmp_LicensingSnRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                e.Value = info.DongleFound ? info.SerialNr : "";
            }
            catch { /**/ }
        }

        private void IbaSnmp_LicensingTimeLimitRequested(object sender, IbaSnmpValueRequestedEventArgs<int> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingTypeRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            // todo
        }


        #endregion

        #region Dat coordinator specific objects

        internal SnmpObjectsData ObjectsData { get; private set; } = new SnmpObjectsData();

        /// <summary> Lock this object while using SnmpWorker.ObjectsData </summary>
        public readonly object LockObject = new object();

        public TimeSpan SnmpObjectsDataValidTimePeriod { get; } = TimeSpan.FromSeconds(2);

        public void RefreshObjectData()
        {
            lock (LockObject)
            {
                if (!IsObjectsDataUpToDate())
                {
                    RebuildTreeCompletely();
                }
            }
        }
        private void RebuildTreeCompletely()
        {
            var man = TaskManager.Manager;
            IbaSnmp ibaSnmp = man?.SnmpWorker.IbaSnmp;
            if (ibaSnmp == null)
                return;

            lock (LockObject)
            {
                // todo probabaly not fully recreate but refresh ?
                man.SnmpGetStatus(ObjectsData);

                // todo this is a probabale dead lock?
                // todo needa look where from does this call comes? 
                // todo wheter we are in lock of ibaSnmp.Dict...
                ibaSnmp.DeleteAllUserValues();

                IbaSnmpOid oidStdJobs = "2";

                for (int i = 0; i < ObjectsData.StandardJobs.Count; i++)
                {
                    IbaSnmpOid oidJob = oidStdJobs + (uint)(i + 1);
                    IbaSnmpOid oidJobGen = oidJob + 0;
                    IbaSnmpOid oidJobTasks = oidJob + 1;
                    SnmpObjectsData.StandardJobInfo jobInfo = ObjectsData.StandardJobs[i];

                    // todo add enum
                    ibaSnmp.CreateUserValue(oidJobGen + 0, jobInfo.JobName, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 1, jobInfo.Status.ToString(), null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 2, jobInfo.TodoCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 3, jobInfo.DoneCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 4, jobInfo.FailedCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 5, jobInfo.PermFailedCount, null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 6, jobInfo.TimestampJobStarted, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 7, jobInfo.LastCycleScanningTime, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + "8.0", jobInfo.LastProcessingLastDatFileProcessed, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + "8.2", jobInfo.LastProcessingFinishTimeStamp, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + "8.1", jobInfo.LastProcessingStartTimeStamp, null, null, UserValueRequested);

                    AddTasks(ibaSnmp, oidJobTasks, jobInfo.Tasks);
                }

                IbaSnmpOid oidSchJobs = "3";
                for (int i = 0; i < ObjectsData.ScheduledJobs.Count; i++)
                {
                    IbaSnmpOid oidJob = oidSchJobs + (uint)(i + 1);
                    IbaSnmpOid oidJobGen = oidJob + 0;
                    IbaSnmpOid oidJobTasks = oidJob + 1;

                    var jobInfo = ObjectsData.ScheduledJobs[i];

                    // todo add enum
                    ibaSnmp.CreateUserValue(oidJobGen + 0, jobInfo.JobName, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 1, jobInfo.Status.ToString(), null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 2, jobInfo.TodoCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 3, jobInfo.DoneCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 4, jobInfo.FailedCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 5, jobInfo.PermFailedCount, null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 6, jobInfo.TimestampLastExecution, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 7, jobInfo.TimestampNextExecution, null, null, UserValueRequested);

                    AddTasks(ibaSnmp, oidJobTasks, jobInfo.Tasks);
                }

                IbaSnmpOid oidOtJobs = "4";
                for (int i = 0; i < ObjectsData.OneTimeJobs.Count; i++)
                {
                    IbaSnmpOid oidJob = oidOtJobs + (uint)(i + 1);
                    IbaSnmpOid oidJobGen = oidJob + 0;
                    IbaSnmpOid oidJobTasks = oidJob + 1;

                    var jobInfo = ObjectsData.OneTimeJobs[i];

                    ibaSnmp.CreateUserValue(oidJobGen + 0, jobInfo.JobName, null, null, UserValueRequested);
                    // todo add enum
                    ibaSnmp.CreateUserValue(oidJobGen + 1, jobInfo.Status.ToString(), null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 2, jobInfo.TodoCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 3, jobInfo.DoneCount, null, null, UserValueRequested);
                    ibaSnmp.CreateUserValue(oidJobGen + 4, jobInfo.FailedCount, null, null, UserValueRequested);

                    ibaSnmp.CreateUserValue(oidJobGen + 5, jobInfo.TimestampLastExecution, null, null, UserValueRequested);

                    AddTasks(ibaSnmp, oidJobTasks, jobInfo.Tasks);
                }


                ibaSnmp.CreateUserValue("0.1", "Stamp=" + ObjectsData.Stamp.ToString(CultureInfo.InvariantCulture),
                    null,null,  Val_Stamp_Requested);
                ibaSnmp.CreateUserValue("0.2", "Reset=" + ObjectsData._tmp_updated_cnt,
                    null, null, Val_Reset_Requested);
                ibaSnmp.CreateUserValue("0.3", "Updated=" + ObjectsData._tmp_updated_cnt,
                    null, null, Val_Updated_Requested);

            }
        }

        private void AddTasks(IbaSnmp ibaSnmp, IbaSnmpOid parent, List<SnmpObjectsData.TaskInfo> tasks)
        {
            if (tasks == null)
            {
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                SnmpObjectsData.TaskInfo taskInfo = tasks[i];
                AddTask(ibaSnmp, parent + (uint)(i + 1), taskInfo);
            }
        }
        private void AddTask(IbaSnmp ibaSnmp, IbaSnmpOid parent, SnmpObjectsData.TaskInfo taskInfo)
        {
            ibaSnmp.CreateUserValue(parent + 0, taskInfo.TaskName, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + 1, taskInfo.TaskType, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + 2, taskInfo.Success, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + 3, taskInfo.DurationOfLastExecution, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + 4, taskInfo.CurrentMemoryUsed, null, null, UserValueRequested);

            var ci = taskInfo.CleanupInfo;
            if (ci == null)
            {
                return;
            }

            ibaSnmp.CreateUserValue(parent + "5.0", ci.LimitChoice.ToString(), null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + "5.1", ci.Subdirectories, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + "5.2", ci.FreeDiskSpace, null, null, UserValueRequested);
            ibaSnmp.CreateUserValue(parent + "5.3", ci.UsedDiskSpace, null, null, UserValueRequested);
        }

        private bool IsObjectsDataUpToDate()
        {
            if (ObjectsData.Stamp == DateTime.MinValue)
            {
                return false;
            }
            TimeSpan age = DateTime.Now - ObjectsData.Stamp;
            // if data is not too old, then it is okay
            return age < SnmpObjectsDataValidTimePeriod;
        }

        private void UserValueRequested(object sender, IbaSnmpObjectValueRequestedEventArgs ibaSnmpObjectValueRequestedEventArgs)
        {
            // refresh data if it is too old, and rebuild all the objects if necessary
            RefreshObjectData();
        }

        private void RegisterProductObjects()
        {
            // todo should be done differently
            RefreshObjectData();
        }

        private void Val_Stamp_Requested(object sender, IbaSnmpObjectValueRequestedEventArgs e)
        {
            e.Value = "Stamp=" + ObjectsData.Stamp.ToString(CultureInfo.InvariantCulture);
        }
        private void Val_Reset_Requested(object sender, IbaSnmpObjectValueRequestedEventArgs e)
        {
            e.Value = "Reset=" + ObjectsData._tmp_updated_cnt;
        }
        private void Val_Updated_Requested(object sender, IbaSnmpObjectValueRequestedEventArgs e)
        {
            e.Value = "Updated=" + ObjectsData._tmp_updated_cnt;
        }

        #endregion

        #endregion

    }
}
