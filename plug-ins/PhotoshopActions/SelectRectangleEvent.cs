// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
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

namespace Gimp.PhotoshopActions
{
  public class SelectRectangleEvent : SelectionEvent
  {
    readonly ObjcParameter _objc;

    public SelectRectangleEvent(SelectionEvent srcEvent, ObjcParameter objc) : 
      base(srcEvent)
    {
      _objc = objc;
    }

    override public bool Execute()
    {
      GetBounds(_objc, out double _x, out double _y, out double _width, out double _height);

      var tool = new RectangleSelectTool(ActiveImage);
      tool.Select(_x, _y, _width, _height, ChannelOps.Replace, false, 0);
      RememberCurrentSelection();

      return true;
    }
  }
}
