## Process this file with automake to produce Makefile.in

if BUILD_ECW
ecw = ecw
endif

if BUILD_INCOMPLETE
Colorize = Colorize
Shatter = Shatter
DifferenceClouds = DifferenceClouds
PictureFrame = PictureFrame
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
	Ministeck		\
	ncp 			\
	neo			\
	$(Pdn)			\
	PhotoshopActions	\
	$(PictureFrame)		\
	PicturePackage 		\
	Pointillize		\
	Raindrops		\
	$(Shatter)		\
	SliceTool 		\
	Splitter 		\
	Swirlies 		\
	$(UnitTest) 		\
	UpdateCheck		\
	wbmp


