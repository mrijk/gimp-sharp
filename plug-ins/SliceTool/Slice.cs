using System;
using Gdk;

namespace Gimp.SliceTool
{
  abstract public class Slice : IComparable
  {
    int _index;
    
    public Slice()
    {
    }
    
    abstract public void Draw(Preview preview, Gdk.GC gc);
    abstract public bool IntersectsWith(Rectangle rectangle);
    abstract public Rectangle SliceRectangle(Rectangle rectangle);
    abstract public void SetPosition(int x, int y);
    abstract public int CompareTo(object obj);
    
    abstract public void Dump();
    
    public int Index
    {
      get {return _index;}
      set {_index = value;}
    }
  }
  }
