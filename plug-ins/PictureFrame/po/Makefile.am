## Process this file with automake to produce Makefile.in

all: PictureFrame.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = PictureFrame.cs

EXTRA_DIST = $(MYSOURCES) PictureFrame PictureFrame.png
SUBDIRS = po

CLEANFILES = PictureFrame.exe

AverageBlur.exe : $(MYSOURCES) PictureFrame.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:PictureFrame.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PictureFrame.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PictureFrame $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/PictureFrame.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/PictureFrame.mo;\
		done
