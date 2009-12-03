// The PictureFrame plug-in
// Copyright (C) 2006-2009 Oded Coster
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

using System;
using System.Collections.Generic;

using Gtk;

namespace Gimp.PictureFrame
{
  public class PictureFrame : Plugin
  {
  
    private string _pictureFrameImagePath;
  
    static void Main(string[] args)
    {
      new PictureFrame(args);
    }

    public PictureFrame(string[] args) : base(args, "PictureFrame")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_picture_frame",
				 _("Picture frame"),
				 _("Picture frame"),
				 "Oded Coster",
				 "(C) Oded Coster",
				 "2006-2009",
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
    	
      var dialog = DialogNew("Picture Frame", "Picture Frame", 
			     IntPtr.Zero, 0, 
			     Gimp.StandardHelpFunc, "Picture Frame");
      dialog.Modal = false;

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);
      
#if false
      var entry = new FileEntry("Load Frame...", "PictureFrame.svg",
				      false, true);
      entry.FilenameChanged += GetNewFileName;
#else
      var entry = new FileChooserButton(_("Load Frame..."), 
					FileChooserAction.Open);
      entry.SelectionChanged += delegate
	{
	  _pictureFrameImagePath = entry.Filename;
	};
#endif
      vbox.PackStart(entry, false, false, 0);

      return dialog;
    }
    
    override protected void Render(Image image, Drawable drawable)
    {
      try
	{
	  var frame = Image.Load(RunMode.Interactive, 
				 _pictureFrameImagePath, 
				 _pictureFrameImagePath);
	  var newLayer = new Layer(frame.ActiveLayer, image) 
	    {Visible = true};
          
	  image.UndoGroupStart();
	  
	  image.AddLayer(newLayer, -1); 
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
