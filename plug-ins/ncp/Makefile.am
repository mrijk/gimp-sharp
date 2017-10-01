## Process this file with automake to produce Makefile.in

all: ncp.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = ncp.cs Calculator.cs Dialog.cs Renderer.cs

EXTRA_DIST = $(MYSOURCES) ncp ncp.png
SUBDIRS = po

CLEANFILES = ncp.exe

ncp.exe : $(MYSOURCES) ncp.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:ncp.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) ncp.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) ncp $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/ncp.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/ncp.mo;\
		done
