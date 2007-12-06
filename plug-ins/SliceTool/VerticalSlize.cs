// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// VerticalSlice.cs
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
  public class VerticalSlice : Slice
  {
    static Cursor _cursor = new Cursor(CursorType.SbHDoubleArrow);

    public VerticalSlice(Slice top, Slice bottom, int x) : base(top, bottom, x)
    {
    }

    public VerticalSlice()
    {
    }

    public VerticalSlice(int index) : base(index)
    {
    }

    override public void Draw(PreviewRenderer renderer)
    {
      renderer.DrawLine(X, Y1, X, Y2);
    }

    override public bool IntersectsWith(Rectangle rectangle)
    {
      return X > rectangle.X1 && X < rectangle.X2
	&& Y1 <= rectangle.Y1 && Y2 >= rectangle.Y2;
    }

    override public bool IsPartOf(Rectangle rectangle)
    {
      return rectangle.HasVerticalSlice(this);
    }

    override public Rectangle SliceRectangle(Rectangle rectangle)
    {
      Rectangle copy = new Rectangle(rectangle);
      rectangle.Right = this;
      copy.Left = this;
      return copy;		
    }

    override public void SetPosition(Coordinate<int> c)
    {
      X = c.X;
    }

    override public bool PointOn(Coordinate<int> c)
    {
      return c.Y >= Y1 && c.Y <= Y2 && Math.Abs(c.X - X) < 5;
    }
 
    override public void Save(StreamWriter w)
    {
      Save(w, "vertical");
    }

    public int X
    {
      get {return Position;}
      set {Position = value;}
    }

    public int Y1
    {
      get {return Begin.Position;}
    }

    public int Y2
    {
      get {return End.Position;}
    }

    override public Cursor Cursor
    {
      get {return _cursor;}
    }
  }
}
