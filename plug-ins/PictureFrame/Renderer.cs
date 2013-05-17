// The PictureFrame plug-in
// Copyright (C) 2006-2013 Oded Coster
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

namespace Gimp.PictureFrame
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image)
    {
      try
	{
	  var imagePath = GetValue<string>("image_path");
	  var frame = Image.Load(RunMode.Interactive, imagePath, imagePath);
	  var newLayer = new Layer(frame.ActiveLayer, image) {Visible = true};
          
	  image.UndoGroupStart();
	  
	  image.Add(newLayer, -1); 
	  image.ActiveLayer = newLayer;
	  
	  image.UndoGroupEnd();
	  
	  frame.Delete();
	}
      catch (Exception ex) 
	{	
	  throw new GimpSharpException(); 
	}
    }  
  }
}
