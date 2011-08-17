// The Ministeck plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Renderer.cs
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

namespace Gimp.Ministeck
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image, Drawable drawable, bool preview)
    {
      int size = GetValue<int>("size");
      
      image.UndoGroupStart();

      var procedure = new Procedure("plug_in_pixelize");
      procedure.Run(image, drawable, size);

      var palette = new MinisteckPalette();
      image.ConvertIndexed(ConvertDitherType.No, ConvertPaletteType.Custom,
			   0, false, false, "Ministeck");
      palette.Delete();
      
      image.ConvertRgb();
      image.UndoGroupEnd();
      
      // And finally calculate the Ministeck pieces
	
      using (var painter = new Painter(drawable, size, GetValue<RGB>("color")))
	{
	  Shape.Painter = painter;

	  int width = drawable.Width / size;
	  int height = drawable.Height / size;

	  var A = new BoolMatrix(width, height);

	  // Fill in shapes
	  bool limit = GetValue<bool>("limit");
	  var shapes = new ShapeSet();
	  shapes.Add((limit) ? 2 : 1, new TwoByTwoShape());
	  shapes.Add((limit) ? 8 : 1, new ThreeByOneShape());
	  shapes.Add((limit) ? 3 : 1, new TwoByOneShape());
	  shapes.Add((limit) ? 2 : 1, new CornerShape());
	  shapes.Add((limit) ? 1 : 1, new OneByOneShape());

	  Action<int> update = null;
	  if (!preview)
	    {
	      var progress = new Progress(_("Ministeck..."));
	      update = y => progress.Update((double) y / height);
	    }

	  var generator = 
	    new CoordinateGenerator(new Rectangle(0, 0, width, height), update);
	  generator.ForEach(c => {if (!A.Get(c)) shapes.Fits(A, c);});
	}

      drawable.Flush();
      drawable.Update();
    }
  }
}
