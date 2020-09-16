//----------------------------------------------------------------------------
//  Project pda
//  IBA
//  Copyright © 1996. All Rights Reserved.
//
//  SUBSYSTEM:    pda.apx Application
//  FILE:         PdaDongle.h
//  AUTHOR:
//
//  OVERVIEW
//  ~~~~~~~~
//  PDA dongle
//
//----------------------------------------------------------------------------
#if !defined(pdadongle_h)              // Sentry, use file only if it's not already included.
#define pdadongle_h

#define PASSWORD_INDEX_PDA 0
#define PASSWORD_INDEX_CAM 1
#define PASSWORD_INDEX_LOGIC 2
#define PASSWORD_INDEX_HD 3

#define EUP_INDEX_PDA 0
#define EUP_INDEX_CAM 1
#define EUP_INDEX_LOGIC 2
#define EUP_INDEX_HD 3
#define EUP_INDEX_ROTATE 4
#define EUP_INDEX_VISION 5
#define EUP_INDEX_DAVIS 6
#define EUP_INDEX_7 7
#define EUP_INDEX_8 8
#define EUP_INDEX_9 9
//#define EUP_INDEX_10 10
//#define EUP_INDEX_11 11
//#define EUP_INDEX_12 12
//#define EUP_INDEX_13 13
//#define EUP_INDEX_14 14
//#define EUP_INDEX_15 15

// DongleLib, Dongle.Net.dll and DongleViewer must be recompiled when any EUP_BIT_* define was changed
#define EUP_BIT_PDA 0		// 16
#define EUP_BIT_CAM 91
#define EUP_BIT_LOGIC 0	// 103
#define EUP_BIT_HD 83
#define EUP_BIT_ROTATE 160
#define EUP_BIT_VISION 90
#define EUP_BIT_DAVIS 192
#define EUP_BIT_7 0
#define EUP_BIT_8 0
#define EUP_BIT_9 0
#define EUP_BIT_10 0
#define EUP_BIT_11 0
#define EUP_BIT_12 0
#define EUP_BIT_13 0
#define EUP_BIT_14 0
#define EUP_BIT_15 0

#pragma pack (push,1)

typedef struct _DemoLicense
{
	unsigned short Index;
	unsigned char Value;
} DemoLicense;

typedef struct _ChangeLogEntry
{
	char User[16];
	unsigned long Timestamp;
} ChangeLogEntry;

struct DongleContents {
	unsigned char	hwId[8];	
	char			serialNr[7];
	char			customer[40];
	char			passwd[4][16];
	unsigned short	day;
	unsigned short	month;
	unsigned short	year;
	short			limit;			//-1: expired, 0: unlimited, >0: days remainining
	short			demolimit;		//-1: expired, 0: unlimited, >0: days remainining
  union
  {
	  unsigned char	multiOptions[4][64];
	  unsigned char	multiOptions2[256];
  };
	__int64			lastAccessTime;
	DemoLicense		demos[8];
	ChangeLogEntry	log[8];
	short           type;
	unsigned char	demosUsed;
	unsigned char	fwMajor;
	unsigned char   fwMinor;
	unsigned short  eup[16];		// 16 * 2 byte -> 16 entries for days since 1.1.2016 => 179 Years until 2195

	char            eupState;		//0: EUP is not active, 1: EUP is ok, 2: EUP not ok but in grace period, 3: EUP not ok and grace period expired, 4: EUP disabled by optout
	char            eupLimit;		//-1: expired, 0: undefined, >0: days remainining
};

#pragma pack (pop)

enum {
	E_NODONGLE = 0,
	E_USBMPI   = 1,
	E_SERIAL   = 2,
	E_USBSx    = 4
};

#ifndef CONTENTS_ONLY

#define _WINSOCKAPI_
#include <windows.h>

//=====================================================================================================================
//forward functions to enable usb notifications. ie. in dongle upgrade
//dwNotificationType
#define USB_NOTIFY_BOX_ATTACHED 1
#define USB_NOTIFY_BOX_REMOVED  2
// network box changed
#define USB_NOTIFY_BOX_CHANGED  3

typedef struct
{
	void* pNotificationParam;
	DWORD dwNotificationID;
	DWORD dwNotificationType;
} USB_NOTIFY_DATA;

typedef DWORD CALLBACK F_USB_NOTIFY_CALLBACK(USB_NOTIFY_DATA NotificationData);

DWORD WINAPI USB_RegisterNotificationCallback(F_USB_NOTIFY_CALLBACK* fNotify, void* pParam);
DWORD WINAPI USB_UnregisterNotificationCallback(F_USB_NOTIFY_CALLBACK* fNotify);
//#ifdef WIN32
DWORD WINAPI USB_RegisterNotificationMessage(HWND hWnd, UINT message, LPARAM lParam);
DWORD WINAPI USB_UnregisterNotificationMessage(HWND hWnd);
//#endif // WIN32

//=====================================================================================================================


enum DongleError
{	
	noError = 0,
	notSupportedHolder,
	noButtonFound,
	moreThanOneButtonFound,
	buttonNotOpen,
	errorReadSubkey,
	errorReadDataByte
};

enum TestDongleResult
{
	Ok = 0,
	NoDongle = 1,
	DongleChanged = 2
};

