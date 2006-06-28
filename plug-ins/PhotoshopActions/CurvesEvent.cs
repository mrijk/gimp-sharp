// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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

namespace Gimp.PhotoshopActions
{
  public class CurvesEvent : ActionEvent
  {
    [Parameter("Adjs")]
    ListParameter _adjustment;

    override public bool Execute()
    {
      if (_adjustment != null)
	{
	  ObjcParameter objc = _adjustment[0] as ObjcParameter;

	  ReferenceParameter obj = objc.Parameters["Chnl"] as 
	    ReferenceParameter;
	  string channel = (obj.Set[0] as EnmrType).Value;

	  if (channel == "Cmps")
	    {
	      ListParameter curve = objc.Parameters["Crv"] as ListParameter;

	      byte[] controlPoints = new byte[2 * curve.Count];
	      int i = 0;

	      foreach (Parameter parameter in curve)
		{
		  ObjcParameter point = parameter as ObjcParameter;
		  double x = 
		    (point.Parameters["Hrzn"] as DoubleParameter).Value;
		  double y = 
		    (point.Parameters["Vrtc"] as DoubleParameter).Value;
		  controlPoints[i] = (byte) x;
		  controlPoints[i + 1] = (byte) y;
		  i += 2;
		}
	      ActiveDrawable.CurvesSpline(HistogramChannel.Value, 
					  controlPoints);
	    }
	  else
	    {
	      Console.WriteLine("CurvesEvent: " + channel);
	    }
	}
      else
	{
	  Console.WriteLine("CurvesEvent: adjustment == null?");
	}
      return true;
    }
  }
}
