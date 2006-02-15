## Process this file with automake to produce Makefile.in

all: Splitter.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0

MYSOURCES = Splitter.cs MathExpressionParser.cs MyClassBase.cs

EXTRA_DIST = $(MYSOURCES) Splitter Splitter.png

Splitter.exe : $(MYSOURCES) Splitter.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Splitter.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Splitter.exe

install:
	$(INSTALL_DATA) Splitter.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Splitter $(DESTDIR)/$(GIMPPLUGINDIR)
