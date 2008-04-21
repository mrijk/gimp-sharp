// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// MakeEvent.cs
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
  public class MakeEvent : ActionEvent
  {
    [Parameter("Nw")]
    Parameter _object;
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public MakeEvent()
    {
    }

    public MakeEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable
    {
      get {return _executable;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_object != null && _object is ObjcParameter)
	{
	  string classID = (_object as ObjcParameter).ClassID2;

	  switch (classID)
	    {
	    case "AdjL":
	      return new AddAdjustmentLayerEvent(this);
	    case "Chnl":
	      return new NewChannelEvent(this, _object as ObjcParameter);
	    case "Dcmn":
	      return new NewDocumentEvent(this, _object as ObjcParameter);
	    case "Gd":
	      return new AddGuideEvent(this, _object as ObjcParameter);
	    case "Lyr":
	      return new AddLayerEvent(this, _object as ObjcParameter);
	    default:
	      Console.WriteLine("MakeEvent-2: {0} not implemented", classID);
	      break;
	    }
	}
      if (_object != null && _object is TypeParameter)
	{
	  return new AddMaskEvent(this);
	}
      else if (_obj != null)
	{
	  ClassType classType = _obj.Set[0] as ClassType;
	  switch (classType.ClassID2)
	    {
	    case "Actn":
	      return new MakeActionEvent(this);
	    case "AdjL":
	      return new AddAdjustmentLayerEvent(this);
	    case "annotation":
	      return new MakeAnnotationEvent(this);
	    case "BckL":
	      return new MakeBackgroundLayerEvent(this);
	    case "contentLayer":
	      return new MakeContentLayerEvent(this);
	    case "layerSection":
	      return new MakeGroupEvent(this);
	    case "Lyr":
	      return new AddLayerEvent(this, _obj.Set);
	    case "Path":
	      return new MakePathEvent(this);
	    case "Ptrn":
	      return new MakePatternEvent(this);
	    case "SnpS":
	      return new MakeSnapshotEvent(this);
	    case "Styl":
	      return new MakeStyleEvent(this);
	    case "TxLr":
	      return new AddTextLayerEvent(this);
	    case "TxLy":
	      return new AddTextLayerEvent(this);
	    default:
	      Console.WriteLine("MakeEvent-1: {0} not implemented", 
				classType.ClassID2);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("MakeEvent: Disaster!");
	}

      return myEvent;
    }
  }
}
