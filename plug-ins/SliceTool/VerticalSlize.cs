using System;

namespace Gimp.SliceTool
{
	public class VerticalSlice : Slice
	{
		public VerticalSlice(Slice top, Slice bottom, int x) : base(top, bottom, x)
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

		override public void SetPosition(int x, int y)
		{
			X = x;
		}

		override public bool PointOn(int x, int y)
		{
			return y >= Y1 && y <= Y2 && Math.Abs(x - X) < 5;
		}
 
		public int X
		{
			get {return Position;}
			set {Position = value;}
		}

		public int Y1
		{
			get {return _begin.Position;}
		}

		public int Y2
		{
			get {return _end.Position;}
		}
	}
}