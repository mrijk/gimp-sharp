// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.Globalization;
using System.Xml;

using Gtk;

namespace Gimp.PicturePackage
{
  public class Rectangle
  {
    double _x, _y, _w, _h;
    CultureInfo _cultureInfo = new CultureInfo("en-US");

    public ImageProvider Provider {get; set;}

    public Rectangle(XmlNode node)
    {
      var attributes = node.Attributes;
      _x = GetAttribute(attributes, "x");
      _y = GetAttribute(attributes, "y");
      _w = GetAttribute(attributes, "width");
      _h = GetAttribute(attributes, "height");
    }

    double GetAttribute(XmlAttributeCollection attributes, string name)
    {
      var val = (XmlAttribute) attributes.GetNamedItem(name);
      return (val == null) ? 0 : Convert.ToDouble(val.Value, _cultureInfo);
    }

    public void Render(Image image, ParentRenderer renderer)
    {
      renderer.Render(image, _x, _y, _w, _h);
    }

    public bool Render(ProviderFactory factory, ParentRenderer renderer)
    {
      bool renderedSomething = false;

      var provider = Provider;

      if (provider == null)
	{
	  provider = factory.Provide();
	  if (provider == null)
	    {
	      return false;
	    }
	  var image = provider.GetImage();
	  if (image == null)
	    {
	      // Console.WriteLine("Couldn't load image!");
	    }
	  else
	    {
	      Render(image, renderer);
	      renderedSomething = true;
	    }
	  factory.Cleanup(provider);
	}
      else
	{
	  Render(provider.GetImage(), renderer);
	  provider.Release();
	  renderedSomething = true;
	}
      return renderedSomething;
    }

    public bool Inside(Coordinate<double> c)
    {
      double x = c.X;
      double y = c.Y;
      return x >= _x && x <= _x + _w && y >= _y && y <= _y + _h;
    }
  }
}
