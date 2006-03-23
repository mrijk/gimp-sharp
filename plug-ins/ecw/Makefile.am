all: ecw.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES = 		\
	ecw.cs 		\
	FileView.cs 	\
	FileViewInfo.cs

EXTRA_DIST = $(MYSOURCES) ecw

lib_LTLIBRARIES = libecwwrapper.la

libecwwrapper_la_SOURCES = ecwwrapper.c

libecwwrapper_la_LDFLAGS = -version-info 0 -lNCSEcw -lNCSUtil -lNCSCnet

ecw.exe : $(MYSOURCES) $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f ecw.exe

install:
	$(INSTALL_DATA) ecw.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) ecw $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) .libs/libecwwrapper.so $(DESTDIR)/$(GIMPPLUGINDIR)
