## Process this file with automake to produce Makefile.in

all: PictureFrame.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = Dialog.cs PictureFrame.cs Renderer.cs

EXTRA_DIST = $(MYSOURCES) PictureFrame PictureFrame.png

CLEANFILES = PictureFrame.exe

PictureFrame.exe : $(MYSOURCES) PictureFrame.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:PictureFrame.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PictureFrame.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PictureFrame $(DESTDIR)$(GIMPPLUGINDIR)
