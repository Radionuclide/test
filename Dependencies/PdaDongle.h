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

#pragma pack (push,1)

typedef struct
{
	unsigned short Index;
	unsigned char Value;
} DemoLicense;

typedef struct
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
	short			limit;
	short			demolimit;
	unsigned char	multiOptions[2][64];
	unsigned char	cameraActivation[128];
	__int64			lastAccessTime;
	DemoLicense		demos[8];
	ChangeLogEntry	log[8];
	short type;
};

#pragma pack (pop)

enum  {
	E_NODONGLE = 0,
	E_USBMPI   = 1,
	E_SERIAL   = 2,
	E_USBSx    = 4
};

#ifndef CONTENTS_ONLY

#define _WINSOCKAPI_
#include <windows.h>

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

#define NR_MULTI_OPTIONS 128

//Abstract base class for all dongle types
class Dongle
{
public:
	Dongle(){};
	virtual ~Dongle(){};

	virtual TestDongleResult TestDongle(unsigned char* romData) = 0;

	virtual int TestDongle() = 0;

	virtual bool WritePasswordForIndex (char* newPassword, int index) = 0;

	virtual char * GetType() = 0;

	virtual bool ReadDongleContents(DongleContents* pContent, DongleReadMode readMode) = 0;

#ifndef DONGLE_LITE	
	virtual int InitDongle() = 0;

	virtual bool WriteDongleContents(DongleContents* pContent) = 0;

#endif

protected:
	//Checks and also sets back the time limits (decrements) and the lastaccesstime for writing back
	bool CheckTimeLimits(short* globalTimeLimit, short* demoTimeLimit, __int64* lastAccessTime);
};


//Wrapper for the serial and USB dongles
class PdaDongle
{
public:
	PdaDongle(HMODULE resMod = NULL, bool rescanUSB = false, bool noUsbInstall = false);
	virtual ~PdaDongle();

	static char * GetLibVersion();
	char * GetType();

	int TestDongle();
	TestDongleResult TestDongle(unsigned char* romData);

	bool ReadDongleContents(DongleContents* pContent, DongleReadMode readMode = Default, DongleTimeLimitMode limitMode = AllLimits);

	bool WritePasswordForIndex (char* newPassword, int index);

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

	Dongle * dongle;
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
	CibaLicense();
	~CibaLicense();

	BOOL HasLicense(DWORD option);

private:
	BYTE options[NR_MULTI_OPTIONS];
	CLicenseClientSocket* clientSocket;
	BOOL IsServiceRunning();
	void ReadDongle();
};
#endif

//#define IBA_TRACE_ENABLED

#ifdef IBA_TRACE_ENABLED

void Trace(const char *fmt, ...);
void DumpMemory(char* msg, unsigned char* pMem, int offset, int length);

#define IBA_TRACE(fmt, ...) Trace(fmt, __VA_ARGS__)
#define IBA_DUMPMEM(msg, pMem, offset, length) DumpMemory(msg, pMem, offset, length)

#else

#define IBA_TRACE(fmt, ...) 
#define IBA_DUMPMEM(msg, pMem, offset, length) 

#endif

#endif //CONTENTS_ONLY

#endif
