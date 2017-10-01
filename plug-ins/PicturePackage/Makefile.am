## Process this file with automake to produce Makefile.in

all: PicturePackage.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

GTKSHARPLIB = `pkg-config --libs gtk-sharp-2.0`

ASSEMBLIES = -r:$(GIMPSHARPLIB) $(GTKSHARPLIB) /target:winexe

MYSOURCES =				\
	Dialog.cs			\
	DirImageProviderFactory.cs	\
	DocumentFrame.cs		\
	Enums.cs			\
	FileImageProvider.cs		\
	FileImageProviderFactory.cs	\
	FrontImageProvider.cs		\
	FrontImageProviderFactory.cs	\
	ImageProvider.cs		\
	ImageRenderer.cs		\
	LabelFrame.cs			\
	Layout.cs			\
	LayoutSet.cs			\
	PageSize.cs			\
	PageSizeSet.cs			\
	ParentRenderer.cs		\
	PicturePackage.cs		\
	PicturePackageFrame.cs		\
	Preview.cs			\
	PreviewRenderer.cs		\
	ProviderFactory.cs		\
	Rectangle.cs			\
	RectangleSet.cs			\
	Renderer.cs			\
	SourceFrame.cs

EXTRA_DIST = $(MYSOURCES) PicturePackage picture-package.xml PicturePackage.png
SUBDIRS = po

CLEANFILES = PicturePackage.exe

PicturePackage.exe : $(MYSOURCES) picture-package.xml PicturePackage.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:picture-package.xml /resource:PicturePackage.png -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PicturePackage.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(DESTDIR)$(GIMPPLUGINDIR)
	@for lang in `ls po | grep .mo`;\
		do localedir=`echo $$lang | sed -e's/.mo//'`;\
		echo "Copying po/$$lang to $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/PicturePackage.mo";\
		$(INSTALL_DATA) -d $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/;\
		$(INSTALL_DATA) po/$$lang $(DESTDIR)$(GIMPLOCALE)/$$localedir/LC_MESSAGES/PicturePackage.mo;\
		done
