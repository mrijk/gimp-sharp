## Process this file with automake to produce Makefile.in

all: SliceTool.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

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

EXTRA_DIST = $(MYSOURCES) 	\
	SliceTool 		\
	SliceTool.png 		\
	blank.png 		\
	stock-arrow.png 	\
	cursor-eraser.png	\
	cursor-select.png	\
	cursor-slice.png	\
	cursor-table.png	\
	javascript.html

SliceTool.exe : $(MYSOURCES) SliceTool.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:SliceTool.png	\
			  /resource:blank.png		\
			  /resource:cursor-eraser.png	\
			  /resource:cursor-select.png	\
			  /resource:cursor-slice.png	\
			  /resource:cursor-table.png	\
			  /resource:stock-arrow.png	\
			  /resource:javascript.html	\
		-out:$@ $(ASSEMBLIES)

clean:
	rm -f SliceTool.exe

install:
	$(INSTALL_DATA) SliceTool.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) SliceTool $(DESTDIR)/$(GIMPPLUGINDIR)
