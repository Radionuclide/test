// RohWriter.h

#pragma once

using namespace System;

namespace iba {

	public enum class DataTypeEnum {F,F4,F8,I,I2,I4,C};

	[Serializable]
	public ref class RohWriterDataLineInput
	{
		public:
			String^ ibaName;
			String^ Bezeichnung;
			String^ KurzBezeichnung;
			String^ Einheit;
			DataTypeEnum dataType;
	};

	[Serializable]
	public ref class RohWriterChannelLineInput
	{
		public:
			String^ ibaName;
			String^ Bezeichnung;
			String^ KurzBezeichnung;
			String^ Einheit;
			DataTypeEnum dataType;
			int Kennung;
			String^ Sollwert;
			String^ Stutzstellen;
	};

	[Serializable]
	public ref class RohWriterInput
	{
	public:
		array<RohWriterDataLineInput^>^ StichDaten;
		array<RohWriterDataLineInput^>^ KopfDaten;
		array<RohWriterDataLineInput^>^ SchlussDaten;
		array<RohWriterChannelLineInput^>^ Kanalen;
		String^ Kommentare;
		String^ Kurzbezeichner;
		String^ Parameter;
	};

	public ref class RohWriter
	{
		private:
			HANDLE hFile;
			DWORD Write (const void *p, int size) {
				DWORD dwWrite;
				if (!::WriteFile(hFile, p, size, &dwWrite, 0))
					throw HRESULT_FROM_WIN32(GetLastError());
				return dwWrite;
			}

			DWORD WriteString (const char* str) {
				DWORD dwWrite;
				if (!::WriteFile(hFile, str, strlen(str), &dwWrite, 0))
					throw HRESULT_FROM_WIN32(GetLastError());

				return dwWrite;
			}

			DWORD Seek (DWORD pos) {
				DWORD dw = ::SetFilePointer(hFile, pos, 0, FILE_BEGIN);
				if (dw == (DWORD)-1)
					throw HRESULT_FROM_WIN32(GetLastError());
				return dw;
			}
			DWORD Seek (DWORD pos, DWORD dwMoveMethod) {
				DWORD dw = ::SetFilePointer(hFile, pos, 0, dwMoveMethod);
				if (dw == (DWORD)-1)
					throw HRESULT_FROM_WIN32(GetLastError());
				return dw;
			}
			DWORD GetPosition() {return Seek(0, FILE_CURRENT);}; 
			void SetErrorMessageFromHResult(HRESULT hr);
			void WriteASCIIIntInBuffer(char* buf, int val); //write ASCII int in buffer, no terminating zero
			void WriteASCIIUIntInBuffer(char* buf, unsigned int val); //write ASCII unsigned int in buffer, no terminating zero
			void WriteASCIIUIntInFile(unsigned int val);
			void WriteDataLine(RohWriterDataLineInput^ line, String^ value, msclr::interop::marshal_context% context);
			void WriteTextBlock(String^ block,msclr::interop::marshal_context% context);
			DWORD WritePartialHeaderLine(const char* headerName, int nrElements);
			void CompleteHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD BlockSize);
			DWORD WritePartialChannelHeaderLine(const char* shortName, int nrElements);
			void CompleteChannelHeaderLine(DWORD offsetHeaderLine, DWORD offsetToData, DWORD blockSize);
			DWORD WriteChannelKanalBeschreibungLine(RohWriterChannelLineInput^ line, msclr::interop::marshal_context% context);
			void WriteChannelData(array<float>^ data, int size, DataTypeEnum dataType);
			void WriteValueInBuffer(unsigned char* buffer,float value,DataTypeEnum dataType);
		public:
			int Write(RohWriterInput^ input, String^ datfile, String^ Rohfile);
			// return value:
			// 0: everything OK
			// 1: missing Kopf in .dat file -> See ErrorDataLineInput
			// 2: missing Stich in .dat file -> See ErrorDataLineInput
			// 3: missing Shluss in .dat file -> See ErrorDataLineInput
			// 4: missing channel in .dat file -> ErrorChannelLineInput
			// 5: problem reading .dat file -> see errorMessage
			// 6: ibaFiles Create problem -> see errorMessage
			// 7: ibaFiles Open problem -> see errorMessage
			// 8: problem writing .roh file -> see errorMessage
			// 9: unexpected error -> see errorMessage
			RohWriterDataLineInput^ errorDataLineInput;
			RohWriterChannelLineInput^ errorChannelLineInput;
			String^ errorMessage;
	};
};
