all: PicturePackage.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	PicturePackage.cs	\
	PP-Layout.cs		\
	PP-LayoutSet.cs		\
	PP-preview.cs		\
	PP-Rectangle.cs		\
	PP-RectangleSet.cs

EXTRA_DIST = $(SOURCES)

PicturePackage.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) PicturePackage.exe $(GIMPPLUGINDIR)
	$(INSTALL) PicturePackage $(GIMPPLUGINDIR)

