## Process this file with automake to produce Makefile.in

all: Raindrops.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = 			\
	Dialog.cs		\
	Raindrop.cs		\
	RaindropFactory.cs	\
	Raindrops.cs		\
	Renderer.cs

EXTRA_DIST = $(MYSOURCES) Raindrops Raindrops.png
SUBDIRS = po 

CLEANFILES = Raindrops.exe

Raindrops.exe : $(MYSOURCES) Raindrops.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Raindrops.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Raindrops.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Raindrops $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Raindrops.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Raindrops.mo;\
		done
