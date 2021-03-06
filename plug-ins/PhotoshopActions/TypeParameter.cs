// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// TypeParameter.cs
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
  public class TypeParameter : Parameter
  {
    public string Type {get; private set;}
    public string Value {get; private set;}

    public override void Parse(ActionParser parser)
    {
      if (parser.PreSix)
	{
	  Value = parser.ReadFourByteString();
	}
      else
	{
	  Type = parser.ReadUnicodeString();
	  Value = parser.ReadTokenOrString();
	}
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      if (CheckFillType(field))
	{
	  field.SetValue(obj, this);
	}
    }

    public override IEnumerable<string> Format()
    {
      yield return $"{UppercaseName}: {Abbreviations.Get(Value)}";
    }
  }
}
