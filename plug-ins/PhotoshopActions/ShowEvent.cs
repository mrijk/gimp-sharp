// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ShowEvent.cs
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
  public class ShowEvent : ActionEvent
  {
    [Parameter("null")]
    ListParameter _list;

    override public bool Execute()
    {
      ReferenceParameter obj = _list.Set[0] as ReferenceParameter;
      EnmrType enmr = obj.Set[0] as EnmrType;

      if (enmr.Key == "Lyr")
	{
	  SelectedLayer.Visible = true;
	}
      else
	{
	  Console.WriteLine("Can't show " + enmr.Key);
	}

      return true;
    }
  }
}
