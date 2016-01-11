// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
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
	  var width = Parameters["Wdth"] as DoubleParameter;
	  var height = Parameters["Hght"] as DoubleParameter;
	  
	  _needScaling = width != null || height != null;
	  
	  var position = Parameters["Pstn"] as ObjcParameter;

	  _needPosition = position != null;

	  return _needScaling || _needPosition;  
	}
    }

    public TransformLayerEvent(TransformEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay =>
      base.EventForDisplay + " current layer";

#if false
    protected override IEnumerable ListParameters()
    {
      var objc = Parameters["Ofst"] as ObjcParameter;
      if (objc != null)
	{
	  yield return "Offset";
	}

      var width = Parameters["Wdth"] as DoubleParameter;
      if (width != null)
	{
	  yield return width.Format();
	}

      var position = Parameters["Pstn"] as ObjcParameter;
      if (position != null)
	{
	  if (position.Contains("Hrzn"))
	    _horizontal = position.GetValueAsDouble("Hrzn");
	  if (position.Contains("Vrtc"))
	    _vertical = position.GetValueAsDouble("Vrtc");
	  yield return $"Position: {_horizontal}, {_vertical}";
	}

      var relative = Parameters["Rltv"] as BoolParameter;
      if (relative != null)
	{
	  yield return relative.Format("Relative");
	}
    }
#endif
    override public bool Execute()
    {
      double newWidth = ActiveDrawable.Width;
      double newHeight = ActiveDrawable.Height;
      double oldWidth = newWidth;
      double oldHeight = newHeight;

      Offset oldOffset = SelectedLayer.Offsets;

      var width = Parameters["Wdth"] as DoubleParameter;
      if (width != null)
	{
	  newWidth = width.GetPixels(SelectedLayer.Width);
	}

      var height = Parameters["Hght"] as DoubleParameter;
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
	    case "Qcsa":
	      Console.WriteLine("TransformLayerEvent: Qcsa not supported yet");
	      break;
	    default:
	      Console.WriteLine("FTcs: " + side.Value);
	      break;
	    }
	}

      Offset();
      Translate();
      Skew();
      Rotate();

      return true;
    }

    void Offset()
    {
      var offset = Parameters["Ofst"] as ObjcParameter;
      if (offset != null)
	{
	  double horizontal = offset.GetValueAsDouble("Hrzn");
	  double vertical = offset.GetValueAsDouble("Vrtc");

	  int h = (int) (horizontal * ActiveDrawable.Width / 100);
	  int v = (int) (vertical * ActiveDrawable.Height / 100);

	  Console.WriteLine($"Offset: {h}, {v}");

	  SelectedLayer.Translate(h, v);
	}
    }

    void Translate()
    {
      if (_needPosition)
	{
	  int horizontal = (int) _horizontal * ActiveDrawable.Width / 100;
	  int vertical = (int) _vertical * ActiveDrawable.Height / 100;
	  SelectedLayer.Translate(horizontal, vertical);
	}
    }

    void Skew()
    {
      var skew = Parameters["Skew"] as ObjcParameter;
      if (skew != null)
	{
	  double horizontal = skew.GetValueAsDouble("Hrzn");
	  double vertical = skew.GetValueAsDouble("Vrtc");

	  Console.WriteLine($"Skew: {horizontal} {vertical}");

	  if (horizontal != 0.0)
	    {
	      double offset = ActiveDrawable.Height * 
		Math.Tan(GetRad(horizontal));
	      SelectedLayer.TransformShear(OrientationType.Horizontal, offset, 
					   true, TransformResize.Adjust);
	    }

	  if (vertical != 0.0)
	    {
	      double offset = ActiveDrawable.Width * Math.Tan(GetRad(vertical));
	      SelectedLayer.TransformShear(OrientationType.Vertical, offset,
					   true, TransformResize.Adjust);
	    }
	}
    }

    void Rotate()
    {
      var angle = Parameters["Angl"] as DoubleParameter;
      if (angle != null)
	{
	  Console.WriteLine("Rotate: " + angle.Value);
	  SelectedLayer.TransformRotate(GetRad(angle.Value), true, 
					0, 0, true, false);
	}
    }

    double GetRad(double d) =>  d * Math.PI /180;
  }
}
