## Process this file with automake to produce Makefile.in

LIBRARY_NAME=	gimp-sharp
LIBRARY=	$(LIBRARY_NAME).dll

ASSEMBLIES=	-r:Mono.Posix /pkg:gtk-sharp-2.0

MYSOURCES = 				\
	AspectPreview.cs 		\
	AssemblyInfo.cs			\
	BaseRenderer.cs			\
	BoolMatrix.cs			\
	Brush.cs			\
	BrushInfo.cs			\
	BrushList.cs			\
	Buffer.cs			\
	ByColorSelectTool.cs		\
	Channel.cs 			\
	ChannelList.cs 			\
	Context.cs			\
	Coordinate.cs			\
	CoordinateGenerator.cs		\
	CoordinateList.cs		\
	DataObject.cs			\
	DataObjectList.cs		\
	Dimensions.cs			\
	Display.cs 			\
	DoubleOffset.cs			\
	Drawable.cs			\
	DrawableList.cs			\
	DrawableComboBox.cs		\
	DrawablePreview.cs		\
	EllipseSelectTool.cs		\
	FileEntry.cs			\
	FilePlugin.cs			\
	FloatingSelection.cs		\
	FontHinting.cs			\
	FontSize.cs			\
	FreeSelectTool.cs		\
	FuzzySelectTool.cs		\
	GimpBaseEnums.cs		\
	GimpCheckButton.cs		\
	GimpColorButton.cs		\
	GimpComboBox.cs			\
	GimpDialog.cs			\
	GimpDialogWithPreview.cs	\
	GimpEntry.cs			\
	GimpEnums.cs			\
	GimpFontSelectWidget.cs		\
	GimpFrame.cs			\
	GimpPreview.cs			\
	GimpParam.cs			\
	GimpParamSet.cs			\
	GimpRadioButton.cs		\
	GimpSharpException.cs		\
	GimpSpinButton.cs		\
	GimpStock.cs			\
	GimpTable.cs			\
	GimpTextView.cs			\
	GimpTypes.cs			\
	GimpUnit.cs			\
	Gimp.cs				\
	Gradient.cs			\
	GradientList.cs			\
	GradientSegment.cs		\
	Grid.cs				\
	GuideCollection.cs		\
	Guide.cs			\
	HorizontalGuide.cs		\
	HSV.cs				\
	ImageComboBox.cs		\
	IntComboBox.cs			\
	IntCoordinate.cs		\
	IntStore.cs			\
	Layer.cs			\
	LayerList.cs			\
	Image.cs			\
	ImageList.cs			\
	IVariable.cs			\
	Mask.cs				\
	Message.cs			\
	Offset.cs			\
	Palette.cs			\
	PaletteEntry.cs			\
	PaletteList.cs			\
	ParamDef.cs			\
	ParamDefList.cs			\
	Parasite.cs			\
	ParasiteList.cs			\
	Path.cs				\
	Pattern.cs			\
	PatternInfo.cs			\
	PatternList.cs			\
	PersistentStorage.cs		\
	Pixel.cs			\
	PixelFetcher.cs			\
	PixelRgn.cs			\
	Plugin.cs			\
	PreviewArea.cs			\
	Procedure.cs			\
	ProcedureSet.cs			\
	ProceduralDb.cs			\
	Progress.cs			\
	PythonPlugin.cs			\
	RGB.cs				\
	RandomCoordinateGenerator.cs	\
	RandomSeed.cs			\
	ReadPixelIterator.cs		\
	Rectangle.cs			\
	RectangleSelectTool.cs		\
	RegionIterator.cs		\
	Resolution.cs			\
	RgnIterator.cs			\
	SaveAttribute.cs		\
	SaveAttributeSet.cs		\
	ScaleEntry.cs			\
	ScrolledPreview.cs		\
	Selection.cs			\
	Spacing.cs			\
	Stroke.cs			\
	Tattoo.cs			\
	TextLayer.cs			\
	Tile.cs				\
	Util.cs				\
	Variable.cs			\
	VariableSet.cs			\
	Vectors.cs			\
	Vector2.cs			\
	Vector3.cs			\
	Version.cs			\
	VerticalGuide.cs		\
	XmlNodeListExtensions.cs	\
	ZoomPreview.cs

EXTRA_DIST = $(MYSOURCES) $(LIBRARY).config

all: $(LIBRARY)

$(LIBRARY): $(MYSOURCES)
	$(CSC) $(CSC_FLAGS) -unsafe+ -target:library $(ASSEMBLIES) $(MYSOURCES) -out:$(LIBRARY)

clean:
	rm -f gimp-sharp.dll

install:
	$(INSTALL_DATA) $(LIBRARY) $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL_DATA) $(LIBRARY).config $(DESTDIR)$(GIMPPLUGINDIR)
