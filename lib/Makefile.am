## Process this file with automake to produce Makefile.in

LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	-r:Mono.Posix /pkg:gtk-sharp-2.0

MYSOURCES = 			\
	AspectPreview.cs 	\
	AssemblyInfo.cs		\
	Brush.cs		\
	BrushList.cs		\
	Channel.cs 		\
	ChannelList.cs 		\
	Context.cs		\
	Coordinate.cs		\
	CoordinateList.cs	\
	Display.cs 		\
	Drawable.cs		\
	DrawableComboBox.cs	\
	DrawablePreview.cs	\
	EllipseSelectTool.cs	\
	FileEntry.cs		\
	FilePlugin.cs		\
	FloatingSelection.cs	\
	FreeSelectTool.cs	\
	GimpBaseEnums.cs	\
	GimpColorButton.cs	\
	GimpDialog.cs		\
	GimpEnums.cs		\
	GimpFontSelectWidget.cs	\
	GimpFrame.cs		\
	GimpPreview.cs		\
	GimpSharpException.cs	\
	GimpStock.cs		\
	GimpTable.cs		\
	GimpTypes.cs		\
	Gimp.cs			\
	Gradient.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	HorizontalGuide.cs	\
	HSV.cs			\
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
	Pixel.cs		\
	PixelFetcher.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	PluginWithPreview.cs	\
	PreviewArea.cs		\
	Procedure.cs		\
	ProcedureSet.cs		\
	Progress.cs		\
	RGB.cs			\
	RandomSeed.cs		\
	ReadPixelIterator.cs	\
	Rectangle.cs		\
	RectangleSelectTool.cs	\
	RegionIterator.cs	\
	RgnIterator.cs		\
	SaveAttribute.cs	\
	SaveAttributeSet.cs	\
	ScaleEntry.cs		\
	ScrolledPreview.cs	\
	Selection.cs		\
	Stroke.cs		\
	Tattoo.cs		\
	Tile.cs			\
	Vectors.cs		\
	Version.cs		\
	VerticalGuide.cs	\
	ZoomPreview.cs

EXTRA_DIST = $(MYSOURCES) $(LIBRARY).config

all: $(LIBRARY)

$(LIBRARY): $(MYSOURCES)
	$(CSC) -target:library $(ASSEMBLIES) $(MYSOURCES) -out:$(LIBRARY)

clean:
	rm -f gimp-sharp.dll

install:
	$(INSTALL_DATA) $(LIBRARY) $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL_DATA) $(LIBRARY).config $(DESTDIR)$(GIMPPLUGINDIR)
