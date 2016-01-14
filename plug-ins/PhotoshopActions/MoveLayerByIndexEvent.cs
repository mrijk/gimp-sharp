// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// MoveLayerByIndexEvent.cs
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
  public class MoveLayerByIndexEvent : MoveEvent
  {
    readonly int _index;

    public MoveLayerByIndexEvent(MoveEvent srcEvent, int index) 
      : base(srcEvent)
    {
      _index = index;
    }

    public override string EventForDisplay
    {
      get 
	{
	  var obj = Parameters["null"] as ReferenceParameter;
	  if (obj == null)
	    {
	      return base.EventForDisplay + " current layer";
	    }
	  else
	    {
	      if (obj.Set[0] is NameType)
		{
		  string name = (obj.Set[0] as NameType).Key;
		  return base.EventForDisplay + " layer \"" + name + "\"";
		}
	      else
		{
		  return base.EventForDisplay + " current layer";
		}
	    }
	}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "To: layer " + _index;

      var adjust = Parameters["Adjs"] as BoolParameter;
      if (adjust != null)
	{
	  yield return ((adjust.Value) ? "With" : "Without") + 
	    " Adjust Selection";
	}
    }

    override public bool Execute()
    {
      var obj = Parameters["null"] as ReferenceParameter;
      Layer layer;

      if (obj == null)
	{
	  layer = SelectedLayer;
	}
      else
	{
	  if (obj.Set[0] is NameType)
	    {
	      string name = (obj.Set[0] as NameType).Key;
	      layer = ActiveImage.Layers[name];
	    }
	  else
	    {
	      layer = SelectedLayer;
	    }
	}

      int diff = layer.Position - _index;
      if (diff < 0)
	{
	  diff = -diff;
	  for (int i = 0; i < diff; i++)
	    {
	      layer.Lower();
	    }
	}
      else if (diff > 0)
	{
	  for (int i = 0; i < diff; i++)
	    {
	      layer.Raise();
	    }
	}

      SelectedLayer = layer;

      return true;
    }
  }
}
