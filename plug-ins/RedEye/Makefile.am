all: RedEye.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = RedEye.cs

EXTRA_DIST = $(SOURCES) RedEye RedEye.png

RedEye.exe : $(SOURCES) RedEye.png
	$(CSC) $(SOURCES) /resource:RedEye.png -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) RedEye.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) RedEye $(DESTDIR)/$(GIMPPLUGINDIR)
