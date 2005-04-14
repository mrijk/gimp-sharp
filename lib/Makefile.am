LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	/pkg:gtk-sharp

all: $(LIBRARY)

SOURCES = AspectPreview.cs 	\
	Channel.cs 		\
	Display.cs 		\
	Drawable.cs		\
	DrawablePreview.cs	\
	FileEntry.cs		\
	FilePlugin.cs		\
	FloatingSelection.cs	\
	GimpBaseEnums.cs	\
	GimpColorButton.cs	\
	GimpDialog.cs		\
	GimpEnums.cs		\
	GimpFontSelectWidget.cs	\
	GimpFrame.cs		\
	GimpPreview.cs		\
	GimpStock.cs		\
	GimpTable.cs		\
	GimpTypes.cs		\
	Gimp.cs			\
	GuideCollection.cs	\
	Guide.cs		\
	ImageComboBox.cs	\
	IntComboBox.cs		\
	IntStore.cs		\
	Layer.cs		\
	Image.cs		\
	ImageList.cs		\
	Mask.cs			\
	Message.cs		\
	Palette.cs		\
	PaletteEntry.cs		\
	PaletteEnumerator.cs	\
	PaletteList.cs		\
	ParamDef.cs		\
	ParamDefList.cs		\
	Parasite.cs		\
	PersistentStorage.cs	\
	PixelFetcher.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	PreviewArea.cs		\
	Progress.cs		\
	RGB.cs			\
	RandomSeed.cs		\
	RgnIterator.cs		\
	SaveAttribute.cs	\
	ScaleEntry.cs		\
	ScrolledPreview.cs	\
	Selection.cs		\
	Tattoo.cs		\
	Tile.cs			\
	VoidObjectSignal.cs

EXTRA_DIST = $(SOURCES)

$(LIBRARY): $(SOURCES)
	$(CSC) --target library $(ASSEMBLIES) $(SOURCES) -o $(LIBRARY)

clean:
	rm -f gimp-sharp.dll

install:
	$(INSTALL_DATA) $(LIBRARY) $(DESTDIR)/$(GIMPPLUGINDIR)
