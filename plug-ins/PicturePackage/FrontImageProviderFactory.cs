using System;

namespace Gimp.PicturePackage
{
	public class FrontImageProviderFactory : ProviderFactory
	{
		ImageProvider _provider;

		public FrontImageProviderFactory(Image image)
		{
			_provider = new FrontImageProvider(image);
		}

		public override ImageProvider Provide()
		{
			return _provider;
		}

		public override void Cleanup()
		{
			_provider.Release();
		}
	}
}
