// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SetEvent.cs
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
  public class SetEvent : ActionEvent
  {
    public SetEvent()
    {
    }
    
    override public ActionEvent Parse(ActionParser parser)
    {
      parser.ParseToken("null");
      parser.ParseFourByteString("obj");
      
      int numberOfItems = parser.ReadInt32();
      Console.WriteLine("\tNumberOfItems: " + numberOfItems);

      string type = parser.ReadFourByteString();
      Console.WriteLine("\ttype: " + type);

      if (type == "prop")
	{
	  string classID = parser.ReadTokenOrUnicodeString();
	  Console.WriteLine("\tClassID: " + classID);

	  classID = parser.ReadTokenOrString();
	  Console.WriteLine("\tClassID: " + classID);

	  string keyID = parser.ReadTokenOrString();
	  if (keyID == "fsel")
	    {
	      return new SelectionEvent(this).Parse(parser);
	    }
	  else if (keyID == "BckC")
	    {
	      return new SetBackgroundColorEvent(this).Parse(parser);
	    }
	  else if (keyID == "FrgC")
	    {
	      return new SetForegroundColorEvent(this).Parse(parser);
	    }
	  else
	    {
	      Console.WriteLine("*** Unknown keyID: " + keyID);
	      throw new GimpSharpException();
	    }
	}
      else
	{
	  Console.WriteLine("*** Unknown type: " + type);
	}

      return this;
    }
  }
}
