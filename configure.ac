dnl Process this file with autoconf to produce a configure script.

AC_INIT
AC_CONFIG_SRCDIR([README.md])
AM_INIT_AUTOMAKE(gimp-sharp, 0.19)
LT_INIT
AC_CONFIG_HEADERS([config.h])

dnl find and test the C compiler
AC_PROG_INSTALL
AC_PROG_MAKE_SET

VERSION="0.19"
AC_SUBST(VERSION)

AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR(['pkg-config' is not in your PATH.])
fi

AC_PATH_PROG(MONO, mono, no)
if test "$MONO" = "no"; then
        AC_MSG_ERROR(['mono' is not in your PATH.])
fi
AC_SUBST(MONO)

PKG_CHECK_MODULES(MONO, mono >= 2.8)
AC_SUBST(MONO_CFLAGS)
AC_SUBST(MONO_LIBS)

PKG_CHECK_MODULES(MONO_DEPENDENCY, mono, has_mono=true, has_mono=false)

if test "x$has_mono" = "xtrue"; then
    AC_PATH_PROG(RUNTIME, mono, no)
    AC_PATH_PROG(CSC, csc, no)
    CSC_FLAGS=-langversion:7.2
    AC_SUBST(CSC_FLAGS)
    LIB_PREFIX=.so
    LIB_SUFFIX=
else
    AC_PATH_PROG(CSC, gcsc.exe, no)
fi

if test x$CSC = "xno"; then
    AC_MSG_ERROR([You need to install either mono or .Net])
fi

