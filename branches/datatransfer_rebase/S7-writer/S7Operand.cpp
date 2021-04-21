#include "stdafx.h"
#include "S7Operand.h"

//#define WIN32_LEAN_AND_MEAN
//#include <windows.h>

#define MAX_OPERAND_INDEX  65535
#define MIN_OPERAND_INDEX  0

#define MAX_S7200_I_INDEX		15
#define MAX_S7200_Q_INDEX		15
#define MAX_S7200_M_INDEX		31
#define MAX_S7200_C_INDEX		255
#define MAX_S7200_T_INDEX		255

#define MIN_LOGO_OPERAND_INDEX  1
#define MAX_LOGO_I_INDEX		24
#define MAX_LOGO_Q_INDEX		16
#define MAX_LOGO_M_INDEX		27
#define MAX_LOGO_AI_INDEX		8
#define MAX_LOGO_AQ_INDEX		2
#define MAX_LOGO_AM_INDEX		16

#define MAX_S5_EA_INDEX         255
#define MAX_S5_M_INDEX          255
#define MAX_S5_S_INDEX          1023
#define MAX_S5_DB_INDEX         255
#define MAX_S5_DB_NR            255
#define MAX_S5_DX_NR            255
#define MAX_S5_CT_INDEX		    255

#define MAX_DB_NR  65535
#define MIN_DB_NR  1

#define MAX_BIT_NR  7
#define MIN_BIT_NR  0

