## Process this file with automake to produce Makefile.in

all: AverageBlur.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = AverageBlur.cs

EXTRA_DIST = $(MYSOURCES) AverageBlur AverageBlur.png

AverageBlur.exe : $(MYSOURCES) AverageBlur.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:AverageBlur.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f AverageBlur.exe

install:
	$(INSTALL_DATA) AverageBlur.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) AverageBlur $(DESTDIR)$(GIMPPLUGINDIR)
