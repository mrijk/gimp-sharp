all: ncp.exe

ASSEMBLIES = -r $(top_builddir)/lib/gimp-sharp.dll /pkg:gtk-sharp

SOURCES = ncp.cs

ncp.exe : $(SOURCES)
	$(CSC) $(SOURCES) -o $@ $(ASSEMBLIES)

install:
	install -m='u=rw,g=r,o=r' ncp.exe /usr/local/lib/gimp/2.0/plug-ins
	install ncp /usr/local/lib/gimp/2.0/plug-ins