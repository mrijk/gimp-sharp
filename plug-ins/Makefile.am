## Process this file with automake to produce Makefile.in

if BUILD_UNITTEST
UnitTest = UnitTest
endif

if BUILD_PDN
Pdn = Pdn
endif

if BUILD_INCOMPLETE
Raindrops = Raindrops
endif

AUTOMAKE_OPTIONS = gnu
SUBDIRS = 		\
	ecw		\
	KoalaPaint 	\
	Ministeck	\
	ncp 		\
	neo		\
	$(Pdn)		\
	PicturePackage 	\
	$(Raindrops)	\
	SliceTool 	\
	Splitter 	\
	Swirlies 	\
	$(UnitTest) 	\
	UpdateCheck	\
	wbmp
