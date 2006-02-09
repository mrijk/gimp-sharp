all: Swirlies.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp-2.0

MYSOURCES = Swirlies.cs Swirly.cs

EXTRA_DIST = $(MYSOURCES) Swirlies Swirlies.png

Swirlies.exe : $(MYSOURCES)
	$(CSC) $(MYSOURCES) /resource:Swirlies.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Swirlies.exe

install:
	$(INSTALL_DATA) Swirlies.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Swirlies $(DESTDIR)/$(GIMPPLUGINDIR)
