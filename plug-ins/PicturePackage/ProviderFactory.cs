using System;

namespace Gimp.PicturePackage
{
	abstract public class ProviderFactory
	{
		public ProviderFactory()
		{
		}

		public virtual void Reset() {}
		public abstract ImageProvider Provide();
		public virtual void Cleanup(ImageProvider provider){}
		public virtual void Cleanup(){}
	}
}
