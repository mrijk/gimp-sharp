## Process this file with automake to produce Makefile.in

if BUILD_COLORIZE
Colorize = Colorize
endif

if BUILD_ECW
ecw = ecw
endif

if BUILD_INCOMPLETE
Mezzotint = Mezzotint
Shatter = Shatter
DifferenceClouds = DifferenceClouds
PictureFrame = PictureFrame
endif

if BUILD_IRONPYTHON
PythonSample = PythonSample
endif

if BUILD_PDN
Pdn = Pdn
endif

if BUILD_UNITTEST
UnitTest = UnitTest
endif

AUTOMAKE_OPTIONS = gnu
SUBDIRS = 			\
	AverageBlur		\
	$(Colorize)		\
	$(DifferenceClouds)	\
	$(ecw)			\
	Forge	 		\
	Fragment		\
	KoalaPaint 		\
	$(Mezzotint)		\
	Ministeck		\
	ncp 			\
	neo			\
	$(Pdn)			\
	PhotoshopActions	\
	$(PictureFrame)		\
	PicturePackage 		\
	Pointillize		\
	$(PythonSample)		\
	Raindrops		\
	$(Shatter)		\
	SliceTool 		\
	Splitter 		\
	Swirlies 		\
	$(UnitTest) 		\
	UpdateCheck		\
	wbmp


