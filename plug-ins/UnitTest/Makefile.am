## Process this file with automake to produce Makefile.in

all: UnitTest.exe gimptest.dll

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /pkg:mono-nunit

MYSOURCES = 	EventCollector.cs		\
		TestReportDialog.cs		\
		UnitTest.cs			\
		UnitTester.cs

TESTSOURCES = 	TestBoolMatrix.cs		\
		TestBrush.cs 			\
		TestBrushList.cs		\
		TestChannel.cs			\
		TestChannelList.cs		\
		TestContext.cs			\
		TestCoordinate.cs		\
		TestCoordinateList.cs		\
		TestDimensions.cs		\
		TestDisplay.cs			\
		TestDrawable.cs			\
		TestFloatingSelection.cs	\
		TestGimpUnit.cs			\
		TestGimp.cs			\
		TestGradientList.cs		\
		TestGrid.cs			\
		TestGuide.cs			\
		TestHSV.cs			\
		TestImage.cs			\
		TestImageList.cs		\
		TestIntCoordinate.cs		\
		TestLayer.cs			\
		TestMask.cs			\
		TestOffset.cs			\
		TestPalette.cs			\
		TestPaletteEntry.cs		\
		TestPaletteList.cs		\
		TestParasite.cs			\
		TestPath.cs			\
		TestPattern.cs			\
		TestPatternList.cs		\
		TestPixel.cs			\
		TestPixelFetcher.cs		\
		TestPixelRgn.cs			\
		TestProceduralDb.cs		\
		TestProcedure.cs		\
		TestRcQuery.cs			\
		TestRectangle.cs		\
		TestRegionIterator.cs		\
		TestRgnIterator.cs		\
		TestRGB.cs			\
		TestSelection.cs		\
		TestStroke.cs			\
		TestTextLayer.cs		\
		TestUndo.cs			\
		TestVectors.cs			\
		TestVector2.cs			\
		TestVector3.cs			\
		TestVersion.cs

#		TestPlugins.cs

EXTRA_DIST = $(MYSOURCES) $(TESTSOURCES) UnitTest UnitTest.png

UnitTest.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) /target:winexe $(MYSOURCES) \
	/resource:UnitTest.png -out:$@ $(ASSEMBLIES)


gimptest.dll : $(TESTSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) -target:library $(ASSEMBLIES) $(TESTSOURCES) \
	-out:gimptest.dll

clean:
	rm -f UnitTest.exe gimptest.dll

install:
	$(INSTALL_DATA) UnitTest.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL_DATA) gimptest.dll $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) UnitTest $(DESTDIR)$(GIMPPLUGINDIR)