//#undef GetObject

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace S7_writer
{

/******************************************************************************
/* S7::S7OperandType
/*****************************************************************************/

	String^ S7::S7OperandType::LocalizedName::get() 
    {
        return S7Operand::English ? englishName : name;
    }

/******************************************************************************
/* S7OperandParser
/*****************************************************************************/

    S7OperandParser::S7OperandParser(String^ op)
    {
        this->op = op;
        Segments = gcnew array<String^>(5);

        int curIndex = 0;
        array<Char>^ temp = gcnew array<Char>(100);
        int tempIndex = 0;
        bool isNumeric = false;
        if(op->Length > 0)
            isNumeric = Char::IsDigit(op, 0);
        
        for(int i=0; (i<op->Length) && (curIndex < Segments->Length); i++)
        {
            if(Char::IsWhiteSpace(op, i))
                continue;

            if(op[i] == '.')
            {
                if(tempIndex > 0)
                {
                    Segments[curIndex++] = gcnew String(temp, 0, tempIndex);
                    tempIndex = 0;
                }
            }
            else
            {
                if(isNumeric != Char::IsDigit(op, i))
                {
                    isNumeric = !isNumeric;
                    if(tempIndex > 0)
                    {
                        Segments[curIndex++] = gcnew String(temp, 0, tempIndex);
                        tempIndex = 0;
                    }
                }
                temp[tempIndex++] = op[i];
            }
        }

        if((tempIndex > 0) && (curIndex < Segments->Length))
            Segments[curIndex++] = gcnew String(temp, 0, tempIndex);
    }

    int S7OperandParser::GetValue(int index, int min, int max)
    {
        if(index >= Segments->Length)
            ThrowError();

        int val = 0;
		if(!Int32::TryParse(Segments[index], val))
			ThrowError();

        if((val < min) || (val > max))
            ThrowError();
        return val;
    }

    void S7OperandParser::ThrowError()
    {
        throw gcnew Exception(String::Format("Error in S7 operand {0}", op));
    }

/******************************************************************************
/* S7Operand
/*****************************************************************************/

    S7Operand::S7Operand (String^ symbolName , int datType)
	    : dataType(-1), bitNr(-1), error(true), operandType(-1), cpuType(S7CPUType::S7_300_400)
    {
        address = gcnew array<int>(2);
        SetName(symbolName);
        SetDataType(datType);
    }

	S7Operand::S7Operand (String^ symbolName , int datType, S7CPUType cpuType)
		: dataType(-1), bitNr(-1), error(true), operandType(-1)
	{
		this->cpuType = cpuType;
		address = gcnew array<int>(2);
		SetName(symbolName);
		SetDataType(datType);
	}

    S7Operand::S7Operand (String^ symbolName , String^ datType)
	    : dataType(-1), bitNr(-1), error(true), operandType(-1)
    {
		this->cpuType = symbolName == "OPT" ? S7CPUType::S7_TIA : S7CPUType::S7_300_400;
        address = gcnew array<int>(2);
	    SetName(symbolName);
        SetDataTypeAsString(datType);
    }
	
    S7Operand::S7Operand (String^ symbolName)
	    : dataType(-1), bitNr(-1), error(true), operandType(-1), cpuType(S7CPUType::S7_200 | S7CPUType::S7_300_400)
    {
        address = gcnew array<int>(2);
	    SetName(symbolName);
        SetDataType(operandType >0 ? S7::OperandTypes[operandType]->defaultDataType : (int) S7DataTypeEnum::S7Int);
    }

	S7Operand::S7Operand (String^ symbolName, S7CPUType cpuType)
		: dataType(-1), bitNr(-1), error(true), operandType(-1)
	{
		this->cpuType = cpuType;
		address = gcnew array<int>(2);
		SetName(symbolName);
		SetDataType(operandType >0 ? S7::OperandTypes[operandType]->defaultDataType : (int) S7DataTypeEnum::S7Int);
	}
	
    S7Operand::S7Operand (S7Operand^ operand)
    {
        error = operand->error;
        operandType = operand->operandType;
        dataType = operand->dataType;
        bitNr = operand->bitNr;
        name = operand->name;
		cpuType = operand->cpuType;

        address = gcnew array<int>(2);
        address[0] = operand->address[0];
        address[1] = operand->address[1];
    }

    double S7Operand::GetMinScale()
    {
        if (dataType >= 0 && dataType < S7::DataTypes->Length)
            return S7::DataTypes[dataType]->minScale;
        return S7::DataTypes[(int) S7DataTypeEnum::S7Real]->minScale;
    }

    double S7Operand::GetMaxScale()
    {
        if (dataType >= 0 && dataType < S7::DataTypes->Length)
            return S7::DataTypes[dataType]->maxScale;
        return S7::DataTypes[(int) S7DataTypeEnum::S7Real]->maxScale;
    }

	bool S7Operand::AreTwoAddressesUsed()
	{
		if((operandType >= 0) && (operandType < S7::OperandTypes->Length))
			return S7::OperandTypes[operandType]->parent != nullptr;
		else
			return false;
	}

    String^ S7Operand::FormatOperand()
    {
        if (!SetOperandType(operandType))
            return String::Empty;

        String^ result = String::Empty;
        if ( S7::OperandTypes[operandType]->parent == nullptr ) // noparent
        {
			if(S7::OperandTypes[operandType]->size == -2)
				result = S7::OperandTypes[operandType]->name;
            else if (S7::OperandTypes[operandType]->size == 0 && S7::OperandTypes[operandType]->cpuType != S7CPUType::Logo) // bit (from non-LOGO! CPUs)
                result = String::Format("{0} {1}.{2}", 
                    S7::OperandTypes[operandType]->name, address[0].ToString(), bitNr.ToString());
            else // non bit value (or bit value from LOGO!-exclusive operands)
                result = String::Format("{0} {1}", 
                    S7::OperandTypes[operandType]->name, address[0].ToString());
        }
        else  // parent present
        {
            if (S7::OperandTypes[operandType]->size == 0) // bit
            {
                array<Object^>^ args = {
                    S7::OperandTypes[operandType]->parent, 
                    address[0].ToString(),
                    S7::OperandTypes[operandType]->name,
                    address[1].ToString(),
                    bitNr.ToString()
                };
                result = String::Format("{0} {1}.{2} {3}.{4}", args);
            }
            else // non bit value
            {
                array<Object^>^ args = {
                    S7::OperandTypes[operandType]->parent, 
                    address[0].ToString(),
                    S7::OperandTypes[operandType]->name,
                    address[1].ToString()
                };
                result = String::Format("{0} {1}.{2} {3}", args);
            }
        }
        
        return result;
    }


    bool S7Operand::FindOperand (String^ str, String^ parent)
    {
        String^ operand = str;

        for (int i=0; i<S7::OperandTypes->Length; i++)
        {
			if((int)(S7::OperandTypes[i]->cpuType & cpuType) == 0)
				continue;

            if (String::Compare(operand, S7::OperandTypes[i]->name, true) == 0)
            {
                if (S7::OperandTypes[i]->parent != parent) 
                    continue;
                //str.remove(0, strlen(s7OperandTypes[i].name));
                if(	(dataType < 0) || 
                    ((S7::OperandTypes[i]->size >= 0) && (
						(S7::DataTypes[dataType]->size != S7::OperandTypes[i]->size) || 
						S7::OperandTypes[i]->index == 30 || 
						S7::OperandTypes[i]->index == 31 ||
						dataType == (int)S7DataTypeEnum::S7Timer ||
						dataType == (int)S7DataTypeEnum::S7Counter)
					)
				) 
				{
					// datatype does not match the operandtype
                    dataType = (int) S7::OperandTypes[i]->defaultDataType;
				}

                operandType = i;
                return true;
            }
        }
        return false;
    }

    void S7Operand::Reset ()
    {
        address[0] = -1;
        address[1] = -1;
        bitNr = -1;
        operandType = -1;
        error = true;
    }

	String^ S7Operand::ParseName(String^ name)
	{
		String^ op = name->ToUpper();

		try
		{
			S7OperandParser^ s7Operand = gcnew S7OperandParser(op);

			Reset();
			if(FindOperand(s7Operand->Segments[0], nullptr)) // root operandtype found
			{
				if (S7::OperandTypes[operandType]->size == 0  ) // bit value root operand
				{
					// For LOGO!-exclusive operands (not V and DB types)
					if(S7::OperandTypes[operandType]->cpuType == S7CPUType::Logo)
					{
						int upperLimit = MAX_OPERAND_INDEX;

						switch(operandType)
						{
						case 32:		// I bits
							upperLimit = MAX_LOGO_I_INDEX;
							break;
						case 33:		// Q bits
							upperLimit = MAX_LOGO_Q_INDEX;
							break;
						case 34:		// M bits
							upperLimit = MAX_LOGO_M_INDEX;
							break;
						default:
							break;
						}

						address[0] = s7Operand->GetValue(1,MIN_LOGO_OPERAND_INDEX,upperLimit);
						bitNr = (address[0]-1) % 8;
					}
					else
					{
						int upperLimit = MAX_OPERAND_INDEX;

						if(cpuType == S7CPUType::S7_200)
						{
							switch(operandType)
							{
							case 0:	// I
								upperLimit = MAX_S7200_I_INDEX;
								break;
							case 1:	// Q
								upperLimit = MAX_S7200_Q_INDEX;
								break;
							case 2:	// M
								upperLimit = MAX_S7200_M_INDEX;
								break;
							default:
								break;
							}
						}
						else if(cpuType == S7CPUType::S5)
						{
							switch(operandType)
							{
							case 0:	 // I
							case 1:	 // Q
								upperLimit = MAX_S5_EA_INDEX;
								break;
							case 38: // M
								upperLimit = MAX_S5_M_INDEX;
								break;
							case 42: // S
								upperLimit = MAX_S5_S_INDEX;
								break;
							}
						}

						address[0] = s7Operand->GetValue(1,MIN_OPERAND_INDEX,upperLimit);
						bitNr = s7Operand->GetValue(2,MIN_BIT_NR,MAX_BIT_NR);
					}
				}
				else if (S7::OperandTypes[operandType]->size == -1) // child present
				{
					int minDbNr = MIN_DB_NR;
					int maxDbNr = MAX_DB_NR;
					int minOpIndex = MIN_OPERAND_INDEX;
					int maxOpIndex = MAX_OPERAND_INDEX;
					int minBitNr = MIN_BIT_NR;
					int maxBitNr = MAX_BIT_NR;
					if(cpuType == S7CPUType::S5)
					{
						minDbNr = 0;
						maxDbNr = MAX_S5_DB_NR;
						minOpIndex = 0;
						maxOpIndex = MAX_S5_DB_INDEX;
						maxBitNr = 15;
					}

					address[0] = s7Operand->GetValue(1, minDbNr, maxDbNr);
					if(FindOperand(s7Operand->Segments[2], S7::OperandTypes[operandType]->name)) // parse child
					{
						address[1] = s7Operand->GetValue(3, minOpIndex, maxOpIndex);
						if (S7::OperandTypes[operandType]->size == 0) // bit value
						{
							bitNr = s7Operand->GetValue(4, minBitNr, maxBitNr);
						}
					}
					else
					{
						return name; //complete DB
						//throw gcnew CIbaHWException("ErrInS7Operand", operand);
					}
				}
				else if (S7::OperandTypes[operandType]->size == -2) // child present
				{
					address[0] = 0;
					bitNr = 0;
				}
				else
				{
					int lowerLimit = MIN_OPERAND_INDEX;
					int upperLimit = MAX_OPERAND_INDEX;

					// For LOGO!-exclusive operands (not V and DB types)
					if(S7::OperandTypes[operandType]->cpuType == S7CPUType::Logo)
					{
						lowerLimit = MIN_LOGO_OPERAND_INDEX;

						switch(operandType)
						{
						case 35:		// AI words
							upperLimit = MAX_LOGO_AI_INDEX;
							break;
						case 36:		// AQ words
							upperLimit = MAX_LOGO_AQ_INDEX;
							break;
						case 37:		// AM words
							upperLimit = MAX_LOGO_AM_INDEX;
							break;
						default:
							break;
						}
					}
					else if(cpuType == S7CPUType::S7_200)
					{
						switch(operandType)
						{
						case 3:	// IB
							upperLimit = MAX_S7200_I_INDEX;
							break;
						case 4:	// QB
							upperLimit = MAX_S7200_Q_INDEX;
							break;
						case 5:	// MB
							upperLimit = MAX_S7200_M_INDEX;
							break;
						case 6:
							upperLimit = MAX_S7200_I_INDEX - 1;
							break;
						case 7:
							upperLimit = MAX_S7200_Q_INDEX - 1;
							break;
						case 8:
							upperLimit = MAX_S7200_M_INDEX - 1;
							break;
						case 9:
							upperLimit = MAX_S7200_I_INDEX - 3;
							break;
						case 10:
							upperLimit = MAX_S7200_Q_INDEX - 3;
							break;
						case 11:
							upperLimit = MAX_S7200_M_INDEX - 3;
							break;
						case 28:
							upperLimit = MAX_S7200_T_INDEX;
							break;
						case 29:
							upperLimit = MAX_S7200_C_INDEX;
							break;
						default:
							break;
						}

					}
					else if(cpuType == S7CPUType::S5)
					{

						switch(operandType)
						{
						case 3:  //IB
						case 4:  //QB
						case 6:  //IW
						case 7:  //QW
						case 9:  //ID
						case 10: //QD
						case 12: //PIB
						case 13: //PIW
							upperLimit = MAX_S5_EA_INDEX;
							break;

						case 30: //T
						case 31: //Z
							upperLimit = MAX_S5_CT_INDEX;
							break;

						case 39: //MB
						case 40: //MW
						case 41: //MD
							upperLimit = MAX_S5_M_INDEX;
							break;

						case 43: //SY
						case 44: //SW
						case 45: //SD
							upperLimit = MAX_S5_S_INDEX;
							break;

						case 48: //DB.DL
						case 49: //DB.DR
						case 50: //DB.DW
						case 51: //DB.DD
						case 54: //DX.DL
						case 55: //DX.DR
						case 56: //DX.DW
						case 57: //DX.DD
							upperLimit = MAX_S5_DB_INDEX;
							break;
						}

						int opSize = S7::OperandTypes[operandType]->size;
						upperLimit = (upperLimit / opSize) * opSize;
					}

					address[0] = s7Operand->GetValue(1, lowerLimit, upperLimit);
				}

				error = false;

				return FormatOperand();
			}
		}
		catch (Exception^ ex)
		{
			Debug::WriteLine("Exception on operand : {0}", ex);
			//ibaLogger::DebugFormat("Exception on operand : {0}", ex->Message);
		}

		return name; //return original symbol unchanged
	}


    int S7Operand::GetError()
    {
	    return  (error ? 1 : 0) ;    //ErrS7SyntaxError
    }

    bool S7Operand::SetOperandType(int value)
    {
        if (value >= 0 && value < S7::OperandTypes->Length)
        {
            operandType = value;
            return true;
        }
        return false;
    }

    bool S7Operand::SetAddress(int index, int addr)
    {
        if (index >= 0 && index <  2 && addr >= 0 )
        {
            address[index]= addr;
            return true;
        }
        return false;
    }

    bool S7Operand::SetBitNr(int bNr)
    {
        if (bNr >= 0 && bNr < GetMaxBitNr())
        {
            bitNr = bNr;
            return true;
        }
        return false;
    }

	int S7Operand::GetMaxBitNr()
	{
		if((cpuType == S7CPUType::S5) && (operandType >= 0))
		{
			S7::S7OperandType^ opType = S7::OperandTypes[operandType];
			if(opType->parent != nullptr)
				return 16; //S5 DB bits go from 0 to 15
		}

		return 8;
	}

    void S7Operand::SetDataType (int datType)
    {
        dataType = Math::Min(Math::Max(-1,datType), S7::DataTypes->Length-1);
    }

    void S7Operand::SetDataTypeAsString (String^ dataTypeName)
    {
        int type = -1;
        for (int i=0; i<S7::DataTypes->Length; i++)
        {
            if (String::Compare(dataTypeName, S7::DataTypes[i]->name, true) ==0)
            {
                type = i;
                break;
            }
        }

        if (type >= 0 )
        {
            if ( (operandType >=0 )  && (S7::OperandTypes[operandType]->size >= 0) &&
                 (S7::OperandTypes[operandType]->size != S7::DataTypes[type]->size) ) // invalid datatype for operandtype
            {
				if((S7::DataTypes[type]->type == (int)S7DataTypeEnum::S7LReal) &&
					(S7::OperandTypes[operandType]->size == 1))
				{
					dataType = type;
					return;
				}

                type = -1;
            }
        }

        dataType = type;
    }

    array<int>^ S7Operand::ListDataTypes(bool bAllowS5Real)
    {
        // list all possible datatypes for the operandtype size
        List<int>^ intList = gcnew List<int>(S7::DataTypes->Length);
        if ((name == nullptr) || (operandType < 0)  || (S7::OperandTypes[operandType]->size < 0)) // list all types since operand unknown
        {
            for (int i=0; i<S7::DataTypes->Length; i++)
			{
				if(bAllowS5Real || (S7::DataTypes[i]->type != (int)S7DataTypeEnum::S5Real))
					intList->Add(i);
			}
        }
		else if(operandType == 30)	// S7-300/400 timer --> only allow TIMER
		{
			for (int i=0; i<S7::DataTypes->Length; i++)
			{
				if (S7::DataTypes[i]->type == (int)S7DataTypeEnum::S7Timer)
				{
					intList->Add(i);
					break;
				}
			}
		}
		else if(operandType == 31) // S7-300/400 counter --> only allow COUNTER
		{
			for (int i=0; i<S7::DataTypes->Length; i++)
			{
				if (S7::DataTypes[i]->type == (int)S7DataTypeEnum::S7Counter)
				{
					intList->Add(i);
					break;
				}
			}
		}
        else
        {
            for (int i=0; i<S7::DataTypes->Length; i++)
            {
				// COUNTER and TIMER are only allowed for their matching operand types
				if (S7::DataTypes[i]->type == (int)S7DataTypeEnum::S7Counter || S7::DataTypes[i]->type == (int)S7DataTypeEnum::S7Timer)
					continue;

                if (S7::OperandTypes[operandType]->size == S7::DataTypes[i]->size)
				{
					if(bAllowS5Real || (S7::DataTypes[i]->type != (int)S7DataTypeEnum::S5Real))
						intList->Add(i);
				}
				else if(S7::DataTypes[i]->type == (int) S7DataTypeEnum::S7LReal)
				{
					if(S7::OperandTypes[operandType]->size == 1)
						intList->Add(i);
				}
            }
        }

		intList->Sort(gcnew Comparison<int>(&S7::S7Type::Compare));
        
        return intList->ToArray();
    }

    int S7Operand::GetDataTypeSize()
    {
        if ((name == nullptr) || (operandType < 0)  || (S7::OperandTypes[operandType]->size < 0)) // list all types since operand unknown
            return -1;
        else
            return S7::OperandTypes[operandType]->size;
    }

    String^ S7Operand::GetDataTypeAsString()
    {
        if (dataType >=0 )
            return S7::DataTypes[dataType]->name;
        
        return "error";
    }

    String^ S7Operand::GetLocalizedName()
    {
        if(!English)
            return name;
        
		// No localization for LOGO!-exclusive operands
		if(cpuType == S7CPUType::Logo)
			return name;

        String^ temp = name->Replace('E', 'I');
        temp = temp->Replace('A', 'Q');
		temp = temp->Replace('Z', 'C');

		if(cpuType == S7CPUType::S5)
		{
			temp = temp->Replace("MB", "FY");
			temp = temp->Replace('M', 'F');
		}

        return temp;
    }

    String^ S7Operand::GetInternalName(String^ translatedName)
    {
        String^ temp = translatedName->ToUpper();

		// No localization for LOGO!-exclusive operands
		if(cpuType == S7CPUType::Logo)
			return temp;

        temp = temp->Replace('I', 'E');
        temp = temp->Replace('Q', 'A');
		temp = temp->Replace('C', 'Z');

		if(cpuType == S7CPUType::S5)
		{
			temp = temp->Replace("FY", "MB");
			temp = temp->Replace('F', 'M');
		}

        return temp;
    }

    void S7Operand::SetName(String^ newName)
    {
        String^ temp = GetInternalName(newName);
	    name = ParseName(temp);
    }

	void S7Operand::SetCpuType(S7CPUType value)
	{
		cpuType = value;
	}

    void S7Operand::UpdateName()
    {
        name = FormatOperand();
    }

    void S7Operand::AdvanceAddress()
    {
        AdvanceAddress(1);
    }
    
    void S7Operand::AdvanceAddress(int nrSteps)
    {
        if(error || (nrSteps <= 0))
            return;

        S7::S7OperandType^ opType = S7::OperandTypes[operandType];
		if(opType->size < 0)
			return;

        for(int i=0; i<nrSteps; i++)
        {
            if (opType->parent == nullptr ) // noparent
            {
				if(opType->cpuType == S7CPUType::Logo)	// For LOGO!-exclusive operands
				{
					address[0]++;
				}
                else if(opType->size == 0) // bit
                {
                    bitNr++;
                    if(bitNr >= 8)
                    {
                        bitNr = 0;
                        address[0]++;
                    }
                }
				else if(operandType >= 28 && operandType <= 31)
				{
					address[0]++; //Timer and counter
				}
                else // non bit value
                {
                    address[0] += opType->size;
                }
            }
            else  // parent present
            {
                if (opType->size == 0) // bit
                {
					int maxBitNr = GetMaxBitNr();
                    bitNr++;
                    if(bitNr >= maxBitNr)
                    {
                        bitNr = 0;
                        address[1]++;
                    }
                }
				else if(operandType == 50 || operandType == 51 || operandType == 56 || operandType == 57)
				{
					address[1] += opType->size/2; //Word addresses for S5 DBs
				}
                else // non bit value
                {
                    address[1] += opType->size;
                }
            }
        }

        UpdateName();
    }

	//int S7Operand::CompareTo(S7Operand^ other)
	//{
	//	if(error != other->error)
	//		return error ? 1 : -1; //Errors last
	//	else if(error)
	//		return name->CompareTo(other->name);

	//	S7::S7OperandType^ opType = S7::OperandTypes[operandType];
	//	S7::S7OperandType^ otherOpType = S7::OperandTypes[other->operandType];
	//	if(opType->varType != otherOpType->varType)
	//		return opType->varType - otherOpType->varType; //Different memory area (P, I, Q, M, DB)

	//	int diff = address[0] - other->address[0];
	//	if(diff != 0)
	//		return diff; //Sort by address
	//	
	//	if(opType->parent != nullptr)
	//	{
	//		//DB operands
	//		diff = address[1] - other->address[1];
	//		if(diff != 0)
	//			return diff; //Sort by address within DB
	//	}
	//	
	//	if(opType->size != otherOpType->size)
	//		return otherOpType->size - opType->size; //Biggest size first

	//	if(opType->size == 0)
	//		return bitNr - other->bitNr; //Sort by bit
	//	else
	//		return 0;
	//}

	//bool S7Operand::IsSameMemoryArea(S7Operand^ other, bool bCheckDBNr)
	//{
	//	if(error == other->error)
	//		return true;
	//	else if(error)
	//		return false;
	//	
	//	S7::S7OperandType^ opType = S7::OperandTypes[operandType];
	//	S7::S7OperandType^ otherOpType = S7::OperandTypes[other->operandType];
	//	if(opType->varType != otherOpType->varType)
	//		return false;
	//	
	//	if(bCheckDBNr && (opType->parent != nullptr))
	//		return address[0] == other->address[0]; //Check in same DB
	//	else
	//		return true;
	//}

}