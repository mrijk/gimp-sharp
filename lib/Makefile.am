LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	/pkg:gtk-sharp

all: $(LIBRARY)

SOURCES = 			\
	Display.cs		\
	Drawable.cs		\
	GimpColorButton.cs	\
	GimpEnums.cs		\
	GimpFontSelectWidget.cs	\
	GimpFrame.cs		\
	GimpTable.cs		\
	GimpTypes.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	Layer.cs		\
	Image.cs		\
	Palette.cs		\
	ParamDef.cs		\
	PixelFetcher.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	RGB.cs			\
	RandomSeed.cs		\
	RgnIterator.cs		\
	SaveAttribute.cs	\
	ScaleEntry.cs

EXTRA_DIST = $(SOURCES)

$(LIBRARY): $(SOURCES)
	$(CSC) --target library $(ASSEMBLIES) $(SOURCES) -o $(LIBRARY)