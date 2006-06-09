// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// DuplicateEvent.cs
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
  public class DuplicateEvent : ActionEvent
  {
    string _name;

    public DuplicateEvent()
    {
    }
    
    public override bool IsExecutable
    {
      get 
	{
	  return false;
	}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      parser.ParseToken("null");
      parser.ParseFourByteString("obj");

      int numberOfItems = parser.ReadInt32();

      parser.ParseFourByteString("Enmr");

      string classID = parser.ReadTokenOrUnicodeString();
      Console.WriteLine("\tClassID: " + classID);

      string keyID = parser.ReadTokenOrString();
      Console.WriteLine("\tkeyID: " + keyID);

      if (keyID == "Dcmn")
	{
	  return new DuplicateDocumentEvent(this).Parse(parser);
	}
      else if (keyID == "Lyr")
	{
	  return new DuplicateLayerEvent(this).Parse(parser);
	}
      else
	{
	  Console.WriteLine("DuplicateEvent: {0} not implemented", keyID);
	  throw new GimpSharpException();
	}

      return this;
    }
  }
}
