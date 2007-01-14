## Process this file with automake to produce Makefile.in

all: PythonSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = /r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = PythonSample.py

EXTRA_DIST = $(MYSOURCES) PythonSample PythonSample.png

CLEANFILES = PythonSample.exe

PythonSample.exe : $(MYSOURCES) PythonSample.png $(GIMPSHARPLIB)
	$(IPY) -O -X:SaveAssemblies $(MYSOURCES) $(ASSEMBLIES) 

install:
	$(INSTALL_DATA) PythonSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PythonSample $(DESTDIR)$(GIMPPLUGINDIR)
