// The Raindrops plug-in
// Copyright (C) 2004-2006 Maurits Rijk, Massimo Perga
//
// Raindrops.cs
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

namespace Gimp.Raindrops
{
  public class Raindrops : Plugin
  {
    DrawablePreview _preview;

    [SaveAttribute("drop_size")]
    int _dropSize = 80;
    [SaveAttribute("number")]
    int _number = 80;
    [SaveAttribute("fish_eye")]
    int _fishEye = 30;

    static void Main(string[] args)
    {
      new Raindrops(args);
    }
    
    public Raindrops(string[] args) : base(args, "Raindrops")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList();
 
      inParams.Add(new ParamDef("drop_size", 80, typeof(int),
				_("Size of raindrops")));
      inParams.Add(new ParamDef("number", 80, typeof(int),
				_("Number of raindrops")));
      inParams.Add(new ParamDef("fish_eye", 30, typeof(int),
				_("Fisheye effect")));

      Procedure procedure = new Procedure("plug_in_raindrops",
					  _("Generates raindrops"),
					  _("Generates raindrops"),
					  "Massimo Perga",
					  "(C) Massimo Perga",
					  "2006",
					  _("Raindrops..."),
					  "RGB*, GRAY*",
					  inParams);
      procedure.MenuPath = "<Image>/Filters/" + 
	_("Light and Shadow") + "/" + _("Glass");
      procedure.IconFile = "Raindrops.png";
      
      yield return procedure;
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Raindrops", true);

      GimpDialog dialog = DialogNew(_("Raindrops 0.1"), _("Raindrops"), 
				    IntPtr.Zero, 0, Gimp.StandardHelpFunc, 
				    _("Raindrops"));

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      ScaleEntry _dropSizeEntry = new ScaleEntry(table, 0, 1, 
						 _("_Drop size:"), 
						 150, 3, _dropSize, 1.0, 
						 256.0, 1.0, 8.0, 0,
						 true, 0, 0, null, null);
      _dropSizeEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  _dropSize = _dropSizeEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      ScaleEntry _numberEntry = new ScaleEntry(table, 0, 2, 
					       _("_Number:"), 
					       150, 3, _number, 1.0, 
					       256.0, 1.0, 8.0, 0,
					       true, 0, 0, null, null);
      _numberEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  _number = _numberEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      ScaleEntry _fishEyeEntry = new ScaleEntry(table, 0, 3, 
						_("_Fish eye:"), 
						150, 3, _fishEye, 1.0, 
						256.0, 1.0, 8.0, 0,
						true, 0, 0, null, null);
      _fishEyeEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  _fishEye = _fishEyeEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      return dialog;
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      Image clone = new Image(_image);
      clone.Crop(_preview.Bounds);

      if (!clone.ActiveDrawable.IsLayer())
	{
	  new Message("This filter can be applied just over layers");
	  return;
	}

      RenderRaindrops(clone, clone.ActiveDrawable, true);

      PixelRgn rgn = new PixelRgn(clone.ActiveDrawable, false, false);
      _preview.DrawRegion(rgn);
	
      clone.Delete();
    }
    
    private int Clamp(int x, int l, int u)
    {
      return (x < l) ? l : ((x > u) ? u : x);
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Just layers are allowed
      if (!drawable.IsLayer())
	{
	  new Message(_("This filter can be applied just over layers"));
	  return;
	}

      Layer active_layer = image.ActiveLayer;
      string original_layer_name =  active_layer.Name;

      Layer newLayer = new Layer(active_layer);
      newLayer.Name = "_raindrops_dummy_" + original_layer_name;      
      newLayer.Visible = false;
      newLayer.Mode = active_layer.Mode;
      newLayer.Opacity = active_layer.Opacity;
      
            
/*      if(!active_layer.HasAlpha)
        active_layer.AddAlpha();*/
    
      RenderRaindrops(image, newLayer, false);
      image.UndoGroupStart();
      image.AddLayer(newLayer, -1); 
      image.RemoveLayer(active_layer);
      newLayer.Name = original_layer_name;
      newLayer.Visible = true;
      image.ActiveLayer = newLayer;
      image.UndoGroupEnd();
      Display.DisplaysFlush();
    }

