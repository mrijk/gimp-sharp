// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// FileExistsDialog.cs
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

using Mono.Unix;

using Gtk;

namespace Gimp.SliceTool
{
  public class FileExistsDialog : MessageDialog
  {
    public FileExistsDialog(string filename) : 
      base(null, DialogFlags.DestroyWithParent,
	   MessageType.Warning, ButtonsType.YesNo, 
	   "File " + filename + Catalog.GetString(" already exists!\n") + 
	   Catalog.GetString("Do you want to overwrite this file?"))
    {
    }

    public bool IsYes()
    {
      var response = (ResponseType) Run();
      Destroy();
      return (response == ResponseType.Yes);
    }
  }
}
