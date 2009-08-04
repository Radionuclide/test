//#include "stdafx.h"
//#define WIN32_LEAN_AND_MEAN
//#include <windows.h>
#include "shlobj.h"
#include "browser.h"

System::String^ iba::BrowseForComputer::Browse()
{
	return iba::BrowseForComputer::Browse(System::IntPtr::Zero);
}

System::String^ iba::BrowseForComputer::Browse(System::IntPtr owner)
{
	LPITEMIDLIST root;
	LPITEMIDLIST comp;
	BROWSEINFO info;
	WCHAR dispname[MAX_PATH];

	SHGetSpecialFolderLocation(0, CSIDL_NETWORK, &root);
	//OleInitialize(0);

	info.pidlRoot = root;
	info.hwndOwner = (HWND) owner.ToPointer();
	info.pszDisplayName = dispname;
	info.lpszTitle = 0;
	info.ulFlags = BIF_BROWSEFORCOMPUTER;
	info.lpfn = 0;
	info.lParam = 0;
	info.iImage = 0;
	comp = SHBrowseForFolder(&info);
	if(comp == 0) //cancel pressed
		return System::String::Empty;
	
    //SHGetPathFromIDList(comp, computername);
	IMalloc* pMalloc;
	SHGetMalloc(&pMalloc);
	pMalloc->Free(root);
	pMalloc->Free(comp);
	pMalloc->Release();

	return System::Runtime::InteropServices::Marshal::PtrToStringUni(System::IntPtr(dispname));
}
