// The Splitter plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Splitter.cs
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
using System.Collections.Generic;
using Gtk;

namespace Gimp.Splitter
{
  public class Splitter : Plugin
  {
    Variable<string> _formula = new Variable<string>
    ("formula", _("Formula for splitting image"), "");

    Variable<int> _translate_1_x = new Variable<int>
    ("translate_1_x", _("Translation in x first layer"), 0);
    Variable<int> _translate_1_y = new Variable<int>
    ("translate_1_y", _("Translation in y first layer"), 0);
    Variable<int> _rotate_1 = new Variable<int>
    ("rotate_1", _("Rotation first layer"), 0);

    Variable<int> _translate_2_x = new Variable<int>
    ("translate_2_x", _("Translation in x second layer"), 0);
    Variable<int> _translate_2_y = new Variable<int>
    ("translate_2_y", _("Translation in y second layer"), 0);
    Variable<int> _rotate_2 = new Variable<int>
    ("rotate_2", _("Rotation second layer"), 0);

    Variable<int> _keepLayer = new Variable<int>
    ("keep_layer", _("Keep first (0), second (1) or both (2) layer(s)"), 0);

    Variable<bool> _merge = new Variable<bool>
    ("merge", _("Merge layers after splitting"), true);

    Variable<UInt32> _seed = new Variable<UInt32>
    ("seed", _("Value for random seed"), 0);
    Variable<bool> _randomSeed = new Variable<bool>
    ("random_seed", _("Use specified random seed"), false);

    static void Main(string[] args)
    {
      GimpMain<Splitter>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = 
	new ParamDefList(_formula, _translate_1_x, _translate_1_y, _rotate_1,
			 _translate_2_x, _translate_2_y, _rotate_2,
			 _keepLayer, _merge, _seed, _randomSeed);

      yield return new Procedure("plug_in_splitter",
				 _("Splits an image."),
				 _("Splits an image in separate parts using a formula of the form f(x, y) = 0"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "1999 - 2011",
				 _("Splitter..."),
				 "RGB*",
				 inParams)
	  {
	    MenuPath = "<Image>/Filters/Generic",
	    IconFile = "Splitter.png"
	  };
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("splitter", true);

      var dialog = DialogNew(_("Splitter"), _("splitter"),
			     IntPtr.Zero, 0, null, _("splitter"));

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 2)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      var hbox = new HBox(false, 6);
      table.Attach(hbox, 0, 2, 0, 1);

      hbox.Add(new Label("f(x, y):"));
      hbox.Add(new GimpEntry(_formula));
      hbox.Add(new Label("= 0"));

      var frame1 = CreateLayerFrame("Layer 1", _translate_1_x, _translate_1_y, 
				    _rotate_1);
      table.Attach(frame1, 0, 1, 1, 2);

      var frame2 = CreateLayerFrame("Layer 2", _translate_2_x, _translate_2_y, 
				    _rotate_2);
      table.Attach(frame2, 1, 2, 1, 2);

      var merge = new GimpCheckButton(_("Merge visible layers"), _merge);
      table.Attach(merge, 0, 1, 3, 4);

      var advanced = new Button(_("Advanced Options..."));
      advanced.Clicked += delegate
	{
	  var advancedDialog = new AdvancedDialog(_seed.Value, 
						  _randomSeed.Value);
	  advancedDialog.ShowAll();
	  if (advancedDialog.Run() == ResponseType.Ok)
	  {
	    _seed.Value = advancedDialog.Seed;
	    _randomSeed.Value = advancedDialog.RandomSeed;
	  }
	  advancedDialog.Destroy();
	};
      table.Attach(advanced, 1, 2, 3, 4);

      var keep = ComboBox.NewText();

      keep.AppendText(_("Both Layers"));
      keep.AppendText(_("Layer 1"));
      keep.AppendText(_("Layer 2"));
      keep.Active = _keepLayer.Value;
      keep.Changed += delegate {_keepLayer.Value = keep.Active;};
      table.AttachAligned(0, 5, _("Keep:"), 0.0, 0.5, keep, 1, true);

      return dialog;
    }

