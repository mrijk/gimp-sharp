// The PicturePackage plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Renderer.cs
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

using Gdk;

namespace Gimp.PicturePackage
{
  abstract public class ParentRenderer
  {
    Image _cache;
    double _w = double.NaN;
    double _h = double.NaN;

    public ParentRenderer()
    {
    }

    abstract public void Render(Image image, double x, double y, 
				double w, double h);

    protected Image RotateAndScale(Image image, double w, double h)
    {
      if (_cache != image || _w != w || _h != h)
	{
	  ClearCache();

	  _cache = new Image(image);
	  _w = w;
	  _h = h;

	  if (w < h ^ _cache.Width < _cache.Height)
	    {
	      _cache.Rotate(RotationType.Rotate90);
	    }

	  double zoom = Math.Min(w / _cache.Width, h / _cache.Height);
	  w = _cache.Width * zoom;
	  h = _cache.Height * zoom;

	  _cache.Scale((int) w, (int) h);
	}
      return _cache;
    }

    void ClearCache()
    {
      _cache?.Delete();
    }

    public void Cleanup()
    {
      ClearCache();
    }
  }
}
