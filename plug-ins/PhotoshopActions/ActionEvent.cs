// The PhotoshopActions plug-in
// Copyright (C) 2006-2010 Maurits Rijk
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
using System.Collections.Generic;
using System.Diagnostics;

using Gtk;

namespace Gimp.PhotoshopActions
{
  abstract public class ActionEvent : IExecutable
  {
    public static List<ActionSet> ActionSetCollection;

    public bool IsEnabled {get; set;}
    public bool HasDescriptor {get; set;}

    /*
    readonly byte _expanded;
    readonly byte _withDialog;
    readonly byte _dialogOptions;
    */

    public int NumberOfItems {get; set;}
    public virtual string EventForDisplay {get; set;}

    readonly ParameterSet _parameters = new ParameterSet();

    static Drawable _activeDrawable;
    static Image _activeImage;

    static LinkedLayersSet _linkedLayersSet = new LinkedLayersSet();

    // TODO: this should become a set
    static List<Layer> _selectedLayers = new List<Layer>();
    static Layer _selectedLayer;

    // TODO: this is a hack
    public static Channel SelectedChannel {get; set;}
    public static string SelectedChannelName {get; set;}

    static Channel _previousSelection;

    public ActionEvent()
    {
    }
    
    public ActionEvent(ActionEvent srcEvent)
    {
      EventForDisplay = srcEvent.EventForDisplay;
      NumberOfItems = srcEvent.NumberOfItems;
      _parameters = srcEvent._parameters;
    }

    public ParameterSet Parameters
    {
      get {return _parameters;}
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
	  ActiveDrawable = value;
	}
    }

    public static List<Layer> SelectedLayers
    {
      get {return _selectedLayers;}
    }

    public static LinkedLayersSet LinkedLayersSet
    {
      get {return _linkedLayersSet;}
    }

    protected string Format(bool value, string s)
    {
      return ((value) ? "With " : "Without ") + Abbreviations.Get(s);
    }

    protected string Format(string value, string s)
    {
      return String.Format("{0}: \"{1}\"", Abbreviations.Get(s), value);
    }

    protected string Format(int value, string s)
    {
      return String.Format("{0}: {1}", Abbreviations.Get(s), value);
    }

    protected string Format(double value, string s)
    {
      return String.Format("{0}: {1:F3}", Abbreviations.Get(s), value);
    }

    protected string Format(EnumParameter parameter, string s)
    {
      if (parameter == null) {
	return Abbreviations.Get(s) + ": fixme!";
      } else {
	return Abbreviations.Get(s) + ": " + 
	  Abbreviations.Get(parameter.Value);
      }
    }

    public void FillStore(TreeStore store, TreeIter iter)
    {
      iter = store.AppendValues(iter, EventForDisplay, this);

      foreach (string s in ListParameters())
	{
	  store.AppendValues(iter, s);
	}
    }

    protected virtual IEnumerable ListParameters()
    {
      foreach (String s in Parameters.ListParameters())
	{
	  yield return s;
	}
    }

    public virtual ActionEvent Parse(ActionParser parser)
    {
      // _parameters = new ParameterSet();
      DebugOutput.Level++;
      _parameters.Parse(parser, this, NumberOfItems);
      DebugOutput.Level--;

      return this;
    }

    public virtual bool Execute()
    {
      Console.WriteLine("Execute {0} not implemented", EventForDisplay);
      return true;
    }

    protected void RunProcedure(string name, params object[] list)
    {
      RunProcedure(name, _activeImage, _activeDrawable, list);
    }

    protected void RunProcedure(string name, Image image, Drawable drawable,
				params object[] list)
    {
      var procedure = new Procedure(name);
      procedure.Run(image, drawable, list);
    }

    protected void RememberCurrentSelection()
    {
      var tmp = SelectedLayer;
      _previousSelection = _activeImage.Selection.Save();
      SelectedLayer = tmp;
    }

    protected void RestorePreviousSelection()
    {
      _activeImage.Selection.Load(_previousSelection);
    }

    protected void GetBounds(ObjcParameter objc, out double x, out double y,
			     out double width, out double height)
    {
      var parameters = objc.Parameters;
      double top = (parameters["Top"] as DoubleParameter).
	GetPixels(ActiveImage.Height);
      double left = (parameters["Left"] as DoubleParameter).
	GetPixels(ActiveImage.Width);
      double bottom = (parameters["Btom"] as DoubleParameter).
	GetPixels(ActiveImage.Height);
      double right = (parameters["Rght"] as DoubleParameter).
	GetPixels(ActiveImage.Width);
      
      x = left;
      y = top;
      width = right - left;
      height = bottom - top;
    }
  }
}
