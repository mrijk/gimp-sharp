using System;
using System.Xml;

using Gtk;

namespace Gimp.PicturePackage
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

    public void Render(Image image, Renderer renderer)
    {
      renderer.Render(image, _x, _y, _w, _h);
    }

    public bool Inside(int x, int y)
    {
      return x >= _x && x <= _x + _w
	&& y >= _y && y <= _y + _h;
    }
  }
  }
