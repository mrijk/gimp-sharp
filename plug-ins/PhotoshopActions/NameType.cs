// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// NameType.cs
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

namespace Gimp.PhotoshopActions
{
  public class NameType : ReferenceType
  {
    public string ClassID {get; private set;}
    public string ClassID2 {get; private set;}
    public string Key {get; private set;}

    public override void Parse(ActionParser parser)
    {
      if (!parser.PreSix)
	{
	  ClassID = parser.ReadTokenOrUnicodeString();
	}
      ClassID2 = parser.ReadTokenOrString();
      Key = parser.ReadTokenOrUnicodeString();

      DebugOutput.Dump("Name: c = {0}, c2 = {1}, k = {2}", ClassID, 
		       ClassID2, Key);
    }

    public override IEnumerable<string> Format()
    {
      yield return String.Format("{0} {1}", Abbreviations.Get(ClassID2), 
				 Abbreviations.Get(Key));
    }
  }
}
