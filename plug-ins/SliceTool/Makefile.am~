all: ncp.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = ncp.cs

EXTRA_DIST = $(SOURCES) ncp ncp.png

ncp.exe : $(SOURCES) ncp.png
	$(CSC) $(SOURCES) /resource:ncp.png -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) ncp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) ncp $(DESTDIR)/$(GIMPPLUGINDIR)
