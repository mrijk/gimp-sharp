## Process this file with automake to produce Makefile.in

AUTOMAKE_OPTIONS = gnu

MSCORLIBDIR="`echo $(MONO) | sed -e 's/mono//'`../lib/mono/2.0/"
IKVMDIR=`echo $(MONO) | sed -e 's/mono//'`../lib/ikvm/

LIBRARIES_NAME = gimp-sharp mscorlib 

ASSEMBLIES=	/pkg:gtk-sharp-2.0

ORIGINAL_DLLS = gimp-sharp.dll mscorlib.dll
STUBS = gimp-sharp.jar mscorlib.jar

EXTRA_DIST = $(MYSOURCES) $(LIBRARY).config

CLEANFILES = $(ORIGINAL_DLLS) $(STUBS)

all: $(STUBS)

SUFFIXES = .dll .jar

.dll.jar:
	mono $(IKVMDIR)/ikvmstub.exe $< 

gimp-sharp.dll:
	cp ../../lib/gimp-sharp.dll .

mscorlib.dll:
	cp $(MSCORLIBDIR)/mscorlib.dll .

##install:
##	$(INSTALL_DATA) $(LIBRARY) $(DESTDIR)$(GIMPPLUGINDIR)
##	$(INSTALL_DATA) $(LIBRARY).config $(DESTDIR)$(GIMPPLUGINDIR)
