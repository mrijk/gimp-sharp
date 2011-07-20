// The Shatter plug-in
// Copyright (C) 2006-2011 Maurits Rijk
//
// Shatter.cs
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

namespace Gimp.Shatter
{
  public class Shatter : PluginWithPreview<AspectPreview>
  {
    Variable<int> _pieces = new Variable<int>("pieces", _("Number of shards"),
					      4);

    static void Main(string[] args)
    {
      GimpMain<Shatter>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_shatter",
			   _("Shatter an image"),
			   _("Shatter an image"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Shatter..."),
			   "RGB*, GRAY*",
			   new ParamDefList(_pieces))
	{
	  MenuPath = "<Image>/Filters/Distorts",
	  IconFile = "Shatter.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      var dialog = DialogNew("Shatter", "Shatter", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "Shatter");

      var table = new GimpTable(4, 3, false) {
	ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);
      
      new ScaleEntry(table, 0, 1, "Pieces:", 150, 3,
		     _pieces, 1.0, 256.0, 1.0, 8.0, 0);

      return dialog;
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Break up image in pieces
      var ul = new Coord(0, 0);
      var lr = new Coord(drawable.Width, drawable.Height);
      var shards = new ShardSet(ul, lr, _pieces.Value);

      // 
      
      var tool = new FreeSelectTool(image);

      foreach (Shard shard in shards)
	{
	  tool.Select(shard.GetValues(), ChannelOps.Replace);
	}
    }
  }
}
