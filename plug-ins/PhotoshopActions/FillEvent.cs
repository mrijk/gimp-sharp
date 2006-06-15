// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// FillEvent.cs
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
  public class FillEvent : ActionEvent
  {
    [Parameter("Usng")]
    string _using;
    [Parameter("Opct")]
    double _opacity;
    [Parameter("Md")]
    string _mode;

    override public bool Execute()
    {
      if (_using == "FrgC")
	{
	  ActiveDrawable.EditFill(FillType.Foreground);
	}
      else if (_using == "BckC")
	{
	  ActiveDrawable.EditFill(FillType.Background);
	}
      else
	{
	  Console.WriteLine("FillEvent: with {0} not supported!", _using);
	}

      return true;
    }
  }
}
