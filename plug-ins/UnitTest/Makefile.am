## Process this file with automake to produce Makefile.in

all: UnitTest.exe gimptest.dll

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /pkg:mono-nunit

MYSOURCES = 	Dialog.cs			\
		EventCollector.cs		\
		TestReportDialog.cs		\
		UnitTest.cs			\
		UnitTester.cs

TESTSOURCES = 	TestBoolMatrix.cs		\
		TestChannel.cs			\
		TestDimensions.cs		\
		TestDisplay.cs			\
		TestFloatingSelection.cs	\
		TestGimpUnit.cs			\
		TestCoordinate.cs		\
		TestCoordinateList.cs		\
		TestDrawable.cs			\
		TestGrid.cs			\
		TestGuide.cs			\
		TestHSV.cs			\
		TestImage.cs			\
		TestItem.cs			\
		TestLayerGroup.cs		\
		TestVectors.cs			\
		TestVector2.cs			\
		TestVector3.cs			\
		TestVersion.cs

FAILING = \
		TestBrush.cs			\
		TestContext.cs			\
		TestBrushList.cs		\
		TestChannelList.cs		\
		TestGimp.cs			\
		TestGradient.cs			\
		TestGradientList.cs		\
		TestGradientSegment.cs		\
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
		TestPlugins.cs			\
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
		TestUndo.cs

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
