// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// ListParameter.cs
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
  public class ListParameter : Parameter
  {
    readonly List<Parameter> _set = new List<Parameter>();

    public List<Parameter> Set => _set;

    public Parameter this[int index] => Set[index];

    public IEnumerator<Parameter> GetEnumerator() => Set.GetEnumerator();

    public int Count => Set.Count;

    public override void Parse(ActionParser parser)
    {
      int number = parser.ReadInt32();
      DebugOutput.Dump("number: " + number);

      for (int i = 0; i < number; i++)
	{
	  string type = parser.ReadFourByteString();
	  DebugOutput.Dump("type: " + type);

	  Parameter parameter = null;

	  switch (type)
	    {
	    case "Objc":
	      parameter = new ObjcParameter();
	      break;
	    case "long":
	      parameter = new LongParameter();
	      break;
	    case "obj":
	      parameter = new ReferenceParameter();
	      break;
	    default:
	      Console.WriteLine("ReadVlLs: type {0} unknown!", type);
	      return;
	    }

	  if (parameter != null)
	    {
	      DebugOutput.Level++;
	      parameter.Parse(parser);
	      DebugOutput.Level--;
	      Set.Add(parameter);
	    }
	}
    }

    public override IEnumerable<string> Format()
    {
      yield return $"{UppercaseName}: list (fix me!)";
      foreach (var parameter in Set)
	{
	  foreach (var s in parameter.Format())
	    {
	      yield return s;
	    }
	}
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }
  }
}
