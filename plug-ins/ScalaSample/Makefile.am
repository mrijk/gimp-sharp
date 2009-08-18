## Process this file with automake to produce Makefile.in

all: ScalaSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -Xassem-path $(GIMPSHARPLIB)

MYSOURCES = ScalaSample.scala

EXTRA_DIST = $(MYSOURCES) ScalaSample ScalaSample.png

CLEANFILES = ScalaSample.exe

ScalaSample.exe : $(MYSOURCES) ScalaSample.png $(GIMPSHARPLIB)
	$(SCALA) $(MYSOURCES) $(ASSEMBLIES)
	ilasm ScalaSample.msil -o $@

install:
	$(INSTALL_DATA) ScalaSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) ScalaSample $(DESTDIR)$(GIMPPLUGINDIR)
