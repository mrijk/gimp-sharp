## Process this file with automake to produce Makefile.in

all: Trim.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = Dialog.cs Renderer.cs Trim.cs

EXTRA_DIST = $(MYSOURCES) Trim

CLEANFILES = Trim.exe

Trim.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Trim.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Trim $(DESTDIR)$(GIMPPLUGINDIR)
