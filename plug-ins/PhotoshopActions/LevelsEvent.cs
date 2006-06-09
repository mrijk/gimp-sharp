// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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

namespace Gimp.PhotoshopActions
{
  public class LevelsEvent : ActionEvent
  {
    public LevelsEvent()
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
      if (NumberOfItems == 1)
	{
	  string token;
	  bool val = parser.ParseBool(out token);
	  return this;
	}

      if (NumberOfItems == 2)
	{
	  string token;
	  bool autoBlackWhite = parser.ParseBool(out token);
	  bool autoNeutral = parser.ParseBool(out token);
	  return this;
	}

      parser.ParseToken("Adjs");
      parser.ParseFourByteString("VlLs");
      parser.ParseInt32(1);

      Objc objc = parser.ParseObjc();

      parser.ParseToken("Chnl");
      parser.ParseFourByteString("obj");
      parser.ParseInt32(1);

      parser.ParseFourByteString("Enmr");

      string classID = parser.ReadTokenOrUnicodeString();
      Console.WriteLine("\tClassID: " + classID);

      string keyID = parser.ReadTokenOrString();
      if (keyID == "Chnl")
	{
	  parser.ParseToken("Chnl");
	  parser.ParseToken("Cmps");
	  parser.ParseToken("Inpt");
	  parser.ParseFourByteString("VlLs");
	  parser.ParseInt32(2);
	  Console.WriteLine("\tLevel 1: " + parser.ReadLong());
	  Console.WriteLine("\tLevel 2: " + parser.ReadLong());
	}
      else
	{
	  Console.WriteLine("LevelsEvent: " + keyID);
	  throw new GimpSharpException();
	}

      return this;
    }
  }
}
