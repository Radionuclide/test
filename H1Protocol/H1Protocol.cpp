// This is the main DLL file.

#include "stdafx.h"
#include "H1Protocol.h"
#include "resource.h"

using namespace System::Runtime::InteropServices;

const int RECPARAMLENGTH = 2048;
const int SENDPARAMLENGTH = 8192;

namespace iba {

	CH1Manager::CH1Manager()
	{
		m_connections = false;	
		int error = H1DriverOpen();
		m_driverloaded = error == 0;
		CString message;
		m_lastError = m_driverloaded?"":LoadError(DRIVER_LOAD_FAILED);
		m_sp = new map<unsigned short, H1_SENDPARAMS*>();
		m_rp = new map<unsigned short, H1_RECPARAMS*>();
		m_blockedBytes = new unsigned char [16*4096];
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
		m_connections = false;
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
		if (m_sp)
		{
			for (map<unsigned short,H1_SENDPARAMS*>::iterator it = m_sp->begin(); it != m_sp->end(); it++)
			{
				if (it->second)
				{
					free(it->second);
				}
			}
			m_sp->clear();
			delete m_sp;
			m_sp = 0;
		}
	}

	CH1Manager::!CH1Manager()
	{
		if (m_connections) DisconnectAll();
		m_connections = false;
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
		if (m_sp)
		{
			for (map<unsigned short,H1_SENDPARAMS*>::iterator it = m_sp->begin(); it != m_sp->end(); it++)
			{
				if (it->second)
				{
					free(it->second);
				}
			}
			m_sp->clear();
			delete m_sp;
			m_sp = 0;
		}
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
		unsigned char setaddress[14];
		memset(setaddress,0,sizeof(setaddress));

		int error = H1SetzeStationsAdresse(address);
		if (error) 
		{
			m_lastError = LoadError(ERR_STATION_ADDRESS) + ": " + error.ToString();
			return false;
		};
		try
		{
			error = H1HoleStationsAdresse(setaddress);
		}
		catch (...)
		{
			return false;
		}
		if (!error)
		{
			for (int i = 0; i < 6; i++)
				if (ownMacAdress[i] != setaddress[i+2])
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

	bool CH1Manager::SetStationAddress2(array<Byte>^ oldPhysicalMacAdress, array<Byte>^ newStationMacAdress)
	{
		if (!m_driverloaded) 
		{
			return false;
		}
		for (unsigned short i = 0; i < 4; i++)
		{
			unsigned char address[14];
			memset(address,0,sizeof(address));
			int error = H1HoleStationsAdresseKarte(address,i);
			if (error)
			{
				m_lastError = LoadError(ERR_STATION_ADDRESS) + ": " + error.ToString();
				return false;
			}
			bool adressfound = true;
			for (int j = 0; j < 6 && adressfound; j++)
				if (oldPhysicalMacAdress[j] != address[j+6])
					adressfound = false;
			if (adressfound)
			{
				memset(address,0,sizeof(address));
				for (int j = 0; j < 6 && adressfound; j++)
				{
					address[j] = newStationMacAdress[j];
				}
				error = H1SetzeStationsAdresseKarte(address,i);
				if (error)
				{
					m_lastError = LoadError(ERR_STATION_ADDRESS) + ": " + error.ToString();
					return false;
				}
				return true;
			}
		}
		m_lastError = LoadError(ERR_STATION_ADDRESS) + ": none of the first four network cards has the correct MAC adress" ;
		return false;
	}

	bool CH1Manager::Connect(u_short% vnr, int priority, bool active,  array<Byte>^ otherMacAdress,  String^ ownTSAP, String^ destTSAP, H1Result% result, int timeout)
	{
		//delete obsolete send parameters and recieve parameters
		unsigned short findval = vnr;
		map<unsigned short,H1_SENDPARAMS*>::iterator it = m_sp->find(findval);
		if (it != m_sp->end())
		{
			free (it->second);
			m_sp->erase(it);
		}
		//delete obsolete recieve parameters
		map<unsigned short,H1_RECPARAMS*>::iterator it2 = m_rp->find(findval);
		if (it2 != m_rp->end())
		{
			free (it2->second);
			m_rp->erase(it2);
		}

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
		
		cr.CrParams.LenDestTSAP = Math::Min(16,destTSAP->Length);
		IntPtr buffer = Marshal::StringToHGlobalAnsi(destTSAP);
		strncpy_s((char*) cr.CrParams.DestTSAP,16, (char*) buffer.ToPointer(),cr.CrParams.LenDestTSAP);
		Marshal::FreeHGlobal(buffer);

		cr.CrParams.LenOwnTSAP = Math::Min(16,ownTSAP->Length);
		buffer = Marshal::StringToHGlobalAnsi(ownTSAP);
		strncpy_s((char*) cr.CrParams.OwnTSAP,16,(char*) buffer.ToPointer(),cr.CrParams.LenOwnTSAP);
		Marshal::FreeHGlobal(buffer);
		cr.CrParams.LenConnParams = 0;

		int error = H1StarteVerbindung(&cr);
		if (error)
		{
			result = (H1Result) cr.Fehler;
			switch (cr.Fehler)
			{
			case H1_BAD_CR_PARAMS: m_lastError = "H1_BAD_CR_PARAMS"; break;
			case H1_BAD_LINE: m_lastError = "H1_BAD_LINE"; break;
			case H1_NO_ADAPTER: m_lastError = "H1_NO_ADAPTER"; break;
			case H1_NO_DRIVER: m_lastError = "H1_NO_DRIVER"; break;
			case H1_NO_SLOT: m_lastError = "H1_NO_SLOT"; break;
			case H1_WAIT_CONNECT: m_lastError = "H1_WAIT_CONNECT"; break;
			default: m_lastError = LoadError(ERR_CONNECT);
			}
			vnr = 0;
			return false;
		}
		
		vnr = cr.Vnr;

		for (int i = 0; i < timeout; i++) //wait maximum timeout
		{
			int err;
			H1_RECPARAMS  rp;
			memset(&rp, 0, sizeof(H1_RECPARAMS));
			rp.Vnr = vnr;
			if (!(err = H1TesteStatus(&rp)))
			{
				switch (rp.Fehler)
				{
  					case 0:
						m_connections = true;
						return true;
					case H1_WAIT_CONNECT :
						break;
					default:
						result = (H1Result) rp.Fehler;
						m_lastError =  "Error while trying to connect... " + rp.Fehler.ToString();
						H1StoppeVerbindung(vnr);
						vnr = 0;
						return false;
				}
			}
			else
			{
				H1StoppeVerbindung(vnr);
				vnr = 0;
				m_lastError =  "Error while trying to connect... " + rp.Fehler.ToString();
				return  false;
			}
			//wait one second
			Sleep(1000);
		}
		//H1StoppeVerbindung(vnr);
		//vnr = 0;
		m_connections = true;
		m_lastError = "Time out trying to connect";
		return false;
	}

	bool CH1Manager::DisconnectAll()
	{
		if (!m_connections) return false;
		else
		{
			int error = H1StoppeVerbindungen();
			m_connections = false;
			return !error;
		}
	}

	//bool CH1Manager::Disconnect(unsigned short vnr)
	//{
	//	if (!m_connections) return false;
	//	else
	//	{
	//		int error = H1StoppeVerbindung(vnr);
	//		m_connections--;
	//		return !error;
	//	}
	//}

	void CH1Manager::GetConnectionStatus(unsigned short vnr, H1Result% result)
	{
		if (!m_connections) 
		{
			result = H1Result::BAD_LINE;
			return;// false;
		}
		H1_RECPARAMS rp;
		memset(&rp,0,sizeof(rp));
		rp.Vnr = vnr;
		int error = H1TesteStatus(&rp);
		result = (H1Result) rp.Fehler;
		if (error) {
			result = H1Result::OPERATING_SYSTEM_ERROR;
			m_lastError = "Operating system error (unplugged cable?)";
			//return false; 
		}
		//return true;
	}

	bool CH1Manager::StartRead(unsigned short vnr, H1Result% result)
	{
		H1_RECPARAMS *rp;
		if (m_rp->find(vnr) != m_rp->end())
			rp = (*m_rp)[vnr];
		else
			rp = (*m_rp)[vnr] = (H1_RECPARAMS *) malloc(RECPARAMLENGTH);

		memset(rp,0,RECPARAMLENGTH);
		rp->DataLen = RECPARAMLENGTH - sizeof(H1_RECPARAMS);
		rp->Vnr = vnr;
		unsigned short err = H1StarteLesen(rp);
		result = (H1Result) rp->Fehler;
		return (!err); 
	}

	bool CH1Manager::FinishRead(unsigned short vnr, ITelegram^% telegram)
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
		else return telegram->ReadFromBuffer(gcnew H1ByteStream(IntPtr((void*)rp->Daten),rp->RecLen,true));
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

	void CH1Manager::GetReadStatus(unsigned short vnr, H1Result% result)
	{
		H1_RECPARAMS *rp;
		if (m_rp->find(vnr) == m_rp->end())
		{
			result = H1Result::NO_REQUEST;		
			return;// false;
		}
		else
			rp = (*m_rp)[vnr];
		int error = H1AbfrageLesen(rp);
		result = (H1Result) rp->Fehler;
		if (result == H1Result::WAIT_CONNECT)
			m_lastError = "connection lost";

//		if (error) return false; 
//		return true;
	}

	bool CH1Manager::StartSend(unsigned short vnr, H1Result% result, ITelegram^ telegram)
	{
		H1_SENDPARAMS *sp;
		if (m_sp->find(vnr) != m_sp->end())
			sp = (*m_sp)[vnr];
		else
			sp = (*m_sp)[vnr] = (H1_SENDPARAMS *) malloc(SENDPARAMLENGTH);

		memset(sp,0,SENDPARAMLENGTH);
		sp->Vnr = vnr;
		sp->DataLen = telegram->ActSize;
		bool ok = telegram->WriteToBuffer(gcnew H1ByteStream(IntPtr((void*)sp->Daten),telegram->ActSize,true));
		if (!ok) {
			result = H1Result::TELEGRAM_ERROR;
			return false;
		}
		int error = H1StarteSenden(sp);
		result = (H1Result) sp->Fehler;
		return !error;
	}

	void CH1Manager::GetSendStatus(unsigned short vnr, H1Result% result)
	{
		H1_SENDPARAMS *sp;
		if (m_sp->find(vnr) != m_sp->end())
			sp = (*m_sp)[vnr];
		else
		{
			result = H1Result::NO_REQUEST;
			return;// false;
		}

		int error = H1AbfrageSenden(sp);
		result = (H1Result) sp->Fehler;
		//if (error) return false; 
		//return true;
	}

	String^ CH1Manager::LastError::get() { return m_lastError; }

	bool CH1Manager::SendNoPoll(unsigned short vnr, H1Result% result, ITelegram^ telegram)
	{
		H1_SENDPARAMS *sp;
		if (m_sp->find(vnr) != m_sp->end())
			sp = (*m_sp)[vnr];
		else
			sp = (*m_sp)[vnr] = (H1_SENDPARAMS *) malloc(SENDPARAMLENGTH);

		result = H1Result::ALL_CLEAR;
		memset(sp,0,SENDPARAMLENGTH);	
		sp->DataLen = telegram->ActSize;
		sp->Vnr = vnr;
		if (!telegram->WriteToBuffer(gcnew H1ByteStream(IntPtr((void*)sp->Daten),telegram->ActSize,true)))
		{
			result = H1Result::TELEGRAM_ERROR;
			m_lastError = "Could not compose telegram";
			return false;
		}

		u_short err = H1SendeDaten(sp);
		if (err)
		{
			m_lastError = "Error sending " + err.ToString() + " " + sp->Fehler.ToString();
		}	
		result = (H1Result) sp->Fehler;
		return (err == 0);
	}

	bool CH1Manager::SetSendTimeout(int timeout)
	{
		////return true;
		//H1_INITVALUES init;
		//memset(&init,0,sizeof(H1_INITVALUES));
		//init.Cb = sizeof(init);
		//bool ok = H1HoleStandardwerte(&init)==0;
		//if (ok) 
		//{
		//	init.TimeoutRetrySend = 20;
		//	init.TimeoutNewSend = 20;
		//	//init.TimeoutLive = 10000;
		//	ok = H1SetzeStandardwerte(&init)==0;
		//}
		return true;
	}
}

