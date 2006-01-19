all: Splitter.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES = Splitter.cs MathExpressionParser.cs MyClassBase.cs

EXTRA_DIST = $(MYSOURCES) Splitter

Splitter.exe : $(MYSOURCES)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f Splitter.exe

install:
	$(INSTALL_DATA) Splitter.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Splitter $(DESTDIR)/$(GIMPPLUGINDIR)
