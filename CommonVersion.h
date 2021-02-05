#ifndef COMMONVERSION_H
#define COMMONVERSION_H


#define MAJOR	2
#define MINOR	4
#define BUILD	0
#define BETATAG " BETA6" // use with leading whitespace, empty string "" when not set 




// Nothing should be changed below

#define STRINGIFY(x) #x
#define TOSTRING(x) STRINGIFY(x)

#define FILEVERNUMBER       MAJOR,MINOR,BUILD,0
#define FILEVERSTRINGSHORT  TOSTRING(MAJOR) "." TOSTRING(MINOR) "." TOSTRING(BUILD)

#define FILEVERSTRING       FILEVERSTRINGSHORT ".0"
#define PRODUCTVERSTRING    FILEVERSTRINGSHORT BETATAG


#ifdef _M_X64
	#define FILEDESC "ibaDatCoordinator (x64)"
	#define PLATFORMINVERSION " (x64)"
#else
	#define FILEDESC "ibaDatCoordinator"
	#define PLATFORMINVERSION ""
#endif


#endif // COMMONVERSION_H
