using System;

namespace Gimp.SliceTool
{
  abstract public class Slice : IComparable
  {
    int _index;
    
    public Slice()
    {
    }
    
    abstract public void Draw(PreviewRenderer renderer);
    abstract public bool IntersectsWith(Rectangle rectangle);
    abstract public Rectangle SliceRectangle(Rectangle rectangle);
    abstract public void SetPosition(int x, int y);
    abstract public int CompareTo(object obj);
    abstract public bool PointOn(int x, int y);
    
    public int Index
    {
      get {return _index;}
      set {_index = value;}
    }
  }
  }
