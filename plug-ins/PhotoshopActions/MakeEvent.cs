// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// MakeEvent.cs
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
  public class MakeEvent : ActionEvent
  {
    public MakeEvent()
    {
    }
    
    public override bool IsExecutable
    {
      get 
	{
	  return false;
	}
    }
#if false
    override public ActionEvent Parse(ActionParser parser)
    {
      string token = parser.ReadTokenOrString();
      if (token == "Nw")
	{
	  Objc objc = parser.ParseObjc();
	  
	  if (objc.ClassId2 == "Gd")
	    {
	      return new AddGuideEvent(this).Parse(parser);
	    }
	  else if (objc.ClassId2 == "Dcmn")
	    {
	      return new NewDocumentEvent(this).Parse(parser);
	    }
	  else
	    {
	      Console.WriteLine("MakeEvent: {0} not implemented", 
				objc.ClassId2);
	      throw new GimpSharpException();
	    }
	}
      else if (token == "null")
	{
	  return new AddLayerEvent(this).Parse(parser);
	}
      return this;
    }
#endif
  }
}
