## Process this file with automake to produce Makefile.in

all: RedEye.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES = RedEye.cs

EXTRA_DIST = $(MYSOURCES) RedEye RedEye.png

RedEye.exe : $(MYSOURCES) RedEye.png
	$(CSC) $(MYSOURCES) /resource:RedEye.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f RedEye.exe

install:
	$(INSTALL_DATA) RedEye.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) RedEye $(DESTDIR)/$(GIMPPLUGINDIR)
