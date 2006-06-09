// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// GaussianBlurEvent.cs
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
  public class GaussianBlurEvent : ActionEvent
  {
    double _radius;

    public GaussianBlurEvent()
    {
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      string units;
      _radius = parser.ReadDouble("Rds", out units);
      Console.WriteLine("\tRadius: " + _radius);

      return this;
    }

    override public bool Execute()
    {
      Procedure procedure = new Procedure("plug_in_gauss");
      procedure.Run(Image, Drawable, _radius, _radius, 0);

      return true;
    }
  }
}
