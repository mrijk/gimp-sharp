using System;
using System.IO;

namespace Gimp.SliceTool
{
  public class SliceData
  {
    RectangleSet _rectangles = new RectangleSet();
    SliceSet _horizontalSlices = new SliceSet();
    SliceSet _verticalSlices = new SliceSet();

    bool _foo; // fix me!
    Rectangle _rectangle;
    Slice _slice;

    public SliceData()
    {
    }

    public void Init(Drawable drawable)
    {
      int width = drawable.Width;
      int height = drawable.Height;

      VerticalSlice left = new VerticalSlice(null, null, 0);
      VerticalSlice right = new VerticalSlice(null, null, width);
      HorizontalSlice top = new HorizontalSlice(left, right, 0);
      HorizontalSlice bottom = new HorizontalSlice(left, right, height);
      _verticalSlices.Add(left);
      _verticalSlices.Add(right);
      _horizontalSlices.Add(top);
      _horizontalSlices.Add(bottom);

      left.Begin = right.Begin = top;
      left.End = right.End = bottom;

      _rectangles.Add(new Rectangle(left, right, top, bottom));
    }

    public void AddSlice(Slice slice)
    {
      if (_foo)
	_horizontalSlices.Add(slice);
      else
	_verticalSlices.Add(slice);
      _rectangles.Slice(slice);
    }

    public Slice GetSlice(int x, int y)
    {
      Rectangle rectangle = _rectangles.Find(x, y);
      Slice slice;

      if (rectangle == _rectangle)
	{
	slice = _slice;
	}
      else
	{
	_rectangle = rectangle;
	if (_foo)
	  slice = rectangle.CreateVerticalSlice(x);
	else
	  slice = rectangle.CreateHorizontalSlice(y);
	_foo = !_foo;
	}
      _slice = slice;
      return slice;
    }

    public Rectangle FindRectangle(int x, int y)
    {
      return _rectangles.Find(x, y);
    }

    public Rectangle SelectRectangle(int x, int y)
    {
      return _rectangles.Select(x, y);
    }

    public Slice FindSlice(int x, int y)
    {
      Slice slice = _horizontalSlices.Find(x, y);
      if (slice == null)
	{
	slice = _verticalSlices.Find(x, y);
	}
      return slice;
    }

    public bool Remove(int x, int y)
    {
      Slice slice = FindSlice(x, y);
      if (slice != null)
	{
	if (_rectangles.Remove(x, y, slice))
	  {
	  _horizontalSlices.Remove(slice);
	  _verticalSlices.Remove(slice);
	  return true;
	  }
	}
      return false;
    }

    public void CreateTable(int x, int y, int rows, int columns)
    {
      Rectangle rectangle = _rectangles.Find(x, y);
      int width = rectangle.Width;
      int height = rectangle.Height;
      int x1 = rectangle.X1;
      int x2 = rectangle.X2;
      int y1 = rectangle.Y1;
      int y2 = rectangle.Y2;

      SliceSet horizontalSlices = new SliceSet();
      for (int row = 1; row < rows; row++)
	{
	int ypos = y1 + row * height / rows;
	horizontalSlices.Add(rectangle.CreateHorizontalSlice(ypos));
	}

      foreach (Slice slice in horizontalSlices)
	{
	_rectangles.Slice(slice);
	_horizontalSlices.Add(slice);
	}

      SliceSet verticalSlices = new SliceSet();
      for (int col = 1; col < columns; col++)
	{
	int xpos = x1 + col * width / columns;
	verticalSlices.Add(rectangle.CreateVerticalSlice(xpos));
	}

      foreach (Slice slice in verticalSlices)
	{
	_rectangles.Slice(slice);
	_verticalSlices.Add(slice);
	}
    }

    public void Draw(PreviewRenderer renderer)
    {
      _horizontalSlices.Draw(renderer);
      _verticalSlices.Draw(renderer);
      _rectangles.Selected.Draw(renderer);
    }

    public void Cleanup(Slice slice)
    {
      RectangleSet set = new RectangleSet();

      foreach (Rectangle rectangle in _rectangles)
	{
	if (rectangle.Normalize(slice))
	  {
	  set.Add(rectangle);
	  }
	}

      foreach (Rectangle rectangle in set)
	{
	_rectangles.Remove(rectangle);
	}
    }

    void WriteBlankLine(StreamWriter w)
    {
      w.WriteLine("<tr>");
      Slice prev = null;
      foreach (Slice slice in _verticalSlices)
	{
	if (prev != null)
	  {
	  int width = slice.Position - prev.Position;
	  w.WriteLine("<td width=\"{0}\" height=\"1\">", width);
	  w.WriteLine("\t<img name=\"blank\" src=\"blank.png\" width=\"{0}\" height=\"1\" border=\"0\"></td>", 
		      width); 
	  }
	prev = slice;
	}
      w.WriteLine("</tr>");
    }

    public void Save(string filename, string extension, Image image, 
		     Drawable drawable)
    {
      _horizontalSlices.Sort();
      _verticalSlices.Sort();

      string name = System.IO.Path.GetFileNameWithoutExtension(image.Name);
      FileStream fs = new FileStream(filename, FileMode.Create,
				     FileAccess.Write);
      StreamWriter w = new StreamWriter(fs);

      w.WriteLine("<html>");
      w.WriteLine("<head>");
      w.WriteLine("<meta name=\"Author\" content=\"{0}\">",
		  Environment.UserName);
      w.WriteLine("<meta name=\"Generator\" content=\"GIMP {0}\">",
		  Gimp.Version);
      w.WriteLine("<title></title>");
      w.WriteLine("</head>");
      w.WriteLine("<body");
      w.WriteLine("");
      w.WriteLine("<!-- Begin Table -->");
      w.WriteLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"{0}\">", 
		  drawable.Width);

      _rectangles.WriteHTML(w, name, extension);
      WriteBlankLine(w);

      w.WriteLine("</table>");
      w.WriteLine("<!-- End Table -->");
      w.WriteLine("");
      w.WriteLine("</body");
      w.WriteLine("</html>");
      w.Close();

      string path = System.IO.Path.GetDirectoryName(filename);
      _rectangles.WriteSlices(image, path, name, extension);		
    }

    public Rectangle Selected
    {
      get {return _rectangles.Selected;}
    }
  }
  }
