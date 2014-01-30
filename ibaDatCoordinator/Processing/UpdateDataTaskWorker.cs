using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iba.Data;
using ibaFilesLiteLib;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.Odbc;
using iba.Utility;

namespace iba.Processing
{
    public class UpdateDataTaskWorker : IDisposable
    {
        private UpdateDataTaskData m_udt;


        public UpdateDataTaskWorker(UpdateDataTaskData udt)
        {
            m_udt = udt;
            m_timesCalled = 0;
        }
        
        public void Init()
        {
            Dispose();
            TimesCalled = 0;
            
           
            //second, initialize ibaFiles
            try
            {
                m_ibaFileUpdater = new IbaFileClass();
            }
            catch (Exception ex)
            {
                throw new Exception(iba.Properties.Resources.logIbaFilesCouldNotBeCreated + ": " + ex.Message);
            }

            try
            {
                m_connection = Connection(m_udt);
                m_connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(iba.Properties.Resources.logUDTConnectFailed + ": " + ex.Message);
            }
        }

        static public DbConnection Connection(UpdateDataTaskData udt)
        {
            switch (udt.DbProvider)
            {
                case UpdateDataTaskData.DbProviderEnum.MsSql:
                    {
                        DbConnection connection = new SqlConnection();
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                        if (udt.DbNamedServer)
                            builder.DataSource = udt.DbServer;
                        else
                            builder.DataSource = "(local)";
                        if (udt.DbAuthenticateNT)
                            builder.IntegratedSecurity = true;
                        else
                        {
                            builder.UserID = udt.DbUserName;
                            builder.Password = udt.Password;
                        }
                        builder.InitialCatalog = udt.DbName;
                        builder.ConnectTimeout = 15;
                        connection.ConnectionString = builder.ConnectionString;
                        return connection;
                    }
                case UpdateDataTaskData.DbProviderEnum.Oracle: //through OleDb
                    {
                        DbConnection connection = new OleDbConnection();
                        OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                        builder.Provider = "OraOLEDB.Oracle";
                        builder.DataSource = udt.DbName;
                        builder["User Id"] = udt.DbUserName;
                        builder["Password"] = udt.DbPassword;
                        connection.ConnectionString = builder.ConnectionString;
                        return connection;
                    }
                case UpdateDataTaskData.DbProviderEnum.Db2: //through OleDb
                    {
                        DbConnection connection = new OleDbConnection();
                        OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                        builder.Provider = "IBMDADB2.1";
                        builder.DataSource = udt.DbName;
                        builder["User ID"] = udt.DbUserName;
                        builder["Password"] = udt.DbPassword;
                        connection.ConnectionString = builder.ConnectionString;
                        return connection;
                    }
                case UpdateDataTaskData.DbProviderEnum.Odbc:
                    {
                        return new OdbcConnection(String.Format("Dsn={0};Uid={1};Pwd={2}", udt.DbName, udt.Username, udt.Password));
                    }
            }
            return null;
        }

        static public string TestConnecton(UpdateDataTaskData udt)
        {
            using (DbConnection con = Connection(udt))
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    return iba.Properties.Resources.logUDTConnectFailed + ": " + ex.Message;
                }

