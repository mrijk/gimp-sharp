all: KoalaPaint.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = KoalaPaint.cs

EXTRA_DIST = $(SOURCES) KoalaPaint

KoalaPaint.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

clean:
	rm -f KoalaPaint.exe

install:
	$(INSTALL_DATA) KoalaPaint.exe $(GIMPPLUGINDIR)
	$(INSTALL) KoalaPaint $(GIMPPLUGINDIR)
