// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// MoveLayerEvent.cs
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
  public class MoveLayerEvent : MoveEvent
  {
    readonly string _direction;

    public MoveLayerEvent(ActionEvent srcEvent, string direction) 
      : base(srcEvent)
    {
      _direction = direction;
    }
    
    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " current layer";}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "To: " + Abbreviations.Get(_direction) + " layer";
    }

    override public bool Execute()
    {
      switch (_direction)
	{
	case "Bckw":
	  SelectedLayer.Lower();
	  break;
	case "Frwr":
	  SelectedLayer.Raise();
	  break;
	case "Prvs":
	  Console.WriteLine("Implement MoveLayerEvent: " + _direction);
	  break;
	default:
	  Console.WriteLine("MoveLayerEvents: " + _direction);
	  break;
	}
      return true;
    }
  }
}
