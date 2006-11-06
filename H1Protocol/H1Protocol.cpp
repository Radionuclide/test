// This is the main DLL file.

#include "stdafx.h"
#include "H1Protocol.h"
#include "resource.h"

using namespace System::Runtime::InteropServices;

const int PARAMLENGTH = 2048;

namespace iba {

	CH1Manager::CH1Manager()
	{
		m_connections = 0;	
		int error = H1DriverOpen();
		m_driverloaded = error == 0;
		CString message;
		m_lastError = m_driverloaded?"":LoadError(DRIVER_LOAD_FAILED);
		H1_SENDPARAMS* m_sp = (H1_SENDPARAMS *) malloc(PARAMLENGTH);
		m_rp = new map<unsigned short, H1_RECPARAMS*>();
		m_blockedBytes = new unsigned char [4096];
		m_blockpos = 0;
	}

	String^ CH1Manager::LoadError(UINT id)
	{
		CString message;
		message.LoadString(id);
		return gcnew String(message);
	}

	CH1Manager::~CH1Manager()
	{
		if (m_connections) DisconnectAll();
		if (m_driverloaded) H1DriverClose();
		m_driverloaded= false;
		if (m_rp)
		{
			for (map<unsigned short,H1_RECPARAMS*>::iterator it = m_rp->begin(); it != m_rp->end(); it++)
			{
				if (it->second)
				{
					free(it->second);
				}
			}
			m_rp->clear();
			delete m_rp;
			m_rp = 0;
		}
		if (m_blockedBytes) delete [] m_blockedBytes;
		m_blockedBytes = NULL;
		if (m_sp) free(m_sp);
		m_sp = NULL;
	}

	CH1Manager::!CH1Manager()
	{
		if (m_connections) DisconnectAll();
		if (m_driverloaded) H1DriverClose();
		m_driverloaded = false;
		if (m_rp)
		{
			for (map<unsigned short,H1_RECPARAMS*>::iterator it = m_rp->begin(); it != m_rp->end(); it++)
			{
				if (it->second)
				{
					free(it->second);
				}
			}
			m_rp->clear();
			delete m_rp;
			m_rp = 0;
		}
		if (m_blockedBytes) delete [] m_blockedBytes;
		m_blockedBytes = NULL;
		if (m_sp) free(m_sp);
		m_rp = NULL;
		m_sp = NULL;
	}

	bool CH1Manager::SetStationAddress(array<Byte>^ ownMacAdress)
	{
		if (!m_driverloaded) 
		{
			return false;
		}
		unsigned char address[6] = {
			ownMacAdress[0],
			ownMacAdress[1],
			ownMacAdress[2],
			ownMacAdress[3],
			ownMacAdress[4],
			ownMacAdress[5]};
		int error = H1SetzeStationsAdresse(address);
		if (error) 
		{
			m_lastError = LoadError(ERR_STATION_ADDRESS) + ": " + error.ToString();
			return false;
		};
		error = H1HoleStationsAdresse(address);
		if (!error)
		{
			for (int i = 0; i < 6; i++)
				if (ownMacAdress[i] != address[i])
				{
					m_lastError = LoadError(ERR_STATION_ADDRESS);
					return false;
				}
			return true;
		}
		else 
		{
			m_lastError = LoadError(ERR_STATION_ADDRESS) + " : " + error.ToString();
			return false;
		}
	}

