// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// MakePatternEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class MakePatternEvent : MakeEvent
  {
    [Parameter("Nm")]
    string _name;

    public MakePatternEvent(MakeEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Pattern";}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Name: \"" + _name + "\"";
    }

    override public bool Execute()
    {
      // Look for script-fu-selection-to-pattern as example. The basic idea
      // is to create a new image and save it as a .pat file 

      Selection selection = ActiveImage.Selection;
      bool nonEmpty;
      Rectangle bounds = selection.Bounds(out nonEmpty);
      int width = bounds.Width;
      int height = bounds.Height;

      Image image = new Image(width, height, ActiveImage.BaseType);
      Layer layer = new Layer(image, "test", width, height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      image.AddLayer(layer, 0);
      Console.WriteLine("{0} {1}", image.Width, image.Height);

      ActiveDrawable.EditCopy();
      layer.EditPaste(true);

      string filename = Gimp.Directory + System.IO.Path.DirectorySeparatorChar 
	+ "patterns" + System.IO.Path.DirectorySeparatorChar + 
	_name + ".pat";

      image.Save(RunMode.Noninteractive, filename, filename);
   
      // Fix me: next lines should work so we can actually set the name
      // of the pattern!
      // Procedure procedure = new Procedure("file_pat_save");
      // procedure.Run(image, layer, filename, filename, _name);

      image.Delete();
      PatternList.Refresh();

      return false;
    }
  }
}
