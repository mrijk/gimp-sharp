// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SimilarEvent.cs
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
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  public class SimilarEvent : ActionEvent
  {
    [Parameter("Tlrn")]
    readonly int _tolerance;
    [Parameter("AntA")]
    readonly bool _antiAlias;

    public override string EventForDisplay
    {
      get => base.EventForDisplay + " Selection";
    }

    override public bool Execute()
    {
      var iter = new RgnIterator(ActiveDrawable, "Select Similar");

      var tool = new ByColorSelectTool(ActiveDrawable);

      var uniqueColors = new HashSet<RGB>();
      iter.IterateSrc(pixel => uniqueColors.Add(pixel.Color));

      Console.WriteLine("Colors: " + uniqueColors.Count);

      // More or less arbitrary hack
      if (uniqueColors.Count > 10)
	return true;

      foreach (var color in uniqueColors) 
	{
	  tool.Select(color, 0, ChannelOps.Add, true, false, 0, false);
	}

      return true;
    }
  }
}
