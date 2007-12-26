// The AverageBlur plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// AverageBlur.cs
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
//

using System;
using System.Collections.Generic;

namespace Gimp.AverageBlur
{
  class AverageBlur : Plugin
  {
    static void Main(string[] args)
    {
      new AverageBlur(args);
    }

    AverageBlur(string[] args) : base(args, "AverageBlur")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_average_blur",
				 _("Average blur"),
				 _("Average blur"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2006-2007",
				 _("Average"),
				 "RGB*, GRAY*")
	{MenuPath = "<Image>/Filters/Blur"};
    }

    override protected void Render(Drawable drawable)
    {
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(_("Average"));

      Pixel average = drawable.CreatePixel();
      iter.IterateSrc(pixel => {average.Add(pixel);});
      average /= iter.Count;

      iter.IterateDest(() => average);
    }
  }
}
