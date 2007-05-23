## Process this file with automake to produce Makefile.in

all: CountTool.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) -r:Mono.Posix /pkg:gtk-sharp-2.0 /target:winexe

MYSOURCES =			\
	CountTool.cs		\
	Preview.cs

EXTRA_DIST = $(MYSOURCES) 	\
	CountTool 		\
	CountTool.png

CLEANFILES = CountTool.exe

CountTool.exe : $(MYSOURCES) CountTool.png $(GIMPSHARPLIB)
	$(CSC) $(MYSOURCES) /resource:CountTool.png	\
		-out:$@ $(ASSEMBLIES)

install:
	$(INSTALL_DATA) CountTool.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) CountTool $(DESTDIR)$(GIMPPLUGINDIR)
