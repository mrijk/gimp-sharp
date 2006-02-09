all: ncp.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp-2.0

MYSOURCES = ncp.cs

EXTRA_DIST = $(MYSOURCES) ncp ncp.png

ncp.exe : $(MYSOURCES) ncp.png
	$(CSC) $(MYSOURCES) /resource:ncp.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f ncp.exe

install:
	$(INSTALL_DATA) ncp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) ncp $(DESTDIR)/$(GIMPPLUGINDIR)
