MCS = mcs

VERSION = 0.2

# Fill in GIMP version here.
# GIMPVERSION = 2.0
GIMPVERSION = 2.1

REFERENCES = -pkg:gtk-sharp

SOURCES = \
	Display.cs		\
	Drawable.cs		\
	GimpFontSelectWidget.cs	\
	GimpFrame.cs		\
	GimpTable.cs		\
	GuideCollection.cs	\
	Guide.cs		\
	Layer.cs		\
	Image.cs		\
	PixelRgn.cs		\
	Plugin.cs		\
	RandomSeed.cs		\
	RgnIterator.cs		\
	ScaleEntry.cs

EXTRADIST =		\
	AUTHORS		\
	COPYING		\
	INSTALL		\
	NEWS		\
	TODO		\
	Makefile	\
	ncp		\
	PicturePackage	\
	picture-package.xml	\
	gimp.c

all: ncp.exe PicturePackage.exe

ncp.exe: ncp.cs $(SOURCES) gimpwrapper.so
	$(MCS) -2 ncp.cs $(RESOURCES) -o $@ $(SOURCES) $(REFERENCES)

PicturePackage.exe: PicturePackage.cs  $(SOURCES) gimpwrapper.so
	$(MCS) -2 PicturePackage.cs $(RESOURCES) -o $@ $(SOURCES) $(REFERENCES)

gimp.o: gimp.c
	gcc `gimptool-$(GIMPVERSION) --cflags` -fPIC -c -o gimp.o gimp.c

gimpwrapper.so: gimp.o
	gcc -shared `gimptool-$(GIMPVERSION) --libs` -o gimpwrapper.so gimp.o

install: *.exe ncp PicturePackage gimpwrapper.so
	chmod +x ncp PicturePackage
	gimptool-$(GIMPVERSION) --install-bin ncp
	gimptool-$(GIMPVERSION) --install-bin PicturePackage
	chmod -x *.exe
	cp -f *.exe ~/.gimp-$(GIMPVERSION)/plug-ins/
	chmod -x gimpwrapper.so
	cp -f gimpwrapper.so ~/.gimp-$(GIMPVERSION)/plug-ins/
	cp -f picture-package.xml ~/.gimp-$(GIMPVERSION)/plug-ins/

dist: 
	rm -rf gimp-sharp-$(VERSION)
	mkdir gimp-sharp-$(VERSION)
	cp $(SOURCES) $(EXTRADIST) gimp-sharp-$(VERSION)
	tar cf gimp-sharp-$(VERSION).tar gimp-sharp-$(VERSION)
	gzip gimp-sharp-$(VERSION).tar
	rm -rf gimp-sharp-$(VERSION)