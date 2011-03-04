// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// MouseFunc.cs
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

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class MouseFunc
  {
    static readonly Cursor _defaultCursor;

    protected Preview _preview;
    bool _useRelease, _useMove;

    public MouseFunc(Preview preview, bool useRelease, bool useMove)
    {
      _preview = preview;
      _useRelease = useRelease;
      _useMove = useMove;
    }

    static MouseFunc()
    {
      _defaultCursor = LoadCursor("cursor-select.png");
    }

    protected static Cursor LoadCursor(string cursorFile)
    {
      var pixbuf = new Pixbuf(Assembly.GetExecutingAssembly(), cursorFile);
      return new Cursor(Gdk.Display.Default, pixbuf, 0, 0);
    }

    protected void Redraw()
    {
      _preview.QueueDraw();
    }

    virtual protected void OnPress(IntCoordinate c) {}
    virtual protected void OnRelease() {}
    virtual protected void OnMove(IntCoordinate c) {}
    
    virtual public Cursor GetCursor(IntCoordinate c)
    {
      return _defaultCursor;
    }

    virtual public MouseFunc GetActualFunc(SliceTool parent, 
					   IntCoordinate c) 
    {
      return this;
    }

    public void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      if (_useRelease)
	{
	  AddReleaseEvent();
	}

      if (_useMove)
	{
	  AddMotionNotifyEvent();
	}

      OnPress(new IntCoordinate((int) args.Event.X, (int) args.Event.Y));
    }

    protected void AddReleaseEvent()
    {
      _preview.ButtonReleaseEvent += OnButtonRelease;
    }

    protected void AddMotionNotifyEvent()
    {
      _preview.MotionNotifyEvent += OnMotionNotify;
    }

    void OnButtonRelease(object o, ButtonReleaseEventArgs args)
    {
      _preview.MotionNotifyEvent -= OnMotionNotify;
      _preview.ButtonReleaseEvent -= OnButtonRelease;
      OnRelease();
    }

    void OnMotionNotify(object o, MotionNotifyEventArgs args)
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

      OnMove(new IntCoordinate(x, y));
    }

    protected bool SliceIsSelectable(Slice slice)
    {
      return !(slice == null || slice.Locked);
    }
  }
}
