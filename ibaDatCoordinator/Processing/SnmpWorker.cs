using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using iba.Data;
using iba.Logging;
using iba.Properties;
using IbaSnmpLib;

// all verbatim strings that are in the file (e.g. @"General") should NOT be localized.
// usual strings (e.g. "General") should be localized later.

namespace iba.Processing
{

    #region Helper classes

    public enum ExtMonWorkerStatus
    {
        Started,
        Stopped,
        Errored
    }
    
    /// <summary> Is used to send MIB file contents from Server to Client via remoting </summary>
    [Serializable]
    public struct SnmpMibFileContainer
    {
        /// <summary> Recommended filename NOT including path </summary>
        public string FileName { get; set; }

        /// <summary> String that represent file's contents. See <see cref="IbaSnmpMibGenerator"/> for details. </summary>
        public string Contents { get; set; }
    }

    #endregion


    public class SnmpWorker
    {
        
        #region Construction, Destruction, Init

        public SnmpWorker()
        {
            Status = ExtMonWorkerStatus.Errored;
            StatusString = Resources.snmpStatusNotInit;
        }

        public void Init()
        {
            if (IbaSnmp != null)
            {
                // disable double initialization
                return;
            }

            IbaSnmp = new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);

            IbaSnmp.DosProtectionInternal.Enabled = false;
            IbaSnmp.DosProtectionExternal.Config(5000, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60));
            RestartAgent();

            // subscribe to tree structure changes 
            ExtMonInstance.ExtMonStructureChanged += (sender, args) => RebuildTree();

