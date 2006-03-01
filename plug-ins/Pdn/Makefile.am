## Process this file with automake to produce Makefile.in

all: Pdn.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll

ASSEMBLIES = -r:$(GIMPSHARPLIB) /pkg:gtk-sharp-2.0 \
/r:System.Windows.Forms.dll /r:System.Drawing.dll \
/r:PaintDotNet.Data.dll \
/r:PaintDotNet.Effects.dll \
/r:PaintDotNet.Resources.dll \
/r:PaintDotNet.StylusReader.dll \
/r:PaintDotNet.SystemLayer.dll \
/r:PdnLib.dll

SOURCES = Pdn.cs

EXTRA_DIST = $(SOURCES) Pdn

Pdn.exe: $(SOURCES) $(GIMPSHARPLIB)
	$(CSC) $(SOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f pdn.exe

install:
	$(INSTALL_DATA) pdn.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) pdn $(DESTDIR)/$(GIMPPLUGINDIR)
