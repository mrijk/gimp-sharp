// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// IntersectWithEvent.cs
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
  public class IntersectWithEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    public override bool IsExecutable
    {
      get 
	{
	  return (_objc.ClassID2 == "Plgn");;
	}
    }

    public override string EventForDisplay
    {
      get 
	{
	  return base.EventForDisplay + " Selection";
	}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Using: " + Abbreviations.Get(_objc.ClassID2);
    }

    override public bool Execute()
    {
      switch (_objc.ClassID2)
	{
	case "Plgn":
	  FreeSelectTool tool = new FreeSelectTool(ActiveImage);

	  ObArParameter array = _objc.Parameters["Pts"] as ObArParameter;
	  CoordinateList<double> points = array.Value;

	  if (array.Units == "#Prc")
	    {
	      int width = ActiveImage.Width;
	      int height = ActiveImage.Height;
	      foreach (Coordinate<double> c in points)
		{
		  c.X *= width / 100.0;
		  c.Y *= height / 100.0;
		}
	    }

	  tool.Select(points, ChannelOps.Intersect);
	  break;
	default:
	  Console.WriteLine("IntersectWithEvent: " + _objc.ClassID2);
	  return false;
	}
      return true;
    }
  }
}
