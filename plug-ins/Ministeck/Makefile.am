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

EXTRA_DIST = $(SOURCES)

Ministeck.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Ministeck.exe $(GIMPPLUGINDIR)
	$(INSTALL) Ministeck $(GIMPPLUGINDIR)