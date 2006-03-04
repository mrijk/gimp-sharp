## Process this file with automake to produce Makefile.in

all: UpdateCheck.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = UpdateCheck.cs

EXTRA_DIST = $(MYSOURCES) UpdateCheck UpdateCheck.png

UpdateCheck.exe : $(MYSOURCES) UpdateCheck.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:UpdateCheck.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f UpdateCheck.exe

install:
	$(INSTALL_DATA) UpdateCheck.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) UpdateCheck $(DESTDIR)/$(GIMPPLUGINDIR)
