// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
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
    bool _needScaling;
    bool _needPosition;

    double _horizontal;
    double _vertical;

    public override bool IsExecutable
    {
      get 
	{
	  DoubleParameter width = Parameters["Wdth"] as DoubleParameter;
	  DoubleParameter height = Parameters["Hght"] as DoubleParameter;
	  
	  _needScaling = width != null || height != null;
	  
	  ObjcParameter position = Parameters["Pstn"] as ObjcParameter;

	  _needPosition = position != null;

	  return _needScaling || _needPosition;  
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
	  yield return width.Format();
	}

      ObjcParameter position = Parameters["Pstn"] as ObjcParameter;
      if (position != null)
	{
	  if (position.Contains("Hrzn"))
	    _horizontal = position.GetValueAsDouble("Hrzn");
	  if (position.Contains("Vrtc"))
	    _vertical = position.GetValueAsDouble("Vrtc");
	  yield return String.Format("Position: {0}, {1}", _horizontal,
				     _vertical);
	}

      BoolParameter relative = Parameters["Rltv"] as BoolParameter;
      if (relative != null)
	{
	  yield return relative.Format("Relative");
	}
    }

    override public bool Execute()
    {
      double newWidth = ActiveDrawable.Width;
      double newHeight = ActiveDrawable.Height;
      double oldWidth = newWidth;
      double oldHeight = newHeight;

      Offset oldOffset = SelectedLayer.Offsets;

      DoubleParameter width = Parameters["Wdth"] as DoubleParameter;
      if (width != null)
	{
	  newWidth = width.GetPixels(SelectedLayer.Width);
	}

      DoubleParameter height = Parameters["Hght"] as DoubleParameter;
      if (height != null)
	{
	  newHeight = width.GetPixels(SelectedLayer.Height);
	}

      if (_needScaling)
	{
	  SelectedLayer.Scale((int) newWidth, (int) newHeight, true);

	  EnumParameter side = Parameters["FTcs"] as EnumParameter;

	  switch (side.Value)
	    {
	    case "Qcs7":
	      SelectedLayer.Offsets = oldOffset;
	      break;
	    case "Qcs5":
	      SelectedLayer.Offsets = 
		new Offset(oldOffset.X + (int) (oldWidth - newWidth), 
			   oldOffset.Y + (int) (oldHeight - newHeight));
	      break;
	    default:
	      Console.WriteLine("FTcs: " + side.Value);
	      break;
	    }
	}

      if (_needPosition)
	{
	  SelectedLayer.Translate((int) _horizontal, (int) _vertical);
	}

      return true;
    }
  }
}
