// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SetEvent.cs
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
  public class SetEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public SetEvent()
    {
    }

    public SetEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable => _executable;

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_obj != null)
	{
	  if (_obj.Set[0] is PropertyType)
	    {
	      var property = _obj.Set[0] as PropertyType;

	      switch (property.ClassID2)
		{
		case "Clr":
		  switch (property.Key)
		    {
		    case "BckC":
		      return new SetBackgroundColorEvent(this);
		    case "ClrT":
		      return new SetColorTableEvent(this);
		    case "FrgC":
		      return new SetForegroundColorEvent(this);
		    default:
		      Console.WriteLine("SetEvent.Clr.Prpr: " + property.Key);
		      break;
		    }
		  break;
		case "Chnl":
		  if (property.Key == "fsel")
		    {
		      return new SelectionEvent(this).Parse(parser);
		    }
		  break;
		case "Lyr":
		  return new SetLayerPropertyEvent(this);
		case "Prpr":
		  switch (property.Key)
		    {
		    case "FlIn":
		      return new SetFileInfoEvent(this);
		    case "Grdn":
		      return new SetGradientEvent(this);
		    case "Lefx":
		      return new SetLayerEffectsEvent(this);
		    case "QucM":
		      return new SetQuickMaskEvent(this);
		    default:
		      Console.WriteLine("SetEvent.Prpr: " + property.Key);
		      break;
		    }
		  break;
		case "HstS":
		  return new SetHistoryStateEvent(this);
		  break;
		default:
		  Console.WriteLine("SetEvent.Parse: " + property.ClassID2);
		  break;
		}
	    }
	  else if (_obj.Set[0] is EnmrType)
	    {
	      EnmrType enmr = _obj.Set[0] as EnmrType;
	      switch (enmr.Key)
		{
		case "AdjL":
		  return new SetAdjustmentLayerEvent(this);
		case "Brsh":
		  return new SetBrushEvent(this);
		case "Chnl":
		  return new SetChannelPropertyEvent(this);
		case "contentLayer":
		  return new SetContentLayerEvent(this);
		case "Lyr":
		  return new SetLayerPropertyEvent(this);
		case "TxLr":
		  return new SetTextLayerPropertyEvent(this);
		default:
		  Console.WriteLine("SetEvent.Parse-1: unknown key " + 
				    enmr.Key);
		  break;
		}
	    }
	  else if (_obj.Set[0] is IndexType)
	    {
	      var index = _obj.Set[0] as IndexType;
	      switch (index.Key)
		{
		case "Chnl":
		  return new SetChannelToSelectionEvent(this, index.Index);
		default:
		  Console.WriteLine("SetEvent.Parse-2: unknown key " + 
				    index.Key);
		  break;
		}
	    }
	  else if (_obj.Set[0] is NameType)
	    {
	      var name = _obj.Set[0] as NameType;
	      switch (name.ClassID2)
		{
		case "Chnl":
		  return new SetChannelByNameToSelectionEvent(this, name.Key);
		case "Lyr":
		  return new SetLayerPropertyEvent(this, name.Key);
		default:
		  Console.WriteLine("SetEvent.Parse-3: unknown class " + 
				    name.ClassID2);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("SetEvent.Parse: {0} unknown type",
				_obj.Set[0]);
	    }
	}

      return this;
    }
  }
}
