using System;
using System.Xml;

namespace Gimp
  {
    public class Layout
    {
      enum Unit
      {
	INCHES,
	PIXELS
      };

      RectangleSet _rectangles = new RectangleSet();
      string _name;
      double _width;
      double _height;
      Unit   _units;

      public Layout(XmlNode node)
      {
	  XmlAttributeCollection attributes = node.Attributes;
	  XmlAttribute name = (XmlAttribute) attributes.GetNamedItem("name");
	  _name = name.Value;

	  XmlAttribute width = (XmlAttribute) attributes.GetNamedItem("width");
	  _width = (width == null) ? 0 :  Convert.ToDouble(width.Value);

	  XmlAttribute height = (XmlAttribute) attributes.GetNamedItem("height");
	  _height = (height == null) ? 0 : Convert.ToDouble(height.Value);

	  XmlAttribute units = (XmlAttribute) attributes.GetNamedItem("units");
	  if (units == null)
	    {
	    _units = Unit.INCHES;
	    }
	  else 
	    {
	    if (units.Value == "inches")
	      {
	      _units = Unit.INCHES;
	      }
	    else if (units.Value == "pixels")
	      {
	      _units = Unit.PIXELS;
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

      public Rectangle Find(int x, int y)
      {
	return _rectangles.Find(x, y);
      }

      public void Draw(Preview preview, int width, int height)
      {
	  double zoom = width / _width;
	  _rectangles.Draw(preview, zoom);
      }

      public string Name
	{
	    get {return _name;}
	}
    }
  }
