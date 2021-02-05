/*******************************************************************************

 Projekt        : Neue Version der AGLink-Bibliothek

 Dateiname      : kbhit.h

 Beschreibung   : Nachbildung von kbhit() und getch() unter Linux, Solaris und VxWorks

 Copyright      : (c) 1998-2017
                  DELTALOGIC Automatisierungstechnik GmbH
                  Stuttgarter Str. 3
                  73525 Schwäbisch Gmünd
                  Web : http://www.deltalogic.de
                  Tel.: +49-7171-916120
                  Fax : +49-7171-916220

 Erstellt       : 03.06.2006  MST

 Geändert       : 03.03.2010  MST

 *******************************************************************************/
#if !defined( KBHIT_H )
#define KBHIT_H

#if defined( LINUX ) || defined( SOLARIS ) || defined( VXWORKS )

  /*******************************************************************************
   Einbinden der Headerfiles
   *******************************************************************************/
#if defined( VXWORKS )
  #include <ioLib.h>
  #include <fcntl.h>
  #include <sys/ioctl.h>
#else
  #include <termios.h>
    #define BSD_COMP
  #include <sys/ioctl.h>
#endif

  /*******************************************************************************
   Nachbildung von getch() etc.
   *******************************************************************************/
  #define _kbhit kbhit
  #define _getch getch
  #define _putch putch

  #define getch getchar
  #define putch putchar
  
  /*******************************************************************************
   Nachbildung von kbhit()
   *******************************************************************************/
  static int kbhit(void)
  {
    int n = 0, rv;
    rv = ioctl( fileno(stdin), FIONREAD, &n);
    if(rv == -1 || n == 0) return(0);
    else                   return(n);
  }

  #if !defined( VXWORKS ) && !defined( KBHIT_NO_INIT )

    class kbhit_init
    {
      public:
        kbhit_init(void) { init_kbhit(1); };
       ~kbhit_init(void) { init_kbhit(0); };
      protected:
        static int init_kbhit(int init);
    };

    /*static*/ int kbhit_init::init_kbhit(int init)
    {
      static struct termios ttyd_save;
             struct termios ttyd;

      if (init != 0)
      { /* init termio */
        if (tcgetattr( fileno(stdin), &ttyd) == -1)
        {
           perror("tcgetattr");
           return(-1);
        }

        ttyd_save = ttyd;

        /* set input mode: no_signal at break, no strip */
        ttyd.c_iflag &=~(BRKINT | ISTRIP |IXON);
        /* set local mode: disable Erase & Kill, no Special controls */
        ttyd.c_lflag &=~(ICANON|IEXTEN|ECHO|ECHOE|ECHOK|ECHONL); //ISIG
        ttyd.c_cc[VMIN]=0;   /* min. charcter */
        ttyd.c_cc[VTIME]=0;  /* timout, 0.1 sec unit */

        if (tcsetattr( fileno(stdin), TCSANOW, &ttyd) == -1)
        {
          perror("tcsetattr");
          return(-1);
        }

        setbuf(stdin, NULL);
        setbuf(stdout, NULL);
      }
      else
      { /* restore termio */
        if (tcsetattr( fileno(stdin), TCSANOW, &ttyd_save) == -1)
        {
          perror("tcsetattr");
          return(-1);
        }
        printf("\n");
      }

      return(0);
    }

    static class kbhit_init s_kbhit_initializer;
  
  #endif // #if !defined( VXWORKS ) && !defined( KBHIT_NO_INIT )

#elif defined( _OS9000 ) /* #if defined( LINUX ) ... #else */

  /*******************************************************************************

   Einbinden der Headerfiles

   *******************************************************************************/
  #include <types.h>
  #include <unistd.h>
  #include <unix/ioctl.h>
  #include <unix/os9def.h>
  #include <unix/os9types.h>

  /*******************************************************************************
   Nachbildung von getch()
   *******************************************************************************/
  #define getch getchar
  #define STDIO 1  
  /*******************************************************************************
   Nachbildung von kbhit()
   *******************************************************************************/
  int kbhit(void)
  {
/*  fd_set readfds;
    FD_ZERO( &readfds );
    FD_SET( STDIO, &readfds );
    timeval time = { 0, 100000 };

    int ret = select( STDIO+1, &readfds, NULL, NULL, &time );
    if( ret < 0 ) printf( "select(stdio) failed. errno=%d\n", errno );

    return( ret > 0 );
*/
    int n = 0, rv;
    rv = ioctl( STDIN_FILENO, FIONREAD, (caddr_t)&n);
    if(rv == -1 || n == 0) return(0);
    else                   return(n);
  }

    
  #endif // #if defined( LINUX ) || defined( SOLARIS ) || defined( VXWORKS )
  
#endif // #if !defined( KBHIT_H )
/*******************************************************************************
 Ende kbhit.h
 *******************************************************************************/
