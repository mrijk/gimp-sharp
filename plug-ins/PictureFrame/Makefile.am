## Process this file with automake to produce Makefile.in

all: PictureFrame.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = PictureFrame.cs

EXTRA_DIST = $(MYSOURCES) PictureFrame PictureFrame.png

CLEANFILES = PictureFrame.exe

PictureFrame.exe : $(MYSOURCES) PictureFrame.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:PictureFrame.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PictureFrame.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PictureFrame $(DESTDIR)$(GIMPPLUGINDIR)
