// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// CreateFunc.cs
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

namespace Gimp.SliceTool
{
  public class CreateFunc : MouseFunc
  {
    static readonly Cursor _cursor;

    PreviewRenderer _renderer;
    SliceData _sliceData;
    Rectangle _rectangle, _endRectangle;
    Slice _slice;
    int _x, _y;
    bool _horizontal;

    public CreateFunc(SliceData sliceData, Preview preview) : 
      base(preview, true, true)
    {
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    static CreateFunc()
    {
      _cursor = LoadCursor("cursor-slice.png");
    }

    override protected void OnPress(IntCoordinate c) 
    {
      _x = c.X;
      _y = c.Y;
      _rectangle = _sliceData.FindRectangle(c);
      _slice = _rectangle.CreateHorizontalSlice(_y);
      _horizontal = true;
      _renderer.Function = Gdk.Function.Equiv;
      _slice.Draw(_renderer);
    }

    override protected void OnRelease() 
    {
      _slice.Draw(_renderer);
      _sliceData.AddSlice(_slice);
      _renderer.Function = Gdk.Function.Copy;
      Redraw();
    }
		
    override protected void OnMove(IntCoordinate c) 
    {
      Rectangle rectangle;
      int x = c.X;
      int y = c.Y;

      if (_horizontal)
	{
	  rectangle = _sliceData.FindRectangle(new IntCoordinate(x, _y));
	}
      else
	{
	  rectangle = _sliceData.FindRectangle(new IntCoordinate(_x, y));
	}

      bool rectangleChanged = rectangle != _endRectangle;
      if (rectangleChanged)
	{
	  _endRectangle = rectangle;
	}

      bool orientationChanged = _horizontal ^ ((Math.Abs(x - _x) > 
						Math.Abs(y - _y)));
      if (orientationChanged)
	{
	  _horizontal = !_horizontal;
	}

      if (orientationChanged || rectangleChanged)
	{
	  _slice.Draw(_renderer);
	  if (_horizontal)
	    {
	      if (rectangle.Left.X <= _rectangle.Left.X)
		{
		  _slice = new HorizontalSlice(rectangle.Left, 
					       _rectangle.Right, _y);
		}
	      else
		{
		  _slice = new HorizontalSlice(_rectangle.Left, 
					       rectangle.Right, _y);
		}
	    }
	  else
	    {
	      if (rectangle.Top.Y <= _rectangle.Top.Y)
		{
		  _slice = new VerticalSlice(rectangle.Top, 
					     _rectangle.Bottom, _x);
		}
	      else
		{
		  _slice = new VerticalSlice(_rectangle.Top, 
					     rectangle.Bottom, _x);
		}
	    }
	  _slice.Draw(_renderer);
	}
    }

    override public Cursor GetCursor(IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) ? slice.Cursor : _cursor;
    }

    override public MouseFunc GetActualFunc(SliceTool parent, 
					    IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) 
	? new SelectFunc(parent, _sliceData) : (MouseFunc) this;
    }
  }
}
