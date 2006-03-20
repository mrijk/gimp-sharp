## Process this file with automake to produce Makefile.in

if BUILD_UNITTEST
UnitTest = UnitTest
endif

if BUILD_PDN
Pdn = Pdn
endif

if BUILD_INCOMPLETE
ecw = ecw
Raindrops = Raindrops
RedEye = RedEye
endif

AUTOMAKE_OPTIONS = gnu
SUBDIRS = 		\
	$(ecw)		\
	KoalaPaint 	\
	Ministeck	\
	ncp 		\
	neo		\
	$(Pdn)		\
	PicturePackage 	\
	$(Raindrops)	\
	$(RedEye)	\
	SliceTool 	\
	Splitter 	\
	Swirlies 	\
	$(UnitTest) 	\
	UpdateCheck	\
	wbmp
