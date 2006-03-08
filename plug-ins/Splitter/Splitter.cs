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

using Gimp;
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

    [STAThread]
    static void Main(string[] args)
    {
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
					  "Splits an image.",
					  "Splits an image in separate parts using a formula of the form f(x, y) = 0",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "1999 - 2006",
					  "Splitter...",
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

      Dialog dialog = DialogNew("Splitter", "splitter",
				IntPtr.Zero, 0, null, "splitter");

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

      CheckButton merge = new CheckButton("Merge visible layers");
      table.Attach(merge, 0, 1, 3, 4);

      Button advanced = new Button("Advanced Options...");
      table.Attach(advanced, 1, 2, 3, 4);

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
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_1_x = spinner.ValueAsInt;
	};
      spinner.Value = 0;
      spinner.WidthChars = 4;
      table.AttachAligned(0, 0, "Translate X:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_1_y = spinner.ValueAsInt;
	};
      spinner.Value = 0;
      spinner.WidthChars = 4;
      table.AttachAligned(0, 1, "Translate Y:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(0, 360, 1);
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
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_2_x = spinner.ValueAsInt;
	};
      spinner.Value = 0;
      spinner.WidthChars = 4;
      table.AttachAligned(0, 0, "Translate X:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(int.MinValue, int.MaxValue, 1);
      spinner.ValueChanged += 
	delegate(object source, System.EventArgs args)
	{
	  _translate_2_y = spinner.ValueAsInt;
	};
      spinner.Value = 0;
      spinner.WidthChars = 4;
      table.AttachAligned(0, 1, "Translate Y:", 0.0, 0.5, spinner, 1, true);

      spinner = new SpinButton(0, 360, 1);
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
      parser.Init(_formula);

      Image clone = new Image(image);

      Layer layer1 = new Layer(clone, "layer_one", clone.Width, clone.Height,
			       ImageType.RGB, 100, 
			       LayerModeEffects.NORMAL);
      layer1.Translate(_translate_1_x, _translate_1_y);
      // layer1.AddAlpha();
      clone.AddLayer(layer1, 0);

      Layer layer2 = new Layer(clone, "layer_two", clone.Width, clone.Height,
			       ImageType.RGB, 100, 
			       LayerModeEffects.NORMAL);
      layer1.Translate(_translate_2_x, _translate_2_y);
      clone.AddLayer(layer2, 0);
      // layer2.AddAlpha();

      byte[] black = new byte[drawable.Bpp];

      int width = drawable.Width;
      int height = drawable.Height;

      PixelRgn srcPR = new PixelRgn(drawable, 0, 0, width, height, 
				    false, false);
			
      PixelRgn destPR1 = new PixelRgn(layer1, 0, 0, width, height, 
				      true, false);

      PixelRgn destPR2 = new PixelRgn(layer2, 0, 0, width, height, 
				      true, false);

      for (IntPtr pr = PixelRgn.Register(srcPR, destPR1, destPR2); 
	   pr != IntPtr.Zero; pr = PixelRgn.Process(pr))
	{
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	      for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		{
		  if (parser.Eval(x, y) < 0)
		    {
		      destPR1[y, x] = srcPR[y, x];
		      destPR2[y, x] = black;
		    }
		  else
		    {
		      destPR1[y, x] = black;
		      destPR2[y, x] = srcPR[y, x];
		    }
		}
	    }				
	}
      layer1.Flush();
      layer2.Flush();
      
      if (_rotate_1 != 0) 
	{
	}

      new Display(clone);
      
      Display.DisplaysFlush();
    }
  }
}
