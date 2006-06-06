// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// Objc.cs
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

namespace Gimp.PhotoshopActions
{
  public class Objc
  {
    string _classId;
    string _classId2;
    int _numberOfItems;

    public string ClassId
    {
      get {return _classId;}
    }
    
    public string ClassId2
    {
      get {return _classId2;}
    }
    
    public int NumberOfItems
    {
      get {return _numberOfItems;}
    }
    
    public void Parse(ActionParser parser)
    {
      parser.ParseFourByteString("Objc");
      _classId = parser.ReadUnicodeString();
      _classId2 = parser.ReadTokenOrString();
      _numberOfItems = parser.ReadInt32();
    }
  }
}
