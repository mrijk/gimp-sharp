MCS = mcs

VERSION = 0.1

# Fill in GIMP version here.
# GIMPVERSION = 2.0
GIMPVERSION = 2.1

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

EXTRADIST =		\
	AUTHORS		\
	COPYING		\
	TODO		\
	Makefile	\
	ncp		\
	gimp.c

all: ncp.exe

ncp.exe: $(SOURCES) gimpwrapper.so
	$(MCS) -2 $(RESOURCES) -o $@ $(SOURCES) $(REFERENCES)

gimp.o: gimp.c
	gcc `gimptool-$(GIMPVERSION) --cflags` -fPIC -c -o gimp.o gimp.c

gimpwrapper.so: gimp.o
	gcc -shared `gimptool-$(GIMPVERSION) --libs` -o gimpwrapper.so gimp.o

install: ncp.exe ncp gimpwrapper.so
	gimptool-$(GIMPVERSION) --install-bin ncp
	chmod -x ncp.exe
	cp -f ncp.exe ~/.gimp-$(GIMPVERSION)/plug-ins/
	chmod -x gimpwrapper.so
	cp -f gimpwrapper.so ~/.gimp-$(GIMPVERSION)/plug-ins/

dist: 
	rm -rf gimp-sharp-$(VERSION)
	mkdir gimp-sharp-$(VERSION)
	cp $(SOURCES) $(EXTRADIST) gimp-sharp-$(VERSION)
	tar cf gimp-sharp-$(VERSION).tar gimp-sharp-$(VERSION)
	gzip gimp-sharp-$(VERSION).tar
	rm -rf gimp-sharp-$(VERSION)