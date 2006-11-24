// The AverageBlur plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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

namespace Gimp.AverageBlur
{
  public class AverageBlur : Plugin
  {
    static void Main(string[] args)
    {
      new AverageBlur(args);
    }

    public AverageBlur(string[] args) : base(args, "AverageBlur")
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      Procedure procedure = new Procedure("plug_in_average_blur",
					  _("Average blur"),
					  _("Average blur"),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  _("Average"),
					  "RGB*, GRAY*");
      procedure.MenuPath = "<Image>/Filters/Blur"; 

      set.Add(procedure);

      return set;
    }

    override protected void Render(Drawable drawable)
    {
      int count = 0;
      Pixel average = new Pixel(drawable.Bpp);

      foreach (Pixel pixel in new ReadPixelIterator(drawable, 
						    RunMode.Interactive))
	{
	  average += pixel;
	  count++;
	}
      average /= count;

      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(_("Average"));
      iter.IterateDest(delegate () {return average;});

      Display.DisplaysFlush();
    }
  }
}
