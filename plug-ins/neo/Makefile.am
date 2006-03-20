all: neo.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = neo.cs

EXTRA_DIST = $(MYSOURCES) neo

neo.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f neo.exe

install:
	$(INSTALL_DATA) neo.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) neo $(DESTDIR)/$(GIMPPLUGINDIR)
