// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// BoolParameter.cs
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
using System.Collections.Generic;
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class BoolParameter : Parameter
  {
    public bool Value {get; private set;}

    public override void Parse(ActionParser parser)
    {
      Value = (parser.ReadByte() == 0) ? false : true;
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, Value);
    }

    public override IEnumerable<string> Format()
    {
      // yield return $"{(Value) ? "With" : "Without"} {Abbreviations.Get(Name)}";
      yield return $"{Abbreviations.Get(Name)}";
    }

    public string Format(string s) => ((Value) ? "With " : "Without ") + s;
  }
}
