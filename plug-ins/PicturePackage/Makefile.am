all: PicturePackage.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =				\
	DirImageProviderFactory.cs	\
	DocumentFrame.cs		\
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

EXTRA_DIST = $(SOURCES) PicturePackage picture-package.xml

PicturePackage.exe : $(SOURCES) picture-package.xml
	$(CSC) $(SOURCES) /resource:picture-package.xml -o $@ $(ASSEMBLIES)

clean:
	rm -f PicturePackage.exe

install:
	$(INSTALL_DATA) PicturePackage.exe $(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(GIMPPLUGINDIR)

