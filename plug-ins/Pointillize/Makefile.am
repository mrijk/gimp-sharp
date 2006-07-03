## Process this file with automake to produce Makefile.in

all: Pointillize.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 			\
	ColorCoordinate.cs	\
	ColorCoordinateSet.cs	\
	Pointillize.cs

EXTRA_DIST = $(MYSOURCES) Pointillize Pointillize.png

Pointillize.exe : $(MYSOURCES) Pointillize.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Pointillize.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Pointillize.exe

install:
	$(INSTALL_DATA) Pointillize.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Pointillize $(DESTDIR)$(GIMPPLUGINDIR)
