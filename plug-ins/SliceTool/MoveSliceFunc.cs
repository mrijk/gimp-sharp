// The Slice Tool plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// MoveSliceFunc.cs
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

using Gdk;

namespace Gimp.SliceTool
{
  public class MoveSliceFunc : MouseFunc
  {
    readonly Slice _slice;

    MoveSliceFunc(SliceData sliceData, Preview preview, Slice slice) :
      base(sliceData, preview)
    {
      _slice = slice;
      Preview.Renderer.Function = Gdk.Function.Equiv;
    }

    override protected void OnRelease() 
    {
      Preview.Renderer.Function = Gdk.Function.Copy;
      SliceData.Cleanup(_slice);
      Redraw();
    }
		
    override protected void OnMove(IntCoordinate c)
    {
      _slice.Draw(Preview.Renderer);
      _slice.SetPosition(c);
      _slice.Draw(Preview.Renderer);
    }

    public static MouseFunc GetActualFunc(IntCoordinate c, MouseFunc func)
    {
      var slice = func.SliceData.FindSlice(c);
      return (func.SliceIsSelectable(slice)) 
	? new MoveSliceFunc(func.SliceData, func.Preview, slice) : func;
    }
  }
}
