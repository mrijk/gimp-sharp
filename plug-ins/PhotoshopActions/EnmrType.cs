// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// EnmrParameter.cs
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
  public class EnmrType : ReferenceType
  {
    public string ClassID {get; private set;}
    public string Key {get; private set;}
    public string Type {get; private set;}
    public string Value {get; private set;}
    
    public override void Parse(ActionParser parser)
    {
      if (!parser.PreSix)
	{
	  ClassID = parser.ReadTokenOrUnicodeString();
	}
      Key = parser.ReadTokenOrString();
      Type = parser.ReadTokenOrString();
      Value = parser.ReadTokenOrString();

      DebugOutput.Dump("Enmr: c = {0}, k = {1}, t = {2}, v = {3}",
		       ClassID, Key, Type, Value);
    }
  }
}
