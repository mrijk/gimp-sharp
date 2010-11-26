## Process this file with automake to produce Makefile.in

all: VisualBasicSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = /r:$(GIMPSHARPLIB) /target:winexe

MYSOURCES = VisualBasicSample.vb

EXTRA_DIST = $(MYSOURCES) VisualBasicSample VisualBasicSample.png

CLEANFILES = VisualBasicSample.exe

VisualBasicSample.exe : $(MYSOURCES) VisualBasicSample.png $(GIMPSHARPLIB)
	$(VBNC) /res:VisualBasicSample.png $(MYSOURCES) $(ASSEMBLIES) \
	-out:$@ 

install:
	$(INSTALL_DATA) VisualBasicSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) VisualBasicSample $(DESTDIR)$(GIMPPLUGINDIR)
