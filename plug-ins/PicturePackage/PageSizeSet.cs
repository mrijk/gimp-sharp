using System;
using System.Collections;

namespace Gimp.PicturePackage
{
  public class PageSizeSet
  {
    ArrayList _set = new ArrayList();

    public PageSizeSet()
    {
    }

    public void Add(PageSize size)
    {
      int index = _set.BinarySearch(size);
      if (index < 0)
	{
	_set.Insert(-index - 1, size);
	}
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public PageSize this[int index]
    {
      get {return (PageSize) _set[index];}
    }
  }
  }
