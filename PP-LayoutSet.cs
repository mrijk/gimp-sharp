using System;
using System.Xml;
using System.Collections;

namespace Gimp
  {
    public class LayoutSet
    {
      ArrayList _set = new ArrayList();

      public LayoutSet()
      {
      }

      public void Add(Layout layout)
      {
	_set.Add(layout);
      }
    }
  }
