// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// CutToLayerEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class CutToLayerEvent : ActionEvent
  {
    override public bool Execute()
    {
      bool nonEmpty;

      var rectangle = ActiveImage.Selection.Bounds(out nonEmpty);

      if (nonEmpty)
	{
	  ActiveDrawable.EditCut();
	  ActiveDrawable.EditPaste(false);
	  
	  ActiveImage.FloatingSelection.Offsets = 
	    new Offset(rectangle.X1, rectangle.Y1);
	  var layer = ActiveImage.FloatingSelection.ToLayer();
	  // Fix me: why can't I use layer here?
	  new Layer(ActiveImage.ActiveDrawable).ResizeToImageSize();
	}
      else
	{
	  // TODO: how does PS handle this?
	}
      return true;
    }
  }
}
