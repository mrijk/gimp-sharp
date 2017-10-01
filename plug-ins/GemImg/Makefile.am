## Process this file with automake to produce Makefile.in

all: GemImg.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix.dll $(GTKSHARPLIB) /target:winexe

MYSOURCES = GemImg.cs

EXTRA_DIST = $(MYSOURCES) GemImg

CLEANFILES = GemImg.exe

GemImg.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) GemImg.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) GemImg $(DESTDIR)$(GIMPPLUGINDIR)
