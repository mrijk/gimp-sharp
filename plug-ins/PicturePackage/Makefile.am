all: PicturePackage.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	PicturePackage.cs	\
	PP-Layout.cs		\
	PP-LayoutSet.cs		\
	PP-preview.cs		\
	PP-Rectangle.cs		\
	PP-RectangleSet.cs

PicturePackage.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	install -m='u=rw,g=r,o=r' PicturePackage.exe /usr/local/lib/gimp/2.0/plug-ins