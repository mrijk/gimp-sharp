// The Slice Tool plug-in
// Copyright (C) 2004-2018 Maurits Rijk
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

using static System.Math;

using Gdk;

namespace Gimp.SliceTool
{
  public class CreateFunc : MouseFunc
  {
    static readonly Cursor _cursor;

    Rectangle _rectangle, _endRectangle;
    Slice _slice;
    int _x, _y;
    bool _horizontal;

    public CreateFunc(SliceData sliceData, Preview preview) :
      base(sliceData, preview)
    {
    }

    static CreateFunc()
    {
      _cursor = LoadCursor("cursor-slice.png");
    }

    override protected void OnPress(IntCoordinate c) 
    {
      (_x, _y) = c;

      _rectangle = SliceData.FindRectangle(c);
      _slice = _rectangle.CreateHorizontalSlice(_y);
      _horizontal = true;
      Preview.Renderer.Function = Gdk.Function.Equiv;
      _slice.Draw(Preview.Renderer);
    }

    override protected void OnRelease() 
    {
      _slice.Draw(Preview.Renderer);
      SliceData.AddSlice(_slice);
      Preview.Renderer.Function = Gdk.Function.Copy;
      Redraw();
    }
		
    override protected void OnMove(IntCoordinate c) 
    {
      var coordinate = (_horizontal) ? new IntCoordinate(c.X, _y) 
	: new IntCoordinate(_x, c.Y);
      var rectangle = SliceData.FindRectangle(coordinate);

      bool rectangleChanged = rectangle != _endRectangle;
      _endRectangle = rectangle;

      bool horizontal = Abs(c.X - _x) > Abs(c.Y - _y);
      bool orientationChanged = horizontal != _horizontal;
      _horizontal = horizontal;

      if (orientationChanged || rectangleChanged)
	{
	  _slice.Draw(Preview.Renderer);
	  if (_horizontal)
	    {
	      if (rectangle.X1 <= _rectangle.X1)
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
	      if (rectangle.Y1 <= _rectangle.Y1)
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
	  _slice.Draw(Preview.Renderer);
	}
    }

    override public Cursor GetCursor(IntCoordinate c)
    {
      var slice = SliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) ? slice.Cursor : _cursor;
    }

    override public MouseFunc GetActualFunc(IntCoordinate c)
    {
      return MoveSliceFunc.GetActualFunc(c, this);
    }
  }
}
