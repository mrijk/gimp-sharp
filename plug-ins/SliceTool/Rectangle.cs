using System;
using System.IO;

namespace Gimp.SliceTool
{
  public class Rectangle : IComparable
  {
    VerticalSlice _left, _right;
    HorizontalSlice _top, _bottom;
    
    string _url = "";
    string _altText = "";
    string _target = "";
    bool _include = true;

    public Rectangle(VerticalSlice left, VerticalSlice right,
		     HorizontalSlice top, HorizontalSlice bottom)
    {
      _left = left;
      _right = right;
      _top = top;
      _bottom = bottom;
    }

    public Rectangle(Rectangle rectangle)
    {
      _left = rectangle._left;
      _right = rectangle._right;
      _top = rectangle._top;
      _bottom = rectangle._bottom;
    }

    public int CompareTo(object obj)
    {
      Rectangle rectangle = obj as Rectangle;
      int y1 = Top.Y;
      int y2 = rectangle.Top.Y;
      if (y1 == y2)
	{
	int x1 = Left.X;
	int x2 = rectangle.Left.X;
	return x1 - x2;
	}
      else
	{
	return y1 - y2;
	}
    }

    public bool IntersectsWith(Slice slice)
    {
      return slice.IntersectsWith(this);
    }

    public bool HasHorizontalSlice(HorizontalSlice slice)
    {
      return (slice.Y == Y1 || slice.Y == Y2)
	&& slice.X1 == X1 && slice.X2 == X2;
    }

    public bool HasVerticalSlice(VerticalSlice slice)
    {
      return (slice.X == X1 || slice.X == X2)
	&& slice.Y1 == Y1 && slice.Y2 == Y2;
    }

    public Slice Contains(int x, int y, Slice slice)
    {
      if (slice == Left && y >= Y1 && y <= Y2)
	{
	return new VerticalSlice(Left.X, Y1, Y2);
	}
      else if (slice == Right && y >= Y1 && y <= Y2)
	{
	return new VerticalSlice(Right.X, Y1, Y2);
	}
      else if (slice == Top && x >= X1 && x <= X2)
	{
	return new HorizontalSlice(X1, X2, Top.Y);
	}
      else if (slice == Bottom && x >= X1 && x <= X2)
	{
	return new HorizontalSlice(X1, X2, Bottom.Y);
	}
      return null;
    }

    public Rectangle Slice(Slice slice)
    {
      return slice.SliceRectangle(this);
    }

    public void Merge(Rectangle rectangle)
    {
      if (Left == rectangle.Right)
	{
	Left = rectangle.Left;
	}
      else if (Right == rectangle.Left)
	{
	Right = rectangle.Right;
	}
      else if (Top == rectangle.Bottom)
	{
	Top = rectangle.Top;
	}
      else if (Bottom == rectangle.Top)
	{
	Bottom = rectangle.Bottom;
	}
    }

    public bool IsInside(int x, int y)
    {
      return x >= X1 && x <= X2 && y >= Y1 && y <= Y2;
    }

    public void Draw(PreviewRenderer renderer)
    {
      renderer.DrawRectangle(X1, Y1, X2 - X1, Y2 - Y1);
    }

    public HorizontalSlice CreateHorizontalSlice(int y)
    {
      return new HorizontalSlice(X1, X2, y);
    }

    public VerticalSlice CreateVerticalSlice(int x)
    {
      return new VerticalSlice(x, Y1, Y2);
    }

    string GetFilename(string name, string extension)
    {
      return  string.Format("{0}_{1}x{2}.{3}", name, Top.Index, Left.Index, 
			    extension);
    }

    public void WriteHTML(StreamWriter w, string name, string extension, int index)
    {
      w.WriteLine("<td rowspan=\"{0}\" colspan = \"{1}\" width=\"{2}\" height=\"{3}\">",
		  Bottom.Index - Top.Index, Right.Index - Left.Index, 
		  Width, Height);
      if (_include)
	{
	w.WriteLine("\t<img name=\"{0}\" src=\"{1}\" width=\"{2}\" height=\"{3}\" border=\"0\" alt=\"\"/></td>", 
		    name + index, GetFilename(name, extension), Width, Height); 
	}
    }

    public void WriteSlice(Image image, string name, string extension)
    {
      Image clone = new Image(image);
      clone.Crop(Width, Height, X1, Y1);
      string filename = GetFilename(name, extension);
      clone.Save(RunMode.NONINTERACTIVE, filename, filename);
      clone.Delete();
    }

    public VerticalSlice Left
    {
      get {return _left;}
      set {_left = value;}
    }

    public VerticalSlice Right
    {
      get {return _right;}
      set {_right = value;}
    }

    public HorizontalSlice Top
    {
      get {return _top;}
      set {_top = value;}
    }

    public HorizontalSlice Bottom
    {
      get {return _bottom;}
      set {_bottom = value;}
    }

    public int X1
    {
      get {return _left.X;}
    }

    public int Y1
    {
      get {return _top.Y;}
    }

    public int X2
    {
      get {return _right.X;}
    }

    public int Y2
    {
      get {return _bottom.Y;}
    }

    public int Width
    {
      get {return X2 - X1 + 1;}
    }

    public int Height
    {
      get {return Y2 - Y1 + 1;}
    }

    public string URL
    {
      get {return _url;}
      set {_url = value;}
    }

    public string AltText
    {
      get {return _altText;}
      set {_altText = value;}
    }

    public string Target
    {
      get {return _target;}
      set {_target = value;}
    }

    public bool Include
    {
      get {return _include;}
      set {_include = value;}
    }
  }
  }
