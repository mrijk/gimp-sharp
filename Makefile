MCS = mcs

VERSION = 0.2

# Fill in GIMP version here.
# GIMPVERSION = 2.0

GIMPTOOL = gimptool
GIMPVERSION = 2.2

PLUGINDIR = ~/.gimp-$(GIMPVERSION)/plug-ins/

REFERENCES = 			\
	-pkg:gtk-sharp

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
	SaveAttribute.cs	\
	ScaleEntry.cs

NCP_SOURCES = \
	ncp.cs

MINISTECK_SOURCES = \
	Ministeck.cs

PICTURE_PACKAGE_SOURCES = 	\
	PicturePackage.cs	\
	PP-Layout.cs		\
	PP-LayoutSet.cs		\
	PP-preview.cs		\
	PP-Rectangle.cs		\
	PP-RectangleSet.cs

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
	gimp.c			\
	$(NCP_SOURCES)		\
	$(PICTURE_PACKAGE_SOURCES)

all: gimp-sharp.dll ncp.exe PicturePackage.exe Ministeck.exe

gimp-sharp.dll: $(SOURCES)
	$(MCS) $(SOURCES) -t:library -o $@ $(REFERENCES)

ncp.exe: $(NCP_SOURCES) gimpwrapper.so
	$(MCS) -2 $(NCP_SOURCES) -o $@ $(REFERENCES) -r:gimp-sharp.dll

PicturePackage.exe: $(PICTURE_PACKAGE_SOURCES) gimpwrapper.so
	$(MCS) -2 $(PICTURE_PACKAGE_SOURCES) -o $@ $(REFERENCES) -r:gimp-sharp.dll

Ministeck.exe: $(MINISTECK_SOURCES) gimpwrapper.so
	$(MCS) -2 $(MINISTECK_SOURCES) -o $@ $(REFERENCES) -r:gimp-sharp.dll

gimp.o: gimp.c
	gcc `gimptool-$(GIMPVERSION) --cflags` -fPIC -c -o gimp.o gimp.c

gimpwrapper.so: gimp.o
	gcc -shared `gimptool-$(GIMPVERSION) --libs` -o gimpwrapper.so gimp.o

install: *.exe ncp PicturePackage gimpwrapper.so gimp-sharp.dll
	chmod +x ncp PicturePackage
	$(GIMPTOOL) --install-bin Ministeck
	$(GIMPTOOL) --install-bin ncp
	$(GIMPTOOL) --install-bin PicturePackage
	chmod -x *.exe gimpwrapper.so gimp-sharp.dll
	cp -f *.exe $(PLUGINDIR)
	cp -f gimpwrapper.so $(PLUGINDIR)
	cp -f gimp-sharp.dll $(PLUGINDIR)
	cp -f picture-package.xml $(PLUGINDIR)

dist: 
	rm -rf gimp-sharp-$(VERSION)
	mkdir gimp-sharp-$(VERSION)
	cp $(SOURCES) $(EXTRADIST) gimp-sharp-$(VERSION)
	tar cf gimp-sharp-$(VERSION).tar gimp-sharp-$(VERSION)
	gzip gimp-sharp-$(VERSION).tar
	rm -rf gimp-sharp-$(VERSION)