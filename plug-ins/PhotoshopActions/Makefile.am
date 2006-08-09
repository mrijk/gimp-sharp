## Process this file with automake to produce Makefile.in

all: PhotoshopActions.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 				\
	Abbreviations.cs		\
	Action.cs			\
	ActionEvent.cs			\
	ActionParser.cs			\
	ActionSet.cs			\
	AddEvent.cs			\
	AddAdjustmentLayerEvent.cs	\
	AddGuideEvent.cs		\
	AddLayerEvent.cs		\
	AddMaskEvent.cs			\
	AddNoiseEvent.cs		\
	AddTextLayerEvent.cs		\
	AddToEvent.cs			\
	AliasParameter.cs		\
	AlignEvent.cs			\
	AverageEvent.cs			\
	BoolParameter.cs		\
	BorderEvent.cs			\
	BrightnessEvent.cs		\
	CanvasSizeEvent.cs		\
	ChannelMixerEvent.cs		\
	ChromeEvent.cs			\
	ClassType.cs			\
	ClearEvent.cs			\
	ClearQuickMaskEvent.cs		\
	CloseEvent.cs			\
	CloudsEvent.cs			\
	ColorBalanceEvent.cs		\
	ContractEvent.cs		\
	ConvertModeEvent.cs		\
	CopyEvent.cs			\
	CopyMergedEvent.cs		\
	CopyToLayerEvent.cs		\
	CropEvent.cs			\
	CurvesEvent.cs			\
	CutEvent.cs			\
	CutToLayerEvent.cs		\
	DebugOutput.cs			\
	DeleteChannelEvent.cs		\
	DeleteEvent.cs			\
	DeleteGuideEvent.cs		\
	DeleteLayerEvent.cs		\
	DesaturateEvent.cs		\
	DiffuseEvent.cs			\
	DiffuseGlowEvent.cs		\
	DoubleParameter.cs		\
	DifferenceCloudsEvent.cs	\
	DuplicateChannelEvent.cs	\
	DuplicateEvent.cs		\
	DuplicateDocumentEvent.cs	\
	DuplicateLayerEvent.cs		\
	DustAndScratchesEvent.cs	\
	EmbossEvent.cs			\
	EnmrType.cs			\
	EnumParameter.cs		\
	EqualizeEvent.cs		\
	EventMap.cs			\
	ExchangeEvent.cs		\
	ExpandEvent.cs			\
	FacetEvent.cs			\
	FeatherEvent.cs			\
	FadeEvent.cs			\
	FillEvent.cs			\
	FilmGrainEvent.cs		\
	FilterGalleryEvent.cs		\
	FindEdgesEvent.cs		\
	FlattenImageEvent.cs		\
	FlipEvent.cs			\
	FragmentEvent.cs		\
	GaussianBlurEvent.cs		\
	GlassEvent.cs			\
	GlowingEdgesEvent.cs		\
	GradientClassEvent.cs		\
	GradientMapEvent.cs		\
	GrainEvent.cs			\
	GrowEvent.cs			\
	HideChannelEvent.cs		\
	HideEvent.cs			\
	HideLayerEvent.cs		\
	HighPassEvent.cs		\
	HueSaturationEvent.cs		\
	IExecutable.cs			\
	ImageSizeEvent.cs		\
	IndexType.cs			\
	InverseEvent.cs			\
	InvertEvent.cs			\
	LensFlareEvent.cs		\
	LevelsEvent.cs			\
	LinkEvent.cs			\
	LinkSelectedLayersEvent.cs	\
	ListParameter.cs		\
	LongParameter.cs		\
	MakeContentLayerEvent.cs	\
	MakeEvent.cs			\
	MakeGroupEvent.cs		\
	MakePathEvent.cs		\
	MakeSnapshotEvent.cs		\
	MaximumEvent.cs			\
	MedianEvent.cs			\
	MergeLayersEvent.cs		\
	MergeVisibleEvent.cs		\
	MezzotintEvent.cs		\
	MinimumEvent.cs			\
	MosaicEvent.cs			\
	MotionBlurEvent.cs		\
	MoveEvent.cs			\
	MoveLayerByIndexEvent.cs	\
	MoveLayerEvent.cs		\
	MoveOffsetEvent.cs		\
	NameType.cs			\
	NewDocumentEvent.cs		\
	ObArParameter.cs		\
	ObjcParameter.cs		\
	OffsetEvent.cs			\
	OpenEvent.cs			\
	PaintDaubsEvent.cs		\
	Parameter.cs			\
	ParameterAttribute.cs		\
	ParameterSet.cs			\
	PasteEvent.cs			\
	PathParameter.cs		\
	PhotocopyEvent.cs		\
	PhotoshopActions.cs		\
	PlasterEvent.cs			\
	PlayEvent.cs			\
	PointillizeEvent.cs		\
	PolarEvent.cs			\
	PosterizationEvent.cs		\
	PropertyType.cs			\
	PurgeEvent.cs			\
	RadialBlurEvent.cs		\
	RasterizeTypeLayerEvent.cs	\
	RawDataParameter.cs		\
	ReferenceParameter.cs		\
	ReferenceType.cs		\
	ReleType.cs			\
	RemoveWhiteMatteEvent.cs	\
	ReplaceColorEvent.cs		\
	RevealAllEvent.cs		\
	RippleEvent.cs			\
	RotateEvent.cs			\
	ResetEvent.cs			\
	SaveEvent.cs			\
	SelectAllEvent.cs		\
	SelectAllLayersEvent.cs		\
	SelectDocumentEvent.cs		\
	SelectEllipseEvent.cs		\
	SelectEvent.cs			\
	SelectionEvent.cs		\
	SelectChannelEvent.cs		\
	SelectLayerByNameEvent.cs	\
	SelectLayerEvent.cs		\
	SelectNoneEvent.cs		\
	SelectPolygonEvent.cs		\
	SelectRectangleEvent.cs		\
	SetBackgroundColorEvent.cs	\
	SetChannelPropertyEvent.cs	\
	SetColorEvent.cs		\
	SetEvent.cs			\
	SetForegroundColorEvent.cs	\
	SetLayerEffectsEvent.cs		\
	SetLayerPropertyEvent.cs	\
	SetQuickMaskEvent.cs		\
	SharpenEvent.cs			\
	SharpenMoreEvent.cs		\
	ShearEvent.cs			\
	ShowChannelEvent.cs		\
	ShowEvent.cs			\
	ShowLayerEvent.cs		\
	SmartBlurEvent.cs		\
	SmoothnessEvent.cs		\
	SolarizeEvent.cs		\
	SpatterEvent.cs			\
	SpherizeEvent.cs		\
	StainedGlassEvent.cs		\
	StopEvent.cs			\
	StrokeEvent.cs			\
	SubtractFromEvent.cs		\
	SurfaceBlurEvent.cs		\
	TextParameter.cs		\
	TexturizerEvent.cs		\
	ThresholdClassEvent.cs		\
	TilesEvent.cs			\
	TransformEvent.cs		\
	TransformLayerEvent.cs		\
	TransformSelectionEvent.cs	\
	TypeParameter.cs		\
	TwirlEvent.cs			\
	UnimplementedEvent.cs		\
	UnlinkEvent.cs			\
	UnsharpMaskEvent.cs		\
	WaterPaperEvent.cs		\
	WaveEvent.cs			\
	WindEvent.cs

EXTRA_DIST = $(MYSOURCES) PhotoshopActions PhotoshopActions.png

PhotoshopActions.exe : $(MYSOURCES) PhotoshopActions.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:PhotoshopActions.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f PhotoshopActions.exe

install:
	$(INSTALL_DATA) PhotoshopActions.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PhotoshopActions $(DESTDIR)$(GIMPPLUGINDIR)
