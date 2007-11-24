## Process this file with automake to produce Makefile.in

all: Forge.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 			\
	Complex.cs		\
	ComplexMatrix.cs	\
	Forge.cs 		\
	FourierTransform.cs	\
	GaussDistribution.cs	\
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
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Forge.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Forge.mo;\
		done
