MCS = mcs

REFERENCES = -pkg:gtk-sharp

SOURCES = \
	Display.cs		\
	Drawable.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	Layer.cs		\
	Image.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	RgnIterator.cs		\
	TestPlugin.cs

all: gimp-sharp.exe

gimp-sharp.exe: $(SOURCES) gimpwrapper.so
	$(MCS) -2 $(RESOURCES) -o $@ $(SOURCES) $(REFERENCES)

gimp.o: gimp.c
	gcc `gimptool-2.1 --cflags` -fPIC -c -o gimp.o gimp.c

gimpwrapper.so: gimp.o
	gcc -shared -o gimpwrapper.so gimp.o

install: gimp-sharp.exe
	cp -f gimp-sharp.exe ~/.gimp-2.1/plug-ins/exe/