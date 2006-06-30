// The Difference Clouds plug-in
// Copyright (C) 2006 Massimo Perga (massimo.perga@gmail.com)
//
// DifferenceClouds.cs
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

using Gtk;


namespace Gimp.Clouds
{
  public class Clouds : PluginWithPreview
  {
    Random _random = null;
    bool _random_seed = true;

    ScaleEntry _turbulenceEntry = null;
    Progress _progressBar = null;

//    [SaveAttribute("turbulence")]
    private double _turbulence;

    private uint   _count;

 //   [SaveAttribute("seed")]
      uint _rseed;	      		// Current random seed
    private int _progress;
    private int _maxProgress;
    private int _ix1, _ix2, _iy1, _iy2;
    private int _alpha, _bpp;
    private bool _hasAlpha;
    private RGB _foregroundColor, _backgroundColor;
    private byte [,] _indexedColorsMap = new byte[256, 3];

    delegate void GenericEventHandler(object o, EventArgs e);

    [STAThread]
      static void Main(string[] args)
      {
        new Clouds(args);
      }

    public Clouds(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      /*
      in_params.Add(new ParamDef("seed", 0, typeof(uint), 
            "Random generated seed"));
      */

      Procedure procedure = new Procedure("plug_in_difference_clouds",
          "Creates difference clouds.",
          "Creates difference clouds.",
          "Massimo Perga",
          "(C) Massimo Perga",
          "2006",
          "Difference Clouds...",
          "RGB*",
          in_params);
      procedure.MenuPath = "<Image>/Filters/Render/Clouds";
      procedure.IconFile = "DifferenceClouds.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Difference Clouds", true);

      Dialog dialog = DialogNew("Difference Clouds 0.1", "Difference Clouds", IntPtr.Zero, 0,
          Gimp.StandardHelpFunc, "Difference Clouds");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      // Create the table widget
      GimpTable table = new GimpTable(8, 4, false);
      table.ColumnSpacing = 10;
      table.RowSpacing = 10;
      table.BorderWidth = 10;

      CreateLabelInTable(table, 0, 0, "Seed:");
      RandomSeed seed = new RandomSeed(ref _rseed, ref _random_seed);
      table.Attach(seed, 1, 3, 0, 1);

      _turbulenceEntry = new ScaleEntry(table, 0, 1, "_Turbulence", 150, 3,
          _turbulence, 0.1, 7.0, 0.1, 1.0, 0, true, 0, 0, null, null);
      _turbulenceEntry.ValueChanged += TurbulenceChangedEventHandler;

      // Set default values
      //SetDefaultValues(); 

      vbox.PackStart(table, false, false, 0);

      dialog.ShowAll();
      return DialogRun();
    }



    void SetDefaultValues()
    {
    }

    void TurbulenceChangedEventHandler(object source, EventArgs e)
    {
      _turbulence = _turbulenceEntry.Value;
//      InvalidatePreview();
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      int width, height;
      /*
      preview.GetSize(out width, out height);

      byte []pixelArray = new byte[width * height * 3];
      //RenderClouds(null, ref pixelArray, width, height);

      preview.DrawBuffer(pixelArray, width * 3);
      //      pixelArray = null;
      */
    }

    override protected void Reset()
    {
      SetDefaultValues(); 
    }

