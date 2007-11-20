# The PythonSample plug-in
# Copyright (C) 2007 Maurits Rijk
#
# PythonSample.py
#
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.

import sys
import System;

if __name__ == sys.executable:
    sys.path.append(System.Environment.GetEnvironmentVariable("PLUGINDIR"))
else:
    sys.path.append(r"../../lib")

import clr
clr.AddReference("gimp-sharp.dll")

from Gimp import *
from System import Array

class PythonSample(PythonPlugin):
    def ListProceduresTwo(self):
        procedure = Procedure("plug_in_python_sample",
                              "Sample IronPython plug-in: takes the average of all colors",
                              "Sample IronPython plug-in: takes the average of all colors",
                              "Maurits Rijk",
                              "(C) Maurits Rijk",
                              "2007",
                              "PythonSample",
                              "RGB*, GRAY*")
        procedure.MenuPath = "<Image>/Filters/Generic"

        # Fix me: I haven't figure out how to embed a resource with IronPython
        # procedure.IconFile = "PythonSample.png"

        yield procedure

    def Render(self, image, drawable):
        iter = RgnIterator(drawable, RunMode.Interactive);
        iter.Progress = Progress("Average");

        average = drawable.CreatePixel()

        iter.IterateSrc(lambda pixel: average.Add(pixel))

        average.Divide(iter.Count)

        iter.IterateDestSimple(lambda: average)

        Display.DisplaysFlush()

if __name__ == sys.executable:
    plugin = PythonSample(Array[str](sys.argv), "PythonSample")

