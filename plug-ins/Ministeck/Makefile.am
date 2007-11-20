## Process this file with automake to produce Makefile.in

all: Ministeck.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES =			\
	CornerShape.cs		\
	Ministeck.cs		\
	MinisteckPalette.cs	\
	OneByOneShape.cs	\
	Painter.cs		\
	Shape.cs		\
	ShapeDescription.cs	\
	ShapeDescriptionSet.cs	\
	ShapeSet.cs		\
	ThreeByOneShape.cs	\
	TwoByOneShape.cs	\
	TwoByTwoShape.cs

EXTRA_DIST = $(MYSOURCES) Ministeck Ministeck.png
SUBDIRS = po

CLEANFILES = Ministeck.exe

Ministeck.exe : $(MYSOURCES) Ministeck.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Ministeck.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Ministeck.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Ministeck $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Ministeck.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Ministeck.mo;\
		done

