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
      
      action.ShiftKey = ReadByte();
      action.CommandKey = ReadByte();
      action.ColorIndex = ReadInt16();
      action.Name = ReadUnicodeString();
      action.Expanded = ReadByte();
      
      int children = ReadInt32();
      Console.WriteLine("Children: " + children);

      for (int i = 0; i < 2; i++)
	{
	  ActionEvent actionEvent = ReadActionEvent();
	  if (actionEvent != null)
	    {
	      action.Add(actionEvent);
	    }
	}

      return action;
    }

    ActionEvent ReadActionEvent()
    {
      byte expanded = ReadByte();
      byte enabled = ReadByte();
      byte withDialog = ReadByte();
      byte dialogOptions = ReadByte();

      char[] identifier = _binReader.ReadChars(4);
      if (identifier[0] != 'T' || identifier[1] != 'E' ||
	  identifier[2] != 'X' || identifier[3] != 'T')
	{
	  Console.WriteLine("Couldn't parse event: " + identifier);
	  return null;
	}

      string eventName = ReadString();

      string eventForDisplay = ReadString();
      Console.WriteLine("\tEventForDisplay: " + eventForDisplay);
      
      int hasDescriptor = ReadInt32();
      Console.WriteLine("\tHasDescriptor: " + hasDescriptor);
      
      string classID = ReadUnicodeString();
      Console.WriteLine("\tClassID: " + classID);

      ActionEvent actionEvent;

      int length = ReadInt32();
      if (length == 0)
	{
	  string classID2 = ReadFourByteString();
	  Console.WriteLine("\tClassID2: " + classID2);

	  // TODO: this should be put into a hash table
	  if (classID2 == "Stop")
	    {
	      actionEvent = new StopEvent();
	    }
	  else if (classID2 == "Mk  ")
	    {
	      actionEvent = new MakeEvent();
	    }
	  else
	    {
	      Console.WriteLine("Event unsupported");
	      return null;
	    }

	  int numberOfItems = ReadInt32();
	  Console.WriteLine("\tNumberOfItems: " + numberOfItems);
	}
      else
	{
	  Console.WriteLine("*** length != 0: not implemented yet");
	  return null;
	}

      // TODO: set expanded, enabled, withDialog, dialogOptions
      actionEvent.EventForDisplay = eventForDisplay;

      actionEvent.Parse(this);

      return actionEvent;
    }

    public byte ReadByte()
    {
      return _binReader.ReadByte();
    }

    int ReadInt16()
    {
      byte[] val = _binReader.ReadBytes(2);
      
      return val[1] + 256 * val[0];
    }

    public int ReadInt32()
    {
      byte[] val = _binReader.ReadBytes(4);
      
      return val[3] + 256 * (val[2] + 256 * (val[1] + 256 * val[0]));
    }

    public string ReadFourByteString()
    {
      byte[] buffer = _binReader.ReadBytes(4);
      Encoding encoding = Encoding.ASCII;
      return encoding.GetString(buffer);
    }

    string ReadString()
    {
      int length = ReadInt32();
      byte[] buffer = _binReader.ReadBytes(length);
      Encoding encoding = Encoding.ASCII;
      return encoding.GetString(buffer);
    }

    public string ReadUnicodeString()
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
