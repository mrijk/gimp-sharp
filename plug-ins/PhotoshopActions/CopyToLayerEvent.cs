// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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
      int x1, y1, x2, y2;
      bool nonEmpty;

      ActiveImage.Selection.Bounds(out nonEmpty, out x1, out y1, 
				   out x2, out y2);

      if (nonEmpty)
	{
	  ActiveDrawable.EditCopy();
	  ActiveDrawable.EditPaste(false);
	  
	  ActiveImage.FloatingSelection.SetOffsets(x1, y1);
	  Layer layer = ActiveImage.FloatingSelection.ToLayer();
	  // Fix me: why can't I use layer here?
	  new Layer(ActiveImage.ActiveDrawable).ResizeToImageSize();
	}
      else
	{
	  // TODO: what does PS handle this?
	}
      return true;
    }
  }
}
