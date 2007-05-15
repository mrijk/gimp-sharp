// The PhotoshopActions plug-in
// Copyright (C) 2007 Maurits Rijk
//
// AlignEvent.cs
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
  public class AlignEvent : ActionEvent
  {
    [Parameter("Usng")]
    EnumParameter _using;

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " linked layer";}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Using: " + Abbreviations.Get(_using.Value);
    }

    override public bool Execute()
    {
      // TODO: assume we are only aligning to the selection

      bool nonEmpty;
      Rectangle bounds = ActiveImage.Selection.Bounds(out nonEmpty);
      Layer layer = SelectedLayer;
      Offset offset = layer.Offsets;

      switch (_using.Value)
	{
	case "AdLf":
	  offset.X = bounds.X1;
	  break;
	case "AdRg":
	  offset.X = bounds.X2 - layer.Width;
	  break;
	case "AdTp":
	  offset.Y = bounds.Y1;
	  break;
	case "AdBt":
	  offset.Y = bounds.Y2 - layer.Height;
	  break;
	default:
	  Console.WriteLine("AlignEvent: " + _using.Value);
	  break;
	}

      layer.Offsets = offset;

      return true;
    }
  }
}
