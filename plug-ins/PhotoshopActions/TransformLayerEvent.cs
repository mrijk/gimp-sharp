// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// TransformLayerEvent.cs
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
  public class TransformLayerEvent : TransformEvent
  {
    public override bool IsExecutable
    {
      get 
	{
	  DoubleParameter width = Parameters["Wdth"] as DoubleParameter;
	  return width != null;
	}
    }

    public TransformLayerEvent(TransformEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " current layer";}
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
    }

    override public bool Execute()
    {
      bool needScaling = false;
      double newWidth = ActiveDrawable.Width;
      double newHeight = ActiveDrawable.Height;
      double oldWidth = newWidth;
      double oldHeight = newHeight;
      int oldOffx, oldOffy;

      SelectedLayer.Offsets(out oldOffx, out oldOffy);

      DoubleParameter width = Parameters["Wdth"] as DoubleParameter;
      if (width != null)
	{
	  newWidth = width.GetPixels(SelectedLayer.Width);
	  needScaling = true;
	}

      DoubleParameter height = Parameters["Hght"] as DoubleParameter;
      if (height != null)
	{
	  newHeight = width.GetPixels(SelectedLayer.Height);
	  needScaling = true;
	}

      SelectedLayer.Scale((int) newWidth, (int) newHeight, true);

      if (needScaling)
	{
	  EnumParameter side = Parameters["FTcs"] as EnumParameter;

	  switch (side.Value)
	    {
	    case "Qcs7":
	      SelectedLayer.SetOffsets(oldOffx, oldOffy);
	      break;
	    case "Qcs5":
	      SelectedLayer.SetOffsets(oldOffx + (int) (oldWidth - newWidth), 
				       oldOffy + (int) (oldHeight - newHeight));
	      break;
	    default:
	      Console.WriteLine("FTcs: " + side.Value);
	      break;
	    }
	}

      return true;
    }
  }
}
