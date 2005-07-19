all: Ministeck.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES =			\
	Coordinate.cs		\
	CornerShape.cs		\
	Ministeck.cs		\
	MinisteckPalette.cs	\
	OneByOneShape.cs	\
	Painter.cs		\
	Shape.cs		\
	ShapeDescription.cs	\
	ShapeSet.cs		\
	ThreeByOneShape.cs	\
	TwoByOneShape.cs	\
	TwoByTwoShape.cs

EXTRA_DIST = $(MYSOURCES) Ministeck Ministeck.png

Ministeck.exe : $(MYSOURCES) Ministeck.png
	$(CSC) $(MYSOURCES) /resource:Ministeck.png -o $@ $(ASSEMBLIES)

clean:
	rm -f Ministeck.exe

install:
	$(INSTALL_DATA) Ministeck.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Ministeck $(DESTDIR)/$(GIMPPLUGINDIR)