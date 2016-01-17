// The Slice Tool plug-in
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

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Preview : PreviewArea
  {
    public PreviewRenderer Renderer {get; set;}

    public MouseFunc Func {private get; set;}

    public Preview(Drawable drawable, SliceData sliceData)
    {
      MaxSize = drawable.Dimensions;

      ExposeEvent += delegate {sliceData.Draw(Renderer);};
      Realized += delegate
	{
	  var gc = new Gdk.GC(GdkWindow);
	  Renderer = new PreviewRenderer(this, gc, drawable.Dimensions);
	  Draw(drawable);
	};
      SizeAllocated += delegate {Draw(drawable);};

      Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
	EventMask.PointerMotionHintMask | EventMask.PointerMotionMask |
	EventMask.LeaveNotifyMask;

      ButtonPressEvent += (o, args) =>
	{
	  var c = new IntCoordinate((int) args.Event.X, (int) args.Event.Y);
	  Func.GetActualFunc(c).OnButtonPress(o, args);
	};

      MotionNotifyEvent += (o, args) =>
	{
	  GdkWindow.Cursor = Func.GetCursor(GetXY(args));
	};

      Func = new SelectFunc(sliceData, this);
    }

    public Toolbox CreateToolbox(SliceData sliceData)
    {
      return new Toolbox(this, sliceData);
    }

    public IntCoordinate GetXY(MotionNotifyEventArgs args)
    {
      int x, y;
      var ev = args.Event;
      
      if (ev.IsHint) 
	{
	  ModifierType s;
	  ev.Window.GetPointer(out x, out y, out s);
	} 
      else 
	{
	  x = (int) ev.X;
	  y = (int) ev.Y;
	}

      return new IntCoordinate(x, y);
    }

    public void SetColors(RGB activeColor, RGB inactiveColor)
    {
      Renderer.ActiveColor = activeColor;
      Renderer.InactiveColor = inactiveColor;
      QueueDraw();
    }
  }
}
