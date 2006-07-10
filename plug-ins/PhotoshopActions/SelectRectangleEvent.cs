// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SelectRectangleEvent.cs
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
  public class SelectRectangleEvent : ActionEvent
  {
    ObjcParameter _objc;

    public SelectRectangleEvent(ActionEvent srcEvent, ObjcParameter objc) : 
      base(srcEvent)
    {
      _objc = objc;
    }

    protected override IEnumerable ListParameters()
    {
      yield return "To: rectangle";
    }

    override public bool Execute()
    {
      double x, y, width, height;
      GetBounds(_objc, out x, out y, out width, out height);
      RectangleSelectTool tool = new RectangleSelectTool(ActiveImage);
      tool.Select(x, y, width, height, ChannelOps.Replace, false, 0);

      return true;
    }
  }
}
