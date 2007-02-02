// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestUndo.cs
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
  public class TestUndo
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
    public void EnableDisable()
    {
      _image.UndoDisable();
      Assert.IsFalse(_image.UndoEnabled);

      _image.UndoEnable();
      Assert.IsTrue(_image.UndoEnabled);
    }

    [Test]
    public void GroupStartEnd()
    {
      // Just call them. I don't know how to test this yet, unless I can 
      // call the Undo operation programmatically
      _image.UndoGroupStart();
      _image.UndoGroupEnd();
    }

    [Test]
    public void GroupFreezeThaw()
    {
      // Just call them. I don't know how to test this yet, unless I can 
      // call the Undo operation programmatically
      _image.UndoFreeze();
      _image.UndoThaw();
    }
  }
}