    override protected void Render(Image image, Drawable original_drawable)
    {
      Tile.CacheNtiles((ulong) (2 * (original_drawable.Width / Gimp.TileWidth + 1)));
      Console.WriteLine("A0");
      _foregroundColor = Context.Foreground;
      _backgroundColor = Context.Background;
      if(_progressBar == null)
        _progressBar = new Progress("Difference Clouds...");
      Console.WriteLine("rseed = {0}", _rseed);
      if(_random == null)
        _random = new Random((int)_rseed);

      Layer active_layer = image.ActiveLayer;
      Layer new_layer = new Layer(active_layer);
      new_layer.Name = "_DifferenceClouds_";      
      new_layer.Visible = false;
      new_layer.Mode = active_layer.Mode;
      new_layer.Opacity = active_layer.Opacity;


      Console.WriteLine("A1");

//      image.AddLayer(new_layer, -1); 
//      new_layer.Visible = true;
//      image.ActiveLayer = new_layer;


      // Initialization steps
//      _bpp = new_layer.Bpp;
      _bpp = original_drawable.Bpp;
//      PixelFetcher _pf = new PixelFetcher(new_layer, true);
      PixelFetcher _pf = new PixelFetcher(original_drawable, true);
      _progress = 0;
      _hasAlpha = new_layer.HasAlpha;
//      _hasAlpha = original_drawable.HasAlpha;
      _alpha = (_hasAlpha) ? _bpp - 1 : _bpp;
      InitializeIndexedColorsMap();

      Console.WriteLine("A2");

//     new_layer.MaskBounds(out _ix1, out _iy1, out _ix2, out _iy2);
      original_drawable.MaskBounds(out _ix1, out _iy1, out _ix2, out _iy2);
      Console.WriteLine("{0} {1} {2} {3}", _ix1, _iy1, _ix2, _iy2);
      _maxProgress = (_ix2 - _ix1) * (_iy2 - _iy1);

      if (_ix1 != _ix2 && _iy1 != _iy2)
      {
        /*
         * This first time only puts in the seed pixels - one in each
         * corner, and one in the center of each edge, plus one in the
         * center of the image.
         */

        DifferenceClouds (_pf, _ix1, _iy1, _ix2 - 1, _iy2 - 1, -1, 0, _random);

        /*
         * Now we recurse through the images, going further each time.
         */
        int depth = 1;
        while (!DifferenceClouds (_pf, _ix1, _iy1, _ix2 - 1, _iy2 - 1, depth, 0, _random))
        {
          depth++;
        }
      }

      // Final steps
      if(_pf != null)
        _pf.Dispose();

      /*
      new_layer.Flush();
      new_layer.MergeShadow(true);
      new_layer.Update(_ix1, _iy1, _ix2 - _ix1, _iy2 - _iy1);
      */



      Console.WriteLine("A5");

      /* Create a region iterator to make difference */

      original_drawable.Flush();
      original_drawable.MergeShadow(true);

      DoDifference(original_drawable, new_layer);
Console.WriteLine("AfterDoDiff");

      original_drawable.Update(_ix1, _iy1, _ix2 - _ix1, _iy2 - _iy1);

      /*
      image.UndoGroupStart();
      image.UndoGroupEnd();
      */
      Display.DisplaysFlush();

      // Remove the new layer 
//      image.RemoveLayer(new_layer);
    }

    Label CreateLabelInTable(Table table, uint row, uint col, string text) 
    {
      Label label = new Label(text);
      label.SetAlignment(0.0f, 0.5f);
      table.Attach(label, col, col+1, row, row+1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);

      return label;
    } 

