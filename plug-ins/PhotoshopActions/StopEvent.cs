// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// StopEvent.cs
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
  public class StopEvent : ActionEvent
  {
    public StopEvent()
    {
    }
    
    override public bool Parse(ActionParser parser)
    {
      int length = parser.ReadInt32();
      Console.WriteLine("\tLength: " + length);

      if (length == 0)
	{
	  // Should be "Msge"
	  string key = parser.ReadFourByteString();
	  Console.WriteLine("\tkey: " + key);

	  // Should be "TEXT"
	  string type = parser.ReadFourByteString();
	  Console.WriteLine("\ttype: " + type);

	  string item = parser.ReadUnicodeString();
	  Console.WriteLine("\titem: " + item);
	}
      else
	{
	  Console.WriteLine("Keylength != 0 not supported yet!");
	  return false;
	}

      length = parser.ReadInt32();
      Console.WriteLine("\tLength: " + length);

      if (length == 0)
	{
	  // Should be "Cntn"
	  string key = parser.ReadFourByteString();
	  Console.WriteLine("\tkey: " + key);

	  // Should be "bool"
	  string type = parser.ReadFourByteString();
	  Console.WriteLine("\ttype: " + type);

	  byte item = parser.ReadByte();
	  Console.WriteLine("\titem: " + item);
	}
      else
	{
	  Console.WriteLine("Keylength != 0 not supported yet!");
	  return false;
	}

      return true;
    }
  }
}
