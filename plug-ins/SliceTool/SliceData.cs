using System;
using System.IO;

namespace Gimp.SliceTool
{
	public class SliceData
	{
		RectangleSet _rectangles = new RectangleSet();
		Rectangle _selected = null;
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

			VerticalSlice left = new VerticalSlice(0, 0, height - 1);
			VerticalSlice right = new VerticalSlice(width - 1, 0, height - 1);
			HorizontalSlice top = new HorizontalSlice(0, width - 1, 0);
			HorizontalSlice bottom = new HorizontalSlice(0, width - 1, height - 1);
			_verticalSlices.Add(left);
			_verticalSlices.Add(right);
			_horizontalSlices.Add(top);
			_horizontalSlices.Add(bottom);
			_selected = new Rectangle(left, right, top, bottom);
			_rectangles.Add(_selected);
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
			_selected = FindRectangle(x, y);
			return _selected;
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
				horizontalSlices.Add(new HorizontalSlice(x1, x2, ypos));
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
				verticalSlices.Add(new VerticalSlice(xpos, y1, y2));
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
			if (_selected != null)
			{
				_selected.Draw(renderer);
			}
		}

		public void Save(string filename, string extension, Image image, Drawable drawable)
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
			w.WriteLine("</table>");
			w.WriteLine("<!-- End Table -->");
			w.WriteLine("");
			w.WriteLine("</body");
			w.WriteLine("</html>");
			w.Close();

			_rectangles.WriteSlices(image, name, extension);		
		}

		public Rectangle Selected
		{
			get {return _selected;}
		}
	}
}
