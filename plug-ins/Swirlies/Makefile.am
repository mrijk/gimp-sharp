all: Swirlies.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES = Swirlies.cs

EXTRA_DIST = $(MYSOURCES) Swirlies

Swirlies.exe : $(MYSOURCES)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f Swirlies.exe

install:
	$(INSTALL_DATA) Swirlies.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Swirlies $(DESTDIR)/$(GIMPPLUGINDIR)
