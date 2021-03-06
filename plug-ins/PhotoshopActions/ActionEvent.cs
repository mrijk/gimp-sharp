// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
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

    static protected Display ActiveDisplay {get; set;}
    static Drawable _activeDrawable;
    static Image _activeImage;

    static LinkedLayersSet _linkedLayersSet = new LinkedLayersSet();

    static List<Layer> _selectedLayers = new List<Layer>();

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

    public ParameterSet Parameters => _parameters;

    public virtual bool IsExecutable => true;

    public static Drawable ActiveDrawable
    {
      get => _activeDrawable;
      set => _activeDrawable = value;
    }

    public static Image ActiveImage
    {
      get => _activeImage;
      set 
	{
	  _activeImage = value;
	  // Fix me: this might be a bit early because this is already called 
	  // during parsing. Move to Execute
	  if (_activeImage != null)
	    {
	      //	      var layers = _activeImage.Layers;
	      //if (layers.Count == 1 && layers[0].Name == "Background")
	      //		{
	      //	  layers[0].Name = "Layer 1";
	      //	}
	      SelectLayer(_activeImage.Layers[0]);
	    }
	}
    }

    public static Layer SelectedLayer
    {
      get => SelectedLayers[0];
      set 
	{
	  SelectLayer(value);
	  //	  _activeImage.ActiveLayer = value;
	  // ActiveDrawable = value;
	}
    }

    public static List<Layer> SelectedLayers => _selectedLayers;

    protected static void SelectLayer(Layer layer, bool add = false)
    {
      Console.WriteLine("SelectLayer: " + layer.Name);
      // Fix me: check if layer is not already in this list
      if (!add)
	SelectedLayers.Clear();
      SelectedLayers.Add(layer);
      ActiveImage.ActiveLayer = layer;
      ActiveDrawable = layer;
    }

    public static LinkedLayersSet LinkedLayersSet => _linkedLayersSet;

    protected string Format(bool value, string s) =>
      ((value) ? "With " : "Without ") + Abbreviations.Get(s);

    protected string Format(string value, string s) => 
      $"{Abbreviations.Get(s)}: \"{value}\"";

    protected string Format(int value, string s) =>
      $"{Abbreviations.Get(s)}: {value}";

    protected string Format(double value, string s) =>
      String.Format("{0}: {1:F3}", Abbreviations.Get(s), value);

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

      foreach (var s in ListParameters())
	{
	  store.AppendValues(iter, s);
	}
    }

    protected virtual IEnumerable ListParameters()
    {
      foreach (var s in Parameters.ListParameters())
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
      Console.WriteLine($"Execute {EventForDisplay} not implemented");
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