                try
                {
                    using (DbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = String.Format("SELECT * FROM {0}", udt.DbTblName);
                        using (DbDataReader dbdr = cmd.ExecuteReader())
                        {
                            dbdr.Read();
                            int[] columnOrds = { -1, -1, -1, -1, -1 };
                            string[] columnNames = { "ID_REF", "ID_NEW", "READY_TO_PROCESS", "PROCESSED", "CREATED" };

                            for (int i = 0; i < 5; i++)
                            {
                                try
                                {
                                    columnOrds[i] = dbdr.GetOrdinal(columnNames[i]);
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    return String.Format(iba.Properties.Resources.logUDTColumnMissing, columnNames[i]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return iba.Properties.Resources.logUDTDataBaseError + ": " + ex.Message;
                }
            }
            return "success";
        }

        private IbaFile m_ibaFileUpdater;
        private DbConnection m_connection;

        private DateTime m_created;
        private int m_timesCalled;

        public int TimesCalled
        {
            get { return m_timesCalled; }
            set { m_timesCalled = value; }
        }
        public DateTime Created
        {
            get { return m_created; }
            set { m_created = value; }
        }

        public string DoWork(string outDirectory, string sourceFile)
        {
            m_bFileOverWritten = false;
            m_fatalError = false;
            string tempFilePath = Path.Combine(Path.GetTempPath(),"updatedatatask{" + m_udt.Guid.ToString() + "}.dat");

            try
            {
                File.Copy(sourceFile, tempFilePath,true);
            }
            catch (Exception ex)
            {
                throw new Exception(iba.Properties.Resources.logUDTTempFileCopyProblem + ": " + ex.Message, ex);
            }

            string key = Path.GetFileNameWithoutExtension(sourceFile);
            string newFileName;
            SortedDictionary<string,string> newInfoFields = new SortedDictionary<string,string>();
            Created = new DateTime();
            try
            {
                using (DbCommand cmd = m_connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("SELECT * FROM {0} WHERE ID_REF='{1}'", m_udt.DbTblName, key);
                    using (DbDataReader dbdr = cmd.ExecuteReader())
                    {
                        if (!dbdr.HasRows) throw new ApplicationException(iba.Properties.Resources.logUDTNoResults);
                        dbdr.Read();

                        int[] columnOrds={-1,-1,-1,-1,-1};
                        string[] columnNames = { "ID_REF", "ID_NEW", "READY_TO_PROCESS", "PROCESSED", "CREATED" };

                        for (int i = 0; i < 5;i++)
                        {
                            try
                            {
                                columnOrds[i] = dbdr.GetOrdinal(columnNames[i]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new ApplicationException(String.Format(iba.Properties.Resources.logUDTColumnMissing, columnNames[i]));
                            }
                            if (dbdr.IsDBNull(columnOrds[i]))
                                throw new ApplicationException(String.Format(iba.Properties.Resources.logUDTColumnIsNull, columnNames[i]));
                        }

                        bool creationTimeGotten = true;
                        try
                        {
                            Created = dbdr.GetDateTime(columnOrds[4]);
                        }
                        catch
                        {
                            creationTimeGotten = false;
                        }

                        if (!Convert.ToBoolean(dbdr.GetValue(columnOrds[2])))
                            throw new ApplicationException(iba.Properties.Resources.logUDTNotReadyYet 
                                + (creationTimeGotten?(" - " + iba.Properties.Resources.logUDTCreationTime + " " + Created.ToString()):""));
                        newFileName = dbdr.GetString(columnOrds[1]) + ".dat";
                        System.Globalization.CultureInfo oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                        for (int i = 0; i < dbdr.FieldCount; i++)
                        {
                            if (columnOrds[0] == i || columnOrds[1] == i || columnOrds[2] == i || columnOrds[3] == i || columnOrds[4] == i || dbdr.IsDBNull(i))
                                continue;
                            string res = dbdr.GetValue(i).ToString();
                            res = res.Replace(System.Environment.NewLine, " ");
                            if (res.Length >= 10000) 
                                res = res.Remove(9999);
                            newInfoFields.Add(dbdr.GetName(i), res);
                        }
                        System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
                    }
                }
            }
            catch (ApplicationException)
            {
                //simply rethrow it
                throw;
            }
            catch (Exception ex)
            {
                m_fatalError = true;
                throw new Exception(iba.Properties.Resources.logUDTDataBaseError + ": " + ex.Message, ex);
            }

            if (newInfoFields.Count > 0)
            {
                try
                {
                    m_ibaFileUpdater.OpenForUpdate(tempFilePath);
                }
                catch (Exception ex)
                {
                    m_fatalError = true;
                    throw new Exception(iba.Properties.Resources.logUDTibaFilesOpenProblem + ": " + ex.Message, ex);
                }

                try
                {
                    foreach (KeyValuePair<string, string> pair in newInfoFields)
                        m_ibaFileUpdater.WriteInfoField(pair.Key, pair.Value);
                    m_ibaFileUpdater.WriteInfoField("$DATCOOR_status", "readyToProcess");
                    m_ibaFileUpdater.WriteInfoField("$DATCOOR_TasksDone", "");
                    m_ibaFileUpdater.WriteInfoField("$DATCOOR_times_tried", "0");
                    m_ibaFileUpdater.WriteInfoField("$DATCOOR_OutputFiles", "");
                    m_ibaFileUpdater.Close();
                }
                catch (Exception ex)
                {
                    m_fatalError = true;
                    throw new Exception(iba.Properties.Resources.logUDTibaFilesWriteProblem + ": " + ex.Message, ex);
                }
            }

            string destPath = Path.Combine(outDirectory,newFileName);
            try
            {
                if (File.Exists(destPath))
                {
                    if (m_udt.OverwriteFiles)
                        m_bFileOverWritten = true;
                    else
                        destPath = DatCoordinatorHostImpl.Host.FindSuitableFileName(destPath);
                }
                File.Copy(tempFilePath, destPath, true);
            }
            catch (Exception ex)
            {
                throw new Exception(iba.Properties.Resources.logUDTFinalFileCopyProblem + ": " + ex.Message);
            }

            try
            {
                using (DbCommand cmd = m_connection.CreateCommand())
                {
                    cmd.CommandText = String.Format("UPDATE {0} SET PROCESSED=1 WHERE ID_REF='{1}'", m_udt.DbTblName, key); 
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(iba.Properties.Resources.logUDTMarkProcessedProblem + ": " + ex.Message);
            }
            m_timesCalled++;
            return destPath;
        }


        private bool m_fatalError;
        public bool FatalError //error that gives cause to delete this worker and construct it again
        {
            get { return m_fatalError; }
        }


        private bool m_bFileOverWritten;
        public bool FileOverWritten
        {
            get { return m_bFileOverWritten; }
        }

        public void Dispose()
        {
            if (m_ibaFileUpdater != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaFileUpdater);
                m_ibaFileUpdater = null;

            }
            if (m_connection != null)
            {
                m_connection.Dispose();
                m_connection = null;
            }
        }
    }
}
