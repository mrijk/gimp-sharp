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
      ActionEvent.Image = image;
      ActionEvent.Drawable = drawable;
    }

    public int ParsingFailed
    {
      get {return _parsingFailed;}
    }

    public ActionSet Parse(string fileName)
    {
      _binReader = new BinaryReader(File.Open(fileName, FileMode.Open));
      try 
	{
	  int version = ReadInt32();
	  if (version != 16)	//  && version != 12)
	    {
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
	      return null;
	    }

	  Console.WriteLine("\tEventName: " + eventName);
	  
	  ActionEvent actionEvent = _map.Lookup(eventName);
	  actionEvent.EventForDisplay = ReadString();

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

    public RGB ReadRGB()
    {
      double red = ReadDouble("Rd") / 255.0;
      double green = ReadDouble("Grn") / 255.0;
      double blue = ReadDouble("Bl") / 255.0;
      return new RGB(red, green, blue);
    }

    public RGB ReadHSBC()
    {
      string units;
      double hue = ReadDouble("H", out units);
      double saturation = ReadDouble("Strt");
      double brightness = ReadDouble("Brgh");
      return new RGB(new HSV(hue, saturation, brightness));
    }

    public RGB ReadColor()
    {
      ParseFourByteString("Objc");
      /* string classID = */ ReadUnicodeString();
      string classID2 = ReadTokenOrString();
      ParseInt32(3);

      if (classID2 == "RGBC")
	{
	  return ReadRGB();
	}
      else if (classID2 == "HSBC")
	{
	  return ReadHSBC();
	}
      else
	{
	  Console.WriteLine("*** Color {0} not supported", classID2);
	  throw new GimpSharpException();
	}
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

    public Objc ParseObjc()
    {
      Objc objc = new Objc();
      objc.Parse(this);
      return objc;
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
      Console.WriteLine("\t\tkey: " + key);
		
      string type = ReadFourByteString();
      Console.WriteLine("\t\ttype: " + type);

      Parameter parameter = null;

      if (type == "UntF")
	{
	  parameter = new DoubleParameter(true);
	}
      else if (type == "bool")
	{
	  parameter = new BoolParameter();
	}
      else if (type == "doub")
	{
	  parameter = new DoubleParameter(false);
	}
      else if (type == "enum")
	{
	  parameter = new EnumParameter();
	}
      else if (type == "obj")
	{
	  parameter = new ReferenceParameter();
	}
      else if (type == "VlLs")
	{
	  parameter = new ListParameter();
	}
      else if (type == "long")
	{
	  parameter = new LongParameter();
	}
      else if (type == "TEXT")
	{
	  parameter = new TextParameter();
	}
      else if (type == "Objc")
	{
	  parameter = new ObjcParameter();
	}
      else if (type == "type")
	{
	  parameter = new TypeParameter();
	}
      else
	{
	  Console.WriteLine("ReadItem: type {0} unknown!", type);
	  throw new GimpSharpException();
	}

      parameter.Parse(this);
      parameter.Name = key;

      return parameter;
    }

    public void ParseClss()
    {
      string classID = ReadTokenOrUnicodeString();
      Console.WriteLine("\t\tClss:classID: " + classID);

      string classID2 = ReadTokenOrString();
      Console.WriteLine("\t\tClss:classID2: " + classID2);
    }

    public string ParseEnmr()
    {
      string classID = ReadTokenOrUnicodeString();
      Console.WriteLine("\t\tclassID: " + classID);
      
      string keyID = ReadTokenOrString();
      Console.WriteLine("\t\tkeyID: " + keyID);
      
      string typeID = ReadTokenOrString();
      Console.WriteLine("\t\ttypeID: " + typeID);
      
      string val = ReadTokenOrString();
      Console.WriteLine("\t\tvalue: " + val);

      return keyID;
    }

    public void ParseProp()
    {
      string classID = ReadTokenOrUnicodeString();
      Console.WriteLine("\t\tprop:classID: " + classID);

      string classID2 = ReadTokenOrString();
      Console.WriteLine("\t\tprop:classID2: " + classID2);

      string keyID = ReadTokenOrString();
      Console.WriteLine("\t\tprop:keyID: " + keyID);
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
