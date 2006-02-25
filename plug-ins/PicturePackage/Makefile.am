## Process this file with automake to produce Makefile.in

all: PicturePackage.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES =				\
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

PicturePackage.exe : $(MYSOURCES) picture-package.xml PicturePackage.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:picture-package.xml /resource:PicturePackage.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f PicturePackage.exe

install:
	$(INSTALL_DATA) PicturePackage.exe $(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(GIMPPLUGINDIR)

