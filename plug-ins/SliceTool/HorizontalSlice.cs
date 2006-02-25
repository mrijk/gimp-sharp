// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
//
// HorizontalSlice.cs
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
using System.IO;

using Gdk;

namespace Gimp.SliceTool
{
  public class HorizontalSlice : Slice
  {
    static Cursor _cursor = new Cursor(CursorType.SbVDoubleArrow);

    public HorizontalSlice(Slice left, Slice right, int y) : 
      base(left, right, y)
    {
    }

    public HorizontalSlice()
    {
    }

    public HorizontalSlice(int index) : base(index)
    {
    }

    override public void Draw(PreviewRenderer renderer)
    {
      renderer.DrawLine(X1, Y, X2, Y);
    }

    override public bool IntersectsWith(Rectangle rectangle)
    {
      return Y > rectangle.Y1 && Y < rectangle.Y2
	&& X1 <= rectangle.X1 && X2 >= rectangle.X2;
    }

    override public bool IsPartOf(Rectangle rectangle)
    {
      return rectangle.HasHorizontalSlice(this);
    }

    override public Rectangle SliceRectangle(Rectangle rectangle)
    {
      Rectangle copy = new Rectangle(rectangle);
      rectangle.Bottom = this;
      copy.Top = this;
      return copy;
    }

    override public void SetPosition(int x, int y)
    {
      Y = y;
    }

    override public bool PointOn(int x, int y)
    {
      return x >= X1 && x <= X2 && Math.Abs(y - Y) < 5;
    }
 
    override public void Save(StreamWriter w)
    {
      Save(w, "horizontal");
    }

    public int Y
    {
      get {return Position;}
      set {Position = value;}
    }

    public int X1
    {
      get {return _begin.Position;}
    }

    public int X2
    {
      get {return _end.Position;}
    }

    override public Cursor Cursor
    {
      get {return _cursor;}
    }
  }
}
