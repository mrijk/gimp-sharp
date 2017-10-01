## Process this file with automake to produce Makefile.in

all: DifferenceClouds.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix.dll $(GTKSHARPLIB) /target:winexe

MYSOURCES = 			\
	Dialog.cs		\
	DifferenceClouds.cs 	\
	IndexedColorsMap.cs	\
	Renderer.cs

EXTRA_DIST = $(MYSOURCES) DifferenceClouds DifferenceClouds.png
SUBDIRS = po

CLEANFILES = DifferenceClouds.exe

DifferenceClouds.exe : $(MYSOURCES) DifferenceClouds.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:DifferenceClouds.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) DifferenceClouds.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) DifferenceClouds $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/DifferenceClouds.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/DifferenceClouds.mo;\
		done
