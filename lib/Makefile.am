LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	/pkg:gtk-sharp

all: $(LIBRARY)

SOURCES = 			\
	AspectPreview.cs	\
	Channel.cs		\
	Display.cs		\
	Drawable.cs		\
	DrawablePreview.cs	\
	FileEntry.cs		\
	FloatingSelection.cs	\
	GimpBaseEnums.cs	\
	GimpColorButton.cs	\
	GimpDialog.cs		\
	GimpEnums.cs		\
	GimpFontSelectWidget.cs	\
	GimpFrame.cs		\
	GimpPreview.cs		\
	GimpTable.cs		\
	GimpTypes.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	Layer.cs		\
	Image.cs		\
	Mask.cs			\
	Message.cs		\
	Palette.cs		\
	PaletteEntry.cs		\
	PaletteEnumerator.cs	\
	ParamDef.cs		\
	ParamDefList.cs		\
	PixelFetcher.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	Progress.cs		\
	RGB.cs			\
	RandomSeed.cs		\
	RgnIterator.cs		\
	SaveAttribute.cs	\
	ScaleEntry.cs		\
	ScrolledPreview.cs	\
	Selection.cs		\
	VoidObjectSignal.cs

EXTRA_DIST = $(SOURCES)

$(LIBRARY): $(SOURCES)
	$(CSC) --target library $(ASSEMBLIES) $(SOURCES) -o $(LIBRARY)

install:
	$(INSTALL_DATA) $(LIBRARY) $(GIMPPLUGINDIR)
