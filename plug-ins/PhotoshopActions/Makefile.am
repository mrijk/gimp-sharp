## Process this file with automake to produce Makefile.in

all: PhotoshopActions.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 			\
	Action.cs		\
	ActionEvent.cs		\
	ActionParser.cs		\
	ActionSet.cs		\
	MakeEvent.cs		\
	PhotoshopActions.cs	\
	StopEvent.cs

EXTRA_DIST = $(MYSOURCES) PhotoshopActions PhotoshopActions.png

PhotoshopActions.exe : $(MYSOURCES) PhotoshopActions.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:PhotoshopActions.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f PhotoshopActions.exe

install:
	$(INSTALL_DATA) PhotoshopActions.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) PhotoshopActions $(DESTDIR)$(GIMPPLUGINDIR)
