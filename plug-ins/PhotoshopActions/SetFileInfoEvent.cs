// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SetFileInfoEvent.cs
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
  public class SetFileInfoEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    [Parameter("Cpyr")]
    bool _copyright;
    [Parameter("CprN")]
    string _copyrightNotice;
    [Parameter("URL")]
    string _url;
    
    public SetFileInfoEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _objc.Fill(this);
    }

    public override string EventForDisplay
    {
      get => base.EventForDisplay + " File Info of current document";
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_copyright, "Copyright");
      yield return "Copyright Notice: " + _copyrightNotice;
      yield return "URL: " + _url;
    }

    override public bool Execute()
    {
      Console.WriteLine("fix me: fileinfo not stored yet!");
      return true;
    }
  }
}
