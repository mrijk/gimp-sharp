using System;
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
				_unit = Unit.INCH;
			}
			else 
			{
				if (units.Value == "inches")
				{
					_unit = Unit.INCH;
				}
				else if (units.Value == "pixels")
				{
					_unit = Unit.PIXEL;
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

		public bool Render(ProviderFactory factory, Renderer renderer, bool foo)
		{
			return _rectangles.Render(factory, renderer, foo);
		}

		public void Render(ProviderFactory factory, Renderer renderer)
		{
			_rectangles.Render(factory, renderer);
		}

		public PageSize GetPageSize(int resolution)
		{
			if (_unit == Unit.INCH)
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
			if (_unit == Unit.INCH)
			{
				return new PageSize(_width * resolution, _height * resolution);
			}
			else
			{
				return new PageSize(_width, _height);
			}
		}

		public string Name
		{
			get {return _name;}
		}

		public double Width
		{
			get {return _width;}
		}

		public double Height
		{
			get {return _height;}
		}

		public Unit Unit
		{
			get {return _unit;}
		}
	}
}