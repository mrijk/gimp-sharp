all: PicturePackage.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	DocumentFrame.cs	\
	ImageRenderer.cs	\
	LabelFrame.cs		\
	Layout.cs		\
	LayoutSet.cs		\
	PageSize.cs		\
	PageSizeSet.cs		\
	PicturePackage.cs	\
	PicturePackageFrame.cs	\
	Preview.cs		\
	PreviewRenderer.cs	\
	Rectangle.cs		\
	RectangleSet.cs		\
	Renderer.cs		\
	SourceFrame.cs

EXTRA_DIST = $(SOURCES) PicturePackage picture-package.xml

PicturePackage.exe : $(SOURCES)
	$(CSC) $(SOURCES) /resource:picture-package.xml -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PicturePackage.exe $(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(GIMPPLUGINDIR)

