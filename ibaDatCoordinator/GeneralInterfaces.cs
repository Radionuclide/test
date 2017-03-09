using System;
using System.Collections;
using System.Windows.Forms;

namespace iba
{
    //Interface that can be used for controls in the left side of the Eyefinder pane manager
    public interface IPropertyPane
	{
		void LoadData(object datasource, IPropertyPaneManager manager);
        void LeaveCleanup(); // do cleanup data
		void SaveData();
    }

    // Interface that can be used to derive a form from that uses the Eyefinder pane manager layout
    public interface IPropertyPaneManager
    {
        Hashtable       PropertyPanes {get;}
        IPropertyPane CurrentPane {get;}
        TreeView LeftTree {get;}
        TreeView getLeftTree(string name);
        void SaveRightPaneControl();
		void SetRightPaneControl(Control newControl, string title, object datasource);
        void AdjustRightPaneControlTitle();
        ToolStripStatusLabel StatusBarLabel
        {
            get;
            set;
        }
    }

    // Interface to indicate that the form can recieve external commands
    public interface IExternalCommand
    {
        void OnExternalActivate();
        void OnExternalClose();
        void OnStartService();
    }
}