## Process this file with automake to produce Makefile.in

all: OxygeneSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -ref:$(GIMPSHARPLIB) -type:winexe

MYSOURCES = OxygeneSample.pas

EXTRA_DIST = $(MYSOURCES) OxygeneSample OxygeneSample.png

CLEANFILES = OxygeneSample.exe

OxygeneSample.exe : $(MYSOURCES) OxygeneSample.png $(GIMPSHARPLIB)
	mono $(Oxygene) $(MYSOURCES) -res:OxygeneSample.png $(ASSEMBLIES) \
	-out:$@

install:
	$(INSTALL_DATA) OxygeneSample.exe/OxygeneSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) OxygeneSample $(DESTDIR)$(GIMPPLUGINDIR)
