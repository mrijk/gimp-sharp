using System;
using System.Xml;

using Gtk;

namespace Gimp
  {
    public class Rectangle
    {
      double _x, _y, _w, _h;

      public Rectangle(XmlNode node)
      {
	XmlAttributeCollection attributes = node.Attributes;

	XmlAttribute x = (XmlAttribute) attributes.GetNamedItem("x");
	_x = (x == null) ? 0 :  Convert.ToDouble(x.Value);

	XmlAttribute y = (XmlAttribute) attributes.GetNamedItem("y");
	_y = (y == null) ? 0 :  Convert.ToDouble(y.Value);

	XmlAttribute w = (XmlAttribute) attributes.GetNamedItem("width");
	_w = (w == null) ? 0 :  Convert.ToDouble(w.Value);

	XmlAttribute h = (XmlAttribute) attributes.GetNamedItem("height");
	_h = (h == null) ? 0 :  Convert.ToDouble(h.Value);
      }

      public void Draw(Preview preview, double zoom)
      {
	  int x = (int) (zoom * _x);
	  int y = (int) (zoom * _y);
	  int w = (int) (zoom * _w);
	  int h = (int) (zoom * _h);
	  preview.DrawRectangle(x, y, w, h);
      }

      public bool Inside(int x, int y)
      {
	return x >= _x && x <= _x + _w
	  && y >= _y && y <= _y + _h;
      }
    }
  }
