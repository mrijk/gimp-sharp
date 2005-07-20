## Process this file with automake to produce Makefile.in

all: SliceTool.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES =			\
	CreateFunc.cs		\
	CreateTableFunc.cs	\
	FileExistsDialog.cs	\
	Format.cs		\
	HorizontalSlice.cs	\
	JavaScriptProperty.cs	\
	MouseFunc.cs		\
	PreferencesDialog.cs	\
	Preview.cs		\
	PreviewRenderer.cs	\
	Property.cs		\
	PropertySet.cs		\
	Rectangle.cs		\
	RectangleSet.cs		\
	RemoveFunc.cs		\
	RolloverDialog.cs	\
	RolloverEntry.cs	\
	SelectFunc.cs		\
	Slice.cs		\
	SliceData.cs		\
	SliceSet.cs		\
	SliceTool.cs		\
	TableDialog.cs		\
	VerticalSlize.cs

EXTRA_DIST = $(MYSOURCES) SliceTool SliceTool.png blank.png stock-arrow.png \
	javascript.html

SliceTool.exe : $(MYSOURCES) SliceTool.png
	$(CSC) $(MYSOURCES) /resource:SliceTool.png	\
			  /resource:blank.png		\
			  /resource:stock-arrow.png	\
			  /resource:javascript.html	\
		-out:$@ $(ASSEMBLIES)

clean:
	rm -f SliceTool.exe

install:
	$(INSTALL_DATA) SliceTool.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) SliceTool $(DESTDIR)/$(GIMPPLUGINDIR)
