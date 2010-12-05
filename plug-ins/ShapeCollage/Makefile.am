## Process this file with automake to produce Makefile.in

all: ShapeCollage.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = ShapeCollage.cs

EXTRA_DIST = $(MYSOURCES) ShapeCollage ShapeCollage.png
SUBDIRS =

CLEANFILES = ShapeCollage.exe

ShapeCollage.exe : $(MYSOURCES) ShapeCollage.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:ShapeCollage.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) ShapeCollage.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) ShapeCollage $(DESTDIR)$(GIMPPLUGINDIR)
