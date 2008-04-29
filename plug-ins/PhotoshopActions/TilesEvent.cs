// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// TilesEvent.cs
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
  public class TilesEvent : ActionEvent
  {
    [Parameter("TlNm")]
    int _numberOfTiles;
    [Parameter("TlOf")]
    int _maximumOffset;
    [Parameter("FlCl")]
    EnumParameter _fillColor;

    override public bool Execute()
    {
      // TODO: check _fillColor type

      int tileSize = (int) Math.Sqrt(ActiveDrawable.Width * 
				     ActiveDrawable.Height /
				     _numberOfTiles);

      RunProcedure("plug_in_papertile", tileSize, 
		   _maximumOffset * 100.0 / tileSize, 0, 1, 1,
		   4, 0, 0);

      return true;
    }
  }
}
