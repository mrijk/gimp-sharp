// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// AddLayerEvent.cs
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
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  public class AddLayerEvent : ActionEvent
  {
    bool _below;

    public AddLayerEvent(ActionEvent srcEvent, List<ReferenceType> set) : 
      base(srcEvent) 
    {
      if (set.Count != 2)
	{
	  Console.WriteLine("AddLayerEvent, Count: " + set.Count);
	  // Fill _below
	}
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " layer";}
    }

    override public bool Execute()
    {
      // Fix me: do something with Image.ImageBaseType
      Image image = ActiveImage;

      Layer layer = new Layer(image, "New Layer", image.Width, image.Height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      image.AddLayer(layer, 0);

      return true;
    }
  }
}
