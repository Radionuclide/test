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
			DWORD p0 = GetPosition();
			strcpy(buf,"stichdaten        ;\n");
			Write(buf,strlen(buf));
			DWORD p1 = GetPosition();
			for each (RohWriterDataLineInput^ line in input->StichDaten)
			{
				WriteDataLine(line,infoFieldsCopy[line->ibaName],context);
			}
			DWORD p2 = GetPosition();
			CompleteHeaderLine(StichDatenOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			p0 = p2;
			strcpy(buf,"kommentare        ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			WriteTextBlock(input->Kommentare, context);
			p2 = GetPosition();
			CompleteHeaderLine(KommentareOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			p0 = p2;
			strcpy(buf,"kopfdaten         ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			for each (RohWriterDataLineInput^ line in input->KopfDaten)
			{
				WriteDataLine(line,infoFieldsCopy[line->ibaName],context);
			}
			p2 = GetPosition();	
			CompleteHeaderLine(KopfDatenOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			p0 = p2;
			strcpy(buf,"schlussdaten      ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			for each (RohWriterDataLineInput^ line in input->SchlussDaten)
			{
				WriteDataLine(line,infoFieldsCopy[line->ibaName],context);
			}
			p2 = GetPosition();	
			CompleteHeaderLine(KopfDatenOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			p0 = p2;
			strcpy(buf,"kurzbezeichner    ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			WriteTextBlock(input->Kurzbezeichner, context);
			p2 = GetPosition();
			CompleteHeaderLine(KurzbezeichnerOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			p0 = p2;
			strcpy(buf,"parameter         ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			WriteTextBlock(input->Parameter, context);
			p2 = GetPosition();
			CompleteHeaderLine(ParameterOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			strcpy(buf,"kanalbeschreibung ;\n");
			Write(buf,strlen(buf));
			p1 = GetPosition();
			cliext::vector<DWORD> channelOffsetKanalBeschreibungPositions;
			for each (RohWriterChannelLineInput^ channel in input->Kanalen)
			{
				channelOffsetKanalBeschreibungPositions.push_back(WriteChannelKanalBeschreibungLine(channel,context));
			}
			p2 = GetPosition();
			CompleteHeaderLine(KanalbeschreibungOffsetHeaderPos,p0,p2-p1);
			Seek(p2);
			//kanalen
			int index = 0;
			for each (RohWriterChannelLineInput^ channel in input->Kanalen)
			{
				p0 = p2;
				strcpy(buf,"kanal             ;\n");
				String^ tmp = channel->KurzBezeichnung + "        ";
				strncpy(buf+9,context.marshal_as<const char*>(tmp), 8);
				Write(buf,strlen(buf));		
				Write(buf,strlen(buf));
				p1 = GetPosition();
				cliext::map<String^,int>::iterator it = VectorNames.find(channel->ibaName);

				int size = -1;
				array<float>^ floatArray;
				float timebase, offset;
				Object^ obj;
				if (it != VectorNames.end())
				{
					cliext::map<int,IbaChannelReader^>^ TheVector = vectorChannelsCopy[VectorNames[channel->ibaName]];
					TheVector[0]->QueryTimebasedData(timebase,offset,obj);
					floatArray = dynamic_cast<array<float>^>(obj);
					size = floatArray->Length;
					WriteChannelData(floatArray,size,channel->dataType);
					cliext::map<int,IbaChannelReader^>::iterator it = TheVector->begin();
					it++;
					for (; it != TheVector->end(); it++)
					{
						it->second->QueryTimebasedData(timebase,offset,obj);
						floatArray = dynamic_cast<array<float>^>(obj);
						WriteChannelData(floatArray,size,channel->dataType);
					}
				}
				else
				{
					channelsCopy[channel->ibaName]->QueryTimebasedData(timebase,offset,obj);
					floatArray = dynamic_cast<array<float>^>(obj);
					size = floatArray->Length;
					WriteChannelData(floatArray,size,channel->dataType);
				}
				Write((const char*)("\n"),1);
				p2 = GetPosition();
				Seek(channelOffsetKanalBeschreibungPositions[index]+52+16+12+5); //complete in kanalbeschreibung number of elements
				WriteASCIIUIntInFile((unsigned int) size);
				CompleteChannelHeaderLine(channelOffsetHeaderPositions[index++],p0,p2-p1);
				Seek(p2);
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
		sprintf (err, "0x%X", hr);
		//::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), err, sizeof(err), 0);
		::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, 0, hr, 0, err, sizeof(err), 0);
		char *pCr = strchr (err, '\r');
		if (pCr)
			*pCr = 0;
		errorMessage = marshal_as<String^>(err);
	}

	void RohWriter::WriteASCIIIntInBuffer(char* buf, int val) //write ASCII int in buffer, no terminating zero
	{
		char bufT[10];
		sprintf(bufT,"%d",val);
		strncpy(buf,bufT,strlen(bufT));
	}

	void RohWriter::WriteASCIIUIntInBuffer(char* buf, unsigned int val) //write ASCII int in buffer, no terminating zero
	{
		char bufT[10];
		sprintf(bufT,"%u",val);

		strncpy(buf,bufT,strlen(bufT));
	}

	void RohWriter::WriteASCIIUIntInFile(unsigned int val) //write ASCII unsigned int in buffer, no terminating zero
	{
		char bufT[10];
		sprintf(bufT,"%u",val);
		Write(bufT,strlen(bufT));
	}

	DWORD RohWriter::WritePartialHeaderLine(const char* headerName, int nrElements)
	{ //writes the headerline but without offset and blocksize //filled in later
		DWORD offset = GetPosition();
		char buf[50];
		int nameSize = strlen(headerName);
		strncpy(buf,headerName,nameSize);
		for (int i = nameSize; i < 48; i++) buf[i] = ' ';
		WriteASCIIIntInBuffer(buf+44,nrElements);
		buf[48] = ';';
		buf[49] = '\n';
		Write(buf,50);
		return offset;
	}
	
	void RohWriter::CompleteHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD blockSize)
	{
		Seek(offsetHeaderLine+20);
		WriteASCIIUIntInFile(offsetToData);
		Seek(offsetHeaderLine+32);
		WriteASCIIUIntInFile(blockSize);
	}

	DWORD RohWriter::WritePartialChannelHeaderLine(const char* shortName, int nrElements)
	{
		DWORD offset = GetPosition();
					//123456712345678123456789abc123456789abc12345	
		char buf[] = "Kanal                                      ;\n";
		strncpy(buf+7,shortName,min(8,strlen(shortName)));
		WriteASCIIIntInBuffer(buf+7+8+12+12,nrElements);
		Write(buf,7+8+12+12+4+2);
		return offset;
	}

	void RohWriter::CompleteChannelHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD blockSize)
	{
		Seek(offsetHeaderLine+7+8);
		WriteASCIIUIntInFile(offsetToData);
		Seek(offsetHeaderLine+7+8+12);
		WriteASCIIUIntInFile(blockSize);
	}

	void RohWriter::WriteDataLine(RohWriterDataLineInput^ line, String^ infostr, marshal_context% context)
	{
		const char* valStr;
		if (line->dataType == DataTypeEnum::C)
		{
			valStr = context.marshal_as<const char*>(infostr);
		}
		else
		{
            double tval; 
			if (!Double::TryParse(infostr, System::Globalization::NumberStyles::Float,System::Globalization::CultureInfo::InvariantCulture, tval)) tval = -1;

			switch (line->dataType)
			{
				case DataTypeEnum::F:
				case DataTypeEnum::F4:
					valStr = context.marshal_as<const char*>(((float) tval).ToString(System::Globalization::CultureInfo::InvariantCulture));
					break;
				case DataTypeEnum::F8:
					valStr = context.marshal_as<const char*>(tval.ToString(System::Globalization::CultureInfo::InvariantCulture));
					break;
				case DataTypeEnum::I:
				case DataTypeEnum::I4:
					valStr = context.marshal_as<const char*>(((Int32) tval).ToString(System::Globalization::CultureInfo::InvariantCulture));
					break;
				case DataTypeEnum::I2:
					valStr = context.marshal_as<const char*>(((Int16) tval).ToString(System::Globalization::CultureInfo::InvariantCulture));
					break;
			}
		}
		char buf[256];
		String^ tmp = line->Bezeichnung;
		const char* varName = context.marshal_as<const char*>(tmp);
		int nameSize = min(strlen(varName),30);
		strncpy(buf,varName,nameSize);
		for (int i = nameSize; i < 30; i++) buf[i] = ' ';
		tmp = line->KurzBezeichnung;
		const char* shortName = context.marshal_as<const char*>(tmp);
		nameSize = min(strlen(shortName),8);
		strncpy(buf+30,shortName,nameSize);
		for (int i = 30+nameSize; i < 38; i++) buf[i] = ' ';
		if (String::IsNullOrEmpty(line->Einheit))
			strncpy(buf+38,"NULL    ",8);
		else
		{
			tmp = line->Einheit;
			const char* unit = context.marshal_as<const char*>(tmp);
			nameSize = min(8,strlen(unit));
			strncpy(buf+38,unit,nameSize);
			for (int i = 38+nameSize; i < 46; i++) buf[i] = ' ';
		}
		switch (line->dataType)
		{
			case DataTypeEnum::C: strncpy(buf+46,"C     ",6); break;
			case DataTypeEnum::F: strncpy(buf+46,"F     ",6); break;
			case DataTypeEnum::F4: strncpy(buf+46,"F4    ",6); break;
			case DataTypeEnum::F8: strncpy(buf+46,"F8    ",6); break;
			case DataTypeEnum::I: strncpy(buf+46,"I     ",6); break;
			case DataTypeEnum::I2: strncpy(buf+46,"I2    ",6); break;
			case DataTypeEnum::I4: strncpy(buf+46,"I4    ",6); break;
		}
		for (int i = 52; i < 62; i++) buf[i] = ' ';
		int valStrSize = min(100,strlen(valStr));
		WriteASCIIIntInBuffer(buf+52,valStrSize);
		WriteASCIIIntInBuffer(buf+57,1);
		strncpy(buf+62,valStr,valStrSize);
		buf[62+valStrSize] = ' ';
		buf[63+valStrSize] = '\n';
		Write(buf,valStrSize+64);
	}

	void RohWriter::WriteTextBlock(String^ block, marshal_context% context)
	{ //filter '/r/n' to '/n'
		String^ newBlock = block->Replace("\r\n","\n");
		if (!newBlock->EndsWith("\n"))
			newBlock = newBlock + "\n";
		const char* blockStr = context.marshal_as<const char*>(newBlock);
		Write(blockStr,strlen(blockStr));
	}

	DWORD RohWriter::WriteChannelKanalBeschreibungLine(RohWriterChannelLineInput^ line, marshal_context% context)
	{
		DWORD offset = GetPosition();
		char buf[256];
		String^ tmp = line->Bezeichnung;
		const char* varName = context.marshal_as<const char*>(tmp);
		int nameSize = min(strlen(varName),30);
		strncpy(buf,varName,nameSize);
		for (int i = nameSize; i < 30; i++) buf[i] = ' ';
		tmp = line->KurzBezeichnung;
		const char* shortName = context.marshal_as<const char*>(tmp);
		nameSize = min(strlen(shortName),8);
		strncpy(buf+30,shortName,nameSize);
		for (int i = 30+nameSize; i < 38; i++) buf[i] = ' ';
		if (String::IsNullOrEmpty(line->Einheit))
			strncpy(buf+38,"NULL    ",8);
		else
		{
			tmp = line->Einheit;
			const char* unit = context.marshal_as<const char*>(tmp);
			nameSize = min(8,strlen(unit));
			strncpy(buf+38,unit,nameSize);
			for (int i = 38+nameSize; i < 46; i++) buf[i] = ' ';
		}
		int dataSize;
		switch (line->dataType)
		{
			case DataTypeEnum::C: strncpy(buf+46,"C     ",6); dataSize = 1; break;
			case DataTypeEnum::F: strncpy(buf+46,"F     ",6); dataSize = 4;break;
			case DataTypeEnum::F4: strncpy(buf+46,"F4    ",6);dataSize = 4; break;
			case DataTypeEnum::F8: strncpy(buf+46,"F8    ",6); dataSize = 8;break;
			case DataTypeEnum::I: strncpy(buf+46,"I     ",6); dataSize = 4; break;
			case DataTypeEnum::I2: strncpy(buf+46,"I2    ",6); dataSize = 2; break;
			case DataTypeEnum::I4: strncpy(buf+46,"I4    ",6); dataSize = 4; break;
		}
		for (int i = 52; i < 90; i++) buf[i] = ' ';
		WriteASCIIIntInBuffer(buf+52,1); //If Faktor DOES happen to be important, write factor here
		WriteASCIIIntInBuffer(buf+52+16,line->Kennung);
		WriteASCIIIntInBuffer(buf+52+16+12,dataSize);
		//WriteASCIIIntInBuffer(buf+52+16+12+5,dim); //not known at the moment, write in later
		if (String::IsNullOrEmpty(line->Sollwert))
		{
			strncpy(buf+90,"NONE     ",9);
		}
		else
		{
			tmp = line->Sollwert;
			varName = context.marshal_as<const char*>(tmp);
			nameSize = min(strlen(varName),8);
			strncpy(buf+90,varName,nameSize);
			for (int i = 90 + nameSize; i < 99; i++)  buf[i] = ' ';
		}
		if (String::IsNullOrEmpty(line->Stutzstellen))
		{
			strncpy(buf+99,"NONE     ",9);
		}
		else
		{
			tmp = line->Stutzstellen;
			varName = context.marshal_as<const char*>(tmp);
			nameSize = min(strlen(varName),8);
			strncpy(buf+99,varName,nameSize);
			for (int i = 99 + nameSize; i < 109; i++)  buf[i] = ' ';
		}
		buf[108] = '\n';
		Write(buf,109);
		return offset;
	}

	inline unsigned int endian_swap(unsigned int& x)
	{
		return (x>>24) | 
			((x<<8) & 0x00FF0000) |
			((x>>8) & 0x0000FF00) |
			(x<<24);
	}

	inline unsigned short endian_swap(unsigned short& x)
	{
		return (x>>8) | 
			(x<<8);
	}

	inline unsigned __int64 endian_swap(unsigned __int64& x)
	{
		return (x>>56) | 
			((x<<40) & 0x00FF000000000000) |
			((x<<24) & 0x0000FF0000000000) |
			((x<<8)  & 0x000000FF00000000) |
			((x>>8)  & 0x00000000FF000000) |
			((x>>24) & 0x0000000000FF0000) |
			((x>>40) & 0x000000000000FF00) |
			(x<<56);
	}


	void RohWriter::WriteValueInBuffer(unsigned char* buffer,float value, DataTypeEnum dataType)
	{
		switch (dataType)
		{
			case DataTypeEnum::C: 
				*buffer = (char) value;
				break;
			case DataTypeEnum::F: 
			case DataTypeEnum::F4: 
				*((unsigned int*)buffer) = endian_swap(*((unsigned int*) &value)); break;
			case DataTypeEnum::F8: 
				*((unsigned __int64*)buffer) = endian_swap(*((unsigned __int64*) &value)); break;
			case DataTypeEnum::I: 
			case DataTypeEnum::I4: 
				*((unsigned int*)buffer) = endian_swap(*((unsigned int*) &value)); break;
			case DataTypeEnum::I2: 
				*((unsigned short*)buffer) = endian_swap(*((unsigned short*) &value)); break;
		}
	}

	void RohWriter::WriteChannelData(array<float>^ data, int size, DataTypeEnum dataType)
	{
		int dataSize;
		switch (dataType)
		{
			case DataTypeEnum::C: dataSize = 1; break;
			case DataTypeEnum::F: dataSize = 4;break;
			case DataTypeEnum::F4: dataSize = 4; break;
			case DataTypeEnum::F8: dataSize = 8;break;
			case DataTypeEnum::I: dataSize = 4; break;
			case DataTypeEnum::I2: dataSize = 2; break;
			case DataTypeEnum::I4: dataSize = 4; break;
		}
		int bufSize = dataSize*size;
		unsigned char* buffer = new unsigned char[bufSize]; 
		for (int i = 0; i < min(size,data->Length); i++)
			WriteValueInBuffer(buffer + i*size, data[i],dataType);
		for (int i = data->Length; i < size; i++)
			WriteValueInBuffer(buffer + i*size, data[data->Length-1],dataType);
		Write(buffer,size*dataSize);
		delete [] buffer;
	}
}

