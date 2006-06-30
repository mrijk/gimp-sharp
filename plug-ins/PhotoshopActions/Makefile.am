## Process this file with automake to produce Makefile.in

all: PhotoshopActions.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 				\
	Action.cs			\
	ActionEvent.cs			\
	ActionParser.cs			\
	ActionSet.cs			\
	AddEvent.cs			\
	AddAdjustmentLayerEvent.cs	\
	AddGuideEvent.cs		\
	AddLayerEvent.cs		\
	AddNoiseEvent.cs		\
	AddTextLayerEvent.cs		\
	AddToEvent.cs			\
	BoolParameter.cs		\
	BorderEvent.cs			\
	BrightnessEvent.cs		\
	CanvasSizeEvent.cs		\
	ChannelMixerEvent.cs		\
	ClassType.cs			\
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
	DebugOutput.cs			\
	DeleteChannelEvent.cs		\
	DeleteEvent.cs			\
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
	FindEdgesEvent.cs		\
	FlattenImageEvent.cs		\
	FlipEvent.cs			\
	GaussianBlurEvent.cs		\
	GlowingEdgesEvent.cs		\
	GradientClassEvent.cs		\
	GradientMapEvent.cs		\
	GrainEvent.cs			\
	GrowEvent.cs			\
	HideChannelEvent.cs		\
	HideEvent.cs			\
	HideLayerEvent.cs		\
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
	MakeEvent.cs			\
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
	NameType.cs			\
	NewDocumentEvent.cs		\
	ObArParameter.cs		\
	ObjcParameter.cs		\
	OffsetEvent.cs			\
	OpenEvent.cs			\
	Parameter.cs			\
	ParameterAttribute.cs		\
	ParameterSet.cs			\
	PasteEvent.cs			\
	PathParameter.cs		\
	PhotocopyEvent.cs		\
	PhotoshopActions.cs		\
	PlayEvent.cs			\
	PointillizeEvent.cs		\
	PolarEvent.cs			\
	PosterizationEvent.cs		\
	PropertyType.cs			\
	RadialBlurEvent.cs		\
	ReferenceParameter.cs		\
	ReferenceType.cs		\
	ReleType.cs			\
	RemoveWhiteMatteEvent.cs	\
	ReplaceColorEvent.cs		\
	RotateEvent.cs			\
	ResetEvent.cs			\
	SelectAllLayersEvent.cs		\
	SelectEvent.cs			\
	SelectionEvent.cs		\
	SelectChannelEvent.cs		\
	SelectLayerEvent.cs		\
	SetBackgroundColorEvent.cs	\
	SetChannelPropertyEvent.cs	\
	SetColorEvent.cs		\
	SetEvent.cs			\
	SetForegroundColorEvent.cs	\
	SetLayerPropertyEvent.cs	\
	SharpenEvent.cs			\
	SharpenMoreEvent.cs		\
	ShowChannelEvent.cs		\
	ShowEvent.cs			\
	ShowLayerEvent.cs		\
	SmartBlurEvent.cs		\
	SmoothnessEvent.cs		\
	SpherizeEvent.cs		\
	StainedGlassEvent.cs		\
	StopEvent.cs			\
	StrokeEvent.cs			\
	SubtractFromEvent.cs		\
	SurfaceBlurEvent.cs		\
	TextParameter.cs		\
	ThresholdClassEvent.cs		\
	TilesEvent.cs			\
	TransformEvent.cs		\
	TypeParameter.cs		\
	TwirlEvent.cs			\
	UnimplementedEvent.cs		\
	UnsharpMaskEvent.cs		\
	WaterPaperEvent.cs

EXTRA_DIST = $(MYSOURCES) PhotoshopActions PhotoshopActions.png

PhotoshopActions.exe : $(MYSOURCES) PhotoshopActions.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:PhotoshopActions.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f PhotoshopActions.exe

install:
	$(INSTALL_DATA) PhotoshopActions.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PhotoshopActions $(DESTDIR)$(GIMPPLUGINDIR)
