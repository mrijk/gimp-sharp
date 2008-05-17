// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// AddToEvent.cs
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
  public class AddToEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Selection";}
    }

    override public bool Execute()
    {
      DoubleParameter top = _objc.Parameters["Top"] as DoubleParameter;
      DoubleParameter left = _objc.Parameters["Left"] as DoubleParameter;
      DoubleParameter bottom = _objc.Parameters["Btom"] 
	as DoubleParameter;
      DoubleParameter right = _objc.Parameters["Rght"] as DoubleParameter;

      double x = left.Value;
      double y = top.Value;
      double width = right.Value - x + 1;
      double height = bottom.Value - y + 1;

      RectangleSelectTool tool = new RectangleSelectTool(ActiveImage);
      tool.Select(x, y, width, height, ChannelOps.Add, false, 0);

      return true;
    }
  }
}
