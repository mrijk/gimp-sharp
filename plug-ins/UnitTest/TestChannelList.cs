// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestChannelList.cs
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
  public class TestChannelList
  {
    int _width = 64;
    int _height = 128;
    Image _image;
    Drawable _drawable;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      var layer = new Layer(_image, "test", _width, _height,
			    ImageType.Rgb, 100, 
			    LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);

      _drawable = _image.ActiveDrawable;
    }

    [TearDown]
    public void Exit()
    {
      // _image.Delete();
    }

    [Test]
    public void Count()
    {
      var channels = _image.Channels;
      int count = 0;
      
      channels.ForEach(channel => count++);
      Assert.AreEqual(channels.Count, count);
    }

    [Test]
    public void This()
    {
      string channelName = "test";
      var channels = _image.Channels;
      var channel = new Channel(_image, ChannelType.Red, channelName);
      _image.AddChannel(channel, 0);
      var found = channels[0];
      Assert.IsTrue(found != null);
      // Assert.AreEqual(channel.Name, found.Name);
    }
  }
}
