## Process this file with automake to produce Makefile.in

all: RedEye.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = RedEye.cs

EXTRA_DIST = $(MYSOURCES) RedEye RedEye.png

RedEye.exe : $(MYSOURCES) RedEye.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:RedEye.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f RedEye.exe

install:
	$(INSTALL_DATA) RedEye.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) RedEye $(DESTDIR)$(GIMPPLUGINDIR)
