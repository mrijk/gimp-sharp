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

using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class MouseFunc
  {
    static readonly Cursor _defaultCursor;

    public SliceData SliceData {get; private set;}
    public Preview Preview {get; private set;}

    public MouseFunc(SliceData sliceData, Preview preview)
    {
      SliceData = sliceData;
      Preview = preview;
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
      Preview.QueueDraw();
    }

    virtual protected void OnPress(IntCoordinate c) {}
    virtual protected void OnRelease() {}
    virtual protected void OnMove(IntCoordinate c) {}
    
    virtual public Cursor GetCursor(IntCoordinate c)
    {
      return _defaultCursor;
    }

    virtual public MouseFunc GetActualFunc(IntCoordinate c) 
    {
      return this;
    }

    public void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      Preview.ButtonReleaseEvent += OnButtonRelease;
      Preview.MotionNotifyEvent += OnMotionNotify;

      OnPress(new IntCoordinate((int) args.Event.X, (int) args.Event.Y));
    }

    void OnButtonRelease(object o, ButtonReleaseEventArgs args)
    {
      Preview.ButtonReleaseEvent -= OnButtonRelease;
      Preview.MotionNotifyEvent -= OnMotionNotify;

      OnRelease();
    }

    void OnMotionNotify(object o, MotionNotifyEventArgs args)
    {
      OnMove((o as Preview).GetXY(args));
    }

    public bool SliceIsSelectable(Slice slice)
    {
      return !(slice == null || slice.Locked);
    }
  }
}
