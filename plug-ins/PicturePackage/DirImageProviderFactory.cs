using System;
using System.IO;

namespace Gimp.PicturePackage
{
	public class DirImageProviderFactory : ProviderFactory
	{
		string _parent;
		string[] _files;
		int _index = 0;

		public DirImageProviderFactory(string parent, bool recursive)
		{
			_parent = parent;
			_files = Directory.GetFiles(parent);
		}

		public override void Reset()
		{
			_index = 0;
		}

		public override ImageProvider Provide()
		{
			while (_index < _files.Length)
			{
				string file = _files[_index++];
				FileImageProvider provider = 
					new FileImageProvider(file, _parent + "/" + file);
				if (provider.GetImage() != null)
				{
					return provider;
				}
			}
			return null;
		}

		public override void Cleanup(ImageProvider provider)
		{
			provider.Release();
		}
	}
}
