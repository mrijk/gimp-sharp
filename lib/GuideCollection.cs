using System;
using System.Collections;

namespace Gimp
{
	public class GuideCollection : IEnumerable
	{
		Image _image;

		public GuideCollection(Image image)
		{
			_image = image;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return new GuideIterator(_image);
		}

		public class GuideIterator : IEnumerator
		{
			Guide _current;
			Image _image;

			public GuideIterator(Image image)
			{
				_image = image;
				Reset();
			}

			public bool MoveNext()
			{
				_current = _current.FindNext();
				return _current != null;
			}

			public object Current
			{
				get {return _current;}
			}

			public void Reset()
			{
				_current = new Guide(_image, 0);
			}
		}
	}
}
