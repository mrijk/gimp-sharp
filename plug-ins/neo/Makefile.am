## Process this file with automake to produce Makefile.in

all: neo.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = neo.cs

EXTRA_DIST = $(MYSOURCES) neo

neo.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)
SUBDIRS = po

CLEANFILES = neo.exe

install:
	$(INSTALL_DATA) neo.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) neo $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/neo.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/neo.mo;\
		done
