## Process this file with automake to produce Makefile.in

if BUILD_UNITTEST
UnitTest = UnitTest
endif

if BUILD_PDN
Pdn = Pdn
endif

if BUILD_INCOMPLETE
Raindrops = Raindrops
RedEye = RedEye
endif

AUTOMAKE_OPTIONS = gnu
SUBDIRS = 		\
	KoalaPaint 	\
	Ministeck	\
	ncp 		\
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
