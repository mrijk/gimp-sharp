// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ActionParser.cs
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
using System.IO;
using System.Text;

namespace Gimp.PhotoshopActions
{
  public class ActionParser
  {
    BinaryReader _binReader;

    public ActionParser()
    {
    }

    public ActionSet Parse(string fileName)
    {
      _binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
      try 
	{
	  int version = ReadInt32();
	  if (version != 16 && version != 12)
	    {
	      return null;
	    }

	  ActionSet actions = new ActionSet(ReadUnicodeString());

	  actions.Expanded = ReadByte();
	  actions.SetChildren = ReadInt32();

	  Action action = ReadAction();
	  if (action == null)
	    {
	      return null;
	    }

	  actions.Add(action);
	  return actions;
	}
      catch(EndOfStreamException e)
	{	
	  Console.WriteLine("{0} caught.", e.GetType().Name);
	}
      finally
	{
	  _binReader.Close();
	}
      return null;
    }

    Action ReadAction()
    {
      Action action = new Action();

      int index = ReadInt16();
      Console.WriteLine("Index: " + index);
      
      byte shiftKey = ReadByte();
      Console.WriteLine("ShiftKey: " + shiftKey);
      
      byte commandKey = ReadByte();
      Console.WriteLine("CommandKey: " + commandKey);
      
      int colorIndex = ReadInt16();
      Console.WriteLine("ColorIndex: " + colorIndex);

      action.Name = ReadUnicodeString();
      Console.WriteLine("ActionName: " + action.Name);
      
      action.Expanded = ReadByte();
      Console.WriteLine("Expanded: " + action.Expanded);
      
      int children = ReadInt32();
      Console.WriteLine("Children: " + children);

      return action;
    }

    byte ReadByte()
    {
      return _binReader.ReadByte();
    }

    int ReadInt16()
    {
      byte[] val = _binReader.ReadBytes(2);
      
      return val[1] + 256 * val[0];
    }

    int ReadInt32()
    {
      byte[] val = _binReader.ReadBytes(4);
      
      return val[3] + 256 * (val[2] + 256 * (val[1] + 256 * val[0]));
    }

    string ReadUnicodeString()
    {
      int length = ReadInt32();
      byte[] buffer = _binReader.ReadBytes(2 * length);
      
      for (int i = 0; i < 2 * length; i += 2)
	{
	  byte tmp = buffer[i];
	  buffer[i] = buffer[i + 1];
	  buffer[i + 1] = tmp;
	}
      Encoding encoding = Encoding.Unicode;
      return encoding.GetString(buffer);
    }
  }
}