    GimpFrame CreateLayerFrame(string frameLabel, Variable<int> translateX,
			       Variable<int> translateY, Variable<int> rotate)
    {
      var frame = new GimpFrame(_(frameLabel));

      var table = new GimpTable(3, 3)
	{BorderWidth = 12, RowSpacing = 12, ColumnSpacing = 12};
      frame.Add(table);

      AddSpinButton(table, 0, int.MinValue, int.MaxValue, "Translate X:",
		    translateX);
      AddSpinButton(table, 1, int.MinValue, int.MaxValue, "Translate Y:",
		    translateY);
      AddSpinButton(table, 2, 0, 360, "Rotate:", rotate);

      return frame;
    }

    void AddSpinButton(GimpTable table, int row, int min, int max, string label,
		       Variable<int> variable)
    {
      var spinner = new GimpSpinButton(min, max, 1, variable) {WidthChars = 4};
      table.AttachAligned(0, row, _(label), 0.0, 0.5, spinner, 1, true);      
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var parser = new MathExpressionParser();

      var rectangle = image.Bounds;

      parser.Init(_formula.Value, image.Dimensions);

      var newImage = new Image(image.Dimensions, image.BaseType);

      Layer layer1;
      PixelRgn destPR1;
      if (_keepLayer.Value == 0 || _keepLayer.Value == 1)
	{
	  layer1 = new Layer(newImage, _("layer_one"), ImageType.Rgba);
	  layer1.Translate(_translate_1_x.Value, _translate_1_y.Value);
	  layer1.AddAlpha();
	  newImage.AddLayer(layer1, 0);

	  destPR1 = new PixelRgn(layer1, rectangle, true, false);
	}
      else
	{
	  layer1 = null;
	  destPR1 = null;
	}

      Layer layer2;
      PixelRgn destPR2;
      if (_keepLayer.Value == 0 || _keepLayer.Value == 2)
	{
	  layer2 = new Layer(newImage, _("layer_two"), ImageType.Rgba);
	  layer2.Translate(_translate_2_x.Value, _translate_2_y.Value);
	  layer2.AddAlpha();
	  newImage.AddLayer(layer2, 0);

	  destPR2 = new PixelRgn(layer2, rectangle, true, false);
	}
      else
	{
	  layer2 = null;
	  destPR2 = null;
	}

      var transparent = new Pixel(4);

      var srcPR = new PixelRgn(drawable, rectangle, false, false);

      if (destPR1 != null && destPR2 != null)
	{
	  var iterator = new RegionIterator(srcPR, destPR1, destPR2);
	  iterator.ForEach((src, dest1, dest2) =>
	    {
	      var tmp = Copy(src);
	      if (parser.Eval(src) < 0)
	      {
		dest1.Set(tmp);
		dest2.Set(transparent);
	      }
	      else
	      {
		dest2.Set(tmp);
		dest1.Set(transparent);
	      }
	    });
	}
      else if (destPR1 != null)
	{
	  var iterator = new RegionIterator(srcPR, destPR1);
	  iterator.ForEach((src, dest) =>
			   dest.Set((parser.Eval(src) < 0) 
				    ? Copy(src) : transparent));
	}
      else	// destPR2 != null
	{
	  var iterator = new RegionIterator(srcPR, destPR2);
	  iterator.ForEach((src, dest) =>
			   dest.Set((parser.Eval(src) >= 0) 
				    ? Copy(src) : transparent));
	}

      Rotate(layer1, _rotate_1.Value);
      Rotate(layer2, _rotate_2.Value);

      if (_merge.Value) 
	{
	  var merged = 
	    newImage.MergeVisibleLayers(MergeType.ExpandAsNecessary);
	  merged.Offsets = new Offset(0, 0);
	  newImage.Resize(merged.Dimensions, merged.Offsets);
	}

      new Display(newImage);
      
      Display.DisplaysFlush();
    }

    void Rotate(Layer layer, int angle)
    {
      if (angle != 0 && layer != null)
	{
	  layer.TransformRotate(angle * Math.PI / 180.0,
				true, 0, 0, true, false);
	}
    }

    Pixel Copy(Pixel src)
    {
      return (src.HasAlpha) 
	? src 
	: new Pixel(4) {Red = src.Red, Green = src.Green, Blue = src.Blue, 
			  Alpha = 255};
    }
  }
}
