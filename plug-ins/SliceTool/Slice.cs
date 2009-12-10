// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Slice.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using System.IO;
using System.Xml;

using Gdk;

namespace Gimp.SliceTool
{
  abstract public class Slice : IComparable
  {
    public Slice Begin {get; set;}
    public Slice End {get; set;}
    protected int _position;
    public int Index {get; set;}
    public bool Locked {get; set;}
    public bool Changed {get; private set;}
    
    protected Slice(Slice begin, Slice end, int position)
    {
      Begin = begin;
      End = end;
      _position = position;
    }
    
    protected Slice()
    {
    }

    protected Slice(int index)
    {
      Index = index;
    }

    public int CompareTo(object obj)
    {
      var slice = obj as Slice;
      return Position - slice.Position;
    }

    abstract public void Draw(PreviewRenderer renderer);
    abstract public bool IntersectsWith(Rectangle rectangle);
    abstract public bool IsPartOf(Rectangle rectangle);
    abstract public Rectangle SliceRectangle(Rectangle rectangle);
    abstract public void SetPosition(Coordinate<int> c);
    abstract public bool PointOn(Coordinate<int> c);
    abstract public void Save(StreamWriter w);
    
    public int Position
    {
      get {return _position;}
      set 
	{
	  Changed = (_position != value);
	  _position = value;
	}
    }

    abstract public Cursor Cursor
    {
      get ;
    }

    protected void Save(StreamWriter w, string type)
    {
      w.WriteLine("\t<slice type=\"{0}\" position=\"{1}\" index=\"{2}\" begin=\"{3}\" end=\"{4}\"/>", 
		  type, Position, Index, Begin.Index, End.Index);
      Changed = false;
    }

    public void Load(XmlNode slice)
    {
      Position = GetValueOfNode(slice, "position");
      Index = GetValueOfNode(slice, "index");
      Begin = new HorizontalSlice(GetValueOfNode(slice, "begin"));
      End = new HorizontalSlice(GetValueOfNode(slice, "end"));
      Changed = false;
    }

    int GetValueOfNode(XmlNode slice, string item)
    {
      var attributes = slice.Attributes;
      var position = (XmlAttribute) attributes.GetNamedItem(item);
      return (int) Convert.ToDouble(position.Value);
    }

    public void Resolve(SliceSet slices)
    {
      Begin = slices[Begin.Index];
      End = slices[End.Index];
    }
  }
}
