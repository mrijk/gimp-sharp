using System;

namespace Gimp.SliceTool
{
  abstract public class Slice : IComparable
  {
    protected Slice _begin;
    protected Slice _end;
    protected int _position;
    int _index;
    
    public Slice(Slice begin, Slice end, int position)
    {
      _begin = begin;
      _end = end;
      _position = position;
    }
    
    public int CompareTo(object obj)
    {
      Slice slice = obj as Slice;
      return _position - slice._position;
    }

    abstract public void Draw(PreviewRenderer renderer);
    abstract public bool IntersectsWith(Rectangle rectangle);
    abstract public bool IsPartOf(Rectangle rectangle);
    abstract public Rectangle SliceRectangle(Rectangle rectangle);
    abstract public void SetPosition(int x, int y);
    abstract public bool PointOn(int x, int y);
    
    public int Index
    {
      get {return _index;}
      set {_index = value;}
    }

    public Slice Begin
    {
      set {_begin = value;}
    }

    public Slice End
    {
      set {_end = value;}
    }

    public int Position
    {
      get {return _position;}
      set {_position = value;}
    }
  }
  }
