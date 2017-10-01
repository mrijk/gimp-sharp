## Process this file with automake to produce Makefile.in

all: Forge.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix.dll $(GTKSHARPLIB) /target:winexe

MYSOURCES = 			\
	AspectPreviewUpdater.cs	\
	Complex.cs		\
	ComplexMatrix.cs	\
	Dialog.cs		\
	DrawableUpdater.cs	\
	Forge.cs 		\
	FourierTransform.cs	\
	GaussDistribution.cs	\
	IUpdater.cs		\
	Land.cs			\
	Mesh.cs			\
	Planet.cs		\
	Renderer.cs		\
	SpectralSynthesis.cs	\
	StarFactory.cs

EXTRA_DIST = $(MYSOURCES) Forge Forge.png
SUBDIRS = po

CLEANFILES = Forge.exe

Forge.exe : $(MYSOURCES) Forge.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Forge.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Forge.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Forge $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Forge.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Forge.mo;\
		done
