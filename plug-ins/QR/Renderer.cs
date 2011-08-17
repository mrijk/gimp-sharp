// The QR plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

using System;

namespace Gimp.QR
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image, Drawable drawable)
    {
      var newImage = GetImageFromGoogleCharts(drawable.Dimensions);
      if (newImage != null)
	{
	  Display.Reconnect(image, newImage);
	}
    }

    public void Render(GimpPreview preview)
    {
      var image = GetImageFromGoogleCharts(preview.Size);
      preview.Redraw(image.ActiveDrawable);
      image.Delete();
    }

    Image GetImageFromGoogleCharts(Dimensions dimensions)
    {
      var chl = "&chl=" + Uri.EscapeDataString(GetValue<string>("text"));
      var chs = string.Format("&chs={0}x{1}", dimensions.Width, 
			      dimensions.Height);
      var choe = "&choe=" + GetEncodingString();
      var chld = string.Format("&chld={0}|{1}", GetValue<string>("error_correction"), 
			       GetValue<int>("margin"));
      var url = "http://chart.apis.google.com/chart?cht=qr" 
	+ chl + chs + choe + chld;

      var procedure = new Procedure("file-uri-load");

      try 
	{
	  var returnArgs = procedure.Run(url, url);

	  return returnArgs[0] as Image;
	}
      catch (GimpSharpException e)
	{
	  new Message(e.Message);
	  return null;
	}
    }

    string GetEncodingString()
    {
      int encoding = GetValue<int>("encoding");
      if (encoding == 1) 
	{
	  return "Shift_JIS";
	}
      else if (encoding == 2)
	{
	  return "ISO-8859-1";
	}
      else
	{
	  return "UTF-8";
	}
    }
  }
}
