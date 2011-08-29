## Process this file with automake to produce Makefile.in

all: Swirlies.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Dialog.cs Renderer.cs Swirlies.cs Swirly.cs

EXTRA_DIST = $(MYSOURCES) Swirlies Swirlies.png
SUBDIRS = po

CLEANFILES = Swirlies.exe

Swirlies.exe : $(MYSOURCES) Swirlies.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Swirlies.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Swirlies.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Swirlies $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Swirlies.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Swirlies.mo;\
		done
