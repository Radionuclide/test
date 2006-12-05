#pragma once

using namespace System;

namespace iba
{

	public ref class H1ByteStream
	{
	private: 
		unsigned long m_len;
		unsigned long m_pos;
		unsigned char* m_buffer;
		bool m_be;
		bool m_ownsbuffer;
	public:
		H1ByteStream(IntPtr pointer, unsigned long len, bool bigendian);
		H1ByteStream(unsigned long len, bool bigendian);
		~H1ByteStream();
		!H1ByteStream();
		bool ReadByte(unsigned char% val);
		bool WriteByte(unsigned char val);
		bool ReadSByte(signed char% val);
		bool WriteSByte(signed char val);
		bool ReadInt16(short% val);
		bool WriteInt16(short val);
		bool ReadUInt16(unsigned short% val);
		bool WriteUInt16(unsigned short val);
		bool ReadInt32(int% val);
		bool WriteInt32(int val);
		bool ReadUInt32(unsigned int% val);
		bool WriteUInt32(unsigned int val);
		bool ReadFloat32(float% val);
		bool WriteFloat32(float val);
		bool ReadString(String^% mystring, unsigned int len);
		bool WriteString(String^ mystring, unsigned int len);
		bool WriteStream(H1ByteStream^ stream);
		void Reset();
		array<unsigned char>^ GetBuffer();
	};
}