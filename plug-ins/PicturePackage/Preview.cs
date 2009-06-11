// The PicturePackage plug-in
// Copyright (C) 2004-2009 Maurits Rijk
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

using Gtk;
using Gdk;
using Pango;

namespace Gimp.PicturePackage
{
  public class Preview : DrawingArea
  {
    PicturePackage _parent;
    Pixmap _pixmap;
    Pixmap _labelPixmap;
    Gdk.GC _gc;
    int _width, _height;
    int _labelX, _labelY;

    bool _firstTime = true;

    public Preview(PicturePackage parent)
    {
      _parent = parent;

      Realized += OnRealized;
      ExposeEvent += OnExposed;

      var targets = new TargetEntry[]{
	new TargetEntry("image/jpeg", 0, 0),
	new TargetEntry("image/png", 0, 0),
	new TargetEntry("text/plain", 0, 1),
	new TargetEntry("STRING", 0, 2)};

      Gtk.Drag.DestSet(this, DestDefaults.All, targets, 
		       DragAction.Copy | DragAction.Move);

      Events = EventMask.ButtonPressMask;
    }

    void OnExposed(object o, ExposeEventArgs args)
    {
      if (_firstTime)
	{
	  _firstTime = false;
	  _pixmap.DrawRectangle(_gc, true, 0, 0, _width, _height);
	  _parent.RenderLayout();
	  GdkWindow.Cursor = new Cursor(CursorType.Hand2);
	}

      GdkWindow.DrawDrawable(_gc, _pixmap, 0, 0, 0, 0, -1, -1);
      if (_labelPixmap != null)
	{
	  GdkWindow.DrawDrawable(_gc, _labelPixmap, 0, 0, _labelX, _labelY, 
				 -1, -1);
	}
    }

    void OnRealized (object o, EventArgs args)
    {
      _width = WidthRequest;
      _height = HeightRequest;
      _pixmap = new Pixmap(GdkWindow, _width, _height, -1);
      _gc = new Gdk.GC(GdkWindow);
    }

    public void Clear()
    {
      _pixmap.DrawRectangle(_gc, true, 0, 0, _width, _height);
      QueueDraw();
    }

    public Renderer GetRenderer(Layout layout)
    {
      return new PreviewRenderer(this, layout, _pixmap, _gc);
    }

    public void DrawLabel(int position, string label)
    {
      var layout = new Pango.Layout(this.PangoContext);
      layout.FontDescription = FontDescription.FromString ("Tahoma 16");
      layout.SetMarkup(label);

      int width, height;
      layout.GetPixelSize(out width, out height);
      if (width != 0 && height != 0)
	{
	  _labelPixmap = new Pixmap(GdkWindow, width, height, -1);
	  CalculateXandY(position, width, height);
	  _labelPixmap.DrawDrawable(_gc, _pixmap, _labelX, _labelY, 0, 0, 
				    width, height);
	  _labelPixmap.DrawLayout(_gc, 0, 0, layout);

	  QueueDraw();
	}
    }

    void CalculateXandY(int position, int width, int height)
    {
      switch (position)
	{
	case 0:
	  _labelX = (_width - width) / 2;
	  _labelY = (_height - height) / 2;
	  break;
	case 1:
	  _labelX = 0;
	  _labelY = 0;
	  break;
	case 2:
	  _labelX = 0;
	  _labelY = _height - height;
	  break;
	case 3:
	  _labelX = _width - width;
	  _labelY = 0;
	  break;
	case 4:
	  _labelX = _width - width;
	  _labelY = _height - height;
	  break;
	}		
    }
  }
}
