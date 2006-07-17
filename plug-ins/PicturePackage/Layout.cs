// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Layout.cs
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
using System.Globalization;
using System.IO;
using System.Xml;

namespace Gimp.PicturePackage
{
  public class Layout
  {
    RectangleSet _rectangles = new RectangleSet();
    string _name;
    double _width;
    double _height;
    Unit   _unit;

    public Layout(XmlNode node)
    {
      CultureInfo cultureInfo = new CultureInfo("en-US");
      XmlAttributeCollection attributes = node.Attributes;
      XmlAttribute name = (XmlAttribute) attributes.GetNamedItem("name");
      _name = name.Value;

      XmlAttribute width = (XmlAttribute) attributes.GetNamedItem("width");
      _width = (width == null) ? 0 :  Convert.ToDouble(width.Value,
						       cultureInfo);

      XmlAttribute height = (XmlAttribute) attributes.GetNamedItem("height");
      _height = (height == null) ? 0 : Convert.ToDouble(height.Value,
							cultureInfo);

      XmlAttribute units = (XmlAttribute) attributes.GetNamedItem("units");
      if (units == null)
	{
	  _unit = Unit.Inch;
	}
      else 
	{
	  if (units.Value == "inches")
	    {
	      _unit = Unit.Inch;
	    }
	  else if (units.Value == "pixels")
	    {
	      _unit = Unit.Pixel;
	    }
	}
	  
      XmlNodeList nodeList = node.SelectNodes("picture");
      foreach (XmlNode rectangle in nodeList)
	{
	  _rectangles.Add(new Rectangle(rectangle));
	}
    }

    public void Add(Rectangle rectangle)
    {
      _rectangles.Add(rectangle);
    }

    public Rectangle Find(double x, double y)
    {
      return _rectangles.Find(x, y);
    }

    public bool Render(ProviderFactory factory, Renderer renderer)
    {
      return _rectangles.Render(factory, renderer);
    }

    public PageSize GetPageSize(int resolution)
    {
      if (_unit == Unit.Inch)
	{
	  return new PageSize(_width, _height);
	}
      else
	{
	  return new PageSize(_width / resolution,
			      _height / resolution);
	}
    }

    public PageSize GetPageSizeInPixels(int resolution)
    {
      if (_unit == Unit.Inch)
	{
	  return new PageSize(_width * resolution, _height * resolution);
	}
      else
	{
	  return new PageSize(_width, _height);
	}
    }

    public double Boundaries(int pw, int ph, out int x, out int y)
    {
      double zoom = Math.Min(pw / _width, ph / _height);

      int w = (int) (_width * zoom);
      int h = (int) (_height * zoom);

      x = (pw - w) / 2;
      y = (ph - h) / 2;

      return zoom;
    }

    public string Name
    {
      get {return _name;}
    }

    public Unit Unit
    {
      get {return _unit;}
    }
  }
}
