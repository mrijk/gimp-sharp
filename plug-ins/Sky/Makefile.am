## Process this file with automake to produce Makefile.in

all: Sky.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 				\
	Sky.cs

EXTRA_DIST = $(MYSOURCES) Sky Sky.png

CLEANFILES = Sky.exe

Sky.exe : $(MYSOURCES) Sky.png $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) /resource:Sky.png \
	-out:$@ $(ASSEMBLIES)
