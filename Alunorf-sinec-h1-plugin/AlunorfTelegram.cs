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
                case AckTelegram.TLGART: m_innerTelegram = new AckTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case NAKTelegram.TLGART: m_innerTelegram = new NAKTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case IniTelegram.TLGART: m_innerTelegram = new IniTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case LiveTelegram.TLGART: m_innerTelegram = new LiveTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case GoTelegram.TLGART: m_innerTelegram = new GoTelegram(AktId, m_Tseqnr, m_timeStamp); break;
                case QdtTelegram.TLGART: m_innerTelegram = new QdtTelegram(null, null, AktId, m_Tseqnr, m_timeStamp); break;
                default: m_innerTelegram = null; return false;
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
        public const byte TLGART = 1;
        public AckTelegram(UInt16 AktId, UInt16 Tseqnr)
        : base(AktId,Tseqnr)
        {
            m_TlgArt = TLGART;
        }

        public AckTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = TLGART;
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
        public const byte TLGART = 2;

        public NAKTelegram(UInt16 AktId, UInt16 Tseqnr)
        : base(AktId,Tseqnr)
        {
            m_TlgArt = TLGART;
            m_fehlerCode = 0;
            m_fehlerText = "";
        }

        public NAKTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = TLGART;
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
        public const byte TLGART = 10;

        public LiveTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = TLGART;
        }

        public LiveTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = TLGART;
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
        public const byte TLGART = 11;
        public IniTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = TLGART;
            m_iniPar = 0;
        }

        public IniTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = TLGART;
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
        public const byte TLGART = 14;
        public GoTelegram(UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_TlgArt = TLGART;
        }

        public GoTelegram(UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_TlgArt = TLGART;
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

    public class QdtTelegram : AlunorfTelegram, IDisposable
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
            errPos = -1;
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

        public const byte TLGART = 22;

        public QdtTelegram(PluginH1Task.Telegram data, IbaFileReader file, UInt16 AktId, UInt16 Tseqnr, TimeStamp stamp)
            : base(AktId, Tseqnr, stamp)
        {
            m_telegramData = data;
            m_TlgArt = TLGART;
            if (data != null) CalcSize();
            if (file != null && m_size >=0)
            {
                m_stream = new H1ByteStream((uint)m_size, true);
                PreWriteBody(file);
            }
        }

        public QdtTelegram(PluginH1Task.Telegram data, IbaFileReader file, UInt16 AktId, UInt16 Tseqnr)
            : base(AktId, Tseqnr)
        {
            m_telegramData = data;
            m_TlgArt = TLGART;
            if (data != null) CalcSize();
            if (file != null && m_size >= 0)
            {
                m_stream = new H1ByteStream((uint)m_size, true);
                PreWriteBody(file);
            }
        }

        H1ByteStream m_stream;

        public override bool ReadBody(H1ByteStream stream)
        {
            m_stream = stream;
            return true;
        }

        public string Interpret(List<PluginH1Task.Telegram> telegrams)
        { //to test the QDT telegram, reading has to implemented at well, by writing the values to a string
            StringBuilder b = new StringBuilder();
            
            SByte v_sbyte=0;
            Byte  v_byte=0;
            Int16 v_int16=0;
            Int32 v_int32=0;
            string v_string="";
            UInt16 v_uint16=0;
            UInt32 v_uint32=0;
            float v_float=0;

            m_stream.ReadInt16(ref v_int16);
            b.Append(v_int16);b.Append(Environment.NewLine);

            m_telegramData = telegrams.Find(delegate(PluginH1Task.Telegram tele) { return tele.QdtType == v_int16; });

            int count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataInfo)
            {
                count++;
                try
                {
                    int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    uint size = uint.Parse(rec.DataType.Substring(pos));
                    string type = rec.DataType.Substring(0, pos);
                    b.Append(rec.Name);
                    b.Append(": ");
                    if (type == "int" && size == 1)
                    {
                        m_stream.ReadSByte(ref v_sbyte);
                        b.Append(v_sbyte);b.Append(Environment.NewLine);
                    }
                    else if (type == "int" && size == 2)
                    {
                        m_stream.ReadInt16(ref v_int16);
                        b.Append(v_int16);b.Append(Environment.NewLine);
                    }
                    else if (type == "int" && size == 4)
                    {
                        m_stream.ReadInt32(ref v_int32);
                        b.Append(v_int32);b.Append(Environment.NewLine);
                    }
                    else if (type == "u. int" && size == 1)
                    {
                        m_stream.ReadByte(ref v_byte);
                        b.Append(v_byte);b.Append(Environment.NewLine);
                    }
                    else if (type == "u. int" && size == 2)
                    {
                        m_stream.ReadUInt16(ref v_uint16);
                        b.Append(v_uint16);b.Append(Environment.NewLine);
                    }
                    else if (type == "u. int" && size == 4)
                    {
                        m_stream.ReadUInt32(ref v_uint32);
                        b.Append(v_uint32);b.Append(Environment.NewLine);
                    }
                    else if (type == "float" && size == 4)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        b.Append(v_float);b.Append(Environment.NewLine);
                    }
                    else if (type == "char" && size > 0)
                    {
                        m_stream.ReadString(ref v_string, size);
                        b.Append(v_string);b.Append(Environment.NewLine);
                    }
                    else
                    {
                        errPos = count;
                        errInInfo = true;
                        return null;
                    }
                }
                catch
                {
                    errPos = count;
                    errInInfo = true;
                    return null;
                }
            }

            count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataSignal)
            {
                count++;
                try
                {
                    b.Append(rec.Name);
                    b.Append(": ");
                    for (int i = 0; i < 400; i++)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        b.Append(v_float);
                        b.Append(" ");
                    }
                    b.Append(Environment.NewLine);
                    b.Append("length: ");
                    for (int i = 0; i < 400; i++)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        b.Append(v_float);
                        b.Append(" ");
                    }
                    b.Append(Environment.NewLine);
                }
                catch
                {
                    errPos = count;
                    errInInfo = false;
                    return null;
                }
            }
            return b.ToString();
        }

        public void WriteToDatFile(List<PluginH1Task.Telegram> telegrams, string filename)
        { //to test the QDT telegram, reading has to implemented at well, by writing the values to a string
            IbaFileWriter2 filewriter = new IbaFileCreatorClass();
            filewriter.Create(filename,1);
            SByte v_sbyte = 0;
            Byte v_byte = 0;
            Int16 v_int16 = 0;
            Int32 v_int32 = 0;
            string v_string = "";
            UInt16 v_uint16 = 0;
            UInt32 v_uint32 = 0;
            float v_float = 0;
            m_stream.ReadInt16(ref v_int16);
            m_telegramData = telegrams.Find(delegate(PluginH1Task.Telegram tele) { return tele.QdtType == v_int16; });
            int count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataInfo)
            {
                count++;
                try
                {
                    int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    uint size = uint.Parse(rec.DataType.Substring(pos));
                    string type = rec.DataType.Substring(0, pos);
                    if (type == "int" && size == 1)
                    {
                        m_stream.ReadSByte(ref v_sbyte);
                        filewriter.WriteInfoField(rec.Name,v_sbyte.ToString());
                    }
                    else if (type == "int" && size == 2)
                    {
                        m_stream.ReadInt16(ref v_int16);
                        filewriter.WriteInfoField(rec.Name, v_int16.ToString());
                    }
                    else if (type == "int" && size == 4)
                    {
                        m_stream.ReadInt32(ref v_int32);
                        filewriter.WriteInfoField(rec.Name, v_int32.ToString());
                    }
                    else if (type == "u. int" && size == 1)
                    {
                        m_stream.ReadByte(ref v_byte);
                        filewriter.WriteInfoField(rec.Name, v_byte.ToString());
                    }
                    else if (type == "u. int" && size == 2)
                    {
                        m_stream.ReadUInt16(ref v_uint16);
                        filewriter.WriteInfoField(rec.Name, v_uint16.ToString());
                    }
                    else if (type == "u. int" && size == 4)
                    {
                        m_stream.ReadUInt32(ref v_uint32);
                        filewriter.WriteInfoField(rec.Name, v_uint32.ToString());
                    }
                    else if (type == "float" && size == 4)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        filewriter.WriteInfoField(rec.Name, v_float.ToString());
                    }
                    else if (type == "char" && size > 0)
                    {
                        m_stream.ReadString(ref v_string, size);
                        filewriter.WriteInfoField(rec.Name, v_string);
                    }
                    else
                    {
                        errPos = count;
                        errInInfo = true;
                        filewriter.Close();
                        return;
                    }
                }
                catch
                {
                    errPos = count;
                    errInInfo = true;
                    filewriter.Close();
                    return;
                }
            }

            count = 0;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataSignal)
            {
                count++;
                try
                {
                    float[] vals = new float[400];
                    for (int i = 0; i < 400; i++)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        vals[i] = v_float;
                    }
                    float offset=0;
                    float lengthbase=0;
                    for (int i = 0; i < 400; i++)
                    {
                        m_stream.ReadFloat32(ref v_float);
                        if (i == 0) offset = v_float;
                        if (i == 1) lengthbase = v_float - offset;
                    }
                    IbaChannelWriter channelwriter;
                    filewriter.CreateAnalogChannel(0, count, rec.Name, lengthbase, out channelwriter);
                    channelwriter.LengthBased = 1;
                    channelwriter.xOffset = offset;
                    for (int i = 0; i < 400; i++) channelwriter.WriteData(vals[i]);
                }
                catch
                {
                    errPos = count;
                    errInInfo = false;
                    break;
                }
            }
            filewriter.Close();
        }

        public override bool WriteBody(H1ByteStream stream)
        {
            return stream.WriteStream(m_stream);
        }

        private bool PreWriteBody(IbaFileReader file)
        {
            if (m_size < 0) return false; //problem detected in Calcsize;
            m_stream.WriteInt16(m_telegramData.QdtType);
            int count = 0;
            errPos = -1;
            foreach (PluginH1Task.TelegramRecord rec in m_telegramData.DataInfo)
            {
                count++;
                try
                {
                    System.Diagnostics.Trace.WriteLine("before" + count.ToString());
                    string data = file.QueryInfoByName(rec.Name);
                    int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    uint size = uint.Parse(rec.DataType.Substring(pos));
                    string type = rec.DataType.Substring(0, pos);
                    System.Diagnostics.Trace.WriteLine("after pos:" + pos.ToString() + " size:  " + size.ToString() + " type: " + type.ToString());

                    if (type == "int" && size == 1)
                    {
                        SByte val;
                        try
                        {
                            val = SByte.Parse(data);
                        }
                        catch
                        {
                            val = -1;
                        }
                        m_stream.WriteSByte(val);
                    }
                    else if (type == "int" && size == 2)
                    {
                        Int16 val;
                        try
                        {
                            val = Int16.Parse(data);
                        }
                        catch
                        {
                            val = -1;
                        }
                        m_stream.WriteInt16(val);
                    }
                    else if (type == "int" && size == 4)
                    {
                        Int32 val;
                        try
                        {
                            val = Int32.Parse(data);
                        }
                        catch
                        {
                            val = -1;
                        }
                        m_stream.WriteInt32(val);
                    }
                    else if (type == "u. int" && size == 1)
                    {
                        Byte val;
                        try
                        {
                            val = Byte.Parse(data);
                        }
                        catch
                        {
                            val = Byte.MaxValue;
                        }
                        m_stream.WriteByte(val);
                    }
                    else if (type == "u. int" && size == 2)
                    {
                        UInt16 val;
                        try
                        {
                            val = UInt16.Parse(data);
                        }
                        catch
                        {
                            val = UInt16.MaxValue;
                        }
                        m_stream.WriteUInt16(val);
                    }
                    else if (type == "u. int" && size == 4)
                    {
                        UInt32 val;
                        try
                        {
                            val = UInt32.Parse(data);
                        }
                        catch
                        {
                            val = UInt32.MaxValue;
                        }
                        m_stream.WriteUInt32(val);
                    }
                    else if (type == "float" && size == 4)
                    {
                        float val;
                        try
                        {
                            val = float.Parse(data);
                        }
                        catch
                        {
                            val = float.NaN;
                        }
                        m_stream.WriteFloat32(val);
                    }
                    else if (type == "char" && size > 0)
                        m_stream.WriteString(data, size);
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
                    file.QueryChannelByName(rec.Name, out reader);
                    float lengthBase, offset;
                    object data;
                    try
                    {
                        reader.QueryLengthbasedData(out lengthBase, out offset, out data);
                    }
                    catch (Exception)
                    {
                        reader.QueryTimebasedData(out lengthBase, out offset, out data);
                    }

                    float[] values = data as float[]; 
                    for (int i = 0; i < 400; i++)
                    {
                        if (i >= values.Length)
                            m_stream.WriteFloat32(float.NaN);
                        else
                            m_stream.WriteFloat32(values[i]);
                    }
                    for (int i = 0; i < 400; i++)
                        m_stream.WriteFloat32(offset + i * lengthBase);
                }
                catch
                {
                    errPos = count;
                    errInInfo = false;
                    return false;
                }
            }
            m_stream.Reset(); //reset for actual write;
            return true;
        }

        public override uint ActSize
        {
            get 
            { 
                return HEADERSIZE+(uint)m_size;
            }
        }

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (disposing) m_stream.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~QdtTelegram()
        {
            Dispose(false);
        }

        #endregion
    }
}
