// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SetLayerPropertyEvent.cs
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

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class SetLayerPropertyEvent : SetEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    public SetLayerPropertyEvent(SetEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " current layer";}
    }

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      foreach (Parameter parameter in _objc.Parameters)
	{
	  switch (parameter.Name)
	    {
	    case "Md":
	      string mode = (parameter as EnumParameter).Value;
	      store.AppendValues(iter, "Mode: " + mode);
	      break;
	    case "Nm":
	      string name = (parameter as TextParameter).Value;
	      store.AppendValues(iter, "Name: " + name);
	      break;
	    case "Opct":
	      double opacity = (parameter as DoubleParameter).Value;
	      store.AppendValues(iter, "Opacity: " + opacity);
	      break;
	    default:
	      Console.WriteLine("SetLayerProperty: " + parameter.Name);
	      break;
	    }
	}
    }

    override public bool Execute()
    {
      foreach (Parameter parameter in _objc.Parameters)
	{
	  switch (parameter.Name)
	    {
	    case "Md":
	      string mode = (parameter as EnumParameter).Value;
	      switch (mode)
		{
		case "Drkn":
		  // TODO: not a perfect match
		  SelectedLayer.Mode = LayerModeEffects.DarkenOnly;
		  break;
		case "Lghn":
		  // TODO: not a perfect match
		  SelectedLayer.Mode = LayerModeEffects.LightenOnly;
		  break;
		case "Nrml":
		  SelectedLayer.Mode = LayerModeEffects.Normal;
		  break;
		case "Ovrl":
		  SelectedLayer.Mode = LayerModeEffects.Overlay;
		  break;
		case "Scrn":
		  SelectedLayer.Mode = LayerModeEffects.Screen;
		  break;
		default:
		  Console.WriteLine("Implement set layer mode: " + mode);
		  break;
		}
	      break;
	    case "Nm":
	      SelectedLayer.Name = (parameter as TextParameter).Value;
	      break;
	    case "Opct":
	      SelectedLayer.Opacity = (parameter as DoubleParameter).Value;
	      break;
	    default:
	      Console.WriteLine("SetLayerPropertyEvent: " + parameter.Name);
	      break;
	    }
	}
      return true;
    }
  }
}
