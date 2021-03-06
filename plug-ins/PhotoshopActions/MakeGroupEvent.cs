// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// MakeGroupEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class MakeGroupEvent : MakeEvent
  {
    public MakeGroupEvent(MakeEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay =>
      base.EventForDisplay + " Group";

    protected override IEnumerable ListParameters()
    {
      yield return "From: current layer";

      var usng = Parameters["Usng"] as ObjcParameter;
      if (usng != null)
	{
	  yield return "Name: \"" + usng.GetValueAsString("Nm") + "\"";
	}
    }

    override public bool Execute()
    {
      var image = ActiveImage;
      var group = new LayerGroup(image);

      image.InsertLayer(group, -1);

      SelectedLayers.ForEach(layer => MoveLayerToGroup(layer, group));

      return true;
    }

    void MoveLayerToGroup(Layer layer, LayerGroup group)
    {
      var name = layer.Name;
      var copy = new Layer(layer);

      group.Insert(copy, -1);      

      ActiveImage.RemoveLayer(layer);
      copy.Name = name;
    }
  }
}
