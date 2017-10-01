## Process this file with automake to produce Makefile.in

all: QR.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = Dialog.cs QR.cs Renderer.cs

EXTRA_DIST = $(MYSOURCES) QR QR.png

CLEANFILES = QR.exe

QR.exe : $(MYSOURCES) QR.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:QR.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) QR.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) QR $(DESTDIR)$(GIMPPLUGINDIR)

