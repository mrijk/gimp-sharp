// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// RectangleSet.cs
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
using System.Collections.Generic;

namespace Gimp.PicturePackage
{
  public class RectangleSet : IEnumerable
  {
    List<Rectangle> _set = new List<Rectangle>();

    public RectangleSet()
    {
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Add(Rectangle rectangle)
    {
      _set.Add(rectangle);
    }

    public Rectangle this[int index]
    {
      get {return _set[index];}
    }

    public Rectangle Find(double x, double y)
    {
      foreach (Rectangle rectangle in _set)
	{
	  if (rectangle.Inside(x, y))
	    return rectangle;
	}
      return null;
    }

    public void Render(ProviderFactory factory, Renderer renderer)
    {
      factory.Reset();
      foreach (Rectangle rectangle in _set)
	{
	  ImageProvider provider = rectangle.Provider;

	  if (provider == null)
	    {
	      provider = factory.Provide();
	      if (provider == null)
		{
		  break;
		}
	      Image image = provider.GetImage();
	      if (image == null)
		{
		  Console.WriteLine("Couldn't load image!");
		}
	      else
		{
		  rectangle.Render(image, renderer);
		}
	      factory.Cleanup(provider);
	    }
	  else
	    {
	      rectangle.Render(provider.GetImage(), renderer);
	      provider.Release();
	    }
	}
      factory.Cleanup();
      renderer.Cleanup();
    }

    public int Count
    {
      get {return _set.Count;}
    }
  }
}
