all: Ministeck.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	Coordinate.cs		\
	CornerShape.cs		\
	Ministeck.cs		\
	OneByOneShape.cs	\
	Painter.cs		\
	Shape.cs		\
	ShapeDescription.cs	\
	ShapeSet.cs		\
	ThreeByOneShape.cs	\
	TwoByOneShape.cs	\
	TwoByTwoShape.cs

EXTRA_DIST = $(SOURCES)

Ministeck.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) Ministeck.exe $(GIMPPLUGINDIR)
	$(INSTALL) Ministeck $(GIMPPLUGINDIR)