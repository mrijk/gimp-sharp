## Process this file with automake to produce Makefile.in

all: GeneticDrawing.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = GeneticDrawing.cs \
	AlphaMutationOperator.cs \
	MutationOperator.cs

EXTRA_DIST = $(MYSOURCES) GeneticDrawing

CLEANFILES = GeneticDrawing.exe

GeneticDrawing.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) GeneticDrawing.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) GeneticDrawing $(DESTDIR)$(GIMPPLUGINDIR)
