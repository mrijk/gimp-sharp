all: ncp.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = ncp.cs

EXTRA_DIST = $(SOURCES) ncp

ncp.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) ncp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) ncp $(DESTDIR)/$(GIMPPLUGINDIR)
