// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// ReleType.cs
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
  public class ReleType : ReferenceType
  {
    string _classID;
    string _classID2;
    int _offset;

    public string ClassID
    {
      get {return _classID;}
    }

    public string ClassID2
    {
      get {return _classID2;}
    }

    public int Offset
    {
      get {return _offset;}
    }

    public override void Parse(ActionParser parser)
    {
      if (!parser.PreSix)
	{
	  _classID = parser.ReadTokenOrUnicodeString();
	}
      _classID2 = parser.ReadTokenOrString();
      _offset = parser.ReadInt32();

      DebugOutput.Dump("Rele: c = {0}, c2 = {1}, i = {2}", _classID, 
		       _classID2, _offset);
    }
  }
}
