## Process this file with automake to produce Makefile.in

all: Mezzotint.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Dialog.cs Mezzotint.cs Renderer.cs

EXTRA_DIST = $(MYSOURCES) Mezzotint Mezzotint.png
SUBDIRS = po

CLEANFILES = Mezzotint.exe

Mezzotint.exe : $(MYSOURCES) Mezzotint.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Mezzotint.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Mezzotint.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Mezzotint $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Mezzotint.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Mezzotint.mo;\
		done
