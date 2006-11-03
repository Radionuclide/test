using System;
using System.Collections.Generic;
using System.Text;
using iba;
using IBAFILESLib;

namespace Alunorf_sinec_h1_plugin 
{
    abstract public class AlunorfTelegram : ITelegram
    {
        //header data
        protected byte m_TlgArt;
        protected UInt16 m_AktId;

        public UInt16 AktId
        {
            get { return m_AktId; }
            set { m_AktId = value; }
        }


        protected UInt16 m_Tseqnr;

        public const uint HEADERSIZE = 12;

        public struct TimeStamp 
        {
            public int hour;
            public int minute;
            public int second;
            public int centisecond; 
        }

        public AlunorfTelegram() { }

        public AlunorfTelegram(UInt16 AktId, UInt16 Tseqnr)
        {
            m_AktId = AktId;
            m_Tseqnr = Tseqnr;
            DateTime now = DateTime.Now;
            m_timeStamp.hour = now.Hour;
            m_timeStamp.minute = now.Minute;
            m_timeStamp.second = now.Second;
            m_timeStamp.centisecond = now.Millisecond / 10;
        }

        public AlunorfTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp timestamp)
        {
            m_AktId = AktId;
            m_Tseqnr = Tseqnr;
            DateTime now = DateTime.Now;
            m_timeStamp = timestamp;
        }

        protected TimeStamp m_timeStamp;

        #region ITelegram Members

    
        public bool ReadFromBuffer(H1ByteStream stream)
        {
            if (ReadHeader(stream))
                return ReadBody(stream);
            return false;
        }

        public bool WriteToBuffer(H1ByteStream stream)
        {
            if (WriteHeader(stream))
                return WriteBody(stream);
            return false;
        }

        public bool WriteHeader(H1ByteStream stream)
        {
            bool ok = stream.WriteByte(97);
            if (ok) ok = stream.WriteByte(4); //this is a guess, I don't know protocolversion
            if (ok) ok = stream.WriteByte(m_TlgArt);
            if (ok) ok = stream.WriteByte(0); // filler
            if (ok) ok = stream.WriteUInt16(m_AktId);
            if (ok) ok = stream.WriteUInt16(m_Tseqnr);
            if (ok) ok = stream.WriteByte((Byte)((m_timeStamp.hour / 10) * 16 + (m_timeStamp.hour % 10)));
            if (ok) ok = stream.WriteByte((Byte)((m_timeStamp.minute / 10) * 16 + (m_timeStamp.minute % 10)));
            if (ok) ok = stream.WriteByte((Byte)((m_timeStamp.second / 10) * 16 + (m_timeStamp.second % 10)));
            if (ok) ok = stream.WriteByte((Byte)((m_timeStamp.centisecond / 10) * 16 + (m_timeStamp.centisecond % 10)));
            return ok;
        }

        abstract public bool WriteBody(H1ByteStream stream);

        public bool ReadHeader(H1ByteStream stream)
        {
            Byte signature = 0;
            bool ok = stream.ReadByte(ref signature);
            if (!ok || signature != 97)
                return false; //not an alunorf telegram
            Byte protVers = 0;
            ok = stream.ReadByte(ref protVers);
            if (ok) ok = stream.ReadByte(ref m_TlgArt);
            Byte filler = 0;
            if (ok) ok = stream.ReadByte(ref filler);
            if (ok) ok = stream.ReadUInt16(ref m_AktId);
            if (ok) ok = stream.ReadUInt16(ref m_Tseqnr);
            Byte hour=0, minute=0, second=0, centisecond=0;
            if (ok) ok = stream.ReadByte(ref hour);
            if (ok) ok = stream.ReadByte(ref minute);
            if (ok) ok = stream.ReadByte(ref second);
            if (ok) ok = stream.ReadByte(ref centisecond);
            m_timeStamp.hour = (hour / 16) * 10 + (hour % 16);
            m_timeStamp.minute = (minute / 16) * 10 + (minute % 16);
            m_timeStamp.second = (second / 16) * 10 + (second % 16);
            m_timeStamp.centisecond = (centisecond / 16) * 10 + (centisecond % 16);
            return ok;
        }

