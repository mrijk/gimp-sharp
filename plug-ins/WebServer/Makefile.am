## Process this file with automake to produce Makefile.in

all: WebServer.exe routes.dll

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /pkg:manos

MYSOURCES = WebServer.cs

TESTSOURCES = Routes.cs

EXTRA_DIST = $(MYSOURCES) $(TESTSOURCES) WebServer WebServer.png

WebServer.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) /target:winexe $(MYSOURCES) \
	/resource:WebServer.png -out:$@ $(ASSEMBLIES)

routes.dll : $(TESTSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(CSC_FLAGS) -target:library $(ASSEMBLIES) $(TESTSOURCES) \
	-out:routes.dll

clean:
	rm -f WebServer.exe routes.dll

install:
	$(INSTALL_DATA) WebServer.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL_DATA) routes.dll $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) WebServer $(DESTDIR)$(GIMPPLUGINDIR)
