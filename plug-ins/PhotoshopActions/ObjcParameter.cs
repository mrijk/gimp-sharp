// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ObjcParameter.cs
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
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class ObjcParameter : Parameter
  {
    string _classID;
    string _classID2;
    ParameterSet _children = new ParameterSet();

    public string ClassID2
    {
      get {return _classID2;}
    }

    public ParameterSet Parameters
    {
      get {return _children;}
    }

    public override void Parse(ActionParser parser)
    {
      if (parser.PreSix)
	{
	  _classID2 = parser.ReadFourByteString();
	}
      else
	{
	  _classID = parser.ReadUnicodeString();
	  DebugOutput.Dump("ClassID: " + _classID);
	  _classID2 = parser.ReadTokenOrString();
	}

      DebugOutput.Dump("ClassID2: " + _classID2);

      int numberOfItems = parser.ReadInt32();
      DebugOutput.Dump("NumberOfItems: " + numberOfItems);

      DebugOutput.Level++;
      _children.Parse(parser, numberOfItems);
      DebugOutput.Level--;
    }

    public void Fill(Object obj)
    {
      _children.Fill(obj);
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }

    public RGB GetColor()
    {
      switch (ClassID2)
	{
	case "RGBC":
	  double red = GetValueAsDouble("Rd") / 255.0;
	  double green = GetValueAsDouble("Grn") / 255.0;
	  double blue = GetValueAsDouble("Bl") / 255.0;

	  return new RGB(red, green, blue);
	case "HSBC":
	  double hue = GetValueAsDouble("H") / 255.0;
	  double saturation = GetValueAsDouble("Strt") / 255.0;
	  double brightness = GetValueAsDouble("Brgh") / 255.0;

	  return new RGB(new HSV(hue, saturation, brightness));
	default:
	  Console.WriteLine("*** Color model {0} not supported", ClassID2);
	  return null;
	}
    }

    public Gradient GetGradient()
    {
      string name = GetValueAsString("Nm");
      ListParameter colors = Parameters["Clrs"] as ListParameter;
      return GradientClassEvent.CreateGradient(name, colors);
    }

    public RGB GetValueAsColor(string name)
    {
      return (_children[name] as ObjcParameter).GetColor();
    }

    public double GetValueAsDouble(string name)
    {
      return (_children[name] as DoubleParameter).Value;
    }

    public long GetValueAsLong(string name)
    {
      return (_children[name] as LongParameter).Value;
    }

    public string GetValueAsString(string name)
    {
      Parameter parameter = _children[name];

      if (parameter is TextParameter)
	{
	  return (parameter as TextParameter).Value;
	}
      else if (parameter is EnumParameter)
	{
	  return (parameter as EnumParameter).Value;
	}
      else
	{
	  Console.WriteLine("GetValueAsString: " + parameter);
	  return null;
	}
    }
  }
}
