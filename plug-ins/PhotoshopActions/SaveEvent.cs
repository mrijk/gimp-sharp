// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// SaveEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class SaveEvent : ActionEvent
  {
    string _path;

    protected override IEnumerable ListParameters()
    {
      Parameter saveAs = Parameters["As"];
      if (saveAs != null)
	{
	  string fileType = "unknown extension";

	  if (saveAs is ObjcParameter)
	    fileType = (saveAs as ObjcParameter).ClassID2;
	  else if (saveAs is TextParameter)
	    fileType = (saveAs as TextParameter).Value;

	  yield return "As: " + Abbreviations.Get(fileType);
	  yield return "Byte Order: ";
	}

      Parameter saveIn = Parameters["In"];
      if (saveIn != null)
	{
	  if (saveIn is AliasParameter)
	    _path = "Fix me: SaveEvent.AliasParameter";
	  else if (saveIn is PathParameter)
	    _path = (saveIn as PathParameter).Path;
	  else
	    _path = "Fix me: unknown path (SaveEvent)";

	  yield return "In: " + _path;
	}
    }

    override public bool Execute()
    {
      string filename = _path + ActiveImage.Filename;
      ActiveImage.Save(RunMode.Noninteractive, filename, filename);
      return true;
    }
  }
}
