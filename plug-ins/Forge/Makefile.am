## Process this file with automake to produce Makefile.in

all: Forge.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Forge.cs

EXTRA_DIST = $(MYSOURCES) Forge Forge.png

Forge.exe : $(MYSOURCES) Forge.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Forge.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Forge.exe

install:
	$(INSTALL_DATA) Forge.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Forge $(DESTDIR)$(GIMPPLUGINDIR)
