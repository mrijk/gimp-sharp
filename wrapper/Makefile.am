AM_CPPFLAGS = `gimptool --cflags`
AM_LDFLAGS = `gimptool --libs`

AUTOMAKE_OPTIONS = gnu
lib_LTLIBRARIES = libgimpwrapper.la

libgimpwrapper_la_SOURCES = gimp.c

libgimpwrapper_la_LDFLAGS = -version-info 0:0:0 `gimptool --libs`
