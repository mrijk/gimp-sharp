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

EXTRA_DIST = $(SOURCES) Pdn PaintDotNet.Data.dll PdnLib.dll

Pdn.exe: $(SOURCES) $(GIMPSHARPLIB)
	$(CSC) $(SOURCES) -out:$@ $(ASSEMBLIES)

clean:
	rm -f Pdn.exe

install:
	$(INSTALL_DATA) Pdn.exe $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL) Pdn $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL_DATA) PaintDotNet.Data.dll $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL_DATA) PaintDotNet.SystemLayer.dll $(DESTDIR)/$(GIMPPLUGINDIR)
	$(INSTALL_DATA) PdnLib.dll $(DESTDIR)/$(GIMPPLUGINDIR)
