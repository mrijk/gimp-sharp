MCS = mcs

REFERENCES = -pkg:gtk-sharp

SOURCES = \
	Display.cs		\
	Drawable.cs		\
	GimpTable.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	Layer.cs		\
	Image.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	RandomSeed.cs		\
	RgnIterator.cs		\
	ScaleEntry.cs		\
	TestPlugin.cs

all: gimp-sharp.exe

gimp-sharp.exe: $(SOURCES) gimpwrapper.so
	$(MCS) -2 $(RESOURCES) -o $@ $(SOURCES) $(REFERENCES)

gimp.o: gimp.c
	gcc `gimptool-2.1 --cflags` -fPIC -c -o gimp.o gimp.c

gimpwrapper.so: gimp.o
	gcc -shared `gimptool-2.1 --libs` -o gimpwrapper.so gimp.o

install: gimp-sharp.exe
	cp -f gimp-sharp.exe ~/.gimp-2.1/plug-ins/exe/