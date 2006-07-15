## Process this file with automake to produce Makefile.in

all: Fragment.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Fragment.cs

EXTRA_DIST = $(MYSOURCES) Fragment

Fragment.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f Fragment.exe

install:
	$(INSTALL_DATA) Fragment.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Fragment $(DESTDIR)$(GIMPPLUGINDIR)
