all: ecw.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = ecw.cs

EXTRA_DIST = $(MYSOURCES) ecw

ecw.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f ecw.exe

install:
	$(INSTALL_DATA) ecw.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) ecw $(DESTDIR)/$(GIMPPLUGINDIR)
