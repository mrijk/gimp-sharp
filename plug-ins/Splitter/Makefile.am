all: Splitter.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = Splitter.cs

EXTRA_DIST = $(SOURCES) Splitter

Splitter.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Splitter.exe $(GIMPPLUGINDIR)
	$(INSTALL) Splitter $(GIMPPLUGINDIR)
