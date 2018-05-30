// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SelectBrushByIndexEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class SelectBrushByIndexEvent : ActionEvent
  {
    readonly int _index;

    public SelectBrushByIndexEvent(ActionEvent srcEvent, int index) :
      base(srcEvent)
    {
      _index = index;
    }

    public override bool IsExecutable
    {
      // Fix me: provide mappings for all brushes
      // 11: soft round, 21 pixels
      get {return _index == 11;}
    }

    public override string EventForDisplay
    {
      get => base.EventForDisplay + " brush " + _index;
    }

    override public bool Execute()
    {
      var brushes = new BrushList();
      var brush = brushes.Find("2. Hardness 025");
      Context.Brush = brush;
      
      return true;
    }
  }
}
