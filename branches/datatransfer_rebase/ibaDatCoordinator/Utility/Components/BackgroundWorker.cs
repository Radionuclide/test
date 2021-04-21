// © 2004 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace iba.Utility.Components
{   
	#region ibaBackgroundWorker
	public class ibaBackgroundWorker : Component
	{
		#region Events
		public event ibaDoWorkEventHandler DoWork;
		public event ibaProgressChangedEventHandler ProgressChanged;
		public event ibaRunWorkerCompletedEventHandler RunWorkerCompleted;
		#endregion

		#region Implementation
		bool m_CancelPending = false;
		bool m_ReportsProgress = false;
		bool m_SupportsCancellation = false;

		void ProcessDelegate(Delegate del,params object[] args)
		{
			Delegate temp = del;
			if(temp == null)
				return;
			
			Delegate[] delegates = temp.GetInvocationList();
			foreach(Delegate handler in delegates)
				InvokeDelegate(handler,args);
		}

        void InvokeDelegate(Delegate del, object[] args)
        {
            try
            {
                ISynchronizeInvoke synchronizer = del.Target as ISynchronizeInvoke;
                if(synchronizer != null)//A Windows Forms object
                {
                    if(synchronizer.InvokeRequired == false)
                        del.DynamicInvoke(args);
                    else
                        synchronizer.Invoke(del, args);
                }
                else//Not a Windows Forms object
                {
                    del.DynamicInvoke(args);
                }
            }
            catch(Exception)
            { 
            }
        }

		void ReportCompletion(IAsyncResult asyncResult)
		{
			AsyncResult ar = (AsyncResult)asyncResult;
			ibaDoWorkEventHandler del  = (ibaDoWorkEventHandler)ar.AsyncDelegate;
         
			ibaDoWorkEventArgs doWorkArgs = (ibaDoWorkEventArgs)asyncResult.AsyncState;
			object result = null;
			Exception error = null;

			try
			{
				del.EndInvoke(asyncResult);
				result = doWorkArgs.Result;
			}
			catch(Exception exception)
			{
				error = exception;
			}

            ibaRunWorkerCompletedEventArgs completedArgs = new ibaRunWorkerCompletedEventArgs(result, error, doWorkArgs.Cancel);
            OnRunWorkerCompleted(completedArgs);
		}

		protected virtual void OnRunWorkerCompleted(ibaRunWorkerCompletedEventArgs completedArgs)
		{
			ProcessDelegate(RunWorkerCompleted,this,completedArgs);
		}

		protected virtual void OnProgressChanged(ibaProgressChangedEventArgs progressArgs)
		{
			ProcessDelegate(ProgressChanged,this,progressArgs);
		}
		#endregion

		#region Public API
		public void RunWorkerAsync()
		{
			RunWorkerAsync(null);
		}

		public void RunWorkerAsync(object argument)
		{
			m_CancelPending = false;
			if(DoWork != null)
			{
				ibaDoWorkEventArgs args = new ibaDoWorkEventArgs(argument);
				AsyncCallback callback;
				callback = new AsyncCallback(ReportCompletion);
                try
                {
                    DoWork.BeginInvoke(this, args, callback, args);
                }
                catch(Exception)
                {
                }
			}
		}

		public void ReportProgress(int percent)
		{
			ReportProgress(percent, null);
		}

        public void ReportProgress(int percent, object tag)
        {
            if(WorkerReportsProgress)
            {
                ibaProgressChangedEventArgs progressArgs = new ibaProgressChangedEventArgs(percent, tag);
                OnProgressChanged(progressArgs);
            }
        }

		public void CancelAsync()
		{
			lock(this)
			{
				m_CancelPending = true;
			}
		}
		public bool CancellationPending
		{
			get
			{
				lock(this)
				{
					return m_CancelPending;
				}
			}
		}
		public bool WorkerSupportsCancellation
		{
			get
			{
				lock(this)
				{
					return m_SupportsCancellation;
				}
			}

			set
			{
				lock(this)
				{
					m_SupportsCancellation = value;
				}
			}
		}
		public bool WorkerReportsProgress
		{
			get
			{
				lock(this)
				{
					return m_ReportsProgress;
				}
			}

			set
			{
				lock(this)
				{
					m_ReportsProgress = value;
				}
			}
		} 
		#endregion
	} 
	#endregion

	#region Event arguments
	public class ibaCancelEventArgs : EventArgs
	{
		protected bool m_Cancel; 
		public bool Cancel
		{
			get
			{
				return m_Cancel;
			}   
			set
			{
				m_Cancel = value;
			}   
		}

	}

	public class ibaDoWorkEventArgs : CancelEventArgs
	{
		object m_Result;
		public object Result
		{
			get
			{
				return m_Result;
			}   
			set
			{
				m_Result = value;
			}   
		}
		public readonly object Argument; 
		public ibaDoWorkEventArgs(object argument)
		{
			Argument = argument;
		}
	}

	public class ibaProgressChangedEventArgs : EventArgs
	{
		public readonly int ProgressPercentage; 
        public readonly object Tag;
		public ibaProgressChangedEventArgs(int percentage, object tag)
		{
			ProgressPercentage = percentage;
            Tag = tag;
		}
	}

	public class ibaAsyncCompletedEventArgs : EventArgs
	{
		public readonly Exception Error;
		public readonly bool Cancelled;

		public ibaAsyncCompletedEventArgs(Exception error,bool cancel)
		{
			Error = error;
			Cancelled = cancel;
		}
	}

	public class ibaRunWorkerCompletedEventArgs : ibaAsyncCompletedEventArgs
	{
		public readonly object Result;
		public ibaRunWorkerCompletedEventArgs(object result,Exception error,bool cancel) : base(error,cancel)
		{
			Result = result;
		}
	}
	#endregion

	#region Delegates
	public delegate void ibaDoWorkEventHandler(object sender, ibaDoWorkEventArgs e);
	public delegate void ibaProgressChangedEventHandler(object sender, ibaProgressChangedEventArgs e);
	public delegate void ibaRunWorkerCompletedEventHandler(object sender, ibaRunWorkerCompletedEventArgs e);
	#endregion
}
