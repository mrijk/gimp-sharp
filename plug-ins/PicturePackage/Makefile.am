all: PicturePackage.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	Painter.cs		\
	PicturePackage.cs	\
	Layout.cs		\
	LayoutSet.cs		\
	Preview.cs		\
	Rectangle.cs		\
	RectangleSet.cs		\
	Renderer.cs

EXTRA_DIST = $(SOURCES) PicturePackage picture-package.xml

PicturePackage.exe : $(SOURCES)
	$(CSC) $(SOURCES) /resource:picture-package.xml -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PicturePackage.exe $(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(GIMPPLUGINDIR)

