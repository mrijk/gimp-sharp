## Process this file with automake to produce Makefile.in

all: DifferenceClouds.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = DifferenceClouds.cs 

EXTRA_DIST = $(MYSOURCES) DifferenceClouds DifferenceClouds.png

DifferenceClouds.exe : $(MYSOURCES) DifferenceClouds.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:DifferenceClouds.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f DifferenceClouds.exe

install:
	$(INSTALL_DATA) DifferenceClouds.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) DifferenceClouds $(DESTDIR)$(GIMPPLUGINDIR)
