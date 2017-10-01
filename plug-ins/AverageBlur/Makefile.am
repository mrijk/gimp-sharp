## Process this file with automake to produce Makefile.in

all: AverageBlur.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = AverageBlur.cs

EXTRA_DIST = $(MYSOURCES) AverageBlur AverageBlur.png
SUBDIRS = po

CLEANFILES = AverageBlur.exe

AverageBlur.exe : $(MYSOURCES) AverageBlur.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:AverageBlur.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) AverageBlur.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) AverageBlur $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/AverageBlur.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/AverageBlur.mo;\
		done
