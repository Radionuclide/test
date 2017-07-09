///*******************************************************************************
//
// Projekt        : Neue Version der AGLink-Bibliothek
//
// Dateiname      : AGL_SymbolicBaseTypes.hpp
//
// Beschreibung   : Definition der Grunddatentypen
//
// Copyright      : (c) 1998-2015
//                  DELTALOGIC Automatisierungstechnik GmbH
//                  Stuttgarter Str. 3
//                  73525 Schwäbisch Gmünd
//                  Web : http://www.deltalogic.de
//                  Tel.: +49-7171-916120
//                  Fax : +49-7171-916220
//
// Erstellt       : 27.07.2015 JBa
//
// Geändert       : 05.08.2015 JBa
//
// *******************************************************************************/
//
//#if !defined( __AGL_SYMBOLIC_BASE_TYPES__ )
//#define __AGL_SYMBOLIC_BASE_TYPES__
//
//#define GCC_VERSION (__GNUC__ * 10000 + __GNUC_MINOR__ * 100 + __GNUC_PATCHLEVEL__)
//
//#if defined( _WINDOWS ) || defined( WIN32 ) || defined(_WIN32)
//	#if(_MSC_VER < 1600) //Visual Studio 2010
//        #if !defined(__BORLANDC__)
//		   #include <crtdefs.h> // ptrdiff_t
//        #endif
//		
//		typedef char int8_t;
//		typedef short int16_t;
//		typedef int int32_t;
//		typedef __int64 int64_t;
//
//		typedef unsigned char uint8_t;
//		typedef unsigned short uint16_t;
//		typedef unsigned int uint32_t;
//		typedef unsigned __int64 uint64_t;
//	#else 		//>= VS2010 
//		#include <stdint.h>
//	#endif
//
//#else //nicht Windows
//
//// Typdefinitionen
//#define __int8  char
//#define __int16 short
//#define __int32 int
//#define __int64 long long
//
//#include <cstddef> //size_t
//#include <cstring> //std::memcmp()
//
//#if( defined(VXWORKS) && (GCC_VERSION <= 40102) )
//  #if !defined(__OSDEF_INC__) && !defined(__FRAME_0x72_CONSTANTS_HPP__)
//		typedef signed char int8_t; // s. vxTypes.h
//		typedef long int int32_t; // s. vxTypes.h
//		typedef long unsigned int uint32_t; // s. vxTypes.h
//  #endif		
//
////  typedef char int8_t;
//  typedef short int16_t;
////typedef int int32_t;
//  typedef __int64 int64_t;
//
//  typedef unsigned char uint8_t;
//  typedef unsigned short uint16_t;
////typedef unsigned int uint32_t;
//  typedef unsigned __int64 uint64_t;
//#else     //>= VS2010 
//  #include <stdint.h>
//#endif
//
//#endif
//
//typedef float float32_t;
//typedef double float64_t;
//
//typedef char char8_t;
//
//#if !defined(_CHAR16T) && !defined(__BORLANDC__) && !(defined(__linux__) && GCC_VERSION >= 60101)
// typedef unsigned short char16_t;
//#endif
//
//#endif //__AGL_SYMBOLIC_BASE_TYPES__
