// H1Protocol.h

#pragma once

#include "ByteStream.h"
#include <H1Def.h>
#include <map>
using namespace std;


using namespace System;

namespace iba {
	
	public interface class ITelegram
	{
		bool ReadFromBuffer(H1ByteStream^ stream);
		bool WriteToBuffer(H1ByteStream^ stream);
//		property unsigned int MaxSize{ unsigned int get();};
		property unsigned int ActSize{ unsigned int get();};
	};

	public ref class CH1Manager
	{
	private:
		bool m_driverloaded;
		bool m_connections;
		String^ m_lastError;
		String^ LoadError(UINT id);
		map<unsigned short, H1_RECPARAMS*>* m_rp;
		map<unsigned short, H1_SENDPARAMS*>* m_sp;
		unsigned char* m_blockedBytes;
		unsigned int m_blockpos;
	public:
		enum class H1Result {	
				ALL_CLEAR=0,
				BAD_CR_PARAMS=H1_BAD_CR_PARAMS,		// CR parameters bad
				NO_SLOT=H1_NO_SLOT,					// Maximun number of connections are running
				WAIT_CONNECT=H1_WAIT_CONNECT,		// Connection doesn't exist
				NOT_IMPLEMENTED=H1_NOT_IMPLEMENTED,	// Function not implemented
				BAD_LINE=H1_BAD_LINE,				// Handle of the connection is not valid
				WAIT_DATA=H1_WAIT_DATA,				// No data available yet
				WAIT_SEND=H1_WAIT_SEND,				// Wait for ack the sent data1
				INTERNAL_ERROR=H1_INTERNAL_ERROR,	// This should happen never
				NO_REQUEST=H1_NO_REQUEST,			// Polling a non existing job
				NO_DRIVER=H1_NO_DRIVER,				// IfDriverOpen not called, or
													// no TCP/IP driver installed
													// or the network cannto be reached
				UEBERLAST=H1_UEBERLAST,				// Overload of the network or the destination station
				BLOCKED_DATA=H1_BLOCKED_DATA,		// successfully received blocked data
				NO_ADAPTER=H1_NO_ADAPTER,			// selected adapter is not valid
				ALREADY_RUNNING=H1_ALREADY_RUNNING,	// job is already running
				NOT_SUPPORTED=H1_NOT_SUPPORTED,		// The function is not supported
				_TRY_AGAIN=H1_TRY_AGAIN,			// Not enough resources temporary. Please call it again later
				NO_MEMORY=H1_NO_MEMORY,				// Open Driver not possible under win3.x
				BAD_SIGNATURE=H1_BAD_SIGNATURE,		// The Industrial Ethernet Signature not received
				TELEGRAM_ERROR=20,					// Own error code for failing to read or write telegram	
				OPERATING_SYSTEM_ERROR=21			// Own error code for operating system error,e.g. disconnected cable
		};
		bool SetStationAddress(array<Byte>^ ownMacAdress);
		bool Connect(u_short% vnr, int priority, bool active,  array<Byte>^ otherMacAdress,  String^ ownTSAP, String^ destTSAP, H1Result% result, int timeout);
		property String^ LastError{ String^ get(); }
		bool DisconnectAll();
		//bool Disconnect(unsigned short);
		bool GetConnectionStatus(unsigned short vnr, H1Result% result);
		bool StartRead(unsigned short vnr, H1Result% result);
		bool StoreBlockedBytes(unsigned short vnr);
		bool FinishRead(unsigned short vnr, ITelegram^ telegram);
		bool GetReadStatus(unsigned short vnr, H1Result% result);
		bool StartSend(unsigned short vnr, H1Result% result, ITelegram^ telegram);
		bool GetSendStatus(unsigned short vnr, H1Result% result);
		bool SendNoPoll(unsigned short vnr, H1Result% result, ITelegram^ telegram);
		bool SetSendTimeout(int timeout);

		CH1Manager();  
		~CH1Manager(); 
	protected:
		!CH1Manager();
	};
}
