## Process this file with automake to produce Makefile.in

all: UnitTest.exe gimptest.dll

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /pkg:mono-nunit

MYSOURCES = 	EventCollector.cs	\
		TestReportDialog.cs		\
		UnitTest.cs		\
		UnitTester.cs

EXTRA_DIST = $(MYSOURCES) UnitTest UnitTest.png

UnitTest.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) /target:winexe $(MYSOURCES) /resource:UnitTest.png -out:$@ $(ASSEMBLIES)

TESTSOURCES = 	TestBrush.cs 			\
		TestBrushList.cs		\
		TestChannel.cs			\
		TestChannelList.cs		\
		TestContext.cs			\
		TestDisplay.cs			\
		TestDrawable.cs			\
		TestFloatingSelection.cs	\
		TestGimp.cs			\
		TestGuide.cs			\
		TestHSV.cs			\
		TestImage.cs			\
		TestLayer.cs			\
		TestPalette.cs			\
		TestPaletteEntry.cs		\
		TestPaletteList.cs		\
		TestParasite.cs			\
		TestPath.cs			\
		TestPatternList.cs		\
		TestPixelFetcher.cs		\
		TestPixelRgn.cs			\
		TestPlugins.cs			\
		TestProcedure.cs		\
		TestRGB.cs			\
		TestRgnIterator.cs		\
		TestVersion.cs

gimptest.dll : $(TESTSOURCES) $(GIMPSHARPLIB)
	$(CSC) -target:library $(ASSEMBLIES) $(TESTSOURCES) -out:gimptest.dll

clean:
	rm -f UnitTest.exe gimptest.dll

install:
	$(INSTALL_DATA) UnitTest.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL_DATA) gimptest.dll $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) UnitTest $(DESTDIR)$(GIMPPLUGINDIR)
