using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Opc.Ua;

namespace iba.ibaOPCServer
{
    /// <summary>
    /// todo
    /// add comments here and comment extensions
    /// todo
    /// unite with VarEnum information
    /// </summary>
    public static class DataTypeDitionaryUaAndVe
    {
        private static readonly SortedList<BuiltInType, VarEnum> _listUaToVe;
        private static readonly SortedList<VarEnum, BuiltInType> _listVeToUa;
        static DataTypeDitionaryUaAndVe()
        {
            _listUaToVe = new SortedList<BuiltInType, VarEnum>();
            _listVeToUa = new SortedList<VarEnum, BuiltInType>();
        }

        public static void AddPair(VarEnum v, BuiltInType b)
        {
            if (_listVeToUa.IndexOfKey(v) != -1) 
                throw new ArgumentException(
                    string.Format("Type '{0}' is already in the dictionary.", v));
            if (_listUaToVe.IndexOfKey(b) != -1) 
                throw new ArgumentException(
                    string.Format("Type '{0}' is already in the dictionary.", b));

            _listVeToUa.Add(v, b);
            _listUaToVe.Add(b, v);
        }

        public static void Initialize()
        {
            // todo
            // later initialize not manually but from xml dictionary?

            AddPair(VarEnum.VT_UNKNOWN, BuiltInType.Null);

            AddPair(VarEnum.VT_BOOL, BuiltInType.Boolean);

            AddPair(VarEnum.VT_I1, BuiltInType.SByte);
            AddPair(VarEnum.VT_I2, BuiltInType.Int16);
            AddPair(VarEnum.VT_I4, BuiltInType.Int32);
            AddPair(VarEnum.VT_I8, BuiltInType.Int64);

            AddPair(VarEnum.VT_UI1, BuiltInType.Byte);
            AddPair(VarEnum.VT_UI2, BuiltInType.UInt16);
            AddPair(VarEnum.VT_UI4, BuiltInType.UInt32);
            AddPair(VarEnum.VT_UI8, BuiltInType.UInt64);

            AddPair(VarEnum.VT_R4, BuiltInType.Float);
            AddPair(VarEnum.VT_R8, BuiltInType.Double);

            // to do check one or two byte...
            AddPair(VarEnum.VT_LPSTR, BuiltInType.String);
            //AddPair(VarEnum.VT_LPWSTR, BuiltInType.String);
            //AddPair(VarEnum.VT_ARRAY, BuiltInType.ar);

            // todo
            // when adding two pairs like
            ////AddPair(VarEnum.VT_LPSTR, BuiltInType.String);
            ////AddPair(VarEnum.VT_LPWSTR, BuiltInType.String);
            // currently i have exc message about inconsistency
            // actually i have to convert both to  BuiltInType.String
            // and back to default one. which? VarEnum.VT_LPSTR?

            // todo
            // here ints probably have different meaning!!!
//            AddPair(VarEnum.VT_INT, BuiltInType.Integer);

            // todo
            // add other types

            // Data types that were not added to my UA implementation
            //AddType(typenames, "WSTRING", VarEnum.VT_LPWSTR, 2048);
            //AddType(typenames, "TOD", VarEnum.VT_R8, 8, VarEnum.VT_I8);
            //AddType(typenames, "DT", VarEnum.VT_R8, 8, VarEnum.VT_I8);

        }
        /// <summary>
        /// Returns corresponding type from VarEnum.
        /// Returns VarEnum.VT_UNKNOWN if supplied type is not found.
        /// </summary>
        public static VarEnum UaToVe(BuiltInType b)
        {
            // todo
            // maybe rather throw KeyNotFoundException?
 
            VarEnum v;
            try
            {
                v = _listUaToVe[b];

                // if we are here then key was found;
                // lets make backward conversion to be completely sure
                if (b != _listVeToUa[v])
                    throw new Exception(
                        string.Format("Data inconsistency in dictionary with types '{0}' and '{1}' .", v, b));
            }
            catch(KeyNotFoundException)
            {
                v = VarEnum.VT_UNKNOWN;
            }
            return v;
        }

        /// <summary>
        /// Returns corresponding type from VarEnum.
        /// Returns BuiltInType.Null if supplied type is not found.
        /// </summary>
        public static BuiltInType VeToUa(VarEnum v)
        {
            // todo
            // maybe rather throw KeyNotFoundException?

            BuiltInType b;
            try
            {
                b = _listVeToUa[v];

                // if we are here then key was found;
                // lets make backward conversion to be completely sure
                if (v != _listUaToVe[b]) 
                    throw new Exception(
                        string.Format("Data inconsistency in dictionary with types '{0}' and '{1}' .", v, b));
            }
            catch (KeyNotFoundException)
            {
                b = BuiltInType.Null;
            }
            return b;
        }

        //public static VarEnum ToVarEnumType(this BuiltInType b)
        //{
        //    return UaToVe(b);
        //}
        public static BuiltInType ToUaType(this VarEnum v)
        {
            return VeToUa(v);
        }

        /// <summary>
        /// Returns ua type for given variable element.
        /// Returns BuiltInType.Null if VarEnum type of varElement is unknown or not supported.
        /// </summary>
        /// <param name="ve"></param>
        /// <returns></returns>
        //public static BuiltInType GetUaType(this VariableInformation.tVariableElement ve)
        //{
        //    // if type is already known then just return it
        //    if (ve.UaType != BuiltInType.Null) return ve.UaType;

        //    // type is unknown, try to get it from VarEnum type
        //    VarEnum t;
        //    if (ve.Type == VarEnum.VT_ARRAY)
        //        t = ((VariableInformation.tArrayInformation) ve.value.extInformation).BaseType;
        //    else
        //        t = ve.Type;

        //    ve.UaType = t.ToUaType(); // can still return BuiltInType.Null again
        //    return ve.UaType;
        //}

    }
}



//public enum VarEnum
//{
//    // Summary:
//    //     Indicates that a value was not specified.
//    VT_EMPTY = 0,
//    //
//    // Summary:
//    //     Indicates a null value, similar to a null value in SQL.
//    VT_NULL = 1,
//    //
//    // Summary:
//    //     Indicates a short integer.
//    VT_I2 = 2,
//    //
//    // Summary:
//    //     Indicates a long integer.
//    VT_I4 = 3,
//    //
//    // Summary:
//    //     Indicates a float value.
//    VT_R4 = 4,
//    //
//    // Summary:
//    //     Indicates a double value.
//    VT_R8 = 5,
//    //
//    // Summary:
//    //     Indicates a currency value.
//    VT_CY = 6,
//    //
//    // Summary:
//    //     Indicates a DATE value.
//    VT_DATE = 7,
//    //
//    // Summary:
//    //     Indicates a BSTR string.
//    VT_BSTR = 8,
//    //
//    // Summary:
//    //     Indicates an IDispatch pointer.
//    VT_DISPATCH = 9,
//    //
//    // Summary:
//    //     Indicates an SCODE.
//    VT_ERROR = 10,
//    //
//    // Summary:
//    //     Indicates a Boolean value.
//    VT_BOOL = 11,
//    //
//    // Summary:
//    //     Indicates a VARIANT far pointer.
//    VT_VARIANT = 12,
//    //
//    // Summary:
//    //     Indicates an IUnknown pointer.
//    VT_UNKNOWN = 13,
//    //
//    // Summary:
//    //     Indicates a decimal value.
//    VT_DECIMAL = 14,
//    //
//    // Summary:
//    //     Indicates a char value.
//    VT_I1 = 16,
//    //
//    // Summary:
//    //     Indicates a byte.
//    VT_UI1 = 17,
//    //
//    // Summary:
//    //     Indicates an unsignedshort.
//    VT_UI2 = 18,
//    //
//    // Summary:
//    //     Indicates an unsignedlong.
//    VT_UI4 = 19,
//    //
//    // Summary:
//    //     Indicates a 64-bit integer.
//    VT_I8 = 20,
//    //
//    // Summary:
//    //     Indicates an 64-bit unsigned integer.
//    VT_UI8 = 21,
//    //
//    // Summary:
//    //     Indicates an integer value.
//    VT_INT = 22,
//    //
//    // Summary:
//    //     Indicates an unsigned integer value.
//    VT_UINT = 23,
//    //
//    // Summary:
//    //     Indicates a C style void.
//    VT_VOID = 24,
//    //
//    // Summary:
//    //     Indicates an HRESULT.
//    VT_HRESULT = 25,
//    //
//    // Summary:
//    //     Indicates a pointer type.
//    VT_PTR = 26,
//    //
//    // Summary:
//    //     Indicates a SAFEARRAY. Not valid in a VARIANT.
//    VT_SAFEARRAY = 27,
//    //
//    // Summary:
//    //     Indicates a C style array.
//    VT_CARRAY = 28,
//    //
//    // Summary:
//    //     Indicates a user defined type.
//    VT_USERDEFINED = 29,
//    //
//    // Summary:
//    //     Indicates a null-terminated string.
//    VT_LPSTR = 30,
//    //
//    // Summary:
//    //     Indicates a wide string terminated by null.
//    VT_LPWSTR = 31,
//    //
//    // Summary:
//    //     Indicates a user defined type.
//    VT_RECORD = 36,
//    //
//    // Summary:
//    //     Indicates a FILETIME value.
//    VT_FILETIME = 64,
//    //
//    // Summary:
//    //     Indicates length prefixed bytes.
//    VT_BLOB = 65,
//    //
//    // Summary:
//    //     Indicates that the name of a stream follows.
//    VT_STREAM = 66,
//    //
//    // Summary:
//    //     Indicates that the name of a storage follows.
//    VT_STORAGE = 67,
//    //
//    // Summary:
//    //     Indicates that a stream contains an object.
//    VT_STREAMED_OBJECT = 68,
//    //
//    // Summary:
//    //     Indicates that a storage contains an object.
//    VT_STORED_OBJECT = 69,
//    //
//    // Summary:
//    //     Indicates that a blob contains an object.
//    VT_BLOB_OBJECT = 70,
//    //
//    // Summary:
//    //     Indicates the clipboard format.
//    VT_CF = 71,
//    //
//    // Summary:
//    //     Indicates a class ID.
//    VT_CLSID = 72,
//    //
//    // Summary:
//    //     Indicates a simple, counted array.
//    VT_VECTOR = 4096,
//    //
//    // Summary:
//    //     Indicates a SAFEARRAY pointer.
//    VT_ARRAY = 8192,
//    //
//    // Summary:
//    //     Indicates that a value is a reference.
//    VT_BYREF = 16384,
//}