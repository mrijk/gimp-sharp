// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SelectLayerByNameEvent.cs
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
  public class SelectLayerByNameEvent : SelectEvent
  {
    readonly string _name;
    readonly EnumParameter _modifier;
    readonly BoolParameter _makeVisible;

    public SelectLayerByNameEvent(ActionEvent srcEvent, string name) : 
      base(srcEvent)
    {
      _name = name;
      _modifier = Parameters["selectionModifier"] as EnumParameter;
      _makeVisible = Parameters["MkVs"] as BoolParameter;
    }

    public override string EventForDisplay
    {
      get => base.EventForDisplay + " layer \"" + _name + "\"";
    }

    protected override IEnumerable ListParameters()
    {
      if (_modifier != null)
	{
	  yield return "Modification: " + Abbreviations.Get(_modifier.Value);
	}

      if (_makeVisible != null)
	{
	  yield return (_makeVisible.Value ? "With" : "Without") + 
	    " Make Visible";
	}
    }

    override public bool Execute()
    {
      bool add = _modifier != null;
      SelectLayer(ActiveImage.Layers[_name], add);

      return true;
    }
  }
}
