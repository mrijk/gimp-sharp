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
	ClassType.cs			\
	CloudsEvent.cs			\
	ConvertModeEvent.cs		\
	CopyEvent.cs			\
	CopyToLayerEvent.cs		\
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
	EventMap.cs			\
	ExchangeEvent.cs		\
	FacetEvent.cs			\
	FeatherEvent.cs			\
	FillEvent.cs			\
	FindEdgesEvent.cs		\
	FlattenImageEvent.cs		\
	FlipEvent.cs			\
	GaussianBlurEvent.cs		\
	HideEvent.cs			\
	HueSaturationEvent.cs		\
	ImageSizeEvent.cs		\
	IndexType.cs			\
	InvertEvent.cs			\
	LevelsEvent.cs			\
	ListParameter.cs		\
	LongParameter.cs		\
	MakeEvent.cs			\
	MedianEvent.cs			\
	MergeLayersEvent.cs		\
	MergeVisibleEvent.cs		\
	MotionBlurEvent.cs		\
	MoveEvent.cs			\
	NewDocumentEvent.cs		\
	Objc.cs				\
	ObjcParameter.cs		\
	Parameter.cs			\
	ParameterAttribute.cs		\
	ParameterSet.cs			\
	PasteEvent.cs			\
	PhotocopyEvent.cs		\
	PhotoshopActions.cs		\
	PropertyType.cs			\
	RadialBlurEvent.cs		\
	ReferenceParameter.cs		\
	ReferenceType.cs		\
	ReplaceColorEvent.cs		\
	RotateEvent.cs			\
	ResetEvent.cs			\
	SelectEvent.cs			\
	SelectionEvent.cs		\
	SetBackgroundColorEvent.cs	\
	SetEvent.cs			\
	SetForegroundColorEvent.cs	\
	SetLayerEvent.cs		\
	SharpenEvent.cs			\
	SharpenMoreEvent.cs		\
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
