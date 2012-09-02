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
		TestGimp.cs			\
		TestGradient.cs			\
		TestGrid.cs			\
		TestGuide.cs			\
		TestHSV.cs			\
		TestImage.cs			\
		TestIntCoordinate.cs		\
		TestItem.cs			\
		TestLayer.cs			\
		TestLayerGroup.cs		\
		TestLayerList.cs		\
		TestMask.cs			\
		TestOffset.cs			\
		TestParasite.cs			\
		TestPath.cs			\
		TestPixel.cs			\
		TestProcedure.cs		\
		TestRcQuery.cs			\
		TestRectangle.cs		\
		TestRGB.cs			\
		TestSelection.cs		\
		TestStroke.cs			\
		TestTextLayer.cs		\
		TestUndo.cs			\
		TestVectors.cs			\
		TestVector2.cs			\
		TestVector3.cs			\
		TestVersion.cs

FAILING = \
		TestBrush.cs			\
		TestContext.cs			\
		TestPlugins.cs			\
		TestProceduralDb.cs		\
		Dummy.cs

UNKNOWN = \
		TestBrushList.cs		\
		TestChannelList.cs		\
		TestGradientList.cs		\
		TestGradientSegment.cs		\
		TestImageList.cs		\
		TestPalette.cs			\
		TestPaletteEntry.cs		\
		TestPaletteList.cs		\
		TestPattern.cs			\
		TestPatternList.cs		\
		TestPixelFetcher.cs		\
		TestPixelRgn.cs			\
		TestRegionIterator.cs		\
		TestRgnIterator.cs		\
		Dummy.cs

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
