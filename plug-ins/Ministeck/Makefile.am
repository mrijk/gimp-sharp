all: Ministeck.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	Ministeck.cs		\
	MS-Coordinate.cs	\
	MS-CornerShape.cs	\
	MS-OneByOneShape.cs	\
	MS-Shape.cs		\
	MS-ShapeDescription.cs	\
	MS-ShapeSet.cs		\
	MS-ThreeByOneShape.cs	\
	MS-TwoByOneShape.cs	\
	MS-TwoByTwoShape.cs


Ministeck.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	install -m='u=rw,g=r,o=r' Ministeck.exe /usr/local/lib/gimp/2.0/plug-ins
	install Ministeck /usr/local/lib/gimp/2.0/plug-ins