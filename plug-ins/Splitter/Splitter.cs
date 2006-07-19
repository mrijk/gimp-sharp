// The Splitter plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
using Mono.Unix;
using Gtk;

namespace Gimp.Splitter
{
  public class Splitter : Plugin
  {
    [SaveAttribute]
    string _formula = "";

    [SaveAttribute]
    int _translate_1_x;
    [SaveAttribute]
    int _translate_1_y;
    [SaveAttribute]
    int _rotate_1;

    [SaveAttribute]
    int _translate_2_x;
    [SaveAttribute]
    int _translate_2_y;
    [SaveAttribute]
    int _rotate_2;
    [SaveAttribute]
    int _keepLayer = 0;

    [SaveAttribute]
    bool _merge = true;

    [SaveAttribute]
    UInt32 _seed;
    [SaveAttribute]
    bool _randomSeed;

    [STAThread]
    static void Main(string[] args)
    {
      string localeDir = Gimp.LocaleDirectory;
      Catalog.Init("Splitter", localeDir);
      new Splitter(args);
    }

    public Splitter(string[] args) : base(args)
    {
    }

    override protected ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();
      
      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_splitter",
					  Catalog.GetString("Splits an image."),
					  Catalog.GetString("Splits an image in separate parts using a formula of the form f(x, y) = 0"),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "1999 - 2006",
					  Catalog.GetString("Splitter..."),
					  "RGB*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Generic";
      procedure.IconFile = "Splitter.png";
      
      set.Add(procedure);
      
      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("splitter", true);

      Dialog dialog = DialogNew(Catalog.GetString("Splitter"), 
        Catalog.GetString("splitter"),
				IntPtr.Zero, 0, null, Catalog.GetString("splitter"));

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      GimpTable table = new GimpTable(4, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      HBox hbox = new HBox(false, 6);
      table.Attach(hbox, 0, 2, 0, 1);

      hbox.Add(new Label("f(x, y):"));
      Entry formula = new Entry();
      formula.Text = _formula;
      formula.Changed +=
	delegate(object sender, EventArgs e)
	{
	  _formula = formula.Text;
	};
      hbox.Add(formula);
      hbox.Add(new Label("= 0"));

      GimpFrame frame1 = CreateLayerFrame1();
      table.Attach(frame1, 0, 1, 1, 2);

      GimpFrame frame2 = CreateLayerFrame2();
      table.Attach(frame2, 1, 2, 1, 2);

      CheckButton merge = new CheckButton(
          Catalog.GetString("Merge visible layers"));
      merge.Active = _merge;
      merge.Toggled += delegate(object sender, EventArgs args)
	{
	  _merge = merge.Active;
	};
      table.Attach(merge, 0, 1, 3, 4);

      Button advanced = new Button(
          Catalog.GetString("Advanced Options..."));
      advanced.Clicked += delegate(object sender, EventArgs args)
	{
	  AdvancedDialog advancedDialog = new AdvancedDialog(_seed, 
							     _randomSeed);
	  advancedDialog.ShowAll();
	  if (advancedDialog.Run() == ResponseType.Ok)
	  {
	    _seed = advancedDialog.Seed;
	    _randomSeed = advancedDialog.RandomSeed;
	  }
	  advancedDialog.Destroy();
	};
      table.Attach(advanced, 1, 2, 3, 4);

      ComboBox keep = ComboBox.NewText();

      keep.AppendText(Catalog.GetString("Both Layers"));
      keep.AppendText("Layer 1");
      keep.AppendText("Layer 2");
      keep.Active = _keepLayer;
      keep.Changed += delegate(object o, EventArgs args) 
	{
	  _keepLayer = keep.Active;
	};
      table.AttachAligned(0, 5, "Keep:", 0.0, 0.5, keep, 1, true);

      dialog.ShowAll();
      return DialogRun();
    }

    GimpFrame CreateLayerFrame1()
    {
      GimpFrame frame = new GimpFrame("Layer 1");

      GimpTable table = new GimpTable(3, 3, false);
      table.BorderWidth = 12;
      table.RowSpacing = 12;
      table.ColumnSpacing = 12;
      frame.Add(table);

      SpinButton spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.Value = _translate_1_x;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_1_x = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 0, "Translate X:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.Value = _translate_1_y;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_1_y = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 1, "Translate Y:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(0, 360, 1);
      spinner.Value = _rotate_1;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _rotate_1 = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 2, "Rotate:", 0.0, 0.5, spinner, 1, true);

      return frame;
    }

    // TODO: find a way to avoid this code duplication. Anymous methods
    // however can't address ref or out parameters :(
    GimpFrame CreateLayerFrame2()
    {
      GimpFrame frame = new GimpFrame("Layer 2");

      GimpTable table = new GimpTable(3, 3, false);
      table.BorderWidth = 12;
      table.RowSpacing = 12;
      table.ColumnSpacing = 12;
      frame.Add(table);

      SpinButton spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.Value = _translate_2_x;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_2_x = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 0, "Translate X:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.Value = _translate_2_y;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_2_y = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 1, "Translate Y:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(0, 360, 1);
      spinner.Value = _rotate_2;
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _rotate_2 = spinner.ValueAsInt;
	};
      spinner.WidthChars = 4;
      table.AttachAligned(0, 2, "Rotate:", 0.0, 0.5, spinner, 1, true);

