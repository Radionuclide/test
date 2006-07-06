using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using iba.Data;
using System.Runtime.InteropServices;


namespace iba.Processing
{
    class Notifier
    {
        private ConfigurationData m_cd;
        private SortedDictionary<TaskData, List<string> > m_successes;
        private SortedDictionary<TaskData, List<string> > m_failures;

        public Notifier(ConfigurationData cd)
        {
            m_cd = cd;
            m_successes = new SortedDictionary<TaskData,List<string>>();
            m_failures = new SortedDictionary<TaskData,List<string>>();
        }

        public void AddSuccess(TaskData task, string datfile)
        {
            lock (m_listLock)
            {
                if (!m_successes.ContainsKey(task))
                {
                    List<string> newList = new List<string>();
                    newList.Add(datfile);
                    m_successes.Add(task, newList);
                }
                else if (!m_successes[task].Contains(datfile))
                    m_successes[task].Add(datfile);
            }
        }

        public void AddFailure(TaskData task, string datfile)
        {
            lock (m_listLock)
            {
                if (!m_failures.ContainsKey(task))
                {
                    List<string> newList = new List<string>();
                    newList.Add(datfile);
                    m_failures.Add(task, newList);
                }
                else if (!m_failures[task].Contains(datfile))
                    m_failures[task].Add(datfile);
            }
        }

        string composeMessage(bool isEmail)
        {
            string message ="";
            string newline = isEmail ? "\n" : Environment.NewLine;
            lock (m_listLock)
            {
                foreach (KeyValuePair<TaskData, List<string>> pair in m_successes)
                {
                    string line = string.Format(iba.Properties.Resources.NotificationSuccesses, pair.Key.Name, m_cd.Name);
                    message += line + "\n";
                    foreach (string s in pair.Value)
                    {
                        message += "    " + s + "\n";
                    }
                }
                foreach (KeyValuePair<TaskData, List<string>> pair in m_failures)
                {
                    string line = string.Format(iba.Properties.Resources.NotificationFailures, pair.Key.Name, m_cd.Name);
                    message += line + "\n";
                    foreach (string s in pair.Value)
                    {
                        message += "    " + s + "\n"; 
                    }
                }
            }
            return message;
        }

        private const int NERR_BASE = 2100;
        private const int NERR_NameNotFound = NERR_BASE + 173; /* The message alias could not be found on the network. */
        private const int NERR_NetworkError = NERR_BASE + 36;  /* A general network error occurred. */
        private const int ERROR_NOT_SUPPORTED = 50;
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_ACCESS_DENIED = 5;

        private Object m_listLock = new Object();

        public void Send()
        {
            lock (m_listLock)
            {
                if (m_failures.Count == 0 && m_successes.Count == 0) return;
            }
            if (m_cd.NotificationData.NotifyOutput == NotificationData.NotifyOutputChoice.EMAIL)
            {
                MailAddress to = null;
                string logFailed = null;
                try
                {
                    to = new MailAddress(m_cd.NotificationData.Email);
                }
                catch (ArgumentNullException)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + iba.Properties.Resources.invalidEmail;
                }
                catch (ArgumentException)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + iba.Properties.Resources.invalidEmail;
                }
                catch (FormatException)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + iba.Properties.Resources.invalidEmail;
                }
                if (logFailed != null)
                {
                    if (LogData.Data.Logger.IsOpen)
                    {
                        LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                        LogData.Data.Logger.Log(Logging.Level.Exception, logFailed, (object)data);
                    }
                    return;
                }

                MailAddress from = new MailAddress("ibaDatCoordinator@iba-ag.com");
                MailMessage message = new MailMessage(from, to);
                message.Subject = iba.Properties.Resources.NotificationEmailSubject;
                message.IsBodyHtml = false;
                message.Body = composeMessage(true);
                // Use the application or machine configuration to get the 
                // host, port, and credentials.
                SmtpClient client = new SmtpClient();
                client.Host = m_cd.NotificationData.SMTPServer;
                client.UseDefaultCredentials = true;
                try
                {
                    client.Send(message);
                }
                catch (InvalidOperationException e1)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + e1.Message;
                }
                catch (SmtpFailedRecipientException e2)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + e2.Message;
                }
                catch (SmtpException e3)
                {
                    logFailed = iba.Properties.Resources.logNotificationFailed + ": " + e3.Message;
                }
                if (logFailed != null && LogData.Data.Logger.IsOpen)
                {
                    LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                    LogData.Data.Logger.Log(Logging.Level.Exception, logFailed, (object)data);
                }
            }
            else // net send
            {
                string message = composeMessage(false);
                string logFailed = iba.Properties.Resources.logNotificationFailed;
                try
                {
                    int returnval = NetMessageBufferSend(null, m_cd.NotificationData.Host, null, message, message.Length * 2 + 2);
                    switch (returnval)
                    {
                        case NERR_NameNotFound:
                            logFailed += ": " + iba.Properties.Resources.logNotificationNameNotFound;
                            goto case 666;
                        case NERR_NetworkError:
                            logFailed += ": " + iba.Properties.Resources.logNotificationNetworkError;
                            goto case 666;
                        case ERROR_NOT_SUPPORTED:
                            logFailed += ": " + iba.Properties.Resources.logNotificationNotSupported;
                            goto case 666;
                        case ERROR_INVALID_PARAMETER:
                            logFailed += ": " + iba.Properties.Resources.logNotifcationInvalidParameter;
                            goto case 666;
                        case ERROR_ACCESS_DENIED:
                            logFailed += ": " + iba.Properties.Resources.logNotificationAccessDenied;
                            goto case 666;
                        case 666:
                            if (LogData.Data.Logger.IsOpen)
                            {
                                LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                                LogData.Data.Logger.Log(Logging.Level.Exception, logFailed, (object)data);
                            }
                            break;
                    }
                }
                catch
                {
                    if (LogData.Data.Logger.IsOpen)
                    {
                        LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                        LogData.Data.Logger.Log(Logging.Level.Exception, logFailed, (object)data);
                    }
                }
            }
            lock (m_listLock)
            {
                m_failures.Clear();
                m_successes.Clear();
            }
        }

        [DllImport("Netapi32", CharSet = CharSet.Unicode)]
        public static extern int NetMessageBufferSend(
            string servername,
            string msgname,
            string fromname,
            string buf,
            int buflen); 
    }
}
