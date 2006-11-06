#include "StdAfx.h"
#include "ByteStream.h"

using namespace System::Runtime::InteropServices;

namespace iba
{
	H1ByteStream::H1ByteStream(IntPtr ptr, unsigned long len, bool bigendian)
	{
		m_len = len;
		m_pos = 0;
		m_be = bigendian;
		m_buffer = (unsigned char*) ptr.ToPointer();
	}

	bool H1ByteStream::ReadByte(unsigned char% val)
	{
		if (m_pos + sizeof(unsigned char) <= m_len )
		{
			val = *((unsigned char*)(m_buffer+m_pos));
			m_pos += sizeof(unsigned char);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteByte(unsigned char val)
	{
		if (m_pos + sizeof(unsigned char) <= m_len )
		{
			*((unsigned char*)(m_buffer+m_pos)) = val;
			m_pos += sizeof(unsigned char);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadSByte(signed char% val)
	{
		if (m_pos + sizeof(signed char) <= m_len )
		{
			val = *((signed char*)(m_buffer+m_pos));
			m_pos += sizeof(signed char);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteSByte(signed char val)
	{
		if (m_pos + sizeof(signed char) <= m_len )
		{
			*((signed char*)(m_buffer+m_pos)) = val;
			m_pos += sizeof(signed char);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadInt16(short% val)
	{
		if (m_pos + sizeof(short) <= m_len )
		{
			val = *((short*)(m_buffer+m_pos));
			m_pos += sizeof(short);
			if (m_be) val = ntohs(val);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteInt16(short val)
	{
		if (m_pos + sizeof(short) <= m_len )
		{
			if (m_be) val = htons(val);
			*((short*)(m_buffer+m_pos)) = val;
			m_pos += sizeof(short);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadUInt16(unsigned short% val)
	{
		if (m_pos + sizeof(unsigned short) <= m_len )
		{
			val = *((unsigned short*)(m_buffer+m_pos));
			m_pos += sizeof(unsigned short);
			if (m_be) val = ntohs(val);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteUInt16(unsigned short val)
	{
		if (m_pos + sizeof(unsigned short) <= m_len )
		{
			if (m_be) val = htons(val);
			*((unsigned short*)(m_buffer+m_pos)) = val;
			m_pos += sizeof(unsigned short);
			return true;
		}
		return false;
	}


	//bool H1ByteStream::ReadInt32(int% val)
	//{
	//	if (m_pos + sizeof(int) <= m_len )
	//	{
	//		val = *(reinterpret_cast<int*>(m_buffer+m_pos));
	//		m_pos += sizeof(int);
	//		if (m_be) val = ntohl(val);
	//		return true;
	//	}
	//	return false;
	//}

	//bool H1ByteStream::WriteInt32(int val)
	//{
	//	if (m_pos + sizeof(int) <= m_len )
	//	{
	//		if (m_be) val = htonl(val);
	//		*((int*)(m_buffer+m_pos)) = val;
	//		m_pos += sizeof(int);
	//		return true;
	//	}
	//	return false;
	//}

	//bool H1ByteStream::ReadUInt32(unsigned int% val)
	//{
	//	if (m_pos + sizeof(unsigned int) <= m_len )
	//	{
	//		val = *((unsigned int*)(m_buffer+m_pos));
	//		m_pos += sizeof(unsigned int);
	//		if (m_be) val = ntohl(val);
	//		return true;
	//	}
	//	return false;
	//}

	//bool H1ByteStream::WriteUInt32(unsigned int val)
	//{
	//	if (m_pos + sizeof(unsigned int) <= m_len )
	//	{
	//		if (m_be) val = htonl(val);
	//		*((unsigned int*)(m_buffer+m_pos)) = val;
	//		m_pos += sizeof(unsigned int);
	//		return true;
	//	}
	//	return false;
	//}


#pragma unmanaged

	template<class T> void Read32BitVal(T* val, unsigned char* buffer, bool bigendian)
	{
			unsigned int temp = *(reinterpret_cast<unsigned int*>(buffer));
			if (bigendian) temp = ntohl(temp);
			*val = * reinterpret_cast<T*>(&temp);
	}

	template<class T> void Write32BitVal(T val, unsigned char* buffer, bool bigendian)
	{
			unsigned int temp = *(reinterpret_cast<unsigned int*>(&val));
			if (bigendian) temp = htonl(temp);
			*(reinterpret_cast<unsigned int*>(buffer)) = temp;
	}

#pragma managed

	bool H1ByteStream::ReadInt32(int% val)
	{
		if (m_pos + sizeof(int) <= m_len )
		{
			pin_ptr<int> v = &val;
			int* va = v;
			Read32BitVal<int>(va, m_buffer+m_pos,m_be);
			m_pos += sizeof(int);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteInt32(int val)
	{
		if (m_pos + sizeof(int) <= m_len )
		{
			Write32BitVal<int>(val, m_buffer+m_pos,m_be);	
			m_pos += sizeof(int);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadUInt32(unsigned int% val)
	{
		if (m_pos + sizeof(unsigned int) <= m_len )
		{
			pin_ptr<unsigned int> v = &val;
			unsigned int* va = v;
			Read32BitVal<unsigned int>(va, m_buffer+m_pos,m_be);
			m_pos += sizeof(unsigned int);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteUInt32(unsigned int val)
	{
		if (m_pos + sizeof(unsigned int) <= m_len )
		{
			Write32BitVal<unsigned int>(val, m_buffer+m_pos,m_be);	
			m_pos += sizeof(unsigned int);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadFloat32(float% val)
	{
		if (m_pos + sizeof(float) <= m_len )
		{
			pin_ptr<float> v = &val;
			float* va = v;
			Read32BitVal<float>(va, m_buffer+m_pos,m_be);
			m_pos += sizeof(float);
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteFloat32(float val)
	{
		if (m_pos + sizeof(float) <= m_len )
		{
			Write32BitVal<float>(val, m_buffer+m_pos,m_be);	
			m_pos += sizeof(float);
			return true;
		}
		return false;
	}

	bool H1ByteStream::ReadString(String^% mystring, unsigned int len)
	{
		if (m_pos + len <= m_len)
		{
			mystring = Marshal::PtrToStringAnsi(IntPtr((void*)(m_buffer+m_pos)), len);
			m_pos += len;
			return true;
		}
		return false;
	}

	bool H1ByteStream::WriteString(String^ mystring, unsigned int len)
	{
		if (m_pos + len <= m_len)
		{
			IntPtr str_ip = Marshal::StringToHGlobalAnsi(mystring);
			unsigned int actlen = min(len, (unsigned int) mystring->Length);
			memcpy(m_buffer + m_pos,str_ip.ToPointer(),actlen);
			Marshal::FreeHGlobal(str_ip);
			if (actlen < len) memset(m_buffer + m_pos + actlen,0,len-actlen);
			m_pos += len;
			return true;
		}
		return false;
	}

	array<unsigned char>^ H1ByteStream::GetBuffer()
	{
		array<unsigned char>^ ans = gcnew array<unsigned char>(m_len);
		for (unsigned int i = 0; i < m_len;i++)
			ans[i] = *(m_buffer+i);
		return ans;
	}
}