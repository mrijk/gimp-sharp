## Process this file with automake to produce Makefile.in

all: JavaFX.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /r:ICSharpCode.SharpZipLib.dll

SOURCES = JavaFX.cs ZipWriter.cs

EXTRA_DIST = $(SOURCES) JavaFX JavaFX.png

JavaFX.exe: $(SOURCES) JavaFX.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(SOURCES) /resource:JavaFX.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f JavaFX.exe

install:
	$(INSTALL_DATA) JavaFX.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) JavaFX $(DESTDIR)$(GIMPPLUGINDIR)

