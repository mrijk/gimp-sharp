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

    public void Render(Image image, Renderer renderer)
    {
      _rectangles.Render(image, renderer);
    }

    public void Render(Renderer renderer)
    {
      _rectangles.Render(renderer);
    }

    public void LoadFromFrontImage(Image image, Renderer renderer)
    {
      FrontImageProvider provider = new FrontImageProvider(image);
      _rectangles.Render(provider, renderer);
      provider.Release();
    }

    public void LoadFromDirectory(string directory, Renderer renderer)
    {
      int count = _rectangles.Count;
      int i = 0;

      foreach	(string	file in	Directory.GetFiles(directory))
	{
	if (i >= count)
	  break;

	FileImageProvider provider = 
	  new FileImageProvider(file, directory + "/" + file);
	if (provider.GetImage() != null)
	  {
	  Rectangle rectangle = _rectangles[i];
	  rectangle.Provider = provider;
	  rectangle.Render(renderer);
	  provider.Release();
	  i++;
	  }
	}
#if false
      foreach	(string	directory in Directory.GetDirectories(parent))
	{
	Iterate(directory);
	}
#endif
    }

    public void LoadFromFile(string file, Renderer renderer)
    {
      FileImageProvider provider = new FileImageProvider(file);
      _rectangles.Render(provider, renderer);
      provider.Release();
      // Fixme: handle failure
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
