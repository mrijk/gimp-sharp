// The PhotoshopActions plug-in
// Copyright (C) 2006-2010 Maurits Rijk
//
// SelectPolygonEvent.cs
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
  public class SelectPolygonEvent : SelectionEvent
  {
    readonly ObjcParameter _objc;

    public SelectPolygonEvent(SelectionEvent srcEvent, ObjcParameter objc) : 
      base(srcEvent)
    {
      _objc = objc;
    }

    protected override IEnumerable ListParameters()
    {
      yield return "To: polygon";
      var array = _objc.Parameters["Pts"] as ObArParameter;

      yield return "Points: point list";
      foreach (var c in array.Value)
	{
	  yield return String.Format("Point: {0} pixels, {1} pixels", 
				     c.X, c.Y);
	}
    }

    override public bool Execute()
    {
      var tool = new FreeSelectTool(ActiveImage);

      var array = _objc.Parameters["Pts"] as ObArParameter;
      tool.Select(array.Value, ChannelOps.Replace);
      RememberCurrentSelection();

      return true;
    }
  }
}
