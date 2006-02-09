all: Splitter.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp-2.0

MYSOURCES = Splitter.cs MathExpressionParser.cs MyClassBase.cs

EXTRA_DIST = $(MYSOURCES) Splitter Splitter.png

Splitter.exe : $(MYSOURCES)
	$(CSC) $(MYSOURCES) /resource:Splitter.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Splitter.exe

install:
	$(INSTALL_DATA) Splitter.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Splitter $(DESTDIR)/$(GIMPPLUGINDIR)
