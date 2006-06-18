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
    EventMap _map = new EventMap();

    int _parsingFailed;

    public ActionParser(Image image, Drawable drawable)
    {
      ActionEvent.ActiveImage = image;
      ActionEvent.ActiveDrawable = drawable;
    }

    public int ParsingFailed
    {
      get {return _parsingFailed;}
    }

    public void DumpStatistics()
    {
      _map.DumpStatistics();
    }

    public ActionSet Parse(string fileName)
    {
      _binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
      try 
	{
	  int version = ReadInt32();
	  if (version != 16)	//  && version != 12)
	    {
	      Console.WriteLine("Old version not supported");
	      _parsingFailed++;
	      return null;
	    }

	  ActionSet actions = new ActionSet(ReadUnicodeString());

	  actions.Expanded = ReadByte();
	  actions.SetChildren = ReadInt32();

	  for (int i = 0; i < actions.SetChildren; i++)
	    {
	      Action action = ReadAction();
	      if (action == null)
		{
		  return null;
		}
	      actions.Add(action);
	    }

	  return actions;
	}
      catch (Exception e)
	{	
	  Console.WriteLine("{0} caught.", e.GetType().Name);
	  Console.WriteLine(e.StackTrace);
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
      
      action.NrOfChildren = ReadInt32();
      Console.WriteLine("{0} ({1})", action.Name, action.NrOfChildren);

      for (int i = 0; i < action.NrOfChildren; i++)
	{
	  ActionEvent actionEvent = ReadActionEvent();
	  if (actionEvent != null)
	    {
	      action.Add(actionEvent);
	    }
	  else
	    {
	      _parsingFailed++;
	      break;
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

      try 
	{
	  string text = ReadFourByteString();
	  string eventName;

	  bool preSix;

	  if (text == "TEXT")
	    {
	      eventName = ReadString();
	      preSix = false;
	    }
	  else if (text == "long")
	    {
	      eventName = ReadFourByteString();
	      preSix = true;
	    }
	  else
	    {
	      Console.WriteLine("Unknow text: " + text);
	      return null;
	    }
	  
	  ActionEvent actionEvent = _map.Lookup(eventName);
	  actionEvent.EventForDisplay = ReadString();

	  Console.WriteLine("\tEventName: {0} ({1})", eventName,
			    actionEvent.IsExecutable);

	  int hasDescriptor = ReadInt32();
	  if (hasDescriptor != -1)
	    {
	      Console.WriteLine("\tHasDescriptor: " + hasDescriptor);
	      return actionEvent;
	    }

	  if (preSix == false)
	    {
	      string classID = ReadUnicodeString();
	      Console.WriteLine("\tClassID: " + classID);
	      
	      string classID2 = ReadTokenOrString();
	      Console.WriteLine("\tClassID2: " + classID2);
	    }

	  actionEvent.PreSix = preSix;
	  actionEvent.NumberOfItems = ReadInt32();
	  Console.WriteLine("\tNumberOfItems: " + actionEvent.NumberOfItems);
	  
	  actionEvent = actionEvent.Parse(this);

	  return actionEvent;
	} 
      catch (GimpSharpException e)
	{
	  Console.WriteLine("Parsing failed");
	  return null;
	}
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

    public int ReadLong(string expected)
    {
      ParseToken(expected);
      return ReadLong();
    }

    public int ReadLong()
    {
      ParseFourByteString("long");
      return ReadInt32();
    }

    public double ReadDouble()
    {
      byte[] buffer = new byte[8];
      for (int i = 0; i < 8; i++)
	{
	  buffer[7 - i] = ReadByte();
	}
      MemoryStream memoryStream = new MemoryStream(buffer);
      BinaryReader reader = new BinaryReader(memoryStream);
      return reader.ReadDouble();
    }

    public double ReadDouble(string expected, out string units)
    {
      ParseToken(expected);
      ParseFourByteString("UntF");
      units = ReadFourByteString();

      return ReadDouble();
    }

    public double ReadDouble(string expected)
    {
      ParseToken(expected);
      ParseFourByteString("doub");
      return ReadDouble();
    }

    public void ParseInt32(int expected)
    {
      int val = ReadInt32();
      if (val != expected)
	{
	  Console.WriteLine("ParseInt32: found: {0}, expected: {1}", val, 
			    expected);
	  throw new GimpSharpException();
	}
    }

    public void ParseToken(string expected)
    {
      int length = ReadInt32();
      if (length == 0)
	{
	  ParseFourByteString(expected);
	}
      else
	{
	  Console.WriteLine("Keylength != 0 not supported yet!");
	  throw new GimpSharpException();
	}
    }

    public string ParseString(string expected)
    {
      ParseToken(expected);
      ParseFourByteString("TEXT");
      return ReadUnicodeString();
    }

    public bool ParseBool(out string name)
    {
      int length = ReadInt32();
      if (length == 0)
	{
	  name = ReadFourByteString();
	}
      else
	{
	  name = ReadString(length);
	}
      ParseFourByteString("bool");
      return (ReadByte() == 0) ? false : true;
    }

    public bool ParseBool(string expected)
    {
      int length = ReadInt32();
      if (length == 0)
	{
	  ParseFourByteString(expected);
	}
      else
	{
	  string result = ReadString(length);
	  if (result != expected)
	    {
	      Console.WriteLine("ParseBool: found: {0}, expected: {1}", result,
				expected);
	      throw new GimpSharpException();
	    }
	}
      ParseFourByteString("bool");
      return (ReadByte() == 0) ? false : true;
    }

    public void ParseFourByteString(string expected)
    {
      string token = ReadFourByteString();
      if (token != expected)
	{
	  Console.WriteLine("***ParseFourByteString Found: {0}, expected: {1}", 
			    token, expected);
	  throw new GimpSharpException();
	}
    }

    public void ReadDescriptor()
    {
      string classID = ReadUnicodeString();
      Console.WriteLine("\tClassID: " + classID);
      
      string classID2 = ReadTokenOrString();
      Console.WriteLine("\tClassID2: " + classID2);

      int numberOfItems = ReadInt32();
      Console.WriteLine("\tNumberOfItems: " + numberOfItems);
	  
      for (int i = 0; i < numberOfItems; i++)
	{
	  ReadItem();
	}
    }

    public Parameter ReadItem()
    {
      string key = ReadTokenOrString();
      string type = ReadFourByteString();

      Console.WriteLine("\t\tkey: {0} ({1})", key, type);

      Parameter parameter = null;

      switch (type)
	{
	case "UntF":
	  parameter = new DoubleParameter(true);
	  break;
	case "bool":
	  parameter = new BoolParameter();
	  break;
	case "doub":
	  parameter = new DoubleParameter(false);
	  break;
	case "enum":
	  parameter = new EnumParameter();
	  break;
	case "obj":
	  parameter = new ReferenceParameter();
	  break;
	case "VlLs":
	  parameter = new ListParameter();
	  break;
	case "long":
	  parameter = new LongParameter();
	  break;
	case "Pth":
	  parameter = new PathParameter();
	  break;
	case "TEXT":
	  parameter = new TextParameter();
	  break;
	case "Objc":
	  parameter = new ObjcParameter();
	  break;
	case "type":
	  parameter = new TypeParameter();
	  break;
	default:
	  Console.WriteLine("ReadItem: type {0} unknown!", type);
	  throw new GimpSharpException();
	  break;
	}

      parameter.Parse(this);
      parameter.Name = key;

      return parameter;
    }

    public void ParseName()
    {
      string classID = ReadTokenOrUnicodeString();
      Console.WriteLine("\t\tname::classID: " + classID);

      string classID2 = ReadTokenOrString();
      Console.WriteLine("\t\tname:classID2: " + classID2);

      string keyID = ReadTokenOrUnicodeString();
      Console.WriteLine("\t\tname:keyID: " + keyID);
    }

    public string ReadFourByteString()
    {
      byte[] buffer = _binReader.ReadBytes(4);
      Encoding encoding = Encoding.ASCII;
      return encoding.GetString(buffer).Trim();
    }

    string ReadString(int length)
    {
      byte[] buffer = _binReader.ReadBytes(length);
      Encoding encoding = Encoding.ASCII;
      return encoding.GetString(buffer);
    }

    public string ReadString()
    {
      return ReadString(ReadInt32());
    }

    public string ReadTokenOrString()
    {
      int length = ReadInt32();
      return (length == 0) ? ReadFourByteString() : ReadString(length);
    }

    public string ReadTokenOrUnicodeString()
    {
      int length = ReadInt32();
      return (length == 0) ? ReadFourByteString() : ReadUnicodeString(length);
    }

    string ReadUnicodeString(int length)
    {
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

    public string ReadUnicodeString()
    {
      return ReadUnicodeString(ReadInt32());
    }
  }
}
