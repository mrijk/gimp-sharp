// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestChannel.cs
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
  public class TestChannel
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void ChannelConstructorOne()
    {
      int before = _image.Channels.Count;

      var channel = new Channel(_image, "test", _width, _height, 100,
				new RGB(0, 255, 0));
      Assert.IsTrue(channel.IsChannel());
      _image.AddChannel(channel, 0);

      Assert.AreEqual(before + 1, _image.Channels.Count);
    }

    [Test]
    public void ChannelConstructorTwo()
    {
      int before = _image.Channels.Count;

      var channel = new Channel(_image, "test", _width, _height, 100,
				new RGB(0, 255, 0));
      _image.AddChannel(channel, 0);

      var copy = new Channel(channel);
      _image.AddChannel(copy, 0);

      Assert.AreEqual(before + 2, _image.Channels.Count);
    }

    [Test]
    public void ChannelConstructorThree()
    {
      int before = _image.Channels.Count;
      var channel = new Channel(_image, ChannelType.Red, "test");
      _image.AddChannel(channel, 0);

      Assert.AreEqual(before + 1, _image.Channels.Count);
    }

    [Test]
    public void GetSetShowMasked()
    {
      var channel = new Channel(_image, "test", _width, _height, 100,
				new RGB(0, 255, 0));
      _image.AddChannel(channel, 0);

      channel.ShowMasked = true;
      Assert.IsTrue(channel.ShowMasked);

      channel.ShowMasked = false;
      Assert.IsFalse(channel.ShowMasked);
    }

    [Test]
    public void GetSetOpacity()
    {
      var channel = new Channel(_image, "test", _width, _height, 100,
				new RGB(0, 255, 0));
      _image.AddChannel(channel, 0);

      Assert.AreEqual(100, channel.Opacity);
      channel.Opacity = 13;
      Assert.AreEqual(13, channel.Opacity);
    }

    [Test]
    public void GetSetColor()
    {
      RGB color = new RGB(0, 255, 0);
      var channel = new Channel(_image, "test", _width, _height, 100,
				color);
      _image.AddChannel(channel, 0);

      Assert.AreEqual(color.Bytes, channel.Color.Bytes);
      RGB red = new RGB(255, 0, 0);
      channel.Color = red;
      Assert.AreEqual(red.Bytes, channel.Color.Bytes);
    }

    [Test]
    public void CombineMasks()
    {
      RGB color = new RGB(0, 255, 0);
      var channelOne = new Channel(_image, "one", color);      
      var channelTwo = new Channel(_image, "two", color);

      _image.AddChannel(channelOne, 0);
      _image.AddChannel(channelTwo, 0);

      channelOne.CombineMasks(channelTwo, ChannelOps.Add, 0, 0);
    }
  }
}
