#pragma once

namespace S7_writer
{
	[System::Reflection::Obfuscation]
	public enum class S7CPUType 
	{
		None        = 0x0,
		S7_200		= 0x1,
		S7_300_400	= 0x2,
		Logo		= 0x4,
		S5          = 0x8,
		S7_TIA      = 0x10,  //symbolic access to S7-1200 and S7-1500
		SINAMICS	= 0x20,	// SINAMICS drive
		SIMOTION	= 0x40
	};

	[System::Reflection::Obfuscation]
	public enum class S7ConnMode
	{
		TCP = 0,
		PCCP = 1,
		TCP_TIA = 2
	};

	[System::Reflection::Obfuscation]
	public enum class S7ProjectType 
	{
		All         = 0x3,
		Step7       = 0x1,
		Tia		    = 0x2,
	};

	[System::Reflection::Obfuscation]
	public enum class S7DataTypeEnum 
	{
		S7Bool,
		S7Byte,
		S7Word,
		S7DWord,
		S7Int,
		S7DInt,
		S7Real,
		S7Char,         // Handled same as S7Byte
		S7Timer,		// Internally converted to REAL
		S7Counter,		// Internally converted to WORD
		S7Time,
		S7SInt,
		S7USInt,
		S7UInt,
		S7UDInt,
		S7LReal,
		S5Real
	};

