using System;

namespace Gimp.PicturePackage
{
  public class PageSize : IComparable
  {
    readonly double _width;
    readonly double _height;

    public PageSize(double width, double height)
    {
      _width = width;
      _height = height;
    }

    public double Width
    {
      get {return _width;}
    }

    public double Height
    {
      get {return _height;}
    }

    public int CompareTo(object obj)
    {
      PageSize size = obj as PageSize;
      int result = _width.CompareTo(size.Width);
      return (result == 0) ? _height.CompareTo(size.Height) : result;
    }
  }
  }
