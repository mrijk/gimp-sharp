using System;
using System.Collections;

namespace Gimp.PicturePackage
{
	public class RectangleSet : IEnumerable
	{
		ArrayList _set = new ArrayList();

		public RectangleSet()
		{
		}

		public IEnumerator GetEnumerator()
		{
			return _set.GetEnumerator();
		}

		public void Add(Rectangle rectangle)
		{
			_set.Add(rectangle);
		}

		public Rectangle this[int index]
		{
			get {return (Rectangle) _set[index];}
		}

		public Rectangle Find(int x, int y)
		{
			foreach (Rectangle rectangle in _set)
			{
				if (rectangle.Inside(x, y))
					return rectangle;
			}
			return null;
		}

		public void Render(ProviderFactory factory, Renderer renderer)
		{
			factory.Reset();
			foreach (Rectangle rectangle in _set)
			{
				ImageProvider provider = factory.Provide();
				rectangle.Render(provider.GetImage(), renderer);
				factory.Cleanup(provider);
			}
			factory.Cleanup();
			renderer.Cleanup();
		}

		int _index = 0;
		public bool Render(ProviderFactory factory, Renderer renderer, bool foo)
		{
			if (_index == 0)
			{
				factory.Reset();
			}

			if (_index == _set.Count)
			{
				factory.Cleanup();
				renderer.Cleanup();
				_index = 0;
				return false;
			}

			Rectangle rectangle = (Rectangle) _set[_index++];
			ImageProvider provider = factory.Provide();
			rectangle.Render(provider.GetImage(), renderer);
			factory.Cleanup(provider);

			return true;
		}

		public int Count
		{
			get {return _set.Count;}
		}
	}
}