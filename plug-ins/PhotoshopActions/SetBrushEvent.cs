// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SetBrushEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class SetBrushEvent : SetEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;
    [Parameter("Dmtr")]
    double _diameter;
    [Parameter("Hrdn")]
    double _hardness;
    [Parameter("Spcn")]
    double _spacing;
    [Parameter("Angl")]
    double _angle;
    [Parameter("Rndn")]
    double _roundness;
    [Parameter("Nm")]
    string _name;

    public SetBrushEvent(SetEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _objc.Fill(this);
    }

    public override bool IsExecutable
    {
      get => false;
    }

    public override string EventForDisplay
    {
      get => base.EventForDisplay + " current brush";
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_diameter, "Dmtr");
      yield return Format(_hardness, "Hrdn");
      yield return Format(_spacing, "Spcn");
      yield return Format(_angle, "Angl");
      yield return Format(_roundness, "Rndn");
      yield return Format(_name, "Nm");
    }

    override public bool Execute() => true;
  }
}
