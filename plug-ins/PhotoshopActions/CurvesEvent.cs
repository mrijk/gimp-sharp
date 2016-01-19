// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// CurvesEvent.cs
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
  public class CurvesEvent : ActionEvent
  {
    [Parameter("Adjs")]
    ListParameter _adjustment;

    protected override IEnumerable ListParameters()
    {
      yield return "Adjustment: curves adjustment list";
      yield return "curves adjustment";
      yield return "Channel: composite channel";
      yield return "Curve: point list";
     
      foreach (var c in GetControlPoints())
	{
	  yield return String.Format($"point: {c.X}, {c.Y}");
	}
    }

    CoordinateList<byte> GetControlPoints()
    {
      var objc = _adjustment[0] as ObjcParameter;
      
      var obj = objc.Parameters["Chnl"] as ReferenceParameter;
      
      var curve = objc.Parameters["Crv"] as ListParameter;

      var controlPoints = new CoordinateList<byte>();

      // Dirty hack. Fixme!
      if (curve == null)
	return controlPoints;
      
      foreach (Parameter parameter in curve)
	{
	  var point = parameter as ObjcParameter;
	  double x = 
	    (point.Parameters["Hrzn"] as DoubleParameter).Value;
	  double y = 
	    (point.Parameters["Vrtc"] as DoubleParameter).Value;
	  
	  controlPoints.Add(new Coordinate<byte>((byte) x, (byte) y));
	}
      
      return controlPoints;
    }

    override public bool Execute()
    {
      if (_adjustment != null)
	{
	  var objc = _adjustment[0] as ObjcParameter;

	  var obj = objc.Parameters["Chnl"] as ReferenceParameter;
	  var origChannel = (obj.Set[0] as EnmrType).Value;

	  var curve = objc.Parameters["Crv"] as ListParameter;
	  
	  var controlPoints = new CoordinateList<byte>();
	  
	  foreach (Parameter parameter in curve)
	    {
	      var point = parameter as ObjcParameter;
	      double x = 
		(point.Parameters["Hrzn"] as DoubleParameter).Value;
	      double y = 
		(point.Parameters["Vrtc"] as DoubleParameter).Value;
	      
	      controlPoints.Add(new Coordinate<byte>((byte) x, (byte) y));
	    }

	  HistogramChannel channel;

	  switch (origChannel)
	    {
	    case "Cmps":
	      channel = HistogramChannel.Value;
	      break;
	    case "Rd":
	      channel = HistogramChannel.Red;
	      break;
	    case "Grn":
	      channel = HistogramChannel.Green;
	      break;
	    case "Bl":
	      channel = HistogramChannel.Blue;
	      break;
	    default: 
	      Console.WriteLine("CurvesEvent: " + origChannel);
	      return false;
	    }
	  
	  ActiveDrawable.CurvesSpline(channel, controlPoints);
	}
      else
	{
	  Console.WriteLine("CurvesEvent: adjustment == null?");
	}
      return true;
    }
  }
}
