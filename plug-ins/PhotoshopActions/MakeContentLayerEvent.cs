// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// MakeContentLayerEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class MakeContentLayerEvent : MakeEvent
  {
    [Parameter("Type")]    
    ObjcParameter _type;
    [Parameter("Clr")]    
    ObjcParameter _clr;

    public MakeContentLayerEvent(MakeEvent srcEvent) : base(srcEvent)
    {
    }

    public override bool IsExecutable
    {
      get {return false;}
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " fill layer";}
    }

    protected override IEnumerable ListParameters()
    {
      ObjcParameter _using = Parameters["Usng"] as ObjcParameter;
      _using.Fill(this);
      _type.Fill(this);
      RGB rgb = _clr.GetColor();

      yield return "Using: fill layer";
      yield return "Type: ";
      yield return "Slot Color: ";
      yield return "Red: " + rgb.R;
      yield return "Green: " + rgb.G;
      yield return "Blue: " + rgb.B;
    }

    override public bool Execute()
    {
      return true;
    }
  }
}
