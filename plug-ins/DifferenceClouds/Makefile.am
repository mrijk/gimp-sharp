## Process this file with automake to produce Makefile.in

all: DifferenceClouds.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = DifferenceClouds.cs 

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
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/DifferenceClouds.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/DifferenceClouds.mo;\
		done