enum DongleReadMode
{
	Default        = 0,       // Customer info, time data, all licenses, demo licenses
	Full           = 1,       // This will read everything (customer info, time data, all licenses, demo licenses, camera activation, change log)
	Only64Licenses = 2,       // time data, first 64 licenses, demo licenses
};

enum DongleTimeLimitMode
{
	AllLimits  = 0,    // Apply global and demo time limits to dongle licenses
	DemoLimits = 1,    // Apply only demo time limits to dongle licenses
	NoLimits   = 2     // Don't apply any limits just return the raw values of all dongle licenses
};

#define NR_MULTI_OPTIONS 256

//Abstract base class for all dongle types
class Dongle
{
public:
	Dongle() { eupIndex = -1; buildDate = 0; };
	virtual ~Dongle(){};

	virtual TestDongleResult TestDongle(unsigned char* romData) = 0;

	virtual int TestDongle() = 0;

	virtual bool WritePasswordForIndex (char* newPassword, int index) = 0;

	virtual char * GetType() = 0;

	virtual bool ReadDongleContents(DongleContents* pContent, DongleReadMode readMode, DongleTimeLimitMode limitMode = AllLimits) = 0;
	void SetEup(int eupIndex, unsigned short buildDate);

#if !defined(DONGLE_LITE) || defined(DONGLE_DEMO)
	virtual bool WriteDemoLicenses (DemoLicense demos[8], int limitInDays, unsigned short day, unsigned short month, unsigned short year) = 0;
#endif

#ifndef DONGLE_LITE	
	virtual int InitDongle() = 0;

	virtual bool WriteDongleContents(DongleContents* pContent) = 0;
#endif


protected:
	//Checks and also sets back the time limits (decrements) and the lastaccesstime for writing back
	bool CheckTimeLimits(short* globalTimeLimit, short* demoTimeLimit, char* eupTimeLimit, __int64* lastAccessTime);

	bool AreDemoLicensesValid(DemoLicense* demos, unsigned char demosUsed);
	unsigned char UpdateDemoUsedMask(DemoLicense* demos, unsigned char demosUsed);

	//EUP is not active when eupIndex < 0 or buildData = 0
	int eupIndex;
	// release date in days since 1.1.2016
	unsigned short buildDate;

};


//Wrapper for the serial and USB dongles
class PdaDongle
{
public:
	PdaDongle(HMODULE resMod = NULL, bool rescanUSB = false, bool noUsbInstall = false);
	virtual ~PdaDongle();

	static void Uninitialize(); //Optional function to uninitialize the SmarXOS library

	static char * GetLibVersion();
	char * GetType();

	int TestDongle();
	TestDongleResult TestDongle(unsigned char* romData);


	void SetEup(int eupIndex, unsigned short buildDate);
	bool ReadDongleContents(DongleContents* pContent, DongleReadMode readMode = Default, DongleTimeLimitMode limitMode = AllLimits);

	bool WritePasswordForIndex (char* newPassword, int index);


#if !defined(DONGLE_LITE) || defined(DONGLE_DEMO)
	bool WriteDemoLicenses(DemoLicense demos[8], int limitInDays, unsigned short day, unsigned short month, unsigned short year);
#endif

#ifndef DONGLE_LITE
	int InitDongle();

	bool WriteDongleContents(DongleContents* pContent);
#endif	

	static BOOL bShowMessageBoxes;

private:
	static BOOL bUseSecurity;
	static BOOL bUseGlobal;

	void CreateDongle();
	void CreateDongle(int dongleType);
	int AutoDetect();

	Dongle* dongle;
	HANDLE hDongleAccess;

	void ApplyTimeLimitsCorrections(DongleContents* pContent, bool bDemoOnly);

};

#ifdef _SERVICE
class CLicenseClientSocket;

//Use this class to request licenses.
//This class uses the license service if it is running.
//If not then it uses the local dongle.

class CibaLicense
{
public:
	CibaLicense(int eupIndex = -1, unsigned short buildDate = 0);
	~CibaLicense();

	unsigned short GetEup();		// Get eup days set at index; return 0 indicates error or not set
	char GetEupState();
	BOOL HasLicense(DWORD option);

private:
	BYTE options[NR_MULTI_OPTIONS];
	int eupIndex;
	unsigned short buildDate;
	char eupState;
	char eupLimit;
	unsigned short eupValue;
	CLicenseClientSocket* clientSocket;
	BOOL IsServiceRunning();
	void ReadDongle();

};
#endif

//#define IBA_TRACE_ENABLED

#ifdef IBA_TRACE_ENABLED

void DongleTrace(const char *fmt, ...);
void DongleTraceFile(const char *fmt, ...);
void DumpMemory(char* msg, unsigned char* pMem, int offset, int length);

#define IBA_TRACE(fmt, ...) DongleTrace(fmt, __VA_ARGS__)
#define IBA_TRACE_FILE(fmt, ...) DongleTraceFile(fmt, __VA_ARGS__)
#define IBA_DUMPMEM(msg, pMem, offset, length) DumpMemory(msg, pMem, offset, length)

#else

#define IBA_TRACE(fmt, ...) 
#define IBA_TRACE_FILE(fmt, ...) 
#define IBA_DUMPMEM(msg, pMem, offset, length) 

#endif

#endif //CONTENTS_ONLY

#endif
