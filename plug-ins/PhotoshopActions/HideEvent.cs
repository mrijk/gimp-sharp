// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// HideEvent.cs
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
  public class HideEvent : ActionEvent
  {
    public HideEvent()
    {
    }
    
    override public ActionEvent Parse(ActionParser parser)
    {
      parser.ParseToken("null");
      parser.ParseFourByteString("VlLs");

      int numberOfItems = parser.ReadInt32();

      parser.ParseFourByteString("obj");
      parser.ParseInt32(1);
      parser.ParseFourByteString("Enmr");

      string classID = parser.ReadTokenOrUnicodeString();
      Console.WriteLine("\tClassID: " + classID);

      string keyID = parser.ReadTokenOrString();
      if (keyID == "Lyr")
	{
	  parser.ParseToken("Ordn");
	  parser.ParseToken("Trgt");
	  // return new DeleteLayerEvent().Parse(parser);
	}
      else
	{
	  Console.WriteLine("Can't hide: " + keyID);
	  throw new GimpSharpException();
	}

      return this;
    }
  }
}
