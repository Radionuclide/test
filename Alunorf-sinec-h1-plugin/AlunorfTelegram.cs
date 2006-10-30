using System;
using System.Collections.Generic;
using System.Text;
using iba;

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
        public QdtTelegram(UInt16 AktId, UInt16 Tseqnr)
        : base(AktId,Tseqnr)
        {
            m_TlgArt = 22;
        }

        public QdtTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = 22;
        }
        
        public override bool ReadBody(H1ByteStream stream)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public override uint ActSize
        {
            get 
            { 
                //throw new Exception("The method or operation is not implemented."
                return HEADERSIZE;
            }
        }
    }
}
