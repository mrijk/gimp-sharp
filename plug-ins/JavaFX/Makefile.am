## Process this file with automake to produce Makefile.in

all: JavaFX.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) -r:ICSharpCode.SharpZipLib.dll /target:winexe

SOURCES = Dialog.cs JavaFX.cs ZipWriter.cs

EXTRA_DIST = $(SOURCES) JavaFX JavaFX.png

JavaFX.exe: $(SOURCES) JavaFX.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(SOURCES) /resource:JavaFX.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f JavaFX.exe

install:
	$(INSTALL_DATA) JavaFX.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) JavaFX $(DESTDIR)$(GIMPPLUGINDIR)

