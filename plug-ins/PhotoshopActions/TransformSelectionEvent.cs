// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// TransformSelectionEvent.cs
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
  public class TransformSelectionEvent : TransformEvent
  {
    public override bool IsExecutable
    {
      get {return false;}
    }

    public TransformSelectionEvent(TransformEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Selection";}
    }

    protected override IEnumerable ListParameters()
    {
      ObjcParameter objc = Parameters["Ofst"] as ObjcParameter;
      if (objc != null)
	{
	  yield return "Offset";
	}

      DoubleParameter width = Parameters["Wdth"] as DoubleParameter;
      if (width != null)
	{
	  yield return "Width: " + width.Value;
	}

      DoubleParameter height = Parameters["Hght"] as DoubleParameter;
      if (height != null)
	{
	  yield return "Hght: " + height.Value;
	}
    }
  }
}
