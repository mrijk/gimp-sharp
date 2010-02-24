// The PhotoshopActions plug-in
// Copyright (C) 2006-2010 Maurits Rijk
//
// OpenEvent.cs
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

using System.Collections;
using System.IO;

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class OpenEvent : ActionEvent
  {
    [Parameter("null")]
    string _path;

    protected override IEnumerable ListParameters()
    {
      yield return _path;
    }

    override public bool Execute()
    {
      Image image = null;
      if (File.Exists(_path)) 
	{
	  image = Image.Load(RunMode.Noninteractive, _path, _path);
	}
      else
	{
	  var choose = new FileChooserDialog("Open...",
					     null,
					     FileChooserAction.Open,
					     "Cancel", ResponseType.Cancel,
					     "Open", ResponseType.Accept);
	  if (choose.Run() == (int) ResponseType.Accept)
	    {
	      string fileName = choose.Filename;
	      image = Image.Load(RunMode.Noninteractive, fileName, fileName);
	    };
	  choose.Destroy();
	}

      if (image != null)
	{
	  image.CleanAll();
	  ActiveDisplay = new Display(image);
	  ActiveImage = image;
	  return true;
	}

      return false;
    }
  }
}
