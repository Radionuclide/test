#ifndef WMKTYPES_H_INTERN // Verriegelung
#define WMKTYPES_H_INTERN
////////////////////////////////////////////////////////////////////////////////////
// Datei WMKTypes.h
//
// Definitionen der Basis- und Aufruftypen über die verschiedenen Compiler und
// Betriebssysteme. 
// 
// Version 1.0
// (C) WKS
// Version vom 6.2.95
// Mail werner.mehrbrodt@inat.de, mehrbrodt@oberberg-online.de or werner.krings@inat.de
// Revision 1 vom 10.2.95: Namens„nderung in WMKTypes.h, weil NT WTypes und WKTypes benutzt
// Revision 2 vom 27.12.98: DPMI_FUNCTIONS_INCLUDED neu


#ifdef __cplusplus
    #define EXTERN_C    extern "C"
#else
    #define EXTERN_C    extern
#endif

#if defined (LINUX_FUNCTIONS_INCLUDED)
 #define DllImport	// weg, gibt es bei Linux nicht
 #define DllExport	// _export
 #define WENTRY_C _cdecl
 #define W_POINTER *
#elif defined (NT_FUNCTIONS_INCLUDED) || defined (MFC_FUNCTIONS_INCLUDED)
 #define DllImport	__declspec( dllimport )
 #define DllExport	__declspec( dllexport )
// #define WENTRY_C pascal
 #define WENTRY_C __stdcall
 #define W_POINTER *
#elif defined (OS2_FUNCTIONS_INCLUDED)

 #define DllImport	// weg, gibt es bei OS/2 nicht
 #define DllExport	// _export

 #if defined (OS2_32BIT)
  #if defined (__BORLANDC__)
   #define WENTRY_C pascal
  #elif defined (__IBMC__) || defined (__IBMCPP__)
   #define WENTRY_C _Pascal
  #elif defined (__WATCOMC__)
   #define WENTRY_C _Pascal
  #else
   #define WENTRY_C pascal
  #endif
 #define W_POINTER *
 #else
  #define WENTRY_C pascal far
  #define W_POINTER far *
 #endif
#elif defined (WINDOWS_FUNCTIONS_INCLUDED)
 #ifdef _MSC_VER == 800	// MSVC 1.5
  #define DllImport	// nicht ber VC1.5
  #define DllExport	__export
 #else
  #define DllImport	_import
  #define DllExport	_export
 #endif
 #if defined (__32BIT__)
  #define WENTRY_C pascal
  #define W_POINTER *
 #else
  #define WENTRY_C _loadds pascal far
  #define W_POINTER far *
 #endif
#elif defined (WIN95_FUNCTIONS_INCLUDED)
  #define DllImport	__declspec( dllimport )
  #define DllExport	__declspec( dllexport )
  #define WENTRY_C __stdcall
  #define W_POINTER *
#elif defined (DPMI_FUNCTIONS_INCLUDED)
 #define DllImport	__declspec( dllimport )
 #define DllExport	__declspec( dllexport )
// #define WENTRY_C pascal
 #define WENTRY_C __stdcall
 #define W_POINTER *
#else // Dos
 #define WENTRY_C _loadds pascal far
 #define W_POINTER far *
 #define DllImport	// weg, gibt es bei Dos nicht
 #define DllExport	// _export
#endif

#endif // WMKTYPES_H_INTERN
