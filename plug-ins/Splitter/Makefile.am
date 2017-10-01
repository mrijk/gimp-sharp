## Process this file with automake to produce Makefile.in

all: Splitter.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES = 			\
	AdvancedDialog.cs	\
	Dialog.cs		\
	MathExpressionParser.cs \
	MyClassBase.cs		\
	Renderer.cs		\
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
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Splitter.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/Splitter.mo;\
		done
