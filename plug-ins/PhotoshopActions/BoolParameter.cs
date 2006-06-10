// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class BoolParameter : Parameter
  {
    bool _value;

    public bool Value
    {
      get {return _value;}
    }

    public override void Parse(ActionParser parser)
    {
      _value = (parser.ReadByte() == 0) ? false : true;
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, _value);
    }
  }
}
