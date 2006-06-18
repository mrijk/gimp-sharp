// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ReferenceParameter.cs
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
  public class ReferenceParameter : Parameter
  {
    List<ReferenceType> _set = new List<ReferenceType>();

    public List<ReferenceType> Set
    {
      get {return _set;}
    }

    public override void Parse(ActionParser parser)
    {
      int number = parser.ReadInt32();

      for (int i = 0; i < number; i++)
	{
	  ReferenceType referenceType = null;

	  string type = parser.ReadFourByteString();

	  switch (type)
	    {
	    case "Clss":
	      referenceType = new ClassType();
	      break;
	    case "Enmr":
	      referenceType = new EnmrType();
	      break;
	    case "indx":
	      referenceType = new IndexType();
	      break;
	    case "name":
	      parser.ParseName();
	      break;
	    case "prop":
	      referenceType = new PropertyType();
	      break;
	    default:
	      Console.WriteLine("ReadObj: type {0} unknown!", type);	      
	      break;
	    }

	  if (referenceType != null)
	    {
	      referenceType.Parse(parser);
	      _set.Add(referenceType);
	    }
	}
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }
  }
}
