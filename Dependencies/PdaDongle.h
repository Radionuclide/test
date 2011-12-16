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

struct DongleContents {
	unsigned char	HWid[8];	
	char			serialNr[7];
	char			customer[40];
	char			passwd[16];
	unsigned short	day;
	unsigned short	month;
	unsigned short	year;
	short			limit;
	unsigned long	maskPda;
	unsigned long	maskNonPda;
	unsigned char	multiOptions[2][64];
	unsigned char	cameraActivation[128];
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

#define NR_MULTI_OPTIONS 64

//Abstract base class for all dongle types
class Dongle
{
public:
	Dongle(){};
	virtual ~Dongle(){};

	virtual TestDongleResult TestDongle(unsigned char* romData) = 0;

	static char * GetLibVersion();

	virtual int TestDongle() = 0;

	virtual bool ReadDongle (
		DWORD &optionMask,
		DWORD &optionMaskNonPda,
		char password[16], char serialNr[7], char customer[40],
		unsigned short &dongleMonth,
		unsigned short &dongleDayOfMonth,
		unsigned short &dongleYear,
		short &timeLimit,
		unsigned char* romData) = 0;
	virtual bool ReadMultiLicenses(BYTE* licenses, int nrLicenses = NR_MULTI_OPTIONS) = 0;

	virtual bool WriteNewPassword (char* newPassword) = 0;
	
	virtual char * GetType() = 0;

	virtual bool ReadDongleContents(DongleContents* pContent) = 0;
	
#ifndef DONGLE_LITE	
	virtual int InitDongle() = 0;
	virtual bool WriteDongle (
		DWORD optionMask,
		DWORD optionMaskNonPda,
		char password[16], char serialNr[7], char customer[40],
		unsigned short dongleMonth,
		unsigned short dongleDayOfMonth,
		unsigned short dongleYear,
		short timeLimit) = 0;
	
	virtual bool WriteOptionsAndTimeLimit (
		DWORD optionMask,
		DWORD optionMaskNonPda,
		unsigned short dongleMonth,
		unsigned short dongleDayOfMonth,
		unsigned short dongleYear,
		short timeLimit) = 0;
	
	virtual bool WriteMultiLicenses(BYTE* licenses, int nrLicenses = NR_MULTI_OPTIONS) = 0;

	virtual bool WriteDongleContents(DongleContents* pContent) = 0;

	virtual int WriteScratch(unsigned char * buffer, int size = 64) = 0;
	virtual int ReadScratch(unsigned char * buffer, int size = 64) = 0;	
#endif
};


//Wrapper for the serial and USB dongles
class PdaDongle : public Dongle
{
public:
#ifndef __BORLANDC__
	PdaDongle::PdaDongle(HMODULE resMod = NULL, bool rescanUSB = false, bool noUsbInstall = false);
#else
	PdaDongle::PdaDongle();
#endif
	virtual ~PdaDongle();

	bool ReadDongle (
		DWORD &optionMask,
		DWORD &optionMaskNonPda,
		char password[16], char serialNr[7], char customer[40],
		unsigned short &dongleMonth,
		unsigned short &dongleDayOfMonth,
		unsigned short &dongleYear,
		short &timeLimit,
		unsigned char* romData);

	bool WriteNewPassword (char* newPassword);
	
	char * GetType();
	int TestDongle();
	TestDongleResult TestDongle(unsigned char* romData);

	//Multi license
	bool ReadMultiLicenses(BYTE* licenses, int nrLicenses = NR_MULTI_OPTIONS);

	bool ReadDongleContents(DongleContents* pContent);
	
#ifndef DONGLE_LITE
	int InitDongle();
	
	bool WriteDongle (
		DWORD optionMask,
		DWORD optionMaskNonPda,
		char password[16], char serialNr[7], char customer[40],
		unsigned short dongleMonth,
		unsigned short dongleDayOfMonth,
		unsigned short dongleYear,
		short timeLimit);
	
	bool WriteOptionsAndTimeLimit (
		DWORD optionMask,
		DWORD optionMaskNonPda,
		unsigned short dongleMonth,
		unsigned short dongleDayOfMonth,
		unsigned short dongleYear,
		short timeLimit);
	
	int WriteScratch(unsigned char * buffer, int size = 64);
	int ReadScratch(unsigned char * buffer, int size = 64);

	bool WriteMultiLicenses(BYTE* licenses, int nrLicenses = NR_MULTI_OPTIONS);

	bool WriteDongleContents(DongleContents* pContent);
#endif	

	static BOOL bShowMessageBoxes;

private:
	static BOOL bUseSecurity;
	static BOOL bUseGlobal;

	void CreateDongle();
	int AutoDetect();

	Dongle * dongle;
	HANDLE hDongleAccess;
};

#endif //CONTENTS_ONLY

#endif
