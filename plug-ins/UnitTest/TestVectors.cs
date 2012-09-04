// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// TestVectors.cs
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

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestVectors
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void Constructor()
    {
      var vectors = new Vectors(_image, "firstVector");
      Assert.AreEqual("firstVector", vectors.Name);
    }

    [Test]
    public void ConstructorFromTextLayer()
    {
      var layer = new TextLayer(_image, "Hello World", "Sans", 
				new FontSize(32, Unit.Pixel));
      _image.InsertLayer(layer, 0);
      var vectors = new Vectors(_image, layer);
      Assert.IsTrue(vectors.IsValid);
      Assert.IsTrue(vectors.Strokes.Count > 0);
    }

    [Test]
    public void RemoveStroke()
    {
      var vectors = new Vectors(_image, "firstVector");
      var stroke = AddStroke(vectors);
      vectors.RemoveStroke(stroke);
      Assert.AreEqual(0, vectors.Strokes.Count);
    }

    [Test]
    public void ToSelection()
    {
      var vectors = new Vectors(_image, "firstVector");
      _image.AddVectors(vectors, -1);
      AddStroke(vectors);
      Assert.IsTrue(_image.Selection.Empty);
      vectors.ToSelection(ChannelOps.Replace, true);
      Assert.IsFalse(_image.Selection.Empty);
    }

    [Test]
    public void NewFromPoints()
    {
      var vectors = new Vectors(_image, "firstVector");
      AddStroke(vectors);
      Assert.AreEqual(1, vectors.Strokes.Count);
    }

    Stroke AddStroke(Vectors vectors)
    {
      var controlpoints = new CoordinateList<double> {
	new Coordinate<double>(10, 10),
	new Coordinate<double>(50, 50),
	new Coordinate<double>(100, 100)
      };
      return vectors.NewFromPoints(VectorsStrokeType.Bezier, controlpoints, 
				   true);
    }

    [Test]
    public void Add()
    {
      var vectors = new Vectors(_image, "firstVector");
      Assert.AreEqual(0, _image.Vectors.Count);
      _image.AddVectors(vectors, -1);
      Assert.AreEqual(1, _image.Vectors.Count);
      Assert.IsTrue(vectors.IsValid);
    }

    [Test]
    public void Remove()
    {
      var first = new Vectors(_image, "firstVector");
      _image.AddVectors(first, -1);
      _image.RemoveVectors(first);
    }

    [Test]
    public void PositionOne()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);
      Assert.AreEqual(1, _image.GetItemPosition(first));
      Assert.AreEqual(0, _image.GetItemPosition(second));
    }

    [Test]
    public void PositionTwo()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);
      Assert.AreEqual(1, first.Position);
      Assert.AreEqual(0, second.Position);
    }

    [Test]
    public void LowerVectors()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);

      _image.LowerItem(second);
      Assert.AreEqual(0, _image.GetItemPosition(first));
      Assert.AreEqual(1, _image.GetItemPosition(second));
    }

    [Test]
    public void RaiseVectors()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);

      _image.RaiseItem(first);
      Assert.AreEqual(0, _image.GetItemPosition(first));
      Assert.AreEqual(1, _image.GetItemPosition(second));
    }

    [Test]
    public void LowerVectorsToBottom()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);

      _image.LowerItemToBottom(second);
      Assert.AreEqual(0, _image.GetItemPosition(first));
      Assert.AreEqual(1, _image.GetItemPosition(second));
    }

    [Test]
    public void RaiseVectorsToTop()
    {
      var first = new Vectors(_image, "firstVector");
      var second = new Vectors(_image, "secondVector");
      _image.AddVectors(first, -1);
      _image.AddVectors(second, -1);

      _image.RaiseItemToTop(first);
      Assert.AreEqual(0, _image.GetItemPosition(first));
      Assert.AreEqual(1, _image.GetItemPosition(second));
    }

    [Test]
    public void ExportToString()
    {
      var vector = new Vectors(_image, "firstVector");
      string s = vector.ExportToString();
      Assert.IsNotNull(s);
    }

    [Test]
    public void ImportFromString()
    {
      string name = "firstVector";
      var vector = new Vectors(_image, name);
      string s = vector.ExportToString();

      var vectors = _image.ImportVectorsFromString(s, false, false);
      Assert.AreEqual(1, vectors.Count);
      Assert.AreEqual(name, vectors[0].Name);
    }
  }
}
