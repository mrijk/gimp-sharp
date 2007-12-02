## Process this file with automake to produce Makefile.in

all: FSharpSample.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIBS = `pkg-config --libs gtk-sharp-2.0 | sed "s/-r:/-r /g"`

ASSEMBLIES = -r $(GIMPSHARPLIB) $(GTKSHARPLIBS)

MYSOURCES = FSharpSample.fs

EXTRA_DIST = $(MYSOURCES) FSharpSample FSharpSample.png

CLEANFILES = FSharpSample.exe

FSharpSample.exe : $(MYSOURCES) FSharpSample.png $(GIMPSHARPLIB)
	$(FSC) $(MYSOURCES) $(ASSEMBLIES) \
	-o $@

install:
	$(INSTALL_DATA) FSharpSample.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) FSharpSample $(DESTDIR)$(GIMPPLUGINDIR)