AC_PATH_PROG(GIMPTOOL, gimptool-2.0, no)
if test "x$GIMPTOOL" = "xno"; then
        AC_MSG_ERROR([You don't have gimptool-2.0. Please install the GIMP development package!])
fi
AC_SUBST(GIMPTOOL)

if test "x$OS" != "xWindows_NT"; then
    dnl set GIMP plug-in path

    dnl GIMPPLUGINDIR=/usr/local/lib/gimp/2.0/plug-ins

    GIMPPLUGINDIR=`gimptool-2.0 -n --install-admin-bin README | head -1 | sed "s/\(.* \)\\(.*\\)/\2/g"`

    AC_SUBST(GIMPPLUGINDIR)

    GIMPLOCALE="`gimptool-2.0 --prefix`/share/locale"
    AC_SUBST(GIMPLOCALE)
fi

AC_SUBST(CSC)

#
# Check if we want to build unit test plug-in.
#

unittest=no
AC_ARG_WITH(unittest,[  --with-unittest         build unittest plug-in (default-no)])
if test "x$with_unittest" == xyes; then
   unittest=yes
fi

#
# Check if we want to build the webserver plug-in.
#

webserver=no
AC_ARG_WITH(webserver,[  --with-webserver        build embedded webserver plug-in (default-no)])
if test "x$with_webserver" == xyes; then
   webserver=yes
fi

#
# Check if we want to build the Paint.NET plug-in.
#

pdn=no
AC_ARG_WITH(pdn,[  --with-pdn              build Paint.NET plug-in (default-no)])
if test "x$with_pdn" == xyes; then
   pdn=yes
fi

#
# Check if we want to build ECW plug-in.
#

have_libecw=no
AC_ARG_WITH(libecw,  [  --without-ecw           do not build the ECW plug-in])
if test x$with_ecw != xno && test -z "$LIBECW"; then
  have_libecw=yes
  AC_CHECK_LIB(NCSEcw, NCSecwInit,
    [AC_CHECK_HEADER(NCSEcw.h,
      ECW='ecw$(EXEEXT)'; LIBECW='-lNCSEcw',
      [have_libecw="no (ECW header file not found)"])],
    [have_libecw="no (ECW library not found)"], -lNCSUtil -lNCSCnet)
fi
AC_SUBST(ECW)
AC_SUBST(LIBECW)


#
# Check if we can build the Colorize plug-in
#

have_umfpack=yes
if test -z "$LIBUMFPACK"; then
  AC_CHECK_LIB(umfpack, umfpack_di_solve,
    [AC_CHECK_HEADER(umfpack.h,
      LIBUMFPACK='-lumfpack',
      [have_umfpack="no"])],
    [have_umfpack="no"], -lblas -lamd -lm)
fi

AC_SUBST(LIBUMFPACK)

AC_SUBST(LIBZ)
AC_SUBST(PSP)

#
# Check if we want to build incomplete plug-ins
#

incomplete=no
AC_ARG_WITH(incomplete,[  --with-incomplete       build incomplete plug-ins (default-no)])
if test "x$with_incomplete" == xyes; then
   incomplete=yes
fi

#
# Check if we want to support Java language for plug-ins.
#

java=no
AC_ARG_WITH(java,[  --with-java             support for the Java language (default-no)])
if test "x$with_java" == xyes; then
   java=yes
fi

#
# Check if we want to support Boo
#

boo=no
AC_ARG_WITH(boo,[  --with-boo              support for Boo (default-no)])
if test "x$with_boo" == xyes; then
   boo=yes
   AC_PATH_PROG(BOOC, booc, no)
   if test "$BOOC" = "no"; then
        AC_MSG_ERROR(['booc' (Boo) is not in your PATH.])
   fi
   AC_SUBST(BOOC)
fi

#
# Check if we want to support F#
#

fsharp=no
AC_ARG_WITH(fsharp,[  --with-fsharp           support for F\# (default-no)])
if test "x$with_fsharp" == xyes; then
   fsharp=yes
   AC_PATH_PROG(FSC, fsc, no)
   if test "$FSC" = "no"; then
        AC_MSG_ERROR(['fsc' (FSharp) is not in your PATH.])
   fi
   AC_SUBST(FSC)
fi

#
# Check if we want to support IronPython
#

iron_python=no
AC_ARG_WITH(python,[  --with-ironpython       support for IronPython (default-no)])
if test "x$with_ironpython" == xyes; then
   iron_python=yes
   AC_PATH_PROG(IPY, ipy, no)
   if test "$IPY" = "no"; then
        AC_MSG_ERROR(['ipy' (IronPython) is not in your PATH.])
   fi
   AC_SUBST(IPY)
fi

#
# Check if we want to support IronRuby
#

iron_ruby=no
AC_ARG_WITH(ironruby,[  --with-ironruby         support for IronRuby (default-no)])
if test "x$with_ironruby" == xyes; then
   iron_ruby=yes
   AC_PATH_PROG(IR, ir.cmd, no)
   if test "$IR" = "no"; then
        AC_MSG_ERROR(['ir' (IronRuby) is not in your PATH.])
   fi
   AC_SUBST(IR)
fi

#
# Check if we want to support Nemerle
#

nemerle=no
AC_ARG_WITH(nemerle,[  --with-nemerle          support for Nemerle (default-no)])
if test "x$with_nemerle" == xyes; then
   nemerle=yes
   AC_PATH_PROG(NCC, ncc, no)
   if test "$NCC" = "no"; then
        AC_MSG_ERROR(['ncc' (Nemerle) is not in your PATH.])
   fi
   AC_SUBST(NCC)
fi

#
# Check if we want to support Oxygene
#

oxygene=no
AC_ARG_WITH(oxygene,[  --with-oxygene          support for Oxygene (default-no)])
if test "x$with_oxygene" == xyes; then
   oxygene=yes
   AC_PATH_PROG(Oxygene, Oxygene.exe, no)
   if test "$Oxygene" = "no"; then
        AC_MSG_ERROR(['Oxygene.exe' (Oxygene) is not in your PATH.])
   fi
   AC_SUBST(Oxygene)
fi

#
# Check if we want to support Scala
#

scala=no
AC_ARG_WITH(scala,[  --with-scala            support for Scala (default-no)])
if test "x$with_scala" == xyes; then
   scala=yes
   AC_PATH_PROG(SCALA, scalac-net, no)
   if test "$Scala" = "no"; then
        AC_MSG_ERROR(['scalac-net' (Scala) is not in your PATH.])
   fi
   AC_SUBST(SCALA)
fi

#
# Check if we want to support Visual Basic
#

visual_basic=no
AC_ARG_WITH(vb,[  --with-vb               support for Visual Basic (default-no)])
if test "x$with_vb" == xyes; then
   visual_basic=yes
   AC_PATH_PROG(VBNC, vbnc, no)
   if test "$VBNC" = "no"; then
        AC_MSG_ERROR(['vbnc' (Visual Basic) is not in your PATH.])
   fi
   AC_SUBST(VBNC)
fi

AM_CONDITIONAL(BUILD_BOO, test x$boo = xyes)
AM_CONDITIONAL(BUILD_COLORIZE, test x$have_umfpack = xyes)
AM_CONDITIONAL(BUILD_FSHARP, test x$fsharp = xyes)
AM_CONDITIONAL(BUILD_UNITTEST, test x$unittest = xyes)
AM_CONDITIONAL(BUILD_WEBSERVER, test x$webserver = xyes)
AM_CONDITIONAL(BUILD_PDN, test x$pdn = xyes)
AM_CONDITIONAL(BUILD_INCOMPLETE, test x$incomplete = xyes)
AM_CONDITIONAL(BUILD_ECW, test x"$have_libecw" = xyes)
AM_CONDITIONAL(BUILD_JAVA, test x$java = xyes)
AM_CONDITIONAL(BUILD_IRONPYTHON, test x$iron_python = xyes)
AM_CONDITIONAL(BUILD_IRONRUBY, test x$iron_ruby = xyes)
AM_CONDITIONAL(BUILD_NEMERLE, test x$nemerle = xyes)
AM_CONDITIONAL(BUILD_OXYGENE, test x$oxygene = xyes)
AM_CONDITIONAL(BUILD_SCALA, test x$scala = xyes)
AM_CONDITIONAL(BUILD_VISUALBASIC, test x$visual_basic = xyes)

dnl read Makefile.in and write Makefile
AC_CONFIG_FILES([
Makefile lib/Makefile 
java/Makefile
java/stubs/Makefile
plug-ins/Makefile 
plug-ins/AverageBlur/Makefile
plug-ins/AverageBlur/po/Makefile
plug-ins/BooSample/Makefile
plug-ins/Colorize/Makefile
plug-ins/Colorize/po/Makefile
plug-ins/CountTool/Makefile
plug-ins/DifferenceClouds/Makefile
plug-ins/DifferenceClouds/po/Makefile
plug-ins/ecw/Makefile
plug-ins/Forge/Makefile
plug-ins/Forge/po/Makefile
plug-ins/Fragment/Makefile
plug-ins/Fragment/po/Makefile
plug-ins/FSharpSample/Makefile
plug-ins/GemImg/Makefile
plug-ins/JavaFX/Makefile
plug-ins/KoalaPaint/Makefile
plug-ins/KoalaPaint/po/Makefile
plug-ins/ncp/Makefile 
plug-ins/ncp/po/Makefile 
plug-ins/Mezzotint/Makefile 
plug-ins/Mezzotint/po/Makefile 
plug-ins/Ministeck/Makefile 
plug-ins/Ministeck/po/Makefile 
plug-ins/NemerleSample/Makefile
plug-ins/neo/Makefile
plug-ins/neo/po/Makefile
plug-ins/OxygeneSample/Makefile
plug-ins/Pdn/Makefile 
plug-ins/PhotoshopActions/Makefile 
plug-ins/PictureFrame/Makefile 
plug-ins/PicturePackage/Makefile 
plug-ins/PicturePackage/po/Makefile 
plug-ins/Pointillize/Makefile
plug-ins/Pointillize/po/Makefile
plug-ins/PythonSample/Makefile
plug-ins/QR/Makefile
plug-ins/Raindrops/Makefile 
plug-ins/Raindrops/po/Makefile 
plug-ins/ScalaSample/Makefile
plug-ins/ShapeCollage/Makefile
plug-ins/Shatter/Makefile
plug-ins/Sky/Makefile
plug-ins/SliceTool/Makefile 
plug-ins/SliceTool/po/Makefile 
plug-ins/Splitter/Makefile 
plug-ins/Splitter/po/Makefile 
plug-ins/Swirlies/Makefile 
plug-ins/Swirlies/po/Makefile 
plug-ins/Trim/Makefile
plug-ins/UnitTest/Makefile 
plug-ins/UpdateCheck/Makefile 
plug-ins/UpdateCheck/po/Makefile 
plug-ins/VisualBasicSample/Makefile
plug-ins/wbmp/Makefile
plug-ins/wbmp/po/Makefile
plug-ins/WebServer/Makefile
])

AC_OUTPUT

dnl Print a summary of features enabled/disabled:
AC_MSG_RESULT([
Building GIMP-SHARP with prefix=$prefix
GIMP-SHARP Features:
  Unit Tests	      : $unittest
  Embedded Webserver  : $webserver
  Paint.NET	      : $pdn
  Incomplete	      : $incomplete
  ECW		      : $have_libecw
  Boo support         : $boo
  FSharp support      : $fsharp
  IronPython support  : $iron_python
  IronRuby support    : $iron_ruby
  Java support        : $java
  Nemerle support     : $nemerle
  Oxygene support     : $oxygene 
  Scala support	      : $scala
  Visual Basic support: $visual_basic
]);
