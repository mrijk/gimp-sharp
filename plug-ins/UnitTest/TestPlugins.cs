// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestPlugins.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestPlugins
  {
    [Test]
    public void TestNCP()
    {
      int width = 129;
      int height = 65;

      Image image = new Image(width, height, ImageBaseType.Rgb);

      Layer layer = new Layer(image, "test", width, height,
			      ImageType.Rgb, 100, LayerModeEffects.Normal);
      image.AddLayer(layer, 0);

      Drawable drawable = image.ActiveDrawable;

      Procedure procedure = new Procedure("plug_in_ncp");
      procedure.Run(image, drawable, 12, 2);

      image.Delete();
    }
  }
}
