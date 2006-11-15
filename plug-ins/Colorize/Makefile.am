## Process this file with automake to produce Makefile.in

all: Colorize.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Colorize.cs

EXTRA_DIST = $(MYSOURCES) Colorize Colorize.png
SUBDIRS = po

CLEANFILES = Colorize.exe

Colorize.exe : $(MYSOURCES) Colorize.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Colorize.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Colorize.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Colorize $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Colorize.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Colorize.mo;\
		done
