## Process this file with automake to produce Makefile.in

all: Shatter.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES =		\
	Coord.cs	\
	Shard.cs	\
	Shatter.cs

EXTRA_DIST = $(MYSOURCES) Shatter Shatter.png

Shatter.exe : $(MYSOURCES) Shatter.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:Shatter.png -out:$@ $(ASSEMBLIES)

clean:
	rm -f Shatter.exe

install:
	$(INSTALL_DATA) Shatter.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) Shatter $(DESTDIR)$(GIMPPLUGINDIR)
