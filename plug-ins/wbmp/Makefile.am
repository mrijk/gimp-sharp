all: wbmp.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = wbmp.cs

EXTRA_DIST = $(SOURCES) wbmp

wbmp.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

clean:
	rm -f wbmp.exe

install:
	$(INSTALL_DATA) wbmp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) wbmp $(DESTDIR)/$(GIMPPLUGINDIR)
