## Process this file with automake to produce Makefile.in

all: Raindrops.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 		\
	BoolMatrix.cs	\
	Raindrops.cs

EXTRA_DIST = $(MYSOURCES) Raindrops Raindrops.png
SUBDIRS = po 

CLEANFILES = Raindrops.exe

Raindrops.exe : $(MYSOURCES) Raindrops.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Raindrops.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Raindrops.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Raindrops $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Raindrops.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Raindrops.mo;\
		done
