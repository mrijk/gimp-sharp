using System;
using System.Collections;

namespace Ministeck
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

      IEnumerator IEnumerable.GetEnumerator() 
      {
	return _set.GetEnumerator();
      }
    }
  }
