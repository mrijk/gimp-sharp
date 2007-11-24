## Process this file with automake to produce Makefile.in

all: Splitter.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 			\
	AdvancedDialog.cs	\
	MathExpressionParser.cs \
	MyClassBase.cs		\
	Splitter.cs

EXTRA_DIST = $(MYSOURCES) Splitter Splitter.png
SUBDIRS = po

CLEANFILES = Splitter.exe

Splitter.exe : $(MYSOURCES) Splitter.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Splitter.png \
	-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Splitter.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Splitter $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Splitter.mo";\
		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/Splitter.mo;\
		done