	bool CH1Manager::Connect(u_short% vnr, int priority, bool active,  array<Byte>^ otherMacAdress,  String^ ownTSAP, String^ destTSAP, H1Result% result, int timeout)
	{
		result = (H1Result) 0;

		if (!m_driverloaded) return  false;
		H1_CONNECT_PARAMS cr;
    	switch (priority)
		{
			case 0:	cr.CrParams.Priority = EXPRESS_PRIORITY_0; break;
			case 1:	cr.CrParams.Priority = EXPRESS_PRIORITY_1; break;
			case 2:	cr.CrParams.Priority = PRIORITY_2; break;
			case 3:	cr.CrParams.Priority = PRIORITY_3; break;
			case 4:	cr.CrParams.Priority = PRIORITY_4; break;
			default:
				m_lastError = LoadError(ERR_PRIORITY);
			return false;

		}

		if (active)
		cr.CrParams.ConnectType = NORMAL_LINE;
		else
		  cr.CrParams.ConnectType = NORMAL_LINE | PASSIVE_LINE;
		cr.CrParams.LenDestAddr = 6;
		for (int i = 0; i < 6; i++) cr.CrParams.DestAddr[i] = otherMacAdress[i];
		cr.CrParams.LenNSAP = 0;
		
		cr.CrParams.LenDestTSAP = Math::Max(16,destTSAP->Length);
		IntPtr buffer = Marshal::StringToHGlobalAnsi(destTSAP);
		strncpy((char*) cr.CrParams.DestTSAP, (char*) buffer.ToPointer(),cr.CrParams.LenDestTSAP);
		Marshal::FreeHGlobal(buffer);

		cr.CrParams.LenOwnTSAP = Math::Max(16,ownTSAP->Length);
		buffer = Marshal::StringToHGlobalAnsi(ownTSAP);
		strncpy((char*) cr.CrParams.OwnTSAP,(char*) buffer.ToPointer(),cr.CrParams.LenOwnTSAP);
		Marshal::FreeHGlobal(buffer);
		cr.CrParams.LenConnParams = 0;

		int error = H1StarteVerbindung(&cr);
		if (error)
		{
			result = (H1Result) cr.Fehler;
			switch (cr.Fehler)
			{
			case H1_BAD_CR_PARAMS: m_lastError = "H1_BAD_CR_PARAMS";
			case H1_BAD_LINE: m_lastError = "H1_BAD_LINE";
			case H1_NO_ADAPTER: m_lastError = "H1_NO_ADAPTER";
			case H1_NO_DRIVER: m_lastError = "H1_NO_DRIVER";
			case H1_NO_SLOT: m_lastError = "H1_NO_SLOT";
			case H1_WAIT_CONNECT: m_lastError = "H1_WAIT_CONNECT";
			default: m_lastError = LoadError(ERR_CONNECT);
			}
			return false;
		}
		
		vnr = cr.Vnr;

		for (int i = 0; i < timeout; i++) //wait maximum timeout
		{
			int err;
			H1_RECPARAMS  rp;
			rp.Vnr = vnr;
			if (!(err = H1TesteStatus(&rp)))
			{
				switch (rp.Fehler)
				{
  					case 0:
						m_connections++;
						return true;
					case H1_WAIT_CONNECT :
						break;
					default:
						result = (H1Result) cr.Fehler;
						m_lastError =  "Error while trying to connect... " + rp.Fehler.ToString();
						H1StoppeVerbindung(vnr);
						return false;
				}
			}
			else
			{
				H1StoppeVerbindung(vnr);
				m_lastError =  "Error while trying to connect... " + rp.Fehler.ToString();
				return  false;
			}
			//wait one second
			Sleep(1000);
		}
		H1StoppeVerbindung(vnr);
		m_lastError = "Time out trying to connect";
		return false;
	}

	bool CH1Manager::DisconnectAll()
	{
		if (!m_connections) return false;
		else
		{
			int error = H1StoppeVerbindungen();
			m_connections = 0;
			return !error;
		}
	}

	bool CH1Manager::Disconnect(unsigned short vnr)
	{
		if (!m_connections) return false;
		else
		{
			int error = H1StoppeVerbindung(vnr);
			m_connections--;
			return !error;
		}
	}

	bool CH1Manager::GetConnectionStatus(unsigned short vnr, H1Result% result)
	{
		if (!m_connections) return false;
		H1_RECPARAMS rp;
		memset(&rp,0,sizeof(rp));
		rp.Vnr = vnr;
		int error = H1TesteStatus(&rp);
		result = (H1Result) rp.Fehler;
		if (error) return false; 
		return true;
	}

