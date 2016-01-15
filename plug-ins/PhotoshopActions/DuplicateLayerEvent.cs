// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DuplicateLayerEvent.cs
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
using System.Text.RegularExpressions;

namespace Gimp.PhotoshopActions
{
  public class DuplicateLayerEvent : DuplicateEvent
  {
    [Parameter("Nm")]
    string _name;

    public DuplicateLayerEvent(DuplicateEvent srcEvent) : base(srcEvent) 
    {
    }

    public override string EventForDisplay =>
      base.EventForDisplay + " current layer";

    protected override IEnumerable ListParameters()
    {
      if (_name != null)
	yield return "Name: \"" + _name + "\"";
    }

    override public bool Execute()
    {
      var layer = new Layer(SelectedLayer);
      ActiveImage.InsertLayer(layer, 0);

      if (_name != null)
	{
	  layer.Name = _name;
	}
      else
	{
	  var rx = new Regex(@"(.* copy)(.+)");
	  var m = rx.Match(layer.Name);
	  if (m.Groups.Count == 3)
	    {
	      int nr = Convert.ToInt32("1") + 1;
	      layer.Name = m.Groups[1] + " " + nr;
	    }
	}

      ActiveDrawable = layer;
      SelectedLayer = layer;

      return true;
    }
  }
}
