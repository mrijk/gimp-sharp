## Process this file with automake to produce Makefile.in

all: PhotoshopActions.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 				\
	Action.cs			\
	ActionEvent.cs			\
	ActionParser.cs			\
	ActionSet.cs			\
	AddGuideEvent.cs		\
	AddLayerEvent.cs		\
	AddNoiseEvent.cs		\
	BoolParameter.cs		\
	BorderEvent.cs			\
	BrightnessEvent.cs		\
	CanvasSizeEvent.cs		\
	ChannelMixerEvent.cs		\
	ClassType.cs			\
	CloseEvent.cs			\
	CloudsEvent.cs			\
	ContractEvent.cs		\
	ConvertModeEvent.cs		\
	CopyEvent.cs			\
	CopyMergedEvent.cs		\
	CopyToLayerEvent.cs		\
	CropEvent.cs			\
	CutEvent.cs			\
	DeleteEvent.cs			\
	DeleteLayerEvent.cs		\
	DesaturateEvent.cs		\
	DoubleParameter.cs		\
	DifferenceCloudsEvent.cs	\
	DuplicateEvent.cs		\
	DuplicateDocumentEvent.cs	\
	DuplicateLayerEvent.cs		\
	EmbossEvent.cs			\
	EnmrType.cs			\
	EnumParameter.cs		\
	EqualizeEvent.cs		\
	EventMap.cs			\
	ExchangeEvent.cs		\
	ExpandEvent.cs			\
	FacetEvent.cs			\
	FeatherEvent.cs			\
	FillEvent.cs			\
	FindEdgesEvent.cs		\
	FlattenImageEvent.cs		\
	FlipEvent.cs			\
	GaussianBlurEvent.cs		\
	GrowEvent.cs			\
	HideChannelEvent.cs		\
	HideEvent.cs			\
	HideLayerEvent.cs		\
	HueSaturationEvent.cs		\
	ImageSizeEvent.cs		\
	IndexType.cs			\
	InverseEvent.cs			\
	InvertEvent.cs			\
	LensFlareEvent.cs		\
	LevelsEvent.cs			\
	LinkEvent.cs			\
	ListParameter.cs		\
	LongParameter.cs		\
	MakeEvent.cs			\
	MaximumEvent.cs			\
	MedianEvent.cs			\
	MergeLayersEvent.cs		\
	MergeVisibleEvent.cs		\
	MinimumEvent.cs			\
	MosaicEvent.cs			\
	MotionBlurEvent.cs		\
	MoveEvent.cs			\
	NameType.cs			\
	NewDocumentEvent.cs		\
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
	PosterizationEvent.cs		\
	PropertyType.cs			\
	RadialBlurEvent.cs		\
	ReferenceParameter.cs		\
	ReferenceType.cs		\
	ReleType.cs			\
	ReplaceColorEvent.cs		\
	RotateEvent.cs			\
	ResetEvent.cs			\
	SelectAllLayersEvent.cs		\
	SelectEvent.cs			\
	SelectionEvent.cs		\
	SelectChannelEvent.cs		\
	SelectLayerEvent.cs		\
	SetBackgroundColorEvent.cs	\
	SetColorEvent.cs		\
	SetEvent.cs			\
	SetForegroundColorEvent.cs	\
	SetLayerPropertyEvent.cs	\
	SharpenEvent.cs			\
	SharpenMoreEvent.cs		\
	ShowChannelEvent.cs		\
	ShowEvent.cs			\
	ShowLayerEvent.cs		\
	SmoothnessEvent.cs		\
	StopEvent.cs			\
	TextParameter.cs		\
	TypeParameter.cs		\
	TwirlEvent.cs			\
	UnimplementedEvent.cs		\
	UnsharpMaskEvent.cs

EXTRA_DIST = $(MYSOURCES) PhotoshopActions PhotoshopActions.png

PhotoshopActions.exe : $(MYSOURCES) PhotoshopActions.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:PhotoshopActions.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f PhotoshopActions.exe

install:
	$(INSTALL_DATA) PhotoshopActions.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PhotoshopActions $(DESTDIR)$(GIMPPLUGINDIR)
