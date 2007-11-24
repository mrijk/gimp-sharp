## Process this file with automake to produce Makefile.in

all: Pointillize.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 				\
	ColorCoordinate.cs		\
	ColorCoordinateSet.cs		\
	Pointillize.cs

EXTRA_DIST = $(MYSOURCES) Pointillize Pointillize.png
SUBDIRS = po

CLEANFILES = Pointillize.exe

Pointillize.exe : $(MYSOURCES) Pointillize.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Pointillize.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Pointillize.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Pointillize $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Pointillize.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Pointillize.mo;\
		done
