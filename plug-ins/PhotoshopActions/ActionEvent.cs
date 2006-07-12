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
using System.Collections;

using Gtk;

namespace Gimp.PhotoshopActions
{
  abstract public class ActionEvent : IExecutable
  {
    /*
    readonly byte _expanded;
    readonly byte _enabled;
    readonly byte _withDialog;
    readonly byte _dialogOptions;
    */
    bool _hasDescriptor;

    string _eventForDisplay;
    protected int _numberOfItems;
    ParameterSet _parameters;

    static Drawable _activeDrawable;
    static Image _activeImage;

    // TODO: this should become a set
    static Layer _selectedLayer;

    public ActionEvent()
    {
    }
    
    public ActionEvent(ActionEvent srcEvent)
    {
      _eventForDisplay = srcEvent._eventForDisplay;
      _numberOfItems = srcEvent._numberOfItems;
      _parameters = srcEvent._parameters;
    }

    public ParameterSet Parameters
    {
      get {return _parameters;}
    }

    public bool HasDescriptor
    {
      get {return _hasDescriptor;}
      set {_hasDescriptor = value;}
    }

    public virtual bool IsExecutable
    {
      get {return true;}
    }

    public static Drawable ActiveDrawable
    {
      get {return _activeDrawable;}
      set {_activeDrawable = value;}
    }

    public static Image ActiveImage
    {
      get {return _activeImage;}
      set 
	{
	  _activeImage = value;
	  if (_activeImage != null)
	    {
	      _selectedLayer = _activeImage.Layers[0];
	    }
	}
    }

    public static Layer SelectedLayer
    {
      get {return _selectedLayer;}
      set 
	{
	  _selectedLayer = value;
	  _activeImage.ActiveLayer = value;
	}
    }

    public virtual string EventForDisplay
    {
      get {return _eventForDisplay;}
      set {_eventForDisplay = value;}
    }

    public int NumberOfItems
    {
      get {return _numberOfItems;}
      set {_numberOfItems = value;}
    }

    public void FillStore(TreeStore store, TreeIter iter)
    {
      iter = store.AppendValues(iter, EventForDisplay, this);
      FillParameters(store, iter);

      foreach (string s in ListParameters())
	{
	  store.AppendValues(iter, s);
	}
    }

    protected virtual void FillParameters(TreeStore store, TreeIter iter)
    {
    }

    protected virtual IEnumerable ListParameters()
    {
      yield break;
    }

    public virtual ActionEvent Parse(ActionParser parser)
    {
      _parameters = new ParameterSet();
      DebugOutput.Level++;
      _parameters.Parse(parser, this, NumberOfItems);
      DebugOutput.Level--;

      return this;
    }

    public virtual bool Execute()
    {
      Console.WriteLine("Execute {0} not implemented", _eventForDisplay);
      return true;
    }

    protected void RunProcedure(string name, params object[] list)
    {
      Procedure procedure = new Procedure(name);
      procedure.Run(_activeImage, _activeDrawable, list);
    }

    protected void GetBounds(ObjcParameter objc, out double x, out double y,
			     out double width, out double height)
    {
      ParameterSet parameters = objc.Parameters;
      double top = (parameters["Top"] as DoubleParameter).Value;
      double left = (parameters["Left"] as DoubleParameter).Value;
      double bottom = (parameters["Btom"] as DoubleParameter).Value;
      double right = (parameters["Rght"] as DoubleParameter).Value;
      
      x = left * ActiveImage.Width / 100;
      y = top * ActiveImage.Height / 100;
      width = (right - left) * ActiveImage.Width / 100 + 1;
      height = (bottom - top) * ActiveImage.Height / 100 + 1;
    }
  }
}
