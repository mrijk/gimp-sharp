## Process this file with automake to produce Makefile.in

all: wbmp.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0

MYSOURCES = wbmp.cs

EXTRA_DIST = $(MYSOURCES) wbmp

wbmp.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f wbmp.exe

install:
	$(INSTALL_DATA) wbmp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) wbmp $(DESTDIR)/$(GIMPPLUGINDIR)
