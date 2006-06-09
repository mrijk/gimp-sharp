// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ActionEvent.cs
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
  abstract public class ActionEvent
  {
    /*
    readonly byte _expanded;
    readonly byte _enabled;
    readonly byte _withDialog;
    readonly byte _dialogOptions;
    */
    string _eventForDisplay;
    protected int _numberOfItems;

    static Drawable _drawable;
    static Image _image;

    public ActionEvent()
    {
    }
    
    public ActionEvent(ActionEvent srcEvent)
    {
      _eventForDisplay = srcEvent._eventForDisplay;
      _numberOfItems = srcEvent._numberOfItems;
    }

    public virtual bool IsExecutable
    {
      get {return true;}
    }

    public static Drawable Drawable
    {
      get {return _drawable;}
      set {_drawable = value;}
    }

    public static Image Image
    {
      get {return _image;}
      set {_image = value;}
    }

    public string EventForDisplay
    {
      get {return _eventForDisplay;}
      set {_eventForDisplay = value;}
    }

    public int NumberOfItems
    {
      get {return _numberOfItems;}
      set {_numberOfItems = value;}
    }

    public virtual ActionEvent Parse(ActionParser parser)
    {
      return this;
    }

    public virtual bool Execute()
    {
      Console.WriteLine("Execute {0} not implemented", _eventForDisplay);
      return true;
    }
  }
}
