using System;
using System.IO;
using System.Xml;

using Gdk;

namespace Gimp.SliceTool
{
  abstract public class Slice : IComparable
  {
    protected Slice _begin;
    protected Slice _end;
    protected int _position;
    int _index;
    bool _locked;
    bool _changed = false;
    
    protected Slice(Slice begin, Slice end, int position)
    {
      _begin = begin;
      _end = end;
      _position = position;
    }
    
    protected Slice()
    {
    }

    protected Slice(int index)
    {
      _index = index;
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
    abstract public void Save(StreamWriter w);
    
    public int Index
    {
      get {return _index;}
      set {_index = value;}
    }

    public Slice Begin
    {
      get {return _begin;}
      set {_begin = value;}
    }

    public Slice End
    {
      get {return _end;}
      set {_end = value;}
    }

    public int Position
    {
      get {return _position;}
      set 
	  {
	  _changed = (_position != value);
	  _position = value;
	  }
    }

    public bool Locked
    {
      get {return _locked;}
      set {_locked = value;}
    }

    public bool Changed
    {
      get {return _changed;}
    }

    abstract public CursorType CursorType
    {
      get ;
    }

    protected void Save(StreamWriter w, string type)
    {
      w.WriteLine("\t<slice type=\"{0}\" position=\"{1}\" index=\"{2}\" begin=\"{3}\" end=\"{4}\"/>", 
		  type, Position, Index, _begin.Index, _end.Index);
      _changed = false;
    }

    public void Load(XmlNode slice)
    {
      _position = GetValueOfNode(slice, "position");
      _index = GetValueOfNode(slice, "index");
      _begin = new HorizontalSlice(GetValueOfNode(slice, "begin"));
      _end = new HorizontalSlice(GetValueOfNode(slice, "end"));
      _changed = false;
    }

    int GetValueOfNode(XmlNode slice, string item)
    {
      XmlAttributeCollection attributes = slice.Attributes;
      XmlAttribute position = (XmlAttribute) attributes.GetNamedItem(item);
      return (int) Convert.ToDouble(position.Value);
    }

    public void Resolve(SliceSet slices)
    {
      _begin = slices[_begin.Index];
      _end = slices[_end.Index];
    }
  }
  }
