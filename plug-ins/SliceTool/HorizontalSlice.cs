using System;

namespace Gimp.SliceTool
{
	public class HorizontalSlice : Slice
	{
		public HorizontalSlice(Slice left, Slice right, int y) : base(left, right, y)
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
	}
}