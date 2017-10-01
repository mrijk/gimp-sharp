## Process this file with automake to produce Makefile.in

all: Sky.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = 				\
	Dialog.cs			\
	Octave.cs			\
	Sky.cs				\
	Perlin3D.cs			\
	Renderer.cs			\
	TMatrix.cs			\
	TRandom.cs

EXTRA_DIST = $(MYSOURCES) Sky Sky.png

CLEANFILES = Sky.exe

Sky.exe : $(MYSOURCES) Sky.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Sky.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Sky.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Sky $(DESTDIR)$(GIMPPLUGINDIR)
