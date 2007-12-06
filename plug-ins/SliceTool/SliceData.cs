// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// SliceData.cs
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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Gimp.SliceTool
{
  public class SliceData
  {
    RectangleSet _rectangles = new RectangleSet();
    SliceSet _horizontalSlices = new SliceSet();
    SliceSet _verticalSlices = new SliceSet();

    public SliceData()
    {
    }

    public void Init(Drawable drawable)
    {
      int width = drawable.Width;
      int height = drawable.Height;

      VerticalSlice left = new VerticalSlice(null, null, 0) 
	{Locked = true};
      _verticalSlices.Add(left);

      VerticalSlice right = new VerticalSlice(null, null, width) 
	{Locked = true};
      _verticalSlices.Add(right);

      HorizontalSlice top = new HorizontalSlice(left, right, 0) 
	{Locked = true};
      _horizontalSlices.Add(top);

      HorizontalSlice bottom = new HorizontalSlice(left, right, height)
	{Locked = true};
      _horizontalSlices.Add(bottom);

      left.Begin = right.Begin = top;
      left.End = right.End = bottom;

      _rectangles.Add(new Rectangle(left, right, top, bottom));

      Changed = false;
    }

    public void AddSlice(Slice slice)
    {
      if (slice is HorizontalSlice)
        _horizontalSlices.Add(slice);
      else
        _verticalSlices.Add(slice);
      _rectangles.Slice(slice);
    }

    public Rectangle FindRectangle(Coordinate<int> c)
    {
      return _rectangles.Find(c);
    }

    public Rectangle SelectRectangle(Coordinate<int> c)
    {
      return _rectangles.Select(c);
    }

    public Slice FindSlice(Coordinate<int> c)
    {
      Slice slice = _horizontalSlices.Find(c);
      if (slice == null)
        {
	  slice = _verticalSlices.Find(c);
        }
      return slice;
    }

    public Slice MayRemove(Coordinate<int> c)
    {
      Slice slice = _horizontalSlices.Find(c);
      if (slice == null)
        {
	  slice = _verticalSlices.Find(c);
	  if (slice != null && !_horizontalSlices.IsEndPoint(slice))
	    {
	      return slice;
	    }
        }
      else if (!_verticalSlices.IsEndPoint(slice))
        {
	  return slice;
        }
      return null;
    }

    public void Remove(Slice slice)
    {
      RectangleSet set1 = new RectangleSet();
      RectangleSet set2 = new RectangleSet();

      foreach (Rectangle rectangle in _rectangles)
        {
	  if (slice == rectangle.Bottom || slice == rectangle.Right)
	    {
	      set1.Add(rectangle);
	    }
	  else if (slice == rectangle.Top || slice == rectangle.Left)
	    {
	      set2.Add(rectangle);
	    }
        }

      foreach (Rectangle r1 in set1)
        {
	  foreach (Rectangle r2 in set2)
	    {
	      if (r1.Left == r2.Left && r1.Right == r2.Right)
		{
		  r1.Bottom = r2.Bottom;
		  _rectangles.Remove(r2);
		  break;
		}
	      else if (r1.Top == r2.Top && r1.Bottom == r2.Bottom)
		{
		  r1.Right = r2.Right;
		  _rectangles.Remove(r2);
		  break;
		}
	    }
        }

      _horizontalSlices.Remove(slice);
      _verticalSlices.Remove(slice);
    }

    public void CreateTable(Coordinate<int> c, int rows, int columns)
    {
      Rectangle rectangle = new Rectangle(_rectangles.Find(c));
      int width = rectangle.Width;
      int height = rectangle.Height;
      int x1 = rectangle.X1;
      int y1 = rectangle.Y1;

      for (int row = 1; row < rows; row++)
        {
	  int ypos = y1 + row * height / rows;
	  AddSlice(rectangle.CreateHorizontalSlice(ypos));
        }

      for (int col = 1; col < columns; col++)
        {
	  int xpos = x1 + col * width / columns;
	  AddSlice(rectangle.CreateVerticalSlice(xpos));
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

    public void Save(string filename, bool useGlobalExtension, 
                     Image image, Drawable drawable)
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
      WriteJavaScript(w);
      w.WriteLine("</head>");
      w.WriteLine("");

      w.Write("<body");

      string[] preload = JavaScriptProperty.Preload;
      int count = preload.Length;
      if (count > 0)
        {
	  w.Write(" onLoad=\"preloadImages('{0}'", 
		  System.IO.Path.GetFileName(preload[0]));
	  for (int i = 1; i < count; i++)
	    {
	      w.Write(", '{0}'", System.IO.Path.GetFileName(preload[i]));
	    }
	  w.Write(")\"");
        }
      w.WriteLine(">");

      w.WriteLine("");
      w.WriteLine("<!-- Begin Table -->");
      w.WriteLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"{0}\">", 
                  drawable.Width);

      _rectangles.WriteHTML(w, name, useGlobalExtension);
      WriteBlankLine(w);

      w.WriteLine("</table>");
      w.WriteLine("<!-- End Table -->");
      w.WriteLine("");
      w.WriteLine("</body>");
      w.WriteLine("</html>");
      w.Close();

      string path = System.IO.Path.GetDirectoryName(filename);
      _rectangles.WriteSlices(image, path, name, useGlobalExtension);		
    }

    void WriteJavaScript(StreamWriter writer)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream stream = 
        assembly.GetManifestResourceStream("javascript.html");

      StreamReader reader = new StreamReader(stream);
      string line;

      while ((line = reader.ReadLine()) != null)
        {
	  writer.WriteLine(line);
        }

      reader.Close();
    }

    public Rectangle Selected
    {
      get {return _rectangles.Selected;}
    }

    public bool Changed
    {
      get 
	{
          return _rectangles.Changed
            || _horizontalSlices.Changed
            || _verticalSlices.Changed;
	}
      set
	{
	  _verticalSlices.Changed = value;
	  _horizontalSlices.Changed = value;
	  _rectangles.Changed = value;
	}
    }

    public void LoadSettings(string filename)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(filename);

      _horizontalSlices.Clear();
      _verticalSlices.Clear();
      _rectangles.Clear();

      XmlElement root = doc.DocumentElement;
      XmlNodeList nodeList = root.SelectNodes("/settings/slices/slice");

      foreach (XmlNode node in nodeList)
        {
	  XmlAttributeCollection attributes = node.Attributes;
	  XmlAttribute type = (XmlAttribute) attributes.GetNamedItem("type");

	  if (type.Value == "horizontal")
	    {
	      HorizontalSlice slice = new HorizontalSlice();
	      slice.Load(node);
	      _horizontalSlices.Add(slice);
	    }
	  else
	    {
	      VerticalSlice slice = new VerticalSlice();
	      slice.Load(node);
	      _verticalSlices.Add(slice);
	    }
        }

      nodeList = root.SelectNodes("/settings/rectangles/rectangle");

      foreach (XmlNode node in nodeList)
        {
	  _rectangles.Add(new Rectangle(node));
        }

      _verticalSlices.Resolve(_horizontalSlices);
      _horizontalSlices.Resolve(_verticalSlices);
      _rectangles.Resolve(_horizontalSlices, _verticalSlices);
    }

    public void SaveSettings(string filename)
    {
      _horizontalSlices.SetIndex();
      _verticalSlices.SetIndex();

      FileStream fs = new FileStream(filename, FileMode.Create,
                                     FileAccess.Write);
      StreamWriter w = new StreamWriter(fs);

      w.WriteLine("<settings>");
      w.WriteLine("<slices>");
      _horizontalSlices.Save(w);
      _verticalSlices.Save(w);
      w.WriteLine("</slices>");
      w.WriteLine("");
      w.WriteLine("<rectangles>");
      _rectangles.Save(w);
      w.WriteLine("</rectangles>");
      w.WriteLine("</settings>");

      w.Close();
    }
  }
}
