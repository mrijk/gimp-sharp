// The Slice Tool plug-in
// Copyright (C) 2004-2013 Maurits Rijk
//
// SliceTool.cs
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

using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {    
    SliceData _sliceData = new SliceData();

    static void Main(string[] args)
    {
      GimpMain<SliceTool>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_slice_tool",
			   _("Slice Tool"),
			   _("The Image Slice Tool is used to apply image slicing and rollovers."),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2005-2013",
			   _("Slice Tool..."),
			   "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Web",
	  IconFile = "SliceTool.png"
	};
    }
    
    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("SliceTool", true);
      return new Dialog(_image, _drawable, _sliceData);
    }
 
    override protected bool OnClose()
    {
      if (_sliceData.Changed)
	{
	  var message = new MessageDialog(null, DialogFlags.DestroyWithParent,
					  MessageType.Warning, 
					  ButtonsType.YesNo, 
					  _("Some data has been changed!\n") + 
			      _("Do you really want to discard your changes?"));
	  return (ResponseType) message.Run() == ResponseType.Yes;
	}
      return true;
    }

    override protected void DialogRun(ResponseType type)
    {
      (Dialog as Dialog).DialogRun(type);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Fix me. Only used to fill in _image and _drawable;
    }
  }
}
