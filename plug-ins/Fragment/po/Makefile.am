## Process this file with automake to produce Makefile.in

AUTOMAKE_OPTIONS = gnu

SHELL=/bin/sh

PLUGIN = Fragment

LINGUAS = it en ru fr

POFILES = $(shell for lang in $(LINGUAS); do printf "$$lang.po "; done)

CATALOGS = $(shell for lang in $(LINGUAS); do printf "$$lang.mo "; done) 

MSGFMT = msgfmt
GMSGFMT = msgfmt

GMOFILES = $(CATALOGS) 
MOFILES = $(CATALOGS) 

CLEANFILES = $(CATALOGS)

EXTRA_DIST = $(POFILES)

SUFFIXES = .po .gmo .mo

.po.mo:
	$(GMSGFMT) -o $@ $<

all: $(GMOFILES)

