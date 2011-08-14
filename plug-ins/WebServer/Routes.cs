// The WebServer plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Routes.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using Manos;
using Manos.Http;

using System;

namespace Gimp.WebServer
{
  public class Routes : ManosApp
  {
    public Routes()
    {
    }

    [Route("/", "/Home", "/Index")]
    public void Index(IManosContext ctx)
    {
      ctx.Response.SendFile("images.html");
      ctx.Response.End();
    }

    [Route("/gimp/images")]
    public void Images(IManosContext ctx)
    {
      Console.WriteLine("Got /images request");
      var images = new ImageList();
      ctx.Response.End(string.Format("{{\"count\" : {0} }}", images.Count));
    }

    [Get("/gimp/images/{index}")]
    public void Images(IManosContext ctx, int index)
    {
      var images = new ImageList();
      var image = images[index];
      ctx.Response.End("Image: {0}, size {1} x {2}", image.Name, image.Width, 
		       image.Height);
    }

    [Route("/gimp/new-image")]
    public void Images(IManosContext ctx, Routes app, int width, int height)
    {
      var image = new Image(width, height, ImageBaseType.Rgb);

      ctx.Response.End("Added image"); 
    }
  }
}
