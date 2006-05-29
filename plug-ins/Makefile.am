## Process this file with automake to produce Makefile.in

if BUILD_ECW
ecw = ecw
endif

if BUILD_INCOMPLETE
PhotoshopActions = PhotoshopActions
Raindrops = Raindrops
Shatter = Shatter
endif

if BUILD_PDN
Pdn = Pdn
endif

if BUILD_UNITTEST
UnitTest = UnitTest
endif

AUTOMAKE_OPTIONS = gnu
SUBDIRS = 			\
	$(ecw)			\
	KoalaPaint 		\
	Ministeck		\
	ncp 			\
	neo			\
	$(Pdn)			\
	$(PhotoshopActions)	\
	PicturePackage 		\
	$(Raindrops)		\
	$(Shatter)		\
	SliceTool 		\
	Splitter 		\
	Swirlies 		\
	$(UnitTest) 		\
	UpdateCheck		\
	wbmp
