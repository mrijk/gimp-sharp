using System;

namespace Gimp.PicturePackage
{
	abstract public class Renderer
	{
		public Renderer()
		{
		}

		abstract public void Render(double x, double y, double w, double h);
	}
}
