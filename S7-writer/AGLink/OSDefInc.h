/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : OSDefInc.H

 Beschreibung   : Einbinden der benötigten betriessystemabhängigen Includedateien

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 23.03.2004  RH

 Geändert       : 02.01.2017  RH

 *******************************************************************************/

#if !defined( __OSDEF_INC__ )
#define __OSDEF_INC__

/*******************************************************************************

  Definition von eigenen Systemkonstanten für alle Module und alle C/C++-Compiler

 *******************************************************************************/

#if defined( TIXI ) || defined( __TIXI__ )

  #if !defined( __TIXI__ )

    #define __TIXI__
  // #define __TIXI_API__

  #endif

  #if defined( MQX )

    //#undef MQX
    #include "TXTypes.h"
    #if !defined( __MQX__ )
      #define __MQX__
    #endif

  #endif

#elif defined( VXWORKS ) || defined( __VXWORKS__ )

  #if !defined( __VXWORKS__ )

    #define __VXWORKS__

  #endif

#elif defined( WINCE ) || defined( _WIN32_WCE ) || defined( __WINCE__ )

  #if !defined( __WINCE__ )

    #define __WINCE__

  #endif

#elif defined( WIN64 ) || defined( _WIN64 ) || defined( __WIN64__ )

  #if !defined( __WIN64__ )

    #define __WIN64__

  #endif

#elif defined( WIN32 ) || defined( _WIN32 ) || defined( __WIN32__ )

  #if !defined( __WIN32__ )

    #define __WIN32__

  #endif

#elif defined( LINUX ) || defined( __LINUX__ )

  #if defined( EMBEDDED )

    #define __EMBEDDED_LINUX__
  
  #endif

  #if !defined( __LINUX__ )

    #define __LINUX__

  #endif

#elif defined( ANDROID ) || defined( __ANDROID__ )

  #if !defined( __ANDROID__ )

    #define __ANDROID__

  #endif

#elif defined( SOLARIS ) || defined( __SOLARIS__ )

  #if !defined( __SOLARIS__ )

    #define __SOLARIS__

  #endif

#elif defined( _OS9000 )

  #if !defined( __OS9__ )

    #define __OS9__

  #endif

#else

  #error "Missing OS-Definition"

#endif


/*******************************************************************************

  Definition der Konstanten

 *******************************************************************************/

#define AGL_TRUE 1
#define AGL_FALSE 0

#if defined( __LP64__ ) || defined( _WIN64 ) || ( defined( __x86_64__ ) && !defined( __ILP32__ ) ) || defined( __ppc64__ ) || defined( _M_X64 ) || defined( __ia64 ) || defined ( _M_IA64 ) || defined( __aarch64__ ) || defined( __powerpc64__ )
  #define AGL_BIT_DEPTH 64
  #if defined (__LINUX__)
    #define AGL_LINUX_64
  #endif
#else
  #define AGL_BIT_DEPTH 32
#endif


/*******************************************************************************

  Definition der Datentypen

 *******************************************************************************/

#if ( defined( _WINDOWS ) || defined( WIN32 ) || defined( _WIN32 ) ) && ( _MSC_VER < 1600 ) //< Visual Studio 2010 (WinCE)
  typedef char agl_int8_t;
#else
  typedef signed char agl_int8_t;
#endif

typedef short agl_int16_t;
typedef int agl_int32_t;
typedef long long int agl_int64_t;

typedef float agl_float32_t;
typedef double agl_float64_t;

#if AGL_BIT_DEPTH == 64
  #ifdef AGL_LINUX_64
    typedef long int agl_ptrdiff_t;
    typedef unsigned long int agl_size_t;
	typedef int agl_long32_t;
	typedef unsigned int agl_ulong32_t;
  #else
    typedef long long int agl_ptrdiff_t;
    typedef unsigned long long int agl_size_t;
	typedef long int agl_long32_t;
	typedef unsigned long int agl_ulong32_t;
  #endif
#else
  typedef int agl_ptrdiff_t;
  typedef unsigned int agl_size_t;
  typedef long int agl_long32_t;
  typedef unsigned long int agl_ulong32_t;
#endif

typedef unsigned char agl_uint8_t;
typedef unsigned short agl_uint16_t;
typedef unsigned int agl_uint32_t;
typedef unsigned long long int agl_uint64_t;

typedef int agl_bool_t;

typedef char agl_char8_t;

typedef unsigned short agl_char16_t;
typedef unsigned int agl_char32_t;  
typedef wchar_t agl_wchar_t;

#define agl_cstr8_t agl_char8_t*

typedef void* agl_handle_t;

typedef agl_ptrdiff_t HandleType;

struct agl_systemtime_t
{
  agl_uint16_t  wYear;
  agl_uint16_t  wMonth;
  agl_uint16_t  wDayOfWeek;
  agl_uint16_t  wDay;
  agl_uint16_t  wHour;
  agl_uint16_t  wMinute;
  agl_uint16_t  wSecond;
  agl_uint16_t  wMilliseconds;
};

struct agl_wsabuf_t
{
  agl_uint32_t len;      // the length of the buffer
  agl_cstr8_t  buf;      // the pointer to the buffer
};


/*******************************************************************************

  Einbinden der Headerfiles

 *******************************************************************************/

#if defined( __TIXI__ )

  #error platform not supported with current version.

#elif defined( __VXWORKS__ )

  #define AGL_API 

#elif defined( __WINCE__ )

  #define AGL_API __stdcall

  #if defined(WINCE_SDK_HMI30)
    #include <stdlib.h> // fuer den WinCE-HMI30 std-lib bug
  #endif

  #include <cstddef> //ptrdiff_t

  #ifndef STRICT
    #define STRICT
  #endif

  #include <winsock2.h>
  #define WIN32_LEAN_AND_MEAN
  #include <windows.h>

  BOOL WINAPI EventModify(HANDLE hEvent, DWORD func);
  #define EventModify
  #include <kfuncs.h>

  #if defined( _CONSOLE )
    #include <stdio.h>
  #endif

#elif defined( __WIN32__ )

  #define AGL_API __stdcall

#elif defined( __WIN64__ )

  #define AGL_API __stdcall

#elif defined( __LINUX__ )

  #define AGL_API

#elif defined( __ANDROID__ )

  #error platform not supported with current version.

#elif defined( __SOLARIS__ )

  #error platform not supported with current version.

#elif defined( __OS9__ )

  #error platform not supported with current version.

#endif


#endif  // #if !defined( __OSDEF_INC__ )

