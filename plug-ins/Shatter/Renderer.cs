// The Shatter plug-in
// Copyright (C) 2006-2011 Maurits Rijk
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

namespace Gimp.Shatter
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image, Drawable drawable)
    {
      // Break up image in pieces
      var ul = new Coord(0, 0);
      var lr = new Coord(drawable.Width, drawable.Height);
      var shards = new ShardSet(ul, lr, GetValue<int>("pieces"));

      // 
      
      var tool = new FreeSelectTool(image);

      foreach (Shard shard in shards)
	{
	  tool.Select(shard.GetValues(), ChannelOps.Replace);
	}
    }
  }
}