    public ref class S7
    {
    public:

        ref class S7Type
        {
        public:
            int      type;
            String^  name;
            int      size;
			int      sortOrder;
            double   minScale;
            double   maxScale;

            S7Type(int t, String^ n, int s, int sort, double min, double max)
            {
                type = t;
                name = n;
                size = s;
				sortOrder = sort;
                minScale = min;
                maxScale = max;
            }

            virtual String^ ToString() override {return name;}

			static int Compare(int t1, int t2)
			{
				S7Type^ type1 = (t1 >= 0) && (t1 < DataTypes->Length) ? DataTypes[t1] : nullptr;
				S7Type^ type2 = (t2 >= 0) && (t2 < DataTypes->Length) ? DataTypes[t2] : nullptr;
				if(type1 == type2)
					return 0;
				else if(type1 == nullptr)
					return 1;
				else if(type2 == nullptr)
					return -1;
				else
					return type1->sortOrder - type2->sortOrder;
			}
        };

        static array<S7Type^>^ DataTypes = {
			gcnew S7Type((int) S7DataTypeEnum::S7Bool,		"BOOL",		0,   0,   0, 1),
			gcnew S7Type((int) S7DataTypeEnum::S7Byte,		"BYTE",		1,   2,   0, 0xFF),
			gcnew S7Type((int) S7DataTypeEnum::S7Word,		"WORD",		2,   3,   0, 0xFFFF),
			gcnew S7Type((int) S7DataTypeEnum::S7DWord,		"DWORD",	4,   4,   0, 4294967295.0),
			gcnew S7Type((int) S7DataTypeEnum::S7Int,		"INT",		2,   6,   -32768.0, 32767.0),
			gcnew S7Type((int) S7DataTypeEnum::S7DInt,		"DINT",		4,   7,   -2147483648.0, 2147483647.0),
			gcnew S7Type((int) S7DataTypeEnum::S7Real,		"REAL",		4,  11,   -1e35, 1e35),
			gcnew S7Type((int) S7DataTypeEnum::S7Char,		"CHAR",		1,   1,   0, 0xFF),
			gcnew S7Type((int) S7DataTypeEnum::S7Timer,		"TIMER",	4,  14,   -1e35, 1e35),
			gcnew S7Type((int) S7DataTypeEnum::S7Counter,	"COUNTER",	2,  15,   0, 0xFFFF),
			gcnew S7Type((int) S7DataTypeEnum::S7Time,		"TIME",		4,  13,   -2147483648.0, 2147483647.0),		
			gcnew S7Type((int) S7DataTypeEnum::S7SInt,		"SINT",		1,   5,   -128, 127),
			gcnew S7Type((int) S7DataTypeEnum::S7USInt,		"USINT",	1,   8,   0, 255),
			gcnew S7Type((int) S7DataTypeEnum::S7UInt,		"UINT",		2,   9,   0, 65535),
			gcnew S7Type((int) S7DataTypeEnum::S7UDInt,		"UDINT",	4,  10,   0, UInt32::MaxValue),
			gcnew S7Type((int) S7DataTypeEnum::S7LReal,		"LREAL",	8,  12,   -1e35, 1e35),
			gcnew S7Type((int) S7DataTypeEnum::S5Real,		"S5 REAL",	4,  16,   -1e35, 1e35),
        };

        ref class S7OperandType
        {
        public:
            int     index;
	        String^ name;
            String^ englishName;
	        int     size;      	// -1 child defines size, -2 no operand, 0 = bit  other 1.2.4 bytes
            int     varType;    //	80H - Peripherie Eingang
								//	81H - Eingang
								//	82H - Ausgang
								//	83H - Merker
								//	84H - Daten

	        String^ parent;     // only for DB elements
            int     defaultDataType;
			int		agLinkType;	//  0  - Eingang	(AREA_IN)
								//  1  - Ausgang	(AREA_OUT)
								//  2  - Merker		(AREA_FLAG)
								//  3  - Daten		(AREA_DATA)
								// 10  - Peripherie (AREA_PERIPHERIE)

			S7CPUType	cpuType;

			property String^ LocalizedName {String^ get();}

            S7OperandType(int idx, String^ n, String^ e, int s, int var, String^ p, int def, S7CPUType s7types)
            {
                index = idx;
                name = n;
                englishName = e;
                size = s;
                varType = var;
                parent = p;
                defaultDataType = def;
				cpuType = s7types;

				agLinkType = varType - 0x81;
				if(agLinkType < 0)
					agLinkType = 10;
            }

            virtual String^ ToString() override {return LocalizedName;}

			S7OperandType^ GetParent()
			{
				if(parent == nullptr)
					return nullptr;

				for each(S7OperandType^ opType in S7::OperandTypes)
				{
					if((opType->name == parent) && (opType->cpuType == cpuType))
						return opType;
				}

				return nullptr;
			}
        };

		// Note that timers and counters behave differently on S7-200 and S7-300/400
		// In the case of S7-300/400 they are BCDs
		// For timers in specific, we need to multiply the 2-byte BCD value with a timebase factor ==> convert to floating point in units of second
        static array<S7OperandType^>^ OperandTypes = {
			gcnew S7OperandType( 0, "E"   , "I"   , 0   , 0x81 , nullptr , (int) S7DataTypeEnum::S7Bool  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 1, "A"   , "Q"   , 0	, 0x82 , nullptr , (int) S7DataTypeEnum::S7Bool  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 2, "M"   , "M"   , 0	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Bool  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						                  ),
			gcnew S7OperandType( 3, "EB"  , "IB"  , 1	, 0x81 , nullptr , (int) S7DataTypeEnum::S7Byte  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 4, "AB"  , "QB"  , 1	, 0x82 , nullptr , (int) S7DataTypeEnum::S7Byte  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 5, "MB"  , "MB"  , 1	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Byte  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						                  ),
			gcnew S7OperandType( 6, "EW"  , "IW"  , 2	, 0x81 , nullptr , (int) S7DataTypeEnum::S7Word  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 7, "AW"  , "QW"  , 2	, 0x82 , nullptr , (int) S7DataTypeEnum::S7Word  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType( 8, "MW"  , "MW"  , 2	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Word  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						                  ),
			gcnew S7OperandType( 9, "ED"  , "ID"  , 4	, 0x81 , nullptr , (int) S7DataTypeEnum::S7Real  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType(10, "AD"  , "QD"  , 4	, 0x82 , nullptr , (int) S7DataTypeEnum::S7Real  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType(11, "MD"  , "MD"  , 4	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Real  , S7CPUType::S7_200	|	S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						                  ),
			gcnew S7OperandType(12, "PEB" , "PIB" , 1	, 0x80 , nullptr , (int) S7DataTypeEnum::S7Byte  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType(13, "PEW" , "PIW" , 2	, 0x80 , nullptr , (int) S7DataTypeEnum::S7Word  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						 |  S7CPUType::S5 ),
			gcnew S7OperandType(14, "PED" , "PID" , 4	, 0x80 , nullptr , (int) S7DataTypeEnum::S7Real  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  						                  ),
            gcnew S7OperandType(15, "DB"  , "DB"  ,-1	, 0x84 , nullptr , (int) S7DataTypeEnum::S7Byte  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  	|	S7CPUType::Logo	                  ),
			gcnew S7OperandType(16, "DBX" , "DBX" , 0	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Bool  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  	|	S7CPUType::Logo	                  ),
            gcnew S7OperandType(17, "DBB" , "DBB" , 1	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Byte  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  	|	S7CPUType::Logo	                  ),
            gcnew S7OperandType(18, "DBW" , "DBW" , 2	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Word  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  	|	S7CPUType::Logo	                  ),
            gcnew S7OperandType(19, "DBD" , "DBD" , 4	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Real  ,							S7CPUType::S7_300_400   |  S7CPUType::S7_TIA  	|	S7CPUType::Logo	                  ),
			gcnew S7OperandType(20, "SM"  , "SM"  , 0	, 0x87 , nullptr , (int) S7DataTypeEnum::S7Bool  , S7CPUType::S7_200							  	   					                  ),
			gcnew S7OperandType(21, "V"   , "V"   , 0	, 0x88 , nullptr , (int) S7DataTypeEnum::S7Bool  , S7CPUType::S7_200							  	                        |	S7CPUType::Logo                   ),
			gcnew S7OperandType(22, "SMB" , "SMB" , 1	, 0x87 , nullptr , (int) S7DataTypeEnum::S7Byte  , S7CPUType::S7_200							  	                        					                  ),
			gcnew S7OperandType(23, "VB"  , "VB"  , 1	, 0x88 , nullptr , (int) S7DataTypeEnum::S7Byte  , S7CPUType::S7_200							  	                        |	S7CPUType::Logo                   ),
			gcnew S7OperandType(24, "SMW" , "SMW" , 2	, 0x87 , nullptr , (int) S7DataTypeEnum::S7Word  , S7CPUType::S7_200							  	                        					                  ),
			gcnew S7OperandType(25, "VW"  , "VW"  , 2	, 0x88 , nullptr , (int) S7DataTypeEnum::S7Word  , S7CPUType::S7_200							  	                        |	S7CPUType::Logo                   ),
			gcnew S7OperandType(26, "SMD" , "SMD" , 4	, 0x87 , nullptr , (int) S7DataTypeEnum::S7Real  , S7CPUType::S7_200							  	                        					                  ),
			gcnew S7OperandType(27, "VD"  , "VD"  , 4	, 0x88 , nullptr , (int) S7DataTypeEnum::S7Real  , S7CPUType::S7_200							  	                        |	S7CPUType::Logo                   ),
			gcnew S7OperandType(28, "T"   , "T"   , 2   , 0x89 , nullptr , (int) S7DataTypeEnum::S7Int   , S7CPUType::S7_200							  	                        				                      ),
			gcnew S7OperandType(29, "Z"   , "C"   , 2   , 0x8A , nullptr , (int) S7DataTypeEnum::S7Int   , S7CPUType::S7_200							  	                        					                  ),
			gcnew S7OperandType(30, "T"   , "T"   , 4   , 0x85 , nullptr , (int) S7DataTypeEnum::S7Timer , 							S7CPUType::S7_300_400                       	                                      ),
			gcnew S7OperandType(31, "Z"   , "C"   , 2   , 0x86 , nullptr , (int) S7DataTypeEnum::S7Counter,							S7CPUType::S7_300_400                       						                  ),
			gcnew S7OperandType(32, "I"   , "I"   , 0   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Bool  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(33, "Q"   , "Q"   , 0   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Bool  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(34, "M"   , "M"   , 0   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Bool  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(35, "AI"  , "AI"  , 2   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Word  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(36, "AQ"  , "AQ"  , 2   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Word  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(37, "AM"  , "AM"  , 2   , 0x84 , nullptr , (int) S7DataTypeEnum::S7Word  ,												  	                            S7CPUType::Logo                   ),
			gcnew S7OperandType(38, "M"   , "F"   , 0	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Bool  ,                                                                                                  S7CPUType::S5 ),
			gcnew S7OperandType(39, "MB"  , "FY"  , 1	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Byte  ,                                                                                                  S7CPUType::S5 ),
			gcnew S7OperandType(40, "MW"  , "FW"  , 2	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Word  ,                                                                                                  S7CPUType::S5 ),
			gcnew S7OperandType(41, "MD"  , "FD"  , 4	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Real  ,                                                                                                  S7CPUType::S5 ),
			gcnew S7OperandType(42, "S"   , "S"   , 0	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Bool  ,                                                                                                  S7CPUType::None/*S7CPUType::S5*/),
			gcnew S7OperandType(43, "SY"  , "SY"  , 1	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Byte  ,                                                                                                  S7CPUType::None/*S7CPUType::S5*/),
			gcnew S7OperandType(44, "SW"  , "SW"  , 2	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Word  ,                                                                                                  S7CPUType::None/*S7CPUType::S5*/),
			gcnew S7OperandType(45, "SD"  , "SD"  , 4	, 0x83 , nullptr , (int) S7DataTypeEnum::S7Real  ,                                                                                                  S7CPUType::None/*S7CPUType::S5*/),
			gcnew S7OperandType(46, "DB"  , "DB"  ,-1	, 0x84 , nullptr , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(47, "D"   , "D"   , 0	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Bool  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(48, "DL"  , "DL"  , 1	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(49, "DR"  , "DR"  , 1	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(50, "DW"  , "DW"  , 2	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Word  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(51, "DD"  , "DD"  , 4	, 0x84 , "DB"    , (int) S7DataTypeEnum::S7Real  ,							                                                                        S7CPUType::S5 ),
			gcnew S7OperandType(52, "DX"  , "DX"  ,-1	, 0x84 , nullptr , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(53, "D"   , "D"   , 0	, 0x84 , "DX"    , (int) S7DataTypeEnum::S7Bool  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(54, "DL"  , "DL"  , 1	, 0x84 , "DX"    , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(55, "DR"  , "DR"  , 1	, 0x84 , "DX"    , (int) S7DataTypeEnum::S7Byte  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(56, "DW"  , "DW"  , 2	, 0x84 , "DX"    , (int) S7DataTypeEnum::S7Word  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(57, "DD"  , "DD"  , 4	, 0x84 , "DX"    , (int) S7DataTypeEnum::S7Real  ,							                                                                        S7CPUType::S5 ), //Extended DBs
			gcnew S7OperandType(58, "OPT" , "OPT" ,-2	, 0x84 , nullptr , (int) S7DataTypeEnum::S7Byte  ,							                           S7CPUType::S7_TIA                     	                  ),
			//gcnew S7OperandType(58, "T"   , "T"   , 2   , 0x85 , nullptr , (int) S7DataTypeEnum::S7Word  , 							                                                     S7CPUType::S5 ),
			//gcnew S7OperandType(59, "Z"   , "C"   , 2   , 0x86 , nullptr , (int) S7DataTypeEnum::S7Word ,							                                                 S7CPUType::S5 ),

        };
    };

    private ref class S7OperandParser
    {
    public:
        S7OperandParser(String^ op);

        array<String^>^ Segments;
        int GetValue(int index, int min, int max);

    protected:
        String^ op;
        void ThrowError();
    };

    [Serializable]
	[System::Reflection::Obfuscation]
	public ref class S7Operand
    {
    public:
		S7Operand(String^ symbolName);
	    S7Operand(String^ symbolName, int datType);
		S7Operand(String^ symbolName, int datType, S7CPUType cpuType);
	    S7Operand(String^ symbolName, String^ datType);
		S7Operand(String^ symbolName, S7CPUType cpuType);
        S7Operand(S7Operand^ operand);
	    
        array<int>^ ListDataTypes(bool bAllowS5Real);
        int GetDataTypeSize();

        int GetOperandType() {return operandType;}
        bool SetOperandType(int value);

        int GetBitNr() {return bitNr;}
        bool SetBitNr(int value);
		int GetMaxBitNr();

        int GetDataType() {return dataType;}
        void SetDataType(int value);
        void SetDataTypeAsString(String^ dataTypeName);
        
        int GetAddress(int index) {return address[index];};
        bool SetAddress(int index, int addr);

        double GetMinScale();
        double GetMaxScale();
	    int GetError();
	    
	    String^ GetDataTypeAsString();

		static bool English = true;
	    String^ GetName() {return name;};
        String^ GetLocalizedName();
		void SetName(String^ newName);		 
		void UpdateName();

		S7CPUType GetCpuType() {return cpuType;}
		void SetCpuType(S7CPUType value);

        void Reset();       

        void AdvanceAddress();
        void AdvanceAddress(int nrSteps);

		bool AreTwoAddressesUsed();

	protected:
		bool error;
		int operandType;
		int dataType;
		int bitNr;
		S7CPUType cpuType;
		array<int>^ address; //primary and sec. index
		String^ name;

		String^ GetInternalName(String^ translatedName);
		String^ ParseName(String^ name);
		String^ FormatOperand();
		bool FindOperand(String^ str, String^ parent);
    };

}//end namespace S7_writer
