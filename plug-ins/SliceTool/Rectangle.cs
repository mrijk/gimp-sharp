// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// Rectangle.cs
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
using System.Xml;

namespace Gimp.SliceTool
{
  public class Rectangle : IComparable
  {
    public VerticalSlice Left {get; set;}
    public VerticalSlice Right {get; set;}
    public HorizontalSlice Top {get; set;}
    public HorizontalSlice Bottom {get; set;}
    
    PropertySet _properties = new PropertySet();

    bool _include = true;

    static string _globalExtension = "png";
    public string Extension {get; set;}

    public Rectangle(VerticalSlice left, VerticalSlice right,
                     HorizontalSlice top, HorizontalSlice bottom)
    {
      Left = left;
      Right = right;
      Top = top;
      Bottom = bottom;
    }

    public Rectangle(Rectangle rectangle)
    {
      Left = rectangle.Left;
      Right = rectangle.Right;
      Top = rectangle.Top;
      Bottom = rectangle.Bottom;
    }

    public Rectangle(XmlNode node)
    {
      Left = new VerticalSlice(GetValueOfNode(node, "left"));
      Right = new VerticalSlice(GetValueOfNode(node, "right"));
      Top = new HorizontalSlice(GetValueOfNode(node, "top"));
      Bottom = new HorizontalSlice(GetValueOfNode(node, "bottom"));
    }

    int GetValueOfNode(XmlNode node, string item)
    {
      XmlAttributeCollection attributes = node.Attributes;
      XmlAttribute position = (XmlAttribute) attributes.GetNamedItem(item);
      return (int) Convert.ToDouble(position.Value);
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

    public bool Normalize(Slice slice)
    {
      if (slice == Left && X1 >= X2)
        {
	  Left.X = Right.X;
	  return true;
        } 
      else if (slice == Right && X2 <= X1)
        {
	  Right.X = Left.X;
	  return true;
        }

      if (slice == Top && Y1 >= Y2)
        {
	  Top.Y = Bottom.Y;
	  return true;
        }
      else if (slice == Bottom && Y2 <= Y1)
        {
	  Bottom.Y = Top.Y;
	  return true;
	  // Fix me: shouldn't this have 'return true'?????
        }
      return false;
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
	  return CreateVerticalSlice(Left.X);
        }
      else if (slice == Right && y >= Y1 && y <= Y2)
        {
	  return CreateVerticalSlice(Right.X);
        }
      else if (slice == Top && x >= X1 && x <= X2)
        {
	  return CreateHorizontalSlice(Top.Y);
        }
      else if (slice == Bottom && x >= X1 && x <= X2)
        {
	  return CreateHorizontalSlice(Bottom.Y);
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

    public bool IsInside(Coordinate<int> c)
    {
      return c.X >= X1 && c.X <= X2 && c.Y >= Y1 && c.Y <= Y2;
    }

    public void Draw(PreviewRenderer renderer)
    {
      renderer.DrawRectangle(X1, Y1, Width, Height);
    }

    public HorizontalSlice CreateHorizontalSlice(int y)
    {
      return new HorizontalSlice(Left, Right, y);
    }

    public VerticalSlice CreateVerticalSlice(int x)
    {
      return new VerticalSlice(Top, Bottom, x);
    }

    string GetFilename(string name, bool useGlobalExtension)
    {
      string extension = (Extension == null || useGlobalExtension)
        ? _globalExtension : Extension;
      return string.Format("{0}_{1}x{2}.{3}", name, Top.Index, Left.Index, 
                           extension);
    }

    public void WriteHTML(StreamWriter w, string name, 
			  bool useGlobalExtension, int index)
    {
      w.WriteLine("<td rowspan=\"{0}\" colspan=\"{1}\" width=\"{2}\" height=\"{3}\">",
                  Bottom.Index - Top.Index, Right.Index - Left.Index, 
                  Width, Height);
      if (_include)
        {
	  w.Write("\t");
	  if (_properties.Exists("href"))
	    {
	      w.Write("<a");
	      _properties.WriteHTML(w, "href");

	      if (_properties.Exists("MouseOver"))
		{
		  w.Write(" onMouseOut=\"swapImgRestore()\"");
		  w.Write(" onMouseOver=\"swapImage('{0}','','{1}',1)\"",
			  name + index, 
			  System.IO.Path.GetFileName(_properties.Get("MouseOver")));
		}

	      w.Write(">");
	    }

	  w.Write("<img name=\"{0}\"", name + index);
	  w.Write(" src=\"{0}\"", GetFilename(name, useGlobalExtension));
	  w.Write(" width=\"{0}\"", Width);
	  w.Write(" height=\"{0}\"", Height);
	  w.Write(" border=\"0\"");

	  _properties.WriteHTML(w, "AltText");
	  _properties.WriteHTML(w, "Target");

	  w.WriteLine("/></td>");
	
	  if (_properties.Exists("href"))
	    {
	      w.Write("</a>");
	    }
        }
      w.WriteLine("</td>");
    }

    public void WriteSlice(Image image, string path, string name, 
                           bool useGlobalExtension)
    {
      Image clone = new Image(image);
      clone.Crop(Width, Height, X1, Y1);
      string filename = path + System.IO.Path.DirectorySeparatorChar + 
        GetFilename(name, useGlobalExtension);
      clone.Save(RunMode.Noninteractive, filename, filename);
      clone.Delete();
    }

    public void Save(StreamWriter w)
    {
      w.WriteLine("\t<rectangle left=\"{0}\" right=\"{1}\" top=\"{2}\" bottom=\"{3}\"/>",
                  Left.Index, Right.Index, Top.Index, Bottom.Index);
    }

    public void Resolve(SliceSet hslices, SliceSet vslices)
    {
      Left = vslices[Left.Index] as VerticalSlice;
      Right = vslices[Right.Index] as VerticalSlice;
      Top = hslices[Top.Index] as HorizontalSlice;
      Bottom = hslices[Bottom.Index] as HorizontalSlice;
    }

    public int X1
    {
      get {return Left.X;}
    }

    public int Y1
    {
      get {return Top.Y;}
    }

    public int X2
    {
      get {return Right.X;}
    }

    public int Y2
    {
      get {return Bottom.Y;}
    }

    public int Width
    {
      get {return X2 - X1;}
    }

    public int Height
    {
      get {return Y2 - Y1;}
    }

    public void SetProperty(string name, string value)
    {
      _properties.Set(name, value);
    }

    public string GetProperty(string name)
    {
      return _properties.Get(name);
    }

    public bool Include
    {
      get {return _include;}
      set {_include = value;}
    }

    public static string GlobalExtension
    {
      get {return _globalExtension;}
      set {_globalExtension = value;}
    }

    public bool Changed
    {
      get {return _properties.Changed;}
    }
  }
}
