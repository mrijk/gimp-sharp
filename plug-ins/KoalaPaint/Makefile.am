## Process this file with automake to produce Makefile.in

all: KoalaPaint.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

SOURCES = KoalaPaint.cs

EXTRA_DIST = $(SOURCES) KoalaPaint
SUBDIRS = po

CLEANFILES = KoalaPaint.exe

KoalaPaint.exe: $(SOURCES) $(GIMPSHARPLIB)
	$(CSC) $(SOURCES) -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) KoalaPaint.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) KoalaPaint $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/KoalaPaint.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/KoalaPaint.mo;\
		done