      return frame;
    }

    override protected void Render(Image image, Drawable drawable)
    {
      MathExpressionParser parser = new MathExpressionParser();

      int width = image.Width;
      int height = image.Height;
      bool hasAlpha = drawable.HasAlpha;

      parser.Init(_formula, width, height);

      Image newImage = new Image(width, height, image.BaseType);

      Layer layer1;
      PixelRgn destPR1;
      if (_keepLayer == 0 || _keepLayer == 1)
	{
	  layer1 = new Layer(newImage, "layer_one", width, height,
			     ImageType.Rgba, 100, LayerModeEffects.Normal);
	  layer1.Translate(_translate_1_x, _translate_1_y);
	  newImage.AddLayer(layer1, 0);

	  destPR1 = new PixelRgn(layer1, 0, 0, width, height, true, false);
	}
      else
	{
	  layer1 = null;
	  destPR1 = null;
	}

      Layer layer2;
      PixelRgn destPR2;
      if (_keepLayer == 0 || _keepLayer == 2)
	{
	  layer2 = new Layer(newImage, "layer_two", width, height,
			     ImageType.Rgba, 100, LayerModeEffects.Normal);
	  layer2.Translate(_translate_2_x, _translate_2_y);
	  newImage.AddLayer(layer2, 0);

	  destPR2 = new PixelRgn(layer2, 0, 0, width, height, true, false);
	}
      else
	{
	  layer2 = null;
	  destPR2 = null;
	}

      byte[] transparent = new byte[4];
      byte[] tmp = new byte[4];

      PixelRgn srcPR = new PixelRgn(drawable, 0, 0, width, height, 
				    false, false);

      if (destPR1 != null && destPR2 != null)
	{
	  for (IntPtr pr = PixelRgn.Register(srcPR, destPR1, destPR2); 
	       pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
	    {
	      for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
		{
		  for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		    {
		      if (parser.Eval(x, y) < 0)
			{
			  if (hasAlpha)
			    {
			      destPR1[y, x] = srcPR[y, x];
			    }
			  else
			    {
			      srcPR[y, x].CopyTo(tmp, 0);
			      tmp[3] = 255;
			      destPR1[y, x] = tmp;
			    }
			  destPR2[y, x] = transparent;
			}
		      else
			{
			  if (hasAlpha)
			    {
			      destPR2[y, x] = srcPR[y, x];
			    }
			  else
			    {
			      srcPR[y, x].CopyTo(tmp, 0);
			      tmp[3] = 255;
			      destPR2[y, x] = tmp;
			    }
			  destPR1[y, x] = transparent;
			}
		    }
		}
	    }
	}
      else if (destPR1 != null)
	{
	  for (IntPtr pr = PixelRgn.Register(srcPR, destPR1); 
	       pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
	    {
	      for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
		{
		  for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		    {
		      if (parser.Eval(x, y) < 0)
			{
			  if (hasAlpha)
			    {
			      destPR1[y, x] = srcPR[y, x];
			    }
			  else
			    {
			      srcPR[y, x].CopyTo(tmp, 0);
			      tmp[3] = 255;
			      destPR1[y, x] = tmp;
			    }
			}
		      else
			{
			  destPR1[y, x] = transparent;
			}
		    }
		}				
	    }
	}
      else	// destPR2 != null
	{
	  for (IntPtr pr = PixelRgn.Register(srcPR, destPR2); 
	       pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
	    {
	      for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
		{
		  for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		    {
		      if (parser.Eval(x, y) < 0)
			{
			  destPR2[y, x] = transparent;
			}
		      else
			{
			  if (hasAlpha)
			    {
			      destPR2[y, x] = srcPR[y, x];
			    }
			  else
			    {
			      srcPR[y, x].CopyTo(tmp, 0);
			      tmp[3] = 255;
			      destPR2[y, x] = tmp;
			    }
			}
		    }
		}				
	    }
	}

      if (_rotate_1 != 0 && layer1 != null) 
	{
	  layer1.TransformRotateDefault(_rotate_1 * Math.PI / 180.0,
					true, 0, 0, true, false);
	}

      if (_rotate_2 != 0 && layer2 != null)
	{
	  layer2.TransformRotateDefault(_rotate_2 * Math.PI / 180.0,
					true, 0, 0, true, false);
	}

      if (_merge) 
	{
	  Layer merged = 
	    newImage.MergeVisibleLayers(MergeType.ExpandAsNecessary);
	  merged.SetOffsets(0, 0);
	  newImage.Resize(merged.Width, merged.Height, 0, 0);
	}

      new Display(newImage);
      
      Display.DisplaysFlush();
    }
  }
}
