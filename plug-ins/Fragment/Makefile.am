## Process this file with automake to produce Makefile.in

all: Fragment.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = Fragment.cs

EXTRA_DIST = $(MYSOURCES) Fragment

Fragment.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) -out:$@ $(ASSEMBLIES)
SUBDIRS = po

CLEANFILES = Fragment.exe

install:
	$(INSTALL_DATA) Fragment.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Fragment $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Fragment.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Fragment.mo;\
		done