        abstract public bool ReadBody(H1ByteStream stream);

        abstract public uint ActSize
        {
            get; 
        }


        #endregion

    }

    public class WrapperTelegram : AlunorfTelegram
    {
        AlunorfTelegram m_innerTelegram;
        public AlunorfTelegram InnerTelegram
        {
            get { return m_innerTelegram; }
        }

        public override bool ReadBody(H1ByteStream stream)
        {
            switch (m_TlgArt)
            {
                case 1: m_innerTelegram = new AckTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case 2: m_innerTelegram = new NAKTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case 11: m_innerTelegram = new IniTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case 12: m_innerTelegram = new LiveTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case 14: m_innerTelegram = new GoTelegram(AktId, m_Tseqnr, m_timeStamp); break;
            }

            return m_innerTelegram.ReadBody(stream);
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return m_innerTelegram.WriteBody(stream);
        }

        public override uint ActSize
        {
            get { return m_innerTelegram.ActSize; }
        }

    }


    public class AckTelegram : AlunorfTelegram
    {
        public AckTelegram(UInt16 AktId, UInt16 Tseqnr)
        : base(AktId,Tseqnr)
        {
            m_TlgArt = 1;
        }

        public AckTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 1;
        }

        public override bool ReadBody(H1ByteStream stream)
        {
            return true;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return true;
        }

        public override uint ActSize
        {
            get { return HEADERSIZE; }
        }
    }

    public class NAKTelegram : AlunorfTelegram
    {
        public NAKTelegram(UInt16 AktId, UInt16 Tseqnr)
        : base(AktId,Tseqnr)
        {
            m_TlgArt = 2;
            m_fehlerCode = 0;
            m_fehlerText = "";
        }

        public NAKTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 2;
            m_fehlerCode = 0;
            m_fehlerText = "";
        }

        private Int16 m_fehlerCode;
        public Int16 FehlerCode
        {
            get { return m_fehlerCode; }
            set { m_fehlerCode = value; }
        }

        private string m_fehlerText;

        public string FehlerText
        {
            get { return m_fehlerText; }
            set { m_fehlerText = value; }
        }


        public override bool ReadBody(H1ByteStream stream)
        {
            bool ok = stream.ReadInt16(ref m_fehlerCode);
            if (ok) ok = stream.ReadString(ref m_fehlerText, 60);
            return ok;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            bool ok = stream.WriteInt16(m_fehlerCode);
            if (ok) ok = stream.WriteString(m_fehlerText, 60);
            return ok;
        }

        public override uint ActSize
        {
            get { return HEADERSIZE + 60 + 2; }
        }
    }

    public class LiveTelegram : AlunorfTelegram
    {
        public LiveTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = 10;
        }

        public LiveTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 10;
        }

        public override bool ReadBody(H1ByteStream stream)
        {
            return true;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return true;
        }

        public override uint ActSize
        {
            get { return HEADERSIZE; }
        }
    }

    public class IniTelegram : AlunorfTelegram
    {
        public IniTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = 11;
            m_iniPar = 0;
        }

        public IniTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 11;
            m_iniPar = 0;
        }

        private Int16 m_iniPar;

        public Int16 IniPar
        {
            get { return m_iniPar; }
            set { m_iniPar = value; }
        }

        public override bool ReadBody(H1ByteStream stream)
        {
            return stream.ReadInt16(ref m_iniPar);
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return stream.WriteInt16(m_iniPar);
        }

        public override uint ActSize
        {
            get { return HEADERSIZE+2; }
        }
    }

    public class GoTelegram : AlunorfTelegram
    {
        public GoTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = 14;
        }

        public GoTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 14;
        }    

        public override bool ReadBody(H1ByteStream stream)
        {
            return true;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return true;
        }

        public override uint ActSize
        {
            get { return HEADERSIZE; }
        }
    }

    public class QdtTelegram : AlunorfTelegram
    {
        private int m_size;

        private int errPos;
        public int ErrPos
        {
            get { return errPos; }
            set { errPos = value; }
        }
        
        private bool errInInfo;
        public bool ErrInInfo
        {
            get { return errInInfo; }
            set { errInInfo = value; }
        }

        private void CalcSize()
        {
            m_size = 2; //qdt field
            int count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataInfo)
            {
                count++;
                if (String.IsNullOrEmpty(rec.DataType))
                {
                    m_size = -1;
                    errPos = count;
                    errInInfo = true;
                    return;
                }

                int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                m_size += int.Parse(rec.DataType.Substring(pos));
            }
            count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataSignal)
            {
                count++;
                if (String.IsNullOrEmpty(rec.DataType) || rec.DataType != "float4")
                {
                    m_size = -1;
                    errPos = count;
                    errInInfo = false;
                }
                m_size += 3200; //2*4*400
            }
        }

        PluginH1Task.Telegram m_telegramData;
        private IbaFileReader m_file;

        public QdtTelegram(PluginH1Task.Telegram data, IbaFileReader file, UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_telegramData = data;
            m_file = file;
            m_TlgArt = 22;
            CalcSize();
        }

        public QdtTelegram(PluginH1Task.Telegram data, IbaFileReader file, UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_file = file;
            m_telegramData = data;
            m_TlgArt = 22;
            CalcSize();
        }

        public override bool ReadBody(H1ByteStream stream)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            if (m_size < 0) return false; //problem detected in Calcsize;
            stream.WriteInt16(m_telegramData.QdtType);
            int count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataInfo)
            {
                count++;
                try
                {
                    string data = m_file.QueryInfoByName(rec.Name);
                    int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    uint size = uint.Parse(rec.DataType.Substring(pos));
                    string type = rec.DataType.Substring(0, pos + 1);

                    if (type == "int" && size == 1)
                        stream.WriteSByte(SByte.Parse(data));
                    else if (type == "int" && size == 2)
                        stream.WriteInt16(Int16.Parse(data));
                    else if (type == "int" && size == 4)
                        stream.WriteInt32(Int32.Parse(data));
                    else if (type == "u. int" && size == 1)
                        stream.WriteByte(Byte.Parse(data));
                    else if (type == "u. int" && size == 2)
                        stream.WriteUInt16(UInt16.Parse(data));
                    else if (type == "u. int" && size == 4)
                        stream.WriteUInt32(UInt32.Parse(data));
                    else if (type == "float" && size == 4)
                        stream.WriteFloat32(float.Parse(data));
                    else if (type == "char" && size > 0)
                        stream.WriteString(data, size);
                    else
                    {
                        errPos = count;
                        errInInfo = true;
                        return false;
                    }
                }
                catch
                {
                    errPos = count;
                    errInInfo = true;
                    return false;
                }
            }
            count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataSignal)
            {
                count++;
                try
                {
                    IbaChannelReader reader; 
                    m_file.QueryChannelByName(rec.Name, out reader);
                    float lengthBase, offset;
                    object data;
                    reader.QueryLengthbasedData(out lengthBase, out offset, out data);
                    float[] values = data as float[]; 
                    for (int i = 0; i < 400; i++)
                    {
                        if (i >= values.Length)
                            stream.WriteFloat32(float.NaN);
                        else
                            stream.WriteFloat32(values[i]);
                    }
                    for (int i = 0; i < 400; i++)
                        stream.WriteFloat32(offset + i * lengthBase);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(reader);
                }
                catch
                {
                    errPos = count;
                    errInInfo = false;
                    return false;
                }
            }
            return true;
        }

        public override uint ActSize
        {
            get 
            { 
                return HEADERSIZE+(uint)m_size;
            }
        }
    }
}
