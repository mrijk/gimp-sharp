using System;
using System.Xml;

using Gtk;

namespace Gimp.PicturePackage
{
  public class Rectangle
  {
    double _x, _y, _w, _h;
    ImageProvider _provider;

    public Rectangle(XmlNode node)
    {
      XmlAttributeCollection attributes = node.Attributes;
      _x = GetAttribute(attributes, "x");
      _y = GetAttribute(attributes, "y");
      _w = GetAttribute(attributes, "width");
      _h = GetAttribute(attributes, "height");
    }

    double GetAttribute(XmlAttributeCollection attributes, string name)
    {
      XmlAttribute val = (XmlAttribute) attributes.GetNamedItem(name);
      return (val == null) ? 0 :  Convert.ToDouble(val.Value);      
    }

    public void Render(Image image, Renderer renderer)
    {
      renderer.Render(image, _x, _y, _w, _h);
    }

    public bool Inside(double x, double y)
    {
      return x >= _x && x <= _x + _w
	&& y >= _y && y <= _y + _h;
    }

    public ImageProvider Provider
    {
      get {return _provider;}
      set {_provider = value;}
    }
  }
  }
