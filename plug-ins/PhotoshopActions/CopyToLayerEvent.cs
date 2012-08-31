// The PhotoshopActions plug-in
// Copyright (C) 2006-2012 Maurits Rijk
//
// CopyToLayerEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class CopyToLayerEvent : ActionEvent
  {
    override public bool Execute()
    {
      bool nonEmpty;

      Rectangle rectangle = ActiveImage.Selection.Bounds(out nonEmpty);

      if (nonEmpty)
	{
	  ActiveDrawable.EditCopy();
	  ActiveDrawable.EditPaste(false);
	  
	  ActiveImage.FloatingSelection.Offsets = 
	    new Offset(rectangle.X1, rectangle.Y1);
	  var layer = ActiveImage.FloatingSelection.ToLayer();

	  // Fix me: why can't I use layer here?
	  var dummy = new Layer(ActiveImage.ActiveDrawable) {Name = "Layer 1"};
	  dummy.ResizeToImageSize();
	}
      else
	{
	  var image = ActiveImage;
	  var layer = new Layer(SelectedLayer) {Name = "Layer 1"};
	  image.InsertLayer(layer, 0);
	  image.ActiveLayer = layer;
	  SelectedLayer = layer;
	}
      return true;
    }
  }
}
