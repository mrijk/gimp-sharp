## Process this file with automake to produce Makefile.in

all: NemerleSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = /r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = NemerleSample.n

EXTRA_DIST = $(MYSOURCES) NemerleSample NemerleSample.png

CLEANFILES = NemerleSample.exe

NemerleSample.exe : $(MYSOURCES) NemerleSample.png $(GIMPSHARPLIB)
	$(NCC) $(MYSOURCES) -resource:NemerleSample.png $(ASSEMBLIES) \
	-out:$@

install:
	$(INSTALL_DATA) NemerleSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) NemerleSample $(DESTDIR)$(GIMPPLUGINDIR)
