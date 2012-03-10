// The AverageBlur plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

namespace Gimp.AverageBlur
{
  class AverageBlur : Plugin
  {
    static void Main(string[] args)
    {
      GimpMain<AverageBlur>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_average_blur",
			   _("Average blur"),
			   _("Average blur"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Average"),
			   "RGB*, GRAY*")
	{MenuPath = "<Image>/Filters/Blur"};
    }

    override protected void Render(Drawable drawable)
    {
      System.Console.WriteLine("0: " + drawable.Name);
      var iter = new RgnIterator(drawable, _("Average"));

      System.Console.WriteLine("1");

      var average = drawable.CreatePixel();
      System.Console.WriteLine("2");
      iter.IterateSrc(pixel => average.Add(pixel));
      average /= iter.Count;

      iter.IterateDest(() => average);
    }
  }
}
