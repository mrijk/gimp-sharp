## Process this file with automake to produce Makefile.in

all: AverageBlur.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = AverageBlur.cs

EXTRA_DIST = $(MYSOURCES) AverageBlur AverageBlur.png
SUBDIRS = po

CLEANFILES = AverageBlur.exe

AverageBlur.exe : $(MYSOURCES) AverageBlur.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:AverageBlur.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) AverageBlur.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) AverageBlur $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/AverageBlur.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/AverageBlur.mo;\
		done
