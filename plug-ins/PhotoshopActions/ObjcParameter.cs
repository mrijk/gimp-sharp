// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class ObjcParameter : Parameter
  {
    string _classID;
    string _classID2;
    ParameterSet _children = new ParameterSet();

    public string ClassID2 => _classID2;

    public ParameterSet Parameters => _children;

    public bool Contains(string name) => _children[name] != null;

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

    public override IEnumerable<string> Format()
    {
      if (Name != null)
	{
	  yield return String.Format("{0}: {1}", UppercaseName, 
				     Abbreviations.Get(ClassID2));
	}
      foreach (var child in _children)
	{
	  foreach (var s in child.Format())
	    {
	      yield return s;
	    }
	}
    }

    public void Fill(Object obj)
    {
      _children.Fill(obj);
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }

    public IEnumerable ListParameters()
    {
      foreach (Parameter child in _children)
	{
	  yield return child.Format();
	}
    }

    public RGB GetColor()
    {
      switch (ClassID2)
	{
	case "CMYC":
	  double cyan = GetValueAsDouble("Cyn") / 255.0;
	  double magenta = GetValueAsDouble("Mgnt") / 255.0;
	  double yellow = GetValueAsDouble("Ylw") / 255.0;
	  double black = GetValueAsDouble("Blck") / 255.0;

	  return new RGB(new CMYK(cyan, magenta, yellow, black));
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
	  return new RGB(0, 0, 0);
	}
    }

    public Gradient GetGradient()
    {
      var name = GetValueAsString("Nm");
      var colors = Parameters["Clrs"] as ListParameter;
      return GradientClassEvent.CreateGradient(name, colors);
    }

    public RGB GetValueAsColor(string name) =>
      (_children[name] as ObjcParameter).GetColor();

    public double GetValueAsDouble(string name) =>
      (_children[name] as DoubleParameter).Value;

    public long GetValueAsLong(string name) =>
      (_children[name] as LongParameter).Value;

    public string GetValueAsString(string name)
    {
      var parameter = _children[name];

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
