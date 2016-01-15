// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// LinkEvent.cs
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
  public class LinkEvent : ActionEvent
  {
    [Parameter("T")]
    ListParameter _list;

    public override string EventForDisplay =>
      base.EventForDisplay + " current layer";

    protected override IEnumerable ListParameters()
    {
      foreach (ReferenceParameter parameter in _list)
	{
	  var name = parameter.Set[0] as NameType;
	  if (name != null)
	    {
	      yield return "layer \"" + name.Key + "\"";
	    }
	}
    }

    override public bool Execute()
    {
      var layers = ActiveImage.Layers;

      foreach (ReferenceParameter parameter in _list)
	{
	  var name = parameter.Set[0] as NameType;
	  if (name != null)
	    {
	      var layer = layers[name.Key];
	      LinkedLayersSet.Link(SelectedLayer, layer);
	    }
	}
      return true;
    }
  }
}
