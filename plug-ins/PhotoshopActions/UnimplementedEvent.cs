// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// UnimplementedEvent.cs
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
  public class UnimplementedEvent : ActionEvent
  {
    public UnimplementedEvent()
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
      for (int i = 0; i < NumberOfItems; i++)
	{
	  ReadItem(parser);
	}
      return this;
    }

    void ReadItem(ActionParser parser)
    {
      string key = parser.ReadTokenOrString();
      Console.WriteLine("\t\tkey: " + key);
		
      string type = parser.ReadFourByteString();
      Console.WriteLine("\t\ttype: " + type);

      if (type == "UntF")
	{
	  string units = parser.ReadFourByteString();
	  double val = parser.ReadDouble();
	  Console.WriteLine("\t\tval: " + val);
	}
      else if (type == "bool")
	{
	  bool val = (parser.ReadByte() == 0) ? false : true;
	  Console.WriteLine("\t\tval: " + val);
	}
      else if (type == "enum")
	{
	  string typeID = parser.ReadTokenOrUnicodeString();
	  Console.WriteLine("\t\ttypeID: " + typeID);
	  
	  string val = parser.ReadTokenOrString();
	  Console.WriteLine("\t\tvalue: " + val);
	}
      else if (type == "Enmr")
	{
	  parser.ParseEnmr();
	}
      else if (type == "long")
	{
	  int val = parser.ReadInt32();
	  Console.WriteLine("\t\tval: " + val);
	}
      else if (type == "Objc")
	{
	}
      else
	{
	  Console.WriteLine("ReadItem: type {0} unknown!", type);
	  throw new GimpSharpException();
	}
    }

    void ReadDescriptor(ActionParser parser)
    {
      string classID = parser.ReadUnicodeString();
      Console.WriteLine("\tClassID: " + classID);
      
      string classID2 = parser.ReadTokenOrString();
      Console.WriteLine("\tClassID2: " + classID2);
	  
      int numberOfItems = parser.ReadInt32();
      Console.WriteLine("\tNumberOfItems: " + numberOfItems);
	  
      for (int i = 0; i < numberOfItems; i++)
	{
	  ReadItem(parser);
	}
    }
  }
}