	bool CH1Manager::StartRead(unsigned short vnr, H1Result% result)
	{
		H1_RECPARAMS *rp;
		if (m_rp->find(vnr) != m_rp->end())
			rp = (*m_rp)[vnr];
		else
			rp = (*m_rp)[vnr] = (H1_RECPARAMS *) malloc(PARAMLENGTH);

		memset(rp,0,PARAMLENGTH);
		rp->DataLen = PARAMLENGTH - sizeof(H1_RECPARAMS);
		rp->Vnr = vnr;
		unsigned short err = H1StarteLesen(rp);
		result = (H1Result) rp->Fehler;
		return (!err); 
	}

	bool CH1Manager::FinishRead(unsigned short vnr, ITelegram^ telegram)
	{
		H1_RECPARAMS *rp=0;
		if (m_rp->find(vnr) == m_rp->end())
			return false;
		else 
			rp = (*m_rp)[vnr];

		if (!m_connections) return false;

		if (m_blockpos != 0)
		{
			StoreBlockedBytes(vnr);
			bool ans = telegram->ReadFromBuffer(gcnew H1ByteStream(IntPtr((void*)m_blockedBytes),m_blockpos,true));
			m_blockpos = 0;
			return ans;
		}
		return telegram->ReadFromBuffer(gcnew H1ByteStream(IntPtr((void*)rp->Daten),rp->RecLen,true));
	}

	bool CH1Manager::StoreBlockedBytes(unsigned short vnr)
	{
		H1_RECPARAMS *rp=0;
		if (m_rp->find(vnr) == m_rp->end())
			return false;
		else 
			rp = (*m_rp)[vnr];
		memcpy(m_blockedBytes + m_blockpos,rp->Daten,rp->RecLen);
		m_blockpos += rp->RecLen;
		return true;
	}

	bool CH1Manager::GetReadStatus(unsigned short vnr, H1Result% result)
	{
		H1_RECPARAMS *rp;
		if (m_rp->find(vnr) == m_rp->end())
			return false;
		else
			rp = (*m_rp)[vnr];
		int error = H1AbfrageLesen(rp);
		result = (H1Result) rp->Fehler;
		if (error) return false; 
		return true;
	}

	bool CH1Manager::StartSend(unsigned short vnr, H1Result% result, ITelegram^ telegram)
	{
		memset(m_sp,0,PARAMLENGTH);
		m_sp->Vnr = vnr;
		bool ok = telegram->WriteToBuffer(gcnew H1ByteStream(IntPtr((void*)m_sp->Daten),telegram->ActSize,true));
		if (!ok) {
			result = H1Result::TELEGRAM_ERROR;
			return false;
		}
		int error = H1StarteSenden(m_sp);
		result = (H1Result) m_sp->Fehler;
		return !error;
	}

	bool CH1Manager::GetSendStatus(H1Result% result)
	{
		int error = H1AbfrageSenden(m_sp);
		result = (H1Result) m_sp->Fehler;
		if (error) return false; 
		return true;
	}

	String^ CH1Manager::LastError::get() { return m_lastError; }

	bool CH1Manager::SendNoPoll(unsigned short vnr, H1Result% result, ITelegram^ telegram)
	{
		result = H1Result::ALL_CLEAR;
		memset(m_sp,0,PARAMLENGTH);	
		m_sp->DataLen = telegram->ActSize;
		m_sp->Vnr = m_vnr;
		if (!telegram->WriteToBuffer(gcnew H1ByteStream(IntPtr((void*)m_sp->Daten),telegram->ActSize,true)))
		{
			result = H1Result::TELEGRAM_ERROR;
			m_lastError = "Could not compose telegram";
			return false;
		}

		u_short err = H1SendeDaten(m_sp);
		if (err)
		{
			m_lastError = "Error sending " + err.ToString() + " " + m_sp->Fehler.ToString();
		}	
		return (err == 0);
	}

	bool CH1Manager::SetSendTimeout(int timeout)
	{
		H1_INITVALUES init;
		bool ok = H1HoleStandardwerte(&init)==0;
		if (ok) 
		{
			init.TimeoutSend = (unsigned short) timeout;
			ok = H1SetzeStandardwerte(&init)==0;
		}
		return ok;
	}
}