    void RenderRaindrops(Image image, Drawable drawable, bool isPreview)
    {
      int x, y;                             // center coordinates
      int width = image.Width;
      int height = image.Height;
      Progress    _progress = null;

      Tile.CacheDefault(drawable);

      if (!isPreview)
        _progress = new Progress(_("Raindrops..."));

      PixelFetcher pf = new PixelFetcher(drawable, false);

      // Create a new matrix containing where to place raindrops
      bool [,] boolMatrix = new bool[width, height];
      int  m, n;                 // loop variables
      int  bpp = drawable.Bpp;

      // TODO: to test the following conditions before uncommenting it
      /*
	bool hasAlpha = drawable.HasAlpha;
	if(hasAlpha)
        bpp--;
      */

      int BlurRadius;                       // Blur Radius
      bool FindAnother = false;             // To search for good coordinates
      int DropSize = _dropSize;
      int Coeff = _fishEye; 
      double OldRadius;                        // Radius before processing

      double NewCoeff = (double)Clamp(Coeff, 1, 100) * 0.01;  // FishEye Coefficients

      Random random = new Random();
     
      // TODO: find an upper bound so that
      // speed on big drop would be improved
      //int upper_bound = ; // upper bound for iteration in  blur search process

      for (int NumBlurs = 0 ; NumBlurs <= _number ; ++NumBlurs)
	{
	  int newSize = random.Next(DropSize);	// Size of current raindrop
	  int halfSize = newSize / 2;		// Half of current raindrop
	  int Radius = halfSize;		// Maximum radius for raindrop
	  double s = Radius / Math.Log (NewCoeff * Radius + 1);

	  int Counter = 0;

	  do
	    {
	      FindAnother = false;
	      y = random.Next(width);
	      x = random.Next(height);
	    
	      if (boolMatrix[y,x])
		{
		  FindAnother = true;
		}
	      else
		{
		  for (int i = x - halfSize ; i <= x + halfSize ; i++)
		    {
		      for (int j = y - halfSize ; j <= y + halfSize ; j++)
			{
			  if (i >= 0 && i < height && j >= 0 && j < width)
			    {
			      if (boolMatrix[j,i])
				{
				  FindAnother = true;
				}
			    }
			}
		    }
		}
	      Counter++;
	    }
	  while (FindAnother && Counter < 10000);

	  if (Counter >= 10000)
	    {
	      NumBlurs = _number;
	      break;
	    }

	  for (int i = -halfSize ; i < newSize - halfSize ; i++)
	    {
	      for (int j = -halfSize ; j < newSize - halfSize ; j++)
		{
		  double r = Math.Sqrt(i * i + j * j);
		  double a = Math.Atan2(i, j);

		  if (r <= Radius)
		    {
		      OldRadius = r;
		      r = (Math.Exp (r / s) - 1) / NewCoeff;

		      int k = x + (int) (r * Math.Sin(a));
		      int l = y + (int) (r * Math.Cos(a));

		      m = x + i;
		      n = y + j;

		      if (k >= 0 && k < height && l >= 0 && l < width)
			{
			  if (m >= 0 && m < height && n >= 0 && n < width)
			    {
			      int bright = GetBright(Radius, OldRadius, a);

			      boolMatrix[n, m] = true;

			      Pixel newColor = pf[l, k] + bright;
			      newColor.Clamp0255();
			      pf[l, k] = newColor;
			    }
			}
		    }
		}
	    }

	  BlurRadius = newSize / 25 + 1;

	  for (int i = -halfSize - BlurRadius ;  
	       i < newSize - halfSize + BlurRadius ; i++)
	    {
	      for (int j = -halfSize - BlurRadius ; 
		   j < newSize - halfSize + BlurRadius ; j++)
		{
		  double r = Math.Sqrt (i * i + j * j);

		  if (r <= Radius * 1.1)
		    {
		      Pixel average = new Pixel(bpp);
		      int BlurPixels = 0;

		      for (int k = -BlurRadius; k < BlurRadius + 1; k++)
			{
			  for (int l = -BlurRadius; l < BlurRadius + 1; l++)
			    {
			      {
				m = x + i + k;
				n = y + j + l;
				
				if (m >= 0 && m < height &&
				    n >= 0 && n < width)
				  {
				    average += pf[n, m];
				    BlurPixels++;
				  }
			      }
			    }
			}

		      m = x + i;
		      n = y + j;

		      if (m >= 0 && m < height && n >= 0 && n < width)
			{
			  pf[n, m] = average / BlurPixels;
			}
		    }
		}
	    }
	  if (!isPreview)
	    _progress.Update((double) NumBlurs / _number);
	}

      pf.Dispose();

      drawable.Flush();
      drawable.Update();

      if (!isPreview)
        Display.DisplaysFlush();

    }

    int GetBright(double Radius, double OldRadius, double a)
    {
      int Bright = 0;

      if (OldRadius >= 0.9 * Radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    Bright = -80;
	  else if ((a <= -2.25) && (a > -2.5))
	    Bright = -40;
	  else if ((a <= 0.25) && (a > 0))
	    Bright = -40;
	}
      else if (OldRadius >= 0.8 * Radius)
	{
	  if ((a <= -0.75) && (a > -1.50))
	    Bright = -40;
	  else if ((a <= 0.10) && (a > -0.75))
	    Bright = -30;
	  else if ((a <= -1.50) && (a > -2.35))
	    Bright = -30;
	}
      else if (OldRadius >= 0.7 * Radius)
	{
	  if ((a <= -0.10) && (a > -2.0))
	    Bright = -20;
	  else if ((a <= 2.50) && (a > 1.90))
	    Bright = 60;
	}
      else if (OldRadius >= 0.6 * Radius)
	{
	  if ((a <= -0.50) && (a > -1.75))
	    Bright = -20;
	  else if ((a <= 0) && (a > -0.25))
	    Bright = 20;
	  else if ((a <= -2.0) && (a > -2.25))
	    Bright = 20;
	}
      else if (OldRadius >= 0.5 * Radius)
	{
	  if ((a <= -0.25) && (a > -0.50))
	    Bright = 30;
	  else if ((a <= -1.75 ) && (a > -2.0))
	    Bright = 30;
	}
      else if (OldRadius >= 0.4 * Radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    Bright = 40;
	}
      else if (OldRadius >= 0.3 * Radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    Bright = 30;
	}
      else if (OldRadius >= 0.2 * Radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    Bright = 20;
	}
      return Bright;
    }
  }
}

