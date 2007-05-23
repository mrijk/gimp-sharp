// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// LevelsEvent.cs
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
  public class LevelsEvent : ActionEvent
  {
    [Parameter("Adjs")]
    ListParameter _adjustment;

    protected override IEnumerable ListParameters()
    {
      if (_adjustment != null)
	{
	  ObjcParameter objc = _adjustment[0] as ObjcParameter;
	  if (objc.Contains("Gmm"))
	    {
	      yield return "Gamma: " + objc.GetValueAsDouble("Gmm");
	    }
	  if (objc.Contains("Chnl"))
	    {
	      ReferenceParameter channel = objc.Parameters["Chnl"] as 
		ReferenceParameter;
	      string value = (channel.Set[0] as EnmrType).Value;
	      yield return String.Format("{0}: {1}", 
					 Abbreviations.Get("Chnl"), 
					 Abbreviations.Get(value));
	    }
	}
    }

    override public bool Execute()
    {
      if (Parameters["AuCo"] != null)
	{
	  RunProcedure("plug_in_autostretch_hsv");
	}
      if (Parameters["autoBlackWhite"] != null)
	{
	  Console.WriteLine("Levels:autoBlackWhite not implemented yet");
	}
      if (Parameters["autoNeutrals"] != null)
	{
	  Console.WriteLine("Levels:autoNeutrals not implemented yet");
	}
      if (Parameters["Auto"] != null)
	{
	  ActiveDrawable.LevelsStretch();
	}

      if (_adjustment != null)
	{
	  ObjcParameter objc = _adjustment[0] as ObjcParameter;
	  double gamma = (objc.Parameters["Gmm"] as DoubleParameter).Value;

	  ReferenceParameter obj = objc.Parameters["Chnl"] as 
	    ReferenceParameter;
	  string channel = (obj.Set[0] as EnmrType).Value;

	  if (channel == "Cmps")
	    {
	      ActiveDrawable.Levels(HistogramChannel.Value , 0, 255, gamma, 
				    0, 255);
	    }
	  else
	    {
	      Console.WriteLine("LevelsEvent: " + channel);
	    }
	}

      return true;
    }
  }
}