    void DoDifference(Drawable sourceDrawable, Drawable toDiffDrawable)
    {
      int x1, y1, x2, y2;
      //      RgnIterator iter = new RgnIterator(, RunMode.Interactive);
      sourceDrawable.MaskBounds(out x1, out y1, out x2, out y2);
      PixelRgn srcPR = new PixelRgn(sourceDrawable, x1, y1, x2 - x1, y2 - y1, 
          true, true);
      PixelRgn destPR = new PixelRgn(toDiffDrawable, x1, y1, x2 - x1, y2 - y1, 
          false, false);

      for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; 
          pr = PixelRgn.Process(pr))
      {
        for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
        {
          for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
          {
//            destPR[y, x] = func(srcPR[y, x]);
              srcPR[y, x] = MakeAbsDiff(destPR[y, x], srcPR[y, x]);
//              Console.WriteLine("{0}", srcPR[y, x][0]);
          }
        }				
      }
      sourceDrawable.Flush();
      sourceDrawable.MergeShadow(false);
      sourceDrawable.Update(x1, y1, x2 - x1, y2 - y1);
    }

    byte[] MakeAbsDiff(byte []dest, byte[] src)
    {
      byte []retVal = new byte[src.Length];
      int tmpVal = 0;
      for(int i = 0; i < src.Length; i++)
          tmpVal += src[i];
      tmpVal /= src.Length;
      for(int i = 0; i < src.Length; i++)
      {
 //       Console.Write("{0} {1} ", dest[i], _indexedColorsMap[tmpVal,i]);
        retVal[i] = (byte)Math.Abs(dest[i] - _indexedColorsMap[tmpVal,i]);
//        Console.WriteLine("{0}", retVal[i]);
//        retVal[i] = (byte)Math.Abs(dest[i] - src[i]);

//        src[i] = (byte)Math.Abs(dest[i] - src[i]);
//        Console.WriteLine("{0} {1} {2}", dest[i], src[i],retVal[i]);
      }
//      src = retVal;
        
      if(_hasAlpha)
        retVal[_bpp - 1] = 255;
      return retVal;
    }   

    bool DifferenceClouds(PixelFetcher pf, int x1, int y1, 
        int x2, int y2, int depth, int scale_depth, Random gr)
    {
      byte [] tl = new byte[_bpp];
      byte [] bl = new byte[_bpp];
      byte [] tr = new byte[_bpp];
      byte [] br = new byte[_bpp];
      byte [] mm = new byte[_bpp];
      byte [] ml = new byte[_bpp];
      byte [] mr = new byte[_bpp];
      byte [] tm = new byte[_bpp];
      byte [] bm = new byte[_bpp];
      byte [] tmp = new byte[_bpp];

      int xm = (x1 + x2) / 2;
      int ym = (y1 + y2) / 2;

//      Console.WriteLine("depth == {0}", depth);
      /*
      Console.WriteLine("progress == {0}", _progress);
      Console.WriteLine("maxProgress == {0}", _maxProgress);
      */

      /* Initial step */
      if(depth == -1)
      {
        tl = RandomRGB(gr);
        PutPixel(pf, x1, y1, tl, ref _progress);
        tr = RandomRGB(gr);
        PutPixel(pf, x2, y1, tr, ref _progress);
        bl = RandomRGB(gr);
        PutPixel(pf, x1, y2, bl, ref _progress);
        br = RandomRGB(gr);
        PutPixel(pf, x2, y2, br, ref _progress);
        mm = RandomRGB(gr);
        PutPixel(pf, xm, ym, mm, ref _progress);
        ml = RandomRGB(gr);
        PutPixel(pf, x1, ym, ml, ref _progress);
        mr = RandomRGB(gr);
        PutPixel(pf, x2, ym, mr, ref _progress);
        tm = RandomRGB(gr);
        PutPixel(pf, xm, y1, tm, ref _progress);
        bm = RandomRGB(gr);
        PutPixel(pf, xm, y2, bm, ref _progress);

        return false;
      }

      /*
      if(depth > 100000)
      {
        Console.WriteLine("depth out of limits = {0}", depth);
        return false;
      }
      */

      if(depth == 0)
      {
        int ran;

        if((x1 == x2) && (y1 == y2))
          return false;

        GetPixel(pf, ref tl, x1, y1);
        GetPixel(pf, ref tr, x2, y1);
        GetPixel(pf, ref bl, x1, y2);
        GetPixel(pf, ref br, x2, y2);

        ran = (int)((256.0 / (2.0 * scale_depth)) * _turbulence);

        if (xm != x1 || xm != x2)
        {
          /* Left. */
          AveragePixel(ref ml, tl, bl);
          AddRandom (gr, ml, ran);
          PutPixel (pf, x1, ym, ml, ref _progress);

          if (x1 != x2)
          {
            /* Right. */
            AveragePixel(ref mr, tr, br);
            AddRandom (gr, mr, ran);
            PutPixel (pf, x2, ym, mr, ref _progress);
          }
        }


        if (ym != y1 || ym != y2)
        {
          if (x1 != xm || ym != y2)
          {
            /* Bottom. */
            AveragePixel(ref bm, bl, br);
            AddRandom (gr, bm, ran);
            PutPixel (pf, xm, y2, bm, ref _progress);
          }

          if (y1 != y2)
          {
            /* Top. */
            AveragePixel(ref tm, tl, tr);
            AddRandom (gr, tm, ran);
            PutPixel (pf, xm, y1, tm, ref _progress);
          }
        }

        if (ym != y1 || ym != y2)
        {
          if (x1 != xm || ym != y2)
          {
            /* Bottom. */
            AveragePixel(ref bm, bl, br);
            AddRandom (gr, bm, ran);
            PutPixel (pf, xm, y1, bm, ref _progress);
          }

          if (y1 != y2)
          {
            /* Top. */
            AveragePixel(ref tm, tl, tr);
            AddRandom (gr, tm, ran);
            PutPixel(pf, xm, y1, tm, ref _progress);
          }
        }

        if (y1 != y2 || x1 != x2)
        {
          /* Middle pixel. */
          AveragePixel(ref mm, tl, br);
          AveragePixel(ref tmp, bl, tr);
          AveragePixel(ref mm, mm, tmp);

          AddRandom (gr, mm, ran);
          PutPixel(pf, xm, ym, mm, ref _progress);
        }

        _count++;

        if ((_count % 2000) == 0 && (pf != null))
        {
          if(_progressBar != null)
            _progressBar.Update((double)_progress / (double) _maxProgress);
        }

//        Console.WriteLine("Nei < 3");
        return ((x2 - x1) < 3) && ((y2 - y1) < 3);
      }
//      Console.WriteLine("x1 < x2 ? {0} {1} {2}", (x1 < x2), x1, x2);
//      Console.WriteLine("y1 < y2 ? {0}", (y1 < y2));
      if (x1 < x2 || y1 < y2)
      {
        /* Top left. */
        DifferenceClouds (pf, x1, y1, xm, ym, depth - 1, scale_depth + 1, gr);
        /* Bottom left. */
        DifferenceClouds (pf, x1, ym, xm ,y2, depth - 1, scale_depth + 1, gr);
        /* Top right. */
        DifferenceClouds (pf, xm, y1, x2 , ym, depth - 1, scale_depth + 1, gr);
        /* Bottom right. */
//        Console.WriteLine("Nei x1 < x2");
        return DifferenceClouds (pf, xm, ym, x2, y2, depth - 1, scale_depth + 1, gr);
      }
      else
      {
 //       Console.WriteLine("return true; {0} {1} {2} {3} {4}", depth, x1, x2, y1, y2);
        return true;
      }
    }

    byte[] RandomRGB(Random r)
    {
      /* choice between foreground and background color */
      /*
      byte []retVal = new byte[3] {0,0,0};
      */
      /*
      byte []retVal = new byte[3];
      r.NextBytes(retVal);
      */
      /*
      byte []retVal = new byte[_bpp];
      r.NextBytes(retVal);
      */
      /*
      Console.Write("retVal[] = {");
      for(int i = 0; i < _bpp; i++)
      {
        retVal[i] = (byte)r.Next(256);
        Console.Write("{0},", retVal[i]);
      } 
      */
      /*
      retVal = (r.Next(2) == 0) ?  _foregroundColor.Bytes: 
                                   _backgroundColor.Bytes;
                                   */
      //  Console.Write("{0},", retVal[i]);
//      r.NextBytes(retVal);
      byte []retVal = new byte[_bpp];

      for(int i = 0; i < _bpp; i++)
      {
        retVal[i] = _indexedColorsMap[r.Next(256), i];
        Console.Write("{0},", retVal[i]);
      } 
      

      if(_hasAlpha)
        retVal[_alpha] = 255;
//      Console.WriteLine("}");
      return retVal;
    }

    void GetPixel(PixelFetcher pf, ref byte [] pixel, int x, int y)
    {
      // TODO: implement the preview mode
      if(pf != null)
      {
        pf.GetPixel(x, y, pixel);
      }
      else
      {
      }
    }

    void PutPixel(PixelFetcher pf, int x, int y, byte [] pixel, ref int progress)
    {
      byte []currentValue = new byte[pixel.Length];
      // TODO: implement the preview mode
      if(pf != null)
      {
        pf.PutPixel(x, y, pixel);
        progress++;
      }
      else
      {
      }
    }

    void AveragePixel(ref byte []dest, byte[] src1, byte[] src2)
    {
      for(int i = 0; i < src1.Length; i++)
        dest[i] = (byte)((int)(src1[i] + src2[i]) / 2);
    }

    //Planet(new_layer, ref pixelArray, width, height);
    void AddRandom (Random gr, byte []pixel, int amount)
    {
      amount /= 2;

      if (amount > 0)
      {
        int i, tmp;

        for (i = 0; i < _alpha; i++)
        {
          tmp = pixel[i] + gr.Next(-amount, amount);

          if(tmp < 0) pixel[i] = 0;
          else if(tmp > 255) pixel[i] = 255;
          else pixel[i] = (byte)tmp; 
          //CLAMP0255 (tmp);
        }
      }
    }
      void InitializeIndexedColorsMap()
      {
        double ratio = 0;

        for(int i = 0; i < 256; i++)
        {
          ratio = (double)((double)i / 256); 
          for(int j = 0; j < 3; j++)
          {
            _indexedColorsMap[i,j] = (byte)(_foregroundColor.Bytes[j] + 
                (byte)((double)(_foregroundColor.Bytes[j] - 
                                _backgroundColor.Bytes[j]) * ratio));
            /*
            Console.WriteLine("bg {0} {1} {2} {3:e}", _backgroundColor.Bytes[j],
                _backgroundColor.Bytes[j] * ratio,
                _foregroundColor.Bytes[j] * ratio,
                ratio);
                */
            /*
            + (byte) 
              ((_foregroundColor.Bytes[j] - _backgroundColor.Bytes[j]) * ratio);
              */
          }
          /*
          Console.WriteLine("cm[{0}] = ({1},{2},{3})", i, 
                        _indexedColorsMap[i,0],
                        _indexedColorsMap[i,1],
                        _indexedColorsMap[i,2]);
                        */
        }
      }

  }
}

