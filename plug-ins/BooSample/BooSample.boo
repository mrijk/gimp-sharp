// The NemerleSample plug-in
// Copyright (C) 2007 Maurits Rijk
//
// BooSample.boo
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.

// namespace Gimp.BooSample

import System
import System.Collections
import System.Collections.Generic

import Gimp

[Module]
class BooSample(Plugin):

  def constructor(args as (string)):
    super(args, "BooSample")

  override def ListProcedures() as IEnumerable[of Procedure]:
    procedure = Procedure("plug_in_boo_sample",
			  "Sample Boo plug-in: takes the average of all colors",
			  "Sample Boo plug-in: takes the average of all colors",
			  "Maurits Rijk",
			  "(C) Maurits Rijk",
			  "2007",
			  "BooSample",
			  "RGB*, GRAY*")
    procedure.MenuPath = "<Image>/Filters/Generic"
    procedure.IconFile = "BooSample.png"
    yield procedure

  override protected def Render(drawable as Drawable):
    iter = RgnIterator(_drawable, RunMode.Interactive)
    iter.Progress = Progress("Average")

    average = _drawable.CreatePixel()
    iter.IterateSrc({pixel as Pixel | average.Add(pixel)})
    average.Divide(iter.Count)

    iter.IterateDest({return average})

def Main(argv as (string)):
  BooSample(argv)
