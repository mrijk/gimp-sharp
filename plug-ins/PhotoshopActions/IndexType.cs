// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// IndexParameter.cs
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
  public class IndexType : ReferenceType
  {
    string _classID;
    string _key;
    int _index;

    public int Index
    {
      get {return _index;}
    }

    public override void Parse(ActionParser parser)
    {
      if (!parser.PreSix)
	{
	  _classID = parser.ReadTokenOrUnicodeString();
	}
      _key = parser.ReadTokenOrString();
      _index = parser.ReadInt32();

      DebugOutput.Dump("Index: c = {0}, k = {1}, i = {2}", _classID, _key,
		       _index); 
    }
  }
}
