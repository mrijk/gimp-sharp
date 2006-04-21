## Process this file with automake to produce Makefile.in

all: Raindrops.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Raindrops.cs

EXTRA_DIST = $(MYSOURCES) Raindrops Raindrops.png

Raindrops.exe : $(MYSOURCES) Raindrops.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Raindrops.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Raindrops.exe

install:
	$(INSTALL_DATA) Raindrops.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Raindrops $(DESTDIR)$(GIMPPLUGINDIR)
