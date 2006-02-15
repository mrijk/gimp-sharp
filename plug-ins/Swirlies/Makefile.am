## Process this file with automake to produce Makefile.in

all: Swirlies.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0

MYSOURCES = Swirlies.cs Swirly.cs

EXTRA_DIST = $(MYSOURCES) Swirlies Swirlies.png

Swirlies.exe : $(MYSOURCES) Swirlies.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Swirlies.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Swirlies.exe

install:
	$(INSTALL_DATA) Swirlies.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Swirlies $(DESTDIR)/$(GIMPPLUGINDIR)
