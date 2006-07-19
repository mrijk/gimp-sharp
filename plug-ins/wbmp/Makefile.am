## Process this file with automake to produce Makefile.in

all: wbmp.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = wbmp.cs

EXTRA_DIST = $(MYSOURCES) wbmp
SUBDIRS = po

CLEANFILES = wbmp.exe

wbmp.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) wbmp.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) wbmp $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/wbmp.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/wbmp.mo;\
		done
