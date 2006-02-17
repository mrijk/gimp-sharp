all: UnitTest.exe gimptest.dll

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /pkg:mono-nunit

MYSOURCES = UnitTest.cs

EXTRA_DIST = $(MYSOURCES) UnitTest

UnitTest.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

TESTSOURCES = TestImage.cs

gimptest.dll : $(TESTSOURCES) $(GIMPSHARPLIB)
	$(CSC) -target:library $(ASSEMBLIES) $(TESTSOURCES) -out:gimptest.dll

clean:
	rm -f UnitTest.exe

install:
	$(INSTALL_DATA) UnitTest.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) UnitTest $(DESTDIR)/$(GIMPPLUGINDIR)
