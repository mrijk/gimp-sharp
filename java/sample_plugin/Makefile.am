## Process this file with automake to produce Makefile.in

all: JAverageBlur.exe

GIMPSHARPLIB = $(top_builddir)/lib/gimp-sharp.dll
IKVMDIR=`echo $(MONO) | sed -e 's/mono//'`../lib/ikvm/
JAVASTUBSDIR = ../../java/stubs

JAVASTUBS = $(JAVASTUBSDIR)/mscorlib.jar:$(JAVASTUBSDIR)/gimp-sharp.jar

MYSOURCES = JAverageBlur.java

EXTRA_DIST = $(MYSOURCES) JAverageBlur 
##SUBDIRS = po

TARGET_FILE = JAverageBlur.exe
CLASSFILES = JAverageBlur*.class
CLEANFILES = $(TARGET_FILE) $(CLASSFILES)

JAverageBlur.exe : $(MYSOURCES) 
	echo $(RUNTIME)
	javac -source 1.4 -target 1.4 -cp $(JAVASTUBS)  $(MYSOURCES) 
	mono $(IKVMDIR)/ikvmc.exe -reference:$(IKVMDIR)/IKVM.GNU.Classpath.dll -reference:$(GIMPSHARPLIB) -target:exe -out:$(TARGET_FILE) $(CLASSFILES) 

install:
	$(INSTALL_DATA) JAverageBlur.exe $(DESTDIR)$(GIMPPLUGINDIR)
	$(INSTALL) JAverageBlur $(DESTDIR)$(GIMPPLUGINDIR)
##	@for lang in `ls po | grep .mo`;\
##		do localedir=`echo $$lang | sed -e's/.mo//'`;\
##		echo "Copying po/$$lang to $(GIMPLOCALE)/$$localedir/LC_MESSAGES/JAverageBlur.mo";\
##		$(INSTALL_DATA) po/$$lang $(GIMPLOCALE)/$$localedir/LC_MESSAGES/JAverageBlur.mo;\
##		done
