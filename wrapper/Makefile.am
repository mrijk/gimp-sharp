AM_CPPFLAGS = `gimptool-2.0 --cflags`
AM_LDFLAGS = `gimptool-2.0 --libs`

AUTOMAKE_OPTIONS = gnu
lib_LTLIBRARIES = libgimpwrapper.la

libgimpwrapper_la_SOURCES = gimp.c

libgimpwrapper_la_LDFLAGS = -version-info 0:0:0 `gimptool --libs`
