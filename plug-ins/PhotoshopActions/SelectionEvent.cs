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

    [Parameter("Top")]
    double _top;
    [Parameter("Left")]
    double _left;
    [Parameter("Btom")]
    double _bottom;
    [Parameter("Rght")]
    double _right;

    public SelectionEvent(ActionEvent srcEvent) 
      : base(srcEvent)
    {
      Parameters.Fill(this);
    }
    
    override public bool Execute()
    {
      if (parameter is EnumParameter)
	{
	  string type = (parameter as EnumParameter).Value;

	  if (type == "Al")
	    {
	      ActiveImage.Selection.All();
	    }
	  else
	    {
	      Console.WriteLine("SelectionEvent: " + type);
	      return false;
	    }
	}
      else if (parameter is ObjcParameter)
	{
	  string classID2 = (parameter as ObjcParameter).ClassID2;
	  Console.WriteLine("SelectionEvent Implement " + classID2);
	}
      else
	{
	  Console.WriteLine("Hm");
	}
      return true;
    }
  }
}
