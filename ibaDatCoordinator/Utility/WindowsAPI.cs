using System;
using System.Runtime.InteropServices;

namespace iba.Utility
{
	/// <summary>
	/// Summary description for WindowsAPI.
	/// </summary>
	internal class WindowsAPI
	{
		#region Shlwapi.dll functions
		[DllImport("Shlwapi.dll")]
		public static extern int SHAutoComplete(IntPtr handle, SHAutoCompleteFlags flags); 
		#endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }



	#region SHAutoCompleteFlags
	[Flags]
	internal enum SHAutoCompleteFlags : uint
	{
		SHACF_DEFAULT                = 0x00000000,  // Currently (SHACF_FILESYSTEM | SHACF_URLALL)
		SHACF_FILESYSTEM             = 0x00000001,  // This includes the File System as well as the rest of the shell (Desktop\My Computer\Control Panel\)
		SHACF_URLHISTORY             = 0x00000002,  // URLs in the User's History
		SHACF_URLMRU                 = 0x00000004,  // URLs in the User's Recently Used list.
		SHACF_USETAB                 = 0x00000008,  // Use the tab to move thru the autocomplete possibilities instead of to the next dialog/window control.
		SHACF_FILESYS_ONLY           = 0x00000010,  // This includes the File System
		SHACF_FILESYS_DIRS           = 0x00000020,  // Same as SHACF_FILESYS_ONLY except it only includes directories, UNC servers, and UNC server shares.
		SHACF_URLALL                 = (SHACF_URLHISTORY | SHACF_URLMRU),
		SHACF_AUTOSUGGEST_FORCE_ON   = 0x10000000,  // Ignore the registry default and force the feature on.
		SHACF_AUTOSUGGEST_FORCE_OFF  = 0x20000000,  // Ignore the registry default and force the feature off.
		SHACF_AUTOAPPEND_FORCE_ON    = 0x40000000,  // Ignore the registry default and force the feature on. (Also know as AutoComplete)
		SHACF_AUTOAPPEND_FORCE_OFF   = 0x80000000   // Ignore the registry default and force the feature off. (Also know as AutoComplete)
	}
	#endregion
}
