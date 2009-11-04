// This is the main DLL file.

#include "stdafx.h"
#include <msclr/marshal.h>
#include "RohWriter.h"
#include <cliext/set> 
#include <cliext/map> 
#include <map> 
#include <cliext/vector>
#include "windows.h"

using namespace System;
using namespace ibaFilesLiteLib;
using namespace msclr::interop;

namespace iba {
	int RohWriter::Write(RohWriterInput^ input, String^ datname, String^ Rohfile)
	{
		errorDataLineInput = nullptr;
		errorChannelLineInput = nullptr;
		errorMessage = nullptr;
		IbaFile^ ibaFile;
		//second, initialize ibaFiles
		try
		{
			ibaFile = gcnew IbaFileClass();
		}
		catch (Exception^ ex)
		{
			errorMessage = ex->Message;
			return 6;
		}

		try
		{
			ibaFile->Open(datname);
		}
		catch (Exception^ ex)
		{
			errorMessage = ex->Message;
			return 7;
		}

		cliext::set<String^> requiredInfoFields; 
		cliext::map<String^,String^> infoFieldsCopy; //copy of the required infofields so ibaFiles needs not to be asked again later 
		cliext::map<String^,int> VectorNames; //vectorNames

		//assemble required infofields
		for each (RohWriterDataLineInput^ dateLine in input->StichDaten)
			requiredInfoFields.insert(dateLine->ibaName);
		for each (RohWriterDataLineInput^ dateLine in input->KopfDaten)
			requiredInfoFields.insert(dateLine->ibaName);
		for each (RohWriterDataLineInput^ dateLine in input->SchlussDaten)
			requiredInfoFields.insert(dateLine->ibaName);

		//get infofields, get vectors
		try
		{
			for (int i = 0; true; i++)
			{
				String^ name;
				String^ value;
				ibaFile->QueryInfoByIndex(i,name, value);
				if (String::IsNullOrEmpty(name) && String::IsNullOrEmpty(value)) break; //no more infofields
				if (name->StartsWith("Vector_name_",StringComparison::InvariantCultureIgnoreCase))
				{
					int vecIndex;
					if (System::Int32::TryParse(name->Substring(12),vecIndex))
					{
						VectorNames.insert(cliext::pair<String^,int>(value,vecIndex));
					}
				}
				else
				{
					cliext::set<String^>::iterator it = requiredInfoFields.find(name);
					if (it != requiredInfoFields.end())
					{
						requiredInfoFields.erase(it);
						infoFieldsCopy.insert(cliext::pair<String^,String^>(name,value));
					}
				}
			}
		}
		catch (Exception^ ex) //unexpected error while reading infofields
		{
			errorMessage = ex->Message;
			return 5;
		}		

		if (!requiredInfoFields.empty()) //report error if an infofield is missing
		{
			for each (RohWriterDataLineInput^ dateLine in input->StichDaten)
			{
				if (requiredInfoFields.find(dateLine->ibaName) != requiredInfoFields.end())
				{	
					errorDataLineInput = dateLine;
					return 1;
				}
			}
			for each (RohWriterDataLineInput^ dateLine in input->KopfDaten)
			{
				if (requiredInfoFields.find(dateLine->ibaName) != requiredInfoFields.end())
				{	
					errorDataLineInput = dateLine;
					return 2;
				}
			}
			for each (RohWriterDataLineInput^ dateLine in input->SchlussDaten)
			{
				if (requiredInfoFields.find(dateLine->ibaName) != requiredInfoFields.end())
				{	
					errorDataLineInput = dateLine;
					return 3;
				}
			}
		}

		//assemble required channels
		cliext::set<String^> requiredChannels;
		cliext::set<int> requiredVectors;
		cliext::map<String^,IbaChannelReader^> channelsCopy; //copy of the required channels so we do not need to iterate through the channels again later
		cliext::map<int,cliext::map<int,IbaChannelReader^>^ > vectorChannelsCopy; //likewise for vectors in twodim map (vector index, index in vector)

		for each (RohWriterChannelLineInput^ channel in input->Kanalen)
		{
			cliext::map<String^,int>::iterator it = VectorNames.find(channel->ibaName);
			if (it != VectorNames.end())
				requiredVectors.insert(it->second);
			else
				requiredChannels.insert(channel->ibaName);
		}

		try //iterate over channels
		{
			IbaEnumChannelReader^ enumerator = dynamic_cast<IbaEnumChannelReader^>(ibaFile->EnumChannels());
			while (!enumerator->IsAtEnd())
			{
				IbaChannelReader^ reader = dynamic_cast<IbaChannelReader^>(enumerator->Next());
				String^ name = reader->QueryInfoByName("name");
				if (!String::IsNullOrEmpty(name))
				{
					cliext::set<String^>::iterator it = requiredChannels.find(name);
					if (it != requiredChannels.end())
					{
						requiredChannels.erase(it);
						channelsCopy.insert(cliext::pair<String^, IbaChannelReader^>(name,reader));
					}
				}
				String^ vecInfo = reader->QueryInfoByName("vector");
				if (!String::IsNullOrEmpty(vecInfo))
				{
					int vectorNr,rowNr;
					int pos = vecInfo->IndexOf(".");
					if (pos > 0 && Int32::TryParse(vecInfo->Substring(0,pos),vectorNr) && Int32::TryParse(vecInfo->Substring(pos+1),rowNr))
						vectorChannelsCopy[vectorNr][rowNr] = reader;
				}
			}
		}
		catch (Exception^ ex) //unexpected error while getting channels
		{
			errorMessage = ex->Message;
			return 5;
		}	

		for (cliext::map<int,cliext::map<int,IbaChannelReader^>^ >::iterator it = vectorChannelsCopy.begin(); it != vectorChannelsCopy.end(); it++)
		{
			if (it->second->size() == it->second->rbegin()->first+1) requiredVectors.erase(it->first);
		}

		if (!requiredChannels.empty() || !requiredVectors.empty())
		{
			for each (RohWriterChannelLineInput^ channel in input->Kanalen)
			{
				if ((requiredChannels.find(channel->ibaName) != requiredChannels.end())
					||(VectorNames.find(channel->ibaName) != VectorNames.end() && requiredVectors.find(VectorNames[channel->ibaName]) != requiredVectors.end()))
				{
					errorChannelLineInput = channel;
					return 4;
				}
			}
		}
		
		marshal_context context;
		const char* RohFileNameStr = context.marshal_as<const char*>(Rohfile);
		hFile = CreateFile(RohFileNameStr, GENERIC_WRITE, FILE_SHARE_READ, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
		if (hFile == INVALID_HANDLE_VALUE)
		{
			errorMessage = "Failed to create .roh file";
			return 8;
		}
		SetFileAttributes(RohFileNameStr,FILE_ATTRIBUTE_NORMAL);

		try
		{
			char buf[256];
			WriteString("Ver 1.00 \n");
			const char* StartTimeStr = context.marshal_as<const char*>(ibaFile->QueryInfoByName("starttime"));
			strncpy(buf,StartTimeStr,17); //only up to minutes and extra ":"
			buf[17] = '\n';
			buf[18]= 0;
			WriteString(buf); //time written
			WriteString("dateiinhalt       ;\n");
			for (int i = 0; i < 48; i++)
				buf[i] = ' ';
			buf[48] = ';';
			buf[49] = '\n';
			DWORD StichDatenOffsetHeaderPos = WritePartialHeaderLine("Stichdaten",input->StichDaten->Length);
			DWORD KommentareOffsetHeaderPos = WritePartialHeaderLine("Kommentare",String::IsNullOrEmpty(input->Kommentare)?0:1);
			DWORD KopfDatenOffsetHeaderPos = WritePartialHeaderLine("Kopfdaten",input->KopfDaten->Length);
			DWORD SchlussDatenOffsetHeaderPos = WritePartialHeaderLine("Shlussdaten",input->KopfDaten->Length);;
			DWORD KurzbezeichnerOffsetHeaderPos = WritePartialHeaderLine("Kurzbezeichner",String::IsNullOrEmpty(input->Kurzbezeichner)?0:1);
			DWORD ParameterOffsetHeaderPos = WritePartialHeaderLine("Parameter",String::IsNullOrEmpty(input->Parameter)?0:1);
			DWORD KanalbeschreibungOffsetHeaderPos = WritePartialHeaderLine("Kanalbeschreibung",input->Kanalen->Length);
			cliext::vector<DWORD> channelOffsetHeaderPositions;
			for each (RohWriterChannelLineInput^ channel in input->Kanalen)
			{
				int size = 1;
				cliext::map<String^,int>::iterator it = VectorNames.find(channel->ibaName);
				if (it != VectorNames.end())
					size = vectorChannelsCopy[it->second]->size();
				String^ name = channel->KurzBezeichnung;
				channelOffsetHeaderPositions.push_back(WritePartialChannelHeaderLine(context.marshal_as<const char*>(name),size));
			}

		}
		catch (HRESULT hr) //thrown by write or seek
		{
			SetErrorMessageFromHResult(hr);
			return 8;
		}
		catch (Exception^ ex) //unexpected error while writing stuff
		{
			errorMessage = ex->Message;
			return 9;
		}	

		return 0;
	}

	void RohWriter::SetErrorMessageFromHResult(HRESULT hr)
	{
		char err[256];
		wsprintf (err, "0x%X", hr);
		//::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), err, sizeof(err), 0);
		::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, hr, 0, err, sizeof(err), 0);
		char *pCr = strchr (err, '\r');
		if (pCr)
			*pCr = 0;
		errorMessage = marshal_as<String^>(err);
	}

	DWORD RohWriter::WritePartialHeaderLine(const char* headerName, int nrElements)
	{ //writes the headerline but without offset and blocksize //filled in later
		DWORD offset = GetPosition();
		char buf[50];
		int nameSize = strlen(headerName);
		strncpy(buf,headerName,nameSize);
		for (int i = nameSize; i < 48; i++) buf[i] = ' ';
		wsprintf(buf+44,"%d",nrElements);
		buf[48] = ';';
		buf[49] = '\n';
		Write(buf,50);
		return offset;
	}
	
	void RohWriter::CompleteHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD blockSize)
	{
		char buf[50];
		wsprintf(buf,"%u",offsetToData);
		int len = strlen(buf);
		Seek(offsetHeaderLine+32);
		Write(buf,len);
		wsprintf(buf,"%u",blockSize);
		len = strlen(buf);
		Seek(offsetHeaderLine+44);
		Write(buf,len);
	}

	DWORD RohWriter::WritePartialChannelHeaderLine(const char* shortName, int nrElements)
	{

		return 0;
	}

	void RohWriter::CompleteChannelHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD blockSize)
	{
	}
}