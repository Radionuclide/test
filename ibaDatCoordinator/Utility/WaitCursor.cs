using System;
using System.Windows.Forms;

namespace iba.Utility
{
	/// <summary>
	/// Summary description for WaitCursor.
	/// </summary>
	public class WaitCursor : IDisposable
	{
		private Cursor savedCursor;

		public WaitCursor()
		{
			savedCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Cursor.Current = savedCursor;
		}

		#endregion
	}
}
