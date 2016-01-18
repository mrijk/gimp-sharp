// The Count Tool plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Preview.cs
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

using Gdk;
using Gtk;
using Pango;

namespace Gimp.CountTool
{
  public class Preview : ZoomPreview
  {
    readonly CoordinateList<int> _coordinates;

    public Preview(Drawable drawable, CoordinateList<int> coordinates) : 
      base(drawable)
    {
      _coordinates = coordinates;

      PreviewArea area = Area;
      area.Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
	EventMask.PointerMotionHintMask | EventMask.PointerMotionMask |
	EventMask.LeaveNotifyMask;

      ButtonPressEvent += (sender, args) =>
	{
	  // Fix me: calculate real-world coordinates
	  _coordinates.Add(new Coordinate<int>((int) args.Event.X,
					       (int) args.Event.Y));
	};

      ExposeEvent += delegate 
	{
	  var layout = new Pango.Layout(area.PangoContext);
	  layout.FontDescription = FontDescription.FromString("Tahoma 16");

	  int i = 0;
	  foreach (var coordinate in _coordinates)
	  {
	    layout.SetMarkup(String.Format("{0}", i));
	  // Fix me: transfer from real-world coordinates
	    area.GdkWindow.DrawLayout(Style.TextGC(StateType.Normal), 
				      coordinate.X, coordinate.Y, 
				      layout);
	    i++;
	  }
	};
    }
  }
}
