// The PicturePackage plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Renderer.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

namespace Gimp.PicturePackage
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Layout layout, ProviderFactory loader)
    {
      int resolution = GetValue<int>("resolution");

      var size = layout.GetPageSizeInPixels(resolution);
      var composed = new Image(size.ToDimensions(), ImageBaseType.Rgb);

      if (layout.Render(loader, 
			new ImageRenderer(layout, composed, resolution)))
	;

      // Fix me: check next couple of lines!
#if false
        DialogState = DialogStateType.SrcImgValid;
      else
        DialogState = DialogStateType.SrcImgInvalid;
#endif
      if (GetValue<bool>("flatten"))
	{
	  composed.Flatten();
	}

      if (GetValue<int>("color_mode") == 0) // ColorMode.GRAY)
	{
	  composed.ConvertGrayscale();
	}

      new Display(composed);
      Display.DisplaysFlush();
    }
  }
}
