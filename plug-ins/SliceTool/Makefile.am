all: SliceTool.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES =			\
	HorizontalSlice.cs	\
	Preview.cs		\
	PreviewRenderer.cs	\
	Rectangle.cs		\
	RectangleSet.cs		\
	Slice.cs		\
	SliceSet.cs		\
	SliceTool.cs		\
	VerticalSlize.cs

EXTRA_DIST = $(SOURCES) SliceTool SliceTool.png stock-arrow.png

SliceTool.exe : $(SOURCES) SliceTool.png
	$(CSC) $(SOURCES) /resource:SliceTool.png /resource:stock-arrow.png -o $@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) SliceTool.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) SliceTool $(DESTDIR)/$(GIMPPLUGINDIR)
