using System;

namespace Gimp.PicturePackage
{
	public class FileImageProvider : ImageProvider
	{
		Image _image;
		string _filename;
		string _rawFilename;

		public FileImageProvider(string filename)
		{
			_filename = filename;
			_rawFilename = filename;
		}

		public FileImageProvider(string filename, string rawFilename)
		{
			_filename = filename;
			_rawFilename = rawFilename;
		}

		override public Image GetImage()
		{
			if (_image == null)
			{
				_image = Image.Load(RunMode.NONINTERACTIVE, _filename, _rawFilename);
			}
			return _image;
		}

		override public void Release()
		{
			_image.Delete();
			_image = null;
		}
	}
}
