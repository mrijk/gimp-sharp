// The PictureFrame plug-in
// Copyright (C) 2006-2011 Oded Coster
//
// PictureFrame.cs
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

namespace Gimp.PictureFrame
{
  public class PictureFrame : Plugin
  {  
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<string>("image_path", _("Path to load frame image from"), "")
      };
      GimpMain<PictureFrame>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_picture_frame",
			   _("Picture frame"),
			   _("Picture frame"),
			   "Oded Coster",
			   "(C) Oded Coster",
			   "2006-2011",
			   _("Picture frame..."),
			   "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "PictureFrame.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Picture Frame", true);
      return new Dialog(Variables);
    }
    
    override protected void Render(Image image)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image);
    }
  }
}
