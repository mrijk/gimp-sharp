## Process this file with automake to produce Makefile.in

LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	/pkg:gtk-sharp-2.0

MYSOURCES = AspectPreview.cs 	\
	Brush.cs		\
	Channel.cs 		\
	ChannelList.cs 		\
	Context.cs		\
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
	Gradient.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	HorizontalGuide.cs	\
	ImageComboBox.cs	\
	IntComboBox.cs		\
	IntStore.cs		\
	Layer.cs		\
	LayerList.cs		\
	Image.cs		\
	ImageList.cs		\
	Mask.cs			\
	Message.cs		\
	Palette.cs		\
	PaletteEntry.cs		\
	PaletteList.cs		\
	ParamDef.cs		\
	ParamDefList.cs		\
	Parasite.cs		\
	Path.cs			\
	Pattern.cs		\
	PatternList.cs		\
	PersistentStorage.cs	\
	PixelFetcher.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	PreviewArea.cs		\
	Procedure.cs		\
	ProcedureSet.cs		\
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
	Version.cs		\
	VerticalGuide.cs

EXTRA_DIST = $(MYSOURCES) $(LIBRARY).config

all: $(LIBRARY)

$(LIBRARY): $(MYSOURCES)
	$(CSC) -target:library $(ASSEMBLIES) $(MYSOURCES) -out:$(LIBRARY)

clean:
	rm -f gimp-sharp.dll

install:
	$(INSTALL_DATA) $(LIBRARY) $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL_DATA) $(LIBRARY).config $(DESTDIR)/$(GIMPPLUGINDIR)
