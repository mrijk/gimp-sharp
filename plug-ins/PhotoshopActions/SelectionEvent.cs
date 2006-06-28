// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SelectionEvent.cs
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
  public class SelectionEvent : ActionEvent
  {
    [Parameter("T")]
    Parameter parameter;

    public SelectionEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
    }
    
    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Selection";}
    }

    override public bool Execute()
    {
      if (parameter is EnumParameter)
	{
	  string type = (parameter as EnumParameter).Value;

	  switch (type)
	    {
	    case "Al":
	      ActiveImage.Selection.All();
	      break;
	    case "None":
	      // ActiveImage.Selection.None();
	      break;
	    default:
	      Console.WriteLine("SelectionEvent: " + type);
	      return false;
	      break;
	    }
	}
      else if (parameter is ObjcParameter)
	{
	  ObjcParameter objc = parameter as ObjcParameter;
	  string classID2 = objc.ClassID2;
	  switch (classID2)
	    {
	    case "Rctn":
	      ParameterSet parameters = objc.Parameters;
	      double top = (parameters["Top"] as DoubleParameter).Value;
	      double left = (parameters["Left"] as DoubleParameter).Value;
	      double bottom = (parameters["Btom"] as DoubleParameter).Value;
	      double right = (parameters["Rght"] as DoubleParameter).Value;

	      double x = left * ActiveImage.Width / 100;
	      double y = top * ActiveImage.Height / 100;
	      double width = (right - left) * ActiveImage.Width / 100 + 1;
	      double height = (bottom - top) * ActiveImage.Height / 100 + 1;

	      RectangleSelectTool tool = new RectangleSelectTool(ActiveImage);
	      tool.Select(x, y, width, height, ChannelOps.Replace, false, 0);

	      break;
	    default:
	      Console.WriteLine("SelectionEvent Implement " + classID2);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("Hm");
	}
      return true;
    }
  }
}
