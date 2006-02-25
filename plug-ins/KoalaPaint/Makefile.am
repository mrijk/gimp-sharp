## Process this file with automake to produce Makefile.in

all: KoalaPaint.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

SOURCES = KoalaPaint.cs

EXTRA_DIST = $(SOURCES) KoalaPaint

KoalaPaint.exe: $(SOURCES) $(GIMPSHARPLIB)
	$(CSC) $(SOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f KoalaPaint.exe

install:
	$(INSTALL_DATA) KoalaPaint.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) KoalaPaint $(DESTDIR)/$(GIMPPLUGINDIR)
