all: wbmp.exe

ASSEMBLIES = -r:$(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

MYSOURCES = wbmp.cs

EXTRA_DIST = $(MYSOURCES) wbmp

wbmp.exe : $(MYSOURCES)
	$(CSC) $(MYSOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f wbmp.exe

install:
	$(INSTALL_DATA) wbmp.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) wbmp $(DESTDIR)/$(GIMPPLUGINDIR)
