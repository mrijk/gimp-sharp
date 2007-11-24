## Process this file with automake to produce Makefile.in

all: UpdateCheck.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 		\
	RequestState.cs	\
	UpdateCheck.cs

EXTRA_DIST = $(MYSOURCES) UpdateCheck UpdateCheck.png
SUBDIRS = po

CLEANFILES = UpdateCheck.exe

UpdateCheck.exe : $(MYSOURCES) UpdateCheck.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:UpdateCheck.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) UpdateCheck.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) UpdateCheck $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/UpdateCheck.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/UpdateCheck.mo;\
		done
