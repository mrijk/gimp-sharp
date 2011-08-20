// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// CoordinatesDisplay.cs
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

using Gtk;

namespace Gimp.SliceTool
{
  public class CoordinatesDisplay : Entry
  {
    public CoordinatesDisplay(Preview preview)
    {
      WidthChars = 16;
      IsEditable = false;
      preview.LeaveNotifyEvent += delegate {Text = "";};
      preview.MotionNotifyEvent += OnShowCoordinates;
    }

    void OnShowCoordinates(object o, MotionNotifyEventArgs args)
    {
      var c = (o as Preview).GetXY(args);
      Text = "x: " + c.X + ", y: " + c.Y;
      args.RetVal = true;
    }
  }
}
