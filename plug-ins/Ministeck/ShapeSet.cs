using System;
using System.Collections;

namespace Gimp.Ministeck
  {
    public class ShapeSet : IEnumerable 
    {
      ArrayList _set;

      public ShapeSet(params ShapeDescription[] shapes)
      {
	_set = new ArrayList();
	foreach (ShapeDescription shape in shapes)
	  {
	  _set.Add(shape);
	  }		
      }

      public ShapeSet(ShapeSet s)
      {
	_set = new ArrayList(s._set);
      }

      IEnumerator IEnumerable.GetEnumerator() 
      {
	return _set.GetEnumerator();
      }

      public void Insert(int index, ShapeDescription val)
      {
	_set.Insert(index, val);
      }

      public int Count
      {
	get {return _set.Count;}
      }
    }
  }
