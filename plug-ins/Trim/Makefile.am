## Process this file with automake to produce Makefile.in

all: Trim.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Trim.cs

EXTRA_DIST = $(MYSOURCES) Trim Trim.png

CLEANFILES = Trim.exe

Trim.exe : $(MYSOURCES) Trim.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Trim.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Trim.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Trim $(DESTDIR)$(GIMPPLUGINDIR)