            RegisterEnums();
            SetGeneralProductInformation();
        }

        #endregion


        #region Configuration of SNMP agent (IbaSnmp libraray)

        public IbaSnmp IbaSnmp { get; private set; }

        private SnmpData _snmpData = new SnmpData();

        public SnmpData SnmpData
        {
            get => _snmpData;
            set
            {
                if (value == null)
                {
                    // do not allow to set null data here
                    return;
                }
                if (_snmpData != null && _snmpData.Equals(value))
                {
                    // Configuration has not changed
                    // do not restart agent
                    return;
                }
                _snmpData = value;

                if (IbaSnmp != null)
                {
                    RestartAgent();
                }
            }
        }

        public ExtMonWorkerStatus Status { get; private set; }

        public string StatusString { get; private set; }

        public void RestartAgent()
        {
            var oldStatus = Status;
            Status = ExtMonWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaSnmp.Stop();
                ApplyConfigurationToIbaSnmp();
                string logMessage;

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                    Status = ExtMonWorkerStatus.Started;
                    StatusString = String.Format(Resources.snmpStatusRunningOnPort, _snmpData.Port);

                    logMessage = Status == oldStatus
                        ?
                       // log 'was restarted' if status has not changed (now is 'Started' as before) 
                       String.Format(Resources.snmpStatusRunningRestarted, StatusString)
                        :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        String.Format(Resources.snmpStatusRunningStarted, StatusString);
                }
                else
                {
                    Status = ExtMonWorkerStatus.Stopped;
                    StatusString = Resources.snmpStatusDisabled;

                    logMessage = Status == oldStatus
                        ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null
                        :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        String.Format(Resources.snmpStatusStopped, StatusString);
                }

                // log the message if it necessary
                if (logMessage != null)
                {
                    LogData.Data.Logger.Log(Level.Info, logMessage);
                }
            }
            catch (Exception ex)
            {
                Status = ExtMonWorkerStatus.Errored;
                StatusString = String.Format(Resources.snmpStatusError, ex.Message);
                if (LogData.Data.Logger.IsOpen) LogData.Data.Logger.Log(Level.Exception, StatusString);
            }
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
            IbaSnmp.SetSecurityForV1AndV2(new List<string> {SnmpData.V1V2Security});
            IbaSnmp.SetSecurityForV3(new List<IbaSnmpUserAccount> {SnmpData.V3Security});
        }

        public bool UseSnmpV2TcForStrings => SnmpData?.UseSnmpV2TcForStrings ?? true;

        #endregion


        #region Handling object tree - building, refreshing

        /// <summary> A quick reference to singleton <see cref="ExtMonData"/> </summary>
        private static ExtMonData ExtMonInstance => ExtMonData.Instance;

        /// <summary> A quick reference to <see cref="ExtMonData"/> instance lock </summary>
        private static object LockObject => ExtMonInstance.LockObject;

        /// <summary> A quick reference to <see cref="ExtMonData"/> recommended lock timeout </summary>
        private static int LockTimeout => ExtMonData.LockTimeout;


        #region Register enums

        private Dictionary<string, IbaSnmpValueType> _registeredEnums;

        private void RegisterEnums()
        {
            _registeredEnums = new Dictionary<string, IbaSnmpValueType>();
            foreach (var kvp in ExtMonData.RegisteredEnums)
            {
                var desc = kvp.Value;
                // change value descriptions to format needed for ibaSnmp lib
                Dictionary<int, string> valNames = new Dictionary<int, string>();
                foreach (var valKvp in desc.Values)
                {
                    valNames.Add(valKvp.Key,valKvp.Value.Name);
                }
                IbaSnmpValueType type = IbaSnmp.RegisterEnumDataType(desc.SnmpMibName, desc.Description, valNames);

                _registeredEnums.Add(kvp.Key, type);
            }
        }

        #endregion


        #region General product information

        /// <summary> Sets product name and version. Registers license value handlers. </summary>
        private void SetGeneralProductInformation()
        {
            // change default gui caption from "ibaDatCo" to "ibaDatCoordinator"
            IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProduct, @"ibaDatCoordinator");

            // all the other captions can be left as is
            // all the general objects already have gui Captions, predefined
            // e.g.:
            // var captionSample = IbaSnmp.GetOidMetadata(IbaSnmp.OidIbaProductGeneralLicensingCustomer).GuiCaption;

            // ibaRoot.DatCoord.General.1 - Title
            //IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProductGeneralTitle, @"Title");
            IbaSnmp.ValueIbaProductGeneralTitle = @"ibaDatCoordinator";

            // ibaRoot.DatCoord.General.2 - Version
            var ver = GetType().Assembly.GetName().Version;
            IbaSnmp.SetValueIbaProductGeneralVersion(ver.Major, ver.Minor, ver.Build);

            // ibaRoot.DatCoord.General.Licensing.1 - IsValid
            IbaSnmp.LicensingIsValidRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.2 - Serial number
            IbaSnmp.LicensingSnRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.3 - Hardware ID
            IbaSnmp.LicensingHwIdRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.4 - Dongle type
            IbaSnmp.LicensingTypeRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.5 - Customer
            IbaSnmp.LicensingCustomerRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.6 - Time limit
            IbaSnmp.LicensingTimeLimitRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.7 - Demo time limit
            IbaSnmp.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingValueRequested;
        }

        #endregion


        #region Building the tree

        public bool RebuildTree()
        {
            ExtMonData.DebugWriteLine(nameof(SnmpWorker), "RebuildTree (start)");


            var man = TaskManager.Manager;
            if (man == null || IbaSnmp == null)
            {
                return false; // rebuild failed
            }

            ExtMonData.DebugWriteLine(nameof(SnmpWorker), "RebuildTree (before lock)");
            // here I use a multiple of the normal timeout to give an overwhelming priority over request-item locks
            if (Monitor.TryEnter(LockObject, LockTimeout * 20))
            {
                ExtMonData.DebugWriteLine(nameof(SnmpWorker), "RebuildTree (after lock)");
                try
                {
                    IbaSnmp.DeleteAllUserValues();
                    IbaSnmp.DeleteAllUserOidMetadata();

                    if (!ExtMonInstance.IsStructureValid)
                    {
                        // ExtMonData structure is invalid;
                        // Normally it should not happen, because usually we rebuild our tree on reaction to ExtMon structure change.
                        // Anyway, it makes no sense to rebuild our tree
                        return false; // rebuild failed
                    }

                    // ibaRoot.DatCoord.1 - Product-Specific
                    IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProductSpecific, "Product-specific");

                    // ibaRoot.DatCoord.Product.1 - Global cleanup
                    BuildFolderRecursively(ExtMonInstance.FolderGlobalCleanup);
                    // ibaRoot.DatCoord.Product.2 - Standard jobs
                    BuildFolderRecursively(ExtMonInstance.FolderStandardJobs);
                    // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                    BuildFolderRecursively(ExtMonInstance.FolderScheduledJobs);
                    // ibaRoot.DatCoord.Product.4 - One time jobs
                    BuildFolderRecursively(ExtMonInstance.FolderOneTimeJobs);
                    // ibaRoot.DatCoord.Product.5 - Event jobs
                    BuildFolderRecursively(ExtMonInstance.FolderEventBasedJobs);
                    // ibaRoot.DatCoord.Product.6 - Computed values
                    BuildFolderRecursively(ExtMonInstance.FolderComputedValues);

                    ExtMonData.DebugWriteLine(nameof(SnmpWorker), "RebuildTree (success)");
                    return true; // rebuilt successfully
                }
                finally
                {
                    ExtMonData.DebugWriteLine(nameof(SnmpWorker), "RebuildTree (lock exit)");
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    ExtMonData.DebugWriteLine(nameof(SnmpWorker), "WARNING! Failed to acquire a lock when rebuilding the tree");
                    // 'Level.Warning' because we have a big timeout here, so, normally it should not happen 
                    // (though not critical, but may indicate that something is wrong) 
                    LogData.Data.Logger.Log(Level.Warning,
                        $"{nameof(SnmpWorker)}. Failed to acquire a lock when rebuilding the tree, {ExtMonData.GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                // failed to rebuild; mark tree invalid to rebuild it later
                ExtMonInstance.IsStructureValid = false;

                return false; // rebuild failed
            }
        }

        private void BuildFolderRecursively(ExtMonData.ExtMonFolder startingFolder)
        {
            try
            {
                SetOidMetadata(startingFolder);
                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case ExtMonData.ExtMonFolder extMonFolder:
                            BuildFolderRecursively(extMonFolder);
                            // mark that the group is fresh (all SNMP values were set just now)
                            if (extMonFolder is ExtMonData.ExtMonGroup xmGroup)
                                xmGroup.SnmpTimeStamp.PutStamp();
                            break;
                        case ExtMonData.ExtMonVariableBase extMonVariableBase:
                            CreateUserValue(extMonVariableBase);
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch 
            {
                // go on with other items 
                // even if current one has failed 
            }

        }


        #region SetOidMetadata and CreateUserValue() 

        private void SetOidMetadata(ExtMonData.ExtMonFolder xmf)
        {
            Debug.Assert(xmf.SnmpFullOid != null);
            IbaSnmp.SetUserOidMetadata(xmf.SnmpFullOid, xmf.SnmpFullMibName, xmf.Description, xmf.Caption);
        }

        private static readonly Dictionary<Type, IbaSnmpValueType> TYPE_DICT = new Dictionary<Type, IbaSnmpValueType>
        {
            {typeof(string), IbaSnmpValueType.String},
            {typeof(bool), IbaSnmpValueType.Bool},
            //{typeof(DateTime), IbaSnmpValueType.DateStr} /*should be handled specially*/,
            {typeof(DateTime), IbaSnmpValueType.DateTimeStr},
            //{typeof(DateTime), IbaSnmpValueType.DateTimeTc} /*should be handled specially*/,
            {typeof(int), IbaSnmpValueType.Int32},
            {typeof(uint), IbaSnmpValueType.Uint32},
            {typeof(float), IbaSnmpValueType.FloatStr},
            //{typeof(float), IbaSnmpValueType.FloatTc} /*should be handled specially*/,
            {typeof(double), IbaSnmpValueType.DoubleStr},
            //{typeof(double), IbaSnmpValueType.DoubleTc} /*should be handled specially*/,
            {typeof(IbaSnmpOid), IbaSnmpValueType.Oid}
        };

        public IbaSnmpValueType GetSnmpType(object value)
        {
            var type = TYPE_DICT.TryGetValue(value.GetType(), out IbaSnmpValueType snmpType) ? snmpType : IbaSnmpValueType.Unknown;
            // handle a special case of possible different date formats
            if (type == IbaSnmpValueType.DateTimeStr && UseSnmpV2TcForStrings)
                type = IbaSnmpValueType.DateTimeTc;
            return type;
        }

        private void CreateUserValue(ExtMonData.ExtMonVariableBase xmv)
        {
            Debug.Assert(xmv.Parent != null);
            Debug.Assert(xmv.SnmpFullOid != null);

            IbaSnmpValueType type;
            if (xmv.ObjValue.GetType().IsEnum)
            {
                string typeName = xmv.ObjValue.GetType().Name;

                if (ExtMonData.RegisteredEnums.TryGetValue(typeName, out ExtMonData.EnumDescription _))
                {
                    Debug.Assert(_registeredEnums.ContainsKey(typeName));
                    type = _registeredEnums[typeName];

                    IbaSnmp.CreateEnumUserValue(xmv.SnmpFullOid, type, (int)xmv.ObjValue, null, null,
                        ProductSpecificItemRequested, xmv.GetGroup());
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(xmv.ObjValue), @"Unsupported enum type");
                }
            }
            else // simple types
            {
                type = GetSnmpType(xmv.ObjValue);
                Debug.Assert(type != IbaSnmpValueType.Unknown);

                IbaSnmp.CreateUserValue(xmv.SnmpFullOid, xmv.ObjValue, type, null, null,
                    ProductSpecificItemRequested, xmv.GetGroup() );
            }

            IbaSnmp.SetUserOidMetadata(xmv.SnmpFullOid, xmv.SnmpFullMibName, xmv.Description, xmv.Caption);
        }

        #endregion


        #endregion


        #region Value-Refresh functions  

        private void SetUserValue(ExtMonData.ExtMonVariableBase xmv)
        {
            // IbaSnmp does not have untyped version of SetUserValue(),
            // so we have to use some workaround here.
            // Either "type dictionary + delegate wrapping" or "switch-case".
            // Since we have mainly only FOUR types (str, DateTime, bool, uint) in use
            // in our current implementation, it's more efficient to use "switch-case". 

            switch (xmv.ObjValue)
            {
                case string val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case uint val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case bool val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case DateTime val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case ExtMonData.JobStatus _:
                case TaskWithTargetDirData.OutputLimitChoiceEnum _:
                case TaskData.WhenToDo _:
                    // ReSharper disable once PossibleInvalidCastException
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, (int)xmv.ObjValue);
                    break;

                // other types (below) are not used in current implementations
                // and stand here for possible future additions.

                case int val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case float val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case double val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                default:
                    // should not happen
                    Debug.Assert(false);
                    break;
            }
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool RefreshLicenseInfo()
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    ExtMonData.LicenseInfo licGroup = ExtMonInstance.License;

                    // check if SNMP data is fresh enough
                    if (licGroup.SnmpTimeStamp.IsUpToDate)
                    {
                        // is fresh; no need to copy it from extMonData or to request it from TaskManager
                        // just return
                        return false; // was not updated
                    }

                    // SNMP data is outdated
                    // we should copy it from extMonGroup; 

                    // first check if extMonGroup itself is fresh enough
                    if (!licGroup.TimeStamp.IsUpToDate)
                    {
                        // extMonGroup is not fresh enough;
                        // request fresh data from TaskManager

                        var man = TaskManager.Manager;
                        if (!man.ExtMonRefreshLicenseInfo(licGroup))
                        {
                            // should not happen
                            // failed to update data
                            // don't rebuild the tree, just return false
                            return false; // was not updated
                        }
                    }

                    // memorize the moment when data was last copied to SNMP variables
                    licGroup.SnmpTimeStamp.PutStamp();

                    // copy values from extMonGroup to SNMP variables
                    IbaSnmp.ValueIbaProductGeneralLicensingIsValid = licGroup.IsValid.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingSn = licGroup.Sn.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingHwId = licGroup.HwId.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingType = licGroup.DongleType.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingCustomer = licGroup.Customer.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingTimeLimit = licGroup.TimeLimit.Value;
                    IbaSnmp.ValueIbaProductGeneralLicensingDemoTimeLimit = licGroup.DemoTimeLimit.Value;
                    
                    return true; // data was updated
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    ExtMonData.DebugWriteLine(nameof(SnmpWorker), "Failed to acquire a lock when updating license");
                    LogData.Data.Logger.Log(Level.Debug,
                        $"{nameof(SnmpWorker)}. Failed to acquire a lock when updating license, {ExtMonData.GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // was not updated
            }
        }

        private bool RefreshGroup(ExtMonData.ExtMonGroup xmGroup)
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (ExtMonInstance.RebuildTreeIfItIsInvalid())
                    {
                        // tree was rebuilt completely
                        // no need to update some parts of it
                        // just return right now
                        return true; // data was updated
                    }

                    // check if SNMP data (values inside SNMP objects) is fresh enough
                    if (xmGroup.SnmpTimeStamp.IsUpToDate)
                    {
                        // is fresh; no need to copy it from extMonData or to request it from TaskManager
                        // just return
                        return false; // data was NOT updated
                    }

                    // SNMP data is outdated
                    // we should copy it from extMonGroup; 

                    // first check if extMonGroup itself is fresh enough
                    if (!xmGroup.TimeStamp.IsUpToDate)
                    {
                        // extMonGroup is not fresh enough;
                        // request fresh data from TaskManager

                        var man = TaskManager.Manager;

                        bool bSuccess;
                        switch (xmGroup)
                        {
                            case ExtMonData.GlobalCleanupDriveInfo driveInfo:
                                bSuccess = man.ExtMonRefreshGlobalCleanupDriveInfo(driveInfo);
                                break;
                            case ExtMonData.JobInfoBase jobInfo:
                                bSuccess = man.ExtMonRefreshJobInfo(jobInfo);
                                break;
                            default:
                                // should not happen
                                Debug.Assert(false);
                                bSuccess = false;
                                break;
                        }

                        if (!bSuccess)
                        {
                            // should not happen; failed to update the data;
                            // mark tree invalid to rebuild it later
                            LogData.Data.Logger.Log(Level.Debug,
                                $"{nameof(SnmpWorker)}.{nameof(RefreshGroup)}. Failed to refresh group {xmGroup.Caption}; tree is marked invalid.");
                            ExtMonInstance.IsStructureValid = false;
                            return false; // data was NOT updated
                        }
                    }

                    // memorize the moment when data was last copied to SNMP variables
                    xmGroup.SnmpTimeStamp.PutStamp();

                    // copy values from extMonGroup to SNMP variables
                    foreach (var xmv in xmGroup.GetFlatListOfAllVariables())
                    {
                        SetUserValue(xmv);
                    }

                    return true; // was updated
                }
                catch (Exception ex)
                {
                    LogData.Data.Logger.Log(Level.Exception,
                        $"{nameof(SnmpWorker)}.{nameof(RefreshGroup)}. Error during refreshing group {xmGroup.Caption}. {ex.Message}.");
                    return false; // was not updated
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    ExtMonData.DebugWriteLine(nameof(SnmpWorker), $"Failed to acquire a lock when updating {xmGroup.Caption}");
                    LogData.Data.Logger.Log(Level.Debug,
                        $"{nameof(SnmpWorker)}. Failed to acquire a lock when updating {xmGroup.Caption}, {ExtMonData.GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // was not updated
            }
        }
        
        #endregion


        #region Value Requested event handlers

        private void IbaSnmp_LicensingValueRequested<T>(object sender, IbaSnmpValueRequestedEventArgs<T> args)
        {
            // refresh data if it is too old 
            RefreshLicenseInfo();

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            try
            {
                args.Value = (T) args.IbaSnmp.GetValue(args.Oid);
            }
            catch
            {
                // suppress possible cast exception from null to ValueType (bool, int, etc.)
            }
        }

        private void ProductSpecificItemRequested(object sender, IbaSnmpObjectValueRequestedEventArgs args)
        {
            try
            {

                // refresh data if it is too old (or rebuild the whole tree if necessary)
                if (args.Tag is ExtMonData.ExtMonGroup group)
                {
                    RefreshGroup(group);
                }
                else
                {
                    // should not happen
                    args.Value = null;
                    Debug.Assert(false);
                    return;
                }

                // re-read the value and send it back via args
                // (we should do re-read independently on whether above call to RefreshXxx()
                // had updated the value or not, because the value could be updated meanwhile by a similar call
                // in another thread if multiple values are requested)
                args.Value = args.IbaSnmp.GetValue(args.Oid);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(ProductSpecificItemRequested)}. Error during requesting item {args?.Oid}. {ex.Message}.");
            }
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI and MIB generation

        public Dictionary<IbaSnmpOid, ExtMonData.GuiTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                ExtMonInstance.RebuildTreeIfItIsInvalid();

                var result = new Dictionary<IbaSnmpOid, ExtMonData.GuiTreeNodeTag>();
                var objList = IbaSnmp.GetListOfAllOids();
                if (objList == null)
                {
                    return null;
                }

                var rootOid = IbaSnmp.OidIbaRoot;

                // get a set of all folders and nodes starting with the root
                var nodesSet = new HashSet<IbaSnmpOid> {rootOid};
                foreach (var oid in objList)
                {
                    // skip everything that is outside selected root
                    if (!oid.StartsWith(rootOid))
                    {
                        continue;
                    }

                    // add object itself
                    nodesSet.Add(oid);

                    // add object's parents (folder-nodes)
                    var parents = oid.GetParents();
                    foreach (var parent in parents)
                    {
                        if (parent.StartsWith(rootOid))
                        {
                            nodesSet.Add(parent);
                        }
                    }
                }

                // retrieve information about each node
                foreach (var oid in nodesSet)
                {
                    var tag = GetTreeNodeTag(oid, true);
                    if (tag != null)
                    {
                        result.Add(oid, tag);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }

        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        public ExtMonData.GuiTreeNodeTag GetTreeNodeTag(IbaSnmpOid oid, bool bUpdate = false)
        {
            try
            {
                var tag = new ExtMonData.GuiTreeNodeTag { SnmpOid = oid};

                IbaSnmpOidMetadata metadata = IbaSnmp.GetOidMetadata(oid);
                if (metadata == null)
                {
                    // this is nonexistent node
                    // leave all fields empty
                    return tag;
                }

                // fill data common for folders and leaves
                tag.SnmpMibName = metadata.MibName;
                tag.Description = metadata.MibDescription;
                tag.Caption = metadata.GuiCaption;

                // try to get value (applicable only to objects=leaves)

                IbaSnmpObjectInfo objInfo;
                try
                {
                    objInfo = IbaSnmp.GetObjectInfo(oid, bUpdate);
                }
                catch (Exception ex)
                {
                    // should not happen, so better to see it if it happens
                    LogData.Data.Logger.Log(Level.Exception,
                        $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}.({oid}). Error calling GetObjectInfo(). {ex.Message}");
                    return null;
                }

                // check  if this is a folder or leaf
                // object (leaf) can miss a value but anyway should have some data type
                if (!String.IsNullOrWhiteSpace(objInfo?.MibDataType))
                {
                    // this is a leaf node
                    tag.IsFolder = false;

                    tag.Value = IbaSnmp.IsEnumDataTypeRegistered(objInfo.ValueType)
                        ?
                        // enum - format it like e.g. "1 (started)"
                        $@"{objInfo.Value} ({IbaSnmp.GetEnumValueName(objInfo.ValueType, (int) objInfo.Value)})"
                        :
                        // other types - just value
                        objInfo.Value?.ToString() ?? "";


                    tag.Type = objInfo.MibDataType;
                }
                else
                {
                    // this is a folder
                    tag.IsFolder = true;
                }
                return tag;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}({oid}). {ex.Message}");
                return null;
            }
        }

        public List<SnmpMibFileContainer> GenerateMibFiles()
        {
            try
            {
                IbaSnmpMibGenerator gen = new IbaSnmpMibGenerator(IbaSnmp);

                gen.Generate();

                var mibFiles = new List<SnmpMibFileContainer>
                {
                    new SnmpMibFileContainer
                    {
                        FileName = gen.GeneralMibFilename,
                        Contents = gen.GeneralFileString
                    },
                    new SnmpMibFileContainer
                    {
                        FileName = gen.ProductMibFilename,
                        Contents = gen.ProductFileString
                    }
                };

                return mibFiles;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpWorker)}.{nameof(GenerateMibFiles)}. {ex.Message}");
                return null;
            }
        }

        #endregion

    }
}
