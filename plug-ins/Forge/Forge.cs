// The Forge plug-in
// Copyright (C) 2006 Massimo Perga (massimo.perga@gmail.com)
//
// Forge.cs
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


namespace Gimp.Forge
{
  public class Forge : PluginWithPreview
  {
    public const int nRand = 4; // Gauss() sample count
    public const double planetAmbient = 0.05;
    Random _random;
    bool _random_seed = true;

    RadioButton _PlanetRadioButton;
    RadioButton _CloudsRadioButton;
    RadioButton _NightRadioButton;
    SpinButton _DimensionSpinButton;
    SpinButton _GlaciersSpinButton;
    SpinButton _IceSpinButton;
    SpinButton _PowerSpinButton;
    SpinButton _HourSpinButton;
    SpinButton _InclinationSpinButton;
    SpinButton _StarsSpinButton;
    SpinButton _SaturationSpinButton;
    Progress _progress = null;
    // Flag for spin buttons values specified by the user
    private bool dimspec, powerspec;
    // Flag for radio buttons values specified by the user
    private bool glacspec, icespec, starspec, hourspec, inclspec, starcspec;
    [SaveAttribute("clouds")]
      bool _clouds;	      	// Just generate clouds
    [SaveAttribute("stars")]
      bool _stars;	      		// Just generate stars
    [SaveAttribute("dimension")]
      double _fracdim;		// Fractal dimension
    [SaveAttribute("power")]
      double _powscale; 	      	// Power law scaling exponent
    [SaveAttribute("glaciers")]
      double _glaciers;
    [SaveAttribute("icelevel")]
      double _icelevel;
    [SaveAttribute("hour")]
      double _hourangle;
    [SaveAttribute("inclination")]
      double _inclangle;
    [SaveAttribute("starsfraction")]
      double _starfraction;
    [SaveAttribute("saturation")]
      double _starcolour;
    [SaveAttribute("seed")]
      uint _rseed;	      		// Current random seed
    private int forced;
    private double arand, gaussadd, gaussfac; // Gaussian random parameters
    private const uint meshsize = 256;	      	// FFT mesh size

    byte [,] pgnd = new byte[99,3] {
      {206, 205, 0}, {208, 207, 0}, {211, 208, 0},
        {214, 208, 0}, {217, 208, 0}, {220, 208, 0},
        {222, 207, 0}, {225, 205, 0}, {227, 204, 0},
        {229, 202, 0}, {231, 199, 0}, {232, 197, 0},
        {233, 194, 0}, {234, 191, 0}, {234, 188, 0},
        {233, 185, 0}, {232, 183, 0}, {231, 180, 0},
        {229, 178, 0}, {227, 176, 0}, {225, 174, 0},
        {223, 172, 0}, {221, 170, 0}, {219, 168, 0},
        {216, 166, 0}, {214, 164, 0}, {212, 162, 0},
        {210, 161, 0}, {207, 159, 0}, {205, 157, 0},
        {203, 156, 0}, {200, 154, 0}, {198, 152, 0},
        {195, 151, 0}, {193, 149, 0}, {190, 148, 0},
        {188, 147, 0}, {185, 145, 0}, {183, 144, 0},
        {180, 143, 0}, {177, 141, 0}, {175, 140, 0},
        {172, 139, 0}, {169, 138, 0}, {167, 137, 0},
        {164, 136, 0}, {161, 135, 0}, {158, 134, 0},
        {156, 133, 0}, {153, 132, 0}, {150, 132, 0},
        {147, 131, 0}, {145, 130, 0}, {142, 130, 0},
        {139, 129, 0}, {136, 128, 0}, {133, 128, 0},
        {130, 127, 0}, {127, 127, 0}, {125, 127, 0},
        {122, 127, 0}, {119, 127, 0}, {116, 127, 0},
        {113, 127, 0}, {110, 128, 0}, {107, 128, 0},
        {104, 128, 0}, {102, 127, 0}, { 99, 126, 0},
        { 97, 124, 0}, { 95, 122, 0}, { 93, 120, 0},
        { 92, 117, 0}, { 92, 114, 0}, { 92, 111, 0},
        { 93, 108, 0}, { 94, 106, 0}, { 96, 104, 0},
        { 98, 102, 0}, {100, 100, 0}, {103,  99, 0},
        {106,  99, 0}, {109,  99, 0}, {111, 100, 0},
        {114, 101, 0}, {117, 102, 0}, {120, 103, 0},
        {123, 102, 0}, {125, 102, 0}, {128, 100, 0},
        {130,  98, 0}, {132,  96, 0}, {133,  94, 0},
        {134,  91, 0}, {134,  88, 0}, {134,  85, 0},
        {133,  82, 0}, {131,  80, 0}, {129,  78, 0}
    };

    delegate void GenericEventHandler(object o, EventArgs e);

    [STAThread]
      static void Main(string[] args)
      {
        new Forge(args);
      }

    public Forge(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("clouds", false, typeof(bool), 
            "Clouds (true), Planet or Stars (false)"));
      in_params.Add(new ParamDef("stars", false, typeof(bool), 
            "Stars (true), Planet or Clouds (false)"));
      in_params.Add(new ParamDef("dimension", 2.4, typeof(double), 
            "Fractal dimension factor"));
      in_params.Add(new ParamDef("power", 1.0, typeof(double), 
            "Power factor"));
      in_params.Add(new ParamDef("glaciers", 0.75, typeof(double), 
            "Glaciers factor"));
      in_params.Add(new ParamDef("ice", 0.4, typeof(double), 
            "Ice factor"));
      in_params.Add(new ParamDef("hour", 0.0, typeof(double), 
            "Hour factor"));
      in_params.Add(new ParamDef("inclination", 0.0, typeof(double), 
            "Inclination factor"));
      in_params.Add(new ParamDef("stars", 100.0, typeof(double), 
            "Stars factor"));
      in_params.Add(new ParamDef("saturation", 100.0, typeof(double), 
            "Saturation factor"));
      in_params.Add(new ParamDef("seed", 0, typeof(uint), 
            "Random generated seed"));

      Procedure procedure = new Procedure("plug_in_forge",
          "Creates an artificial world.",
          "Creates an artificial world.",
          "Massimo Perga, Maurits Rijk",
          "(C) Massimo Perga, Maurits Rijk",
          "2006",
          "Forge...",
          "RGB*",
          in_params);
      procedure.MenuPath = "<Image>/Filters/Render";
      procedure.IconFile = "Forge.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Forge", true);

      Dialog dialog = DialogNew("Forge 0.1", "Forge", IntPtr.Zero, 0,
          Gimp.StandardHelpFunc, "Forge");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      // Create the table widget
      GimpTable table = new GimpTable(11, 3, false);
      table.ColumnSpacing = 10;
      table.RowSpacing = 10;
      table.BorderWidth = 10;

      // Create the frame widget 
      GimpFrame frame = new GimpFrame("Type");
      table.Attach(frame, 0, 2, 0, 1);

      HBox hbox = new HBox(false,1);
      frame.Add(hbox);

      _PlanetRadioButton = CreateRadioButtonInHBox(hbox, null,
          PlanetRadioButtonEventHandler, "Planet");

      _CloudsRadioButton = CreateRadioButtonInHBox(hbox, _PlanetRadioButton,
          CloudsRadioButtonEventHandler, "Clouds");

      _NightRadioButton = CreateRadioButtonInHBox(hbox, _PlanetRadioButton,
          NightRadioButtonEventHandler, "Night");

      CreateLabelInTable(table, 2, 0, "Dimension (0.0 - 3.0):");
      _DimensionSpinButton = CreateFloatSpinButtonInTable(table, 2, 1, 2.4, 0, 3,
          DimensionSpinButtonEventHandler);

      CreateLabelInTable(table, 3, 0, "Power:");
      _PowerSpinButton = CreateFloatSpinButtonInTable(table, 3, 1, 1.2, 0, 
          Double.MaxValue,
          PowerSpinButtonEventHandler);

      CreateLabelInTable(table, 4, 0, "Glaciers:");
      _GlaciersSpinButton = CreateFloatSpinButtonInTable(table, 4, 1, 0.75, 0, 
          Double.MaxValue,
          GlaciersSpinButtonEventHandler);

      CreateLabelInTable(table, 5, 0, "Ice:");
      _IceSpinButton = CreateFloatSpinButtonInTable(table, 5, 1, 0.4, 0, 
          Double.MaxValue,
          IceSpinButtonEventHandler);

      CreateLabelInTable(table, 6, 0, "Hour (0 - 24):");
      _HourSpinButton = CreateFloatSpinButtonInTable(table, 6, 1, 0, 0, 24, 
          HourSpinButtonEventHandler);

      CreateLabelInTable(table, 7, 0, "Inclination (-90 - 90):");
      _InclinationSpinButton = CreateFloatSpinButtonInTable(table, 7, 1, 0, -90, 90,
          InclinationSpinButtonEventHandler);

      CreateLabelInTable(table, 8, 0, "Stars (0 - 100):");
      _StarsSpinButton = CreateIntSpinButtonInTable(table, 8, 1, 100, 0, 100, 
          StarsSpinButtonEventHandler);

      CreateLabelInTable(table, 9, 0, "Saturation:");
      _SaturationSpinButton = CreateIntSpinButtonInTable(table, 9, 1, 125, 0, 
          Int32.MaxValue, 
          SaturationSpinButtonEventHandler);

      CreateLabelInTable(table, 10, 0, "Seed:");
      RandomSeed seed = new RandomSeed(ref _rseed, ref _random_seed);
      table.Attach(seed, 1, 2, 10, 11);

      // Set default values
      //SetDefaultValues(); 

      vbox.PackStart(table, false, false, 0);

      dialog.ShowAll();
      return DialogRun();
    }

    RadioButton CreateRadioButtonInHBox(HBox hbox, 
        RadioButton radioButtonGroup,
        GenericEventHandler radioButtonEventHandler, 
        string radioButtonLabel)
    { 
      RadioButton radioButton = new RadioButton(radioButtonGroup, 
          radioButtonLabel);
      radioButton.Clicked += new EventHandler(radioButtonEventHandler);
      hbox.PackStart(radioButton, true, true, 10);

      return radioButton;
    }


    SpinButton CreateIntSpinButtonInTable(Table table, uint row, uint col, 
        uint initialValue, 
        int min, int max,
        GenericEventHandler spinnerEventHandler)
    {
      Adjustment adjustment = new Adjustment(initialValue, min, 
          max, 1, 1, 1);
      SpinButton spinner = new SpinButton(adjustment, 1, 0);
      spinner.Numeric = true;
      spinner.ValueChanged += new EventHandler(spinnerEventHandler);
      table.Attach(spinner, col, col+1, row, row+1);

      return spinner;
    }

    SpinButton CreateFloatSpinButtonInTable(Table table, uint row, uint col, 
        double initialValue, 
        double min, double max,
        GenericEventHandler spinnerEventHandler)
    {
      Adjustment adjustment = new Adjustment(initialValue, min, 
          max, 0.1, 1, 1);
      SpinButton spinner = new SpinButton(adjustment, 0.1, 1);
      spinner.Numeric = true;
      spinner.ValueChanged += new EventHandler(spinnerEventHandler);
      table.Attach(spinner, col, col+1, row, row+1);

      return spinner;
    }

    void SetDefaultValues()
    {
      _PlanetRadioButton.Active = true;
      _GlaciersSpinButton.Value = 0.75;
      _IceSpinButton.Value = 0.4;
      _HourSpinButton.Value = 0;
      _InclinationSpinButton.Value = 0;
      _StarsSpinButton.Value = 100.0;
      _SaturationSpinButton.Value = 125.0;
    }

    void DimensionSpinButtonEventHandler(object source, EventArgs e)
    {
      if(forced > 0)
        forced--;
      else
        dimspec = true;
      _fracdim = _DimensionSpinButton.Value;
      InvalidatePreview();
    }

    void PowerSpinButtonEventHandler(object source, EventArgs e)
    {
      if(forced > 0)
        forced--;
      else
        powerspec = true;
      _powscale = _PowerSpinButton.Value;
      InvalidatePreview();
    }

    void PlanetRadioButtonEventHandler(object source, EventArgs e)
    {
      if (_PlanetRadioButton.Active == true)
      {
        if (!dimspec)
        {
          forced++;
          _DimensionSpinButton.Value = 2.4;
        }
        if (!powerspec)
        {
          forced++;
          _PowerSpinButton.Value = 1.2;
        }
      }

      // Enable all the spin buttons
      _DimensionSpinButton.Sensitive = true; 
      _PowerSpinButton.Sensitive = true; 
      _GlaciersSpinButton.Sensitive = true; 
      _IceSpinButton.Sensitive = true; 
      _HourSpinButton.Sensitive = true; 
      _InclinationSpinButton.Sensitive = true; 
      _StarsSpinButton.Sensitive = true; 
      _SaturationSpinButton.Sensitive = true; 

      InvalidatePreview();
    }

    void CloudsRadioButtonEventHandler(object source, EventArgs e)
    {
      if ((_clouds = _CloudsRadioButton.Active) == true)
      {
        if (!dimspec)
        {
          forced++;
          _DimensionSpinButton.Value = 2.15;
        }
        if (!powerspec)
        {
          forced++;
          _PowerSpinButton.Value = 0.75;
        }
      }
      // Disable some spin buttons
      _DimensionSpinButton.Sensitive = true; 
      _PowerSpinButton.Sensitive = true; 
      _GlaciersSpinButton.Sensitive = false; 
      _IceSpinButton.Sensitive = false; 
      _HourSpinButton.Sensitive = false; 
      _InclinationSpinButton.Sensitive = false; 
      _StarsSpinButton.Sensitive = false; 
      _SaturationSpinButton.Sensitive = false; 

      InvalidatePreview();
    }

    void NightRadioButtonEventHandler(object source, EventArgs e)
    {
      if ((_stars = _NightRadioButton.Active) == true)
      {
        if (!dimspec)
        {
          forced++;
          _DimensionSpinButton.Value = 2.4;
        }
        if (!powerspec)
        {
          forced++;
          _PowerSpinButton.Value = 1.2;
        }
      }
      // Enable just the star spin button
      _DimensionSpinButton.Sensitive = false; 
      _PowerSpinButton.Sensitive = false; 
      _GlaciersSpinButton.Sensitive = false; 
      _IceSpinButton.Sensitive = false; 
      _HourSpinButton.Sensitive = false; 
      _InclinationSpinButton.Sensitive = false; 
      _StarsSpinButton.Sensitive = true; 
      _SaturationSpinButton.Sensitive = false; 

      InvalidatePreview();
    }

    void GlaciersSpinButtonEventHandler(object source, EventArgs e)
    {
      glacspec = true;
      _glaciers = _GlaciersSpinButton.Value;
      InvalidatePreview();
    }

    void IceSpinButtonEventHandler(object source, EventArgs e)
    {
      icespec = true;
      _icelevel = _IceSpinButton.Value;
      InvalidatePreview();
    }

    void HourSpinButtonEventHandler(object source, EventArgs e)
    {
      hourspec = true;
      _hourangle = _HourSpinButton.Value;
      InvalidatePreview();
    }

    void InclinationSpinButtonEventHandler(object source, EventArgs e)
    {
      inclspec = true;
      _inclangle = _InclinationSpinButton.Value;  
      InvalidatePreview();
    }

    void StarsSpinButtonEventHandler(object source, EventArgs e)
    {
      starspec = true;
      _starfraction = _StarsSpinButton.Value;
      InvalidatePreview();
    }

    void SaturationSpinButtonEventHandler(object source, EventArgs e)
    {
      starcspec = true;
      _starcolour = _SaturationSpinButton.Value;
      InvalidatePreview();
    }

    Label CreateLabelInTable(Table table, uint row, uint col, string text) 
    {
      Label label = new Label(text);
      label.SetAlignment(0.0f, 0.5f);
      table.Attach(label, col, col+1, row, row+1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 0, 0);

      return label;
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      int width, height;
      preview.GetSize(out width, out height);

      byte []pixelArray = new byte[width * height * 3];
      RenderForge(null, ref pixelArray, width, height);

      preview.DrawBuffer(pixelArray, width * 3);
      //      pixelArray = null;
    }

    override protected void Reset()
    {
      SetDefaultValues(); 
    }

    override protected void Render(Image image, Drawable original_drawable)
    {
      Tile.CacheNtiles((ulong) (2 * (original_drawable.Width / Gimp.TileWidth + 1)));
      if(_progress == null)
        _progress = new Progress("Forge...");

      // Just layers are allowed
      if(!original_drawable.IsLayer())
      {
        new Message("This filter can be applied just over layers");
        return;
      }
      if(original_drawable.Width < original_drawable.Height)
      {
        new Message("This filter can be applied just if height <= width");
        return;
      }

      Layer active_layer = image.ActiveLayer;
      Layer new_layer = new Layer(active_layer);
      new_layer.Name = "_Forge_";      
      new_layer.Visible = false;
      new_layer.Mode = active_layer.Mode;
      new_layer.Opacity = active_layer.Opacity;
      byte []pixelArray = null;
      RenderForge(new_layer, ref pixelArray, new_layer.Width, new_layer.Height);

      new_layer.Flush();
      new_layer.Update(0, 0, new_layer.Width, new_layer.Height);

      image.UndoGroupStart();
      image.AddLayer(new_layer, -1); 
      new_layer.Visible = true;
      image.ActiveLayer = new_layer;
      image.UndoGroupEnd();
      Display.DisplaysFlush();
    }

    void RenderForge(Drawable new_layer, ref byte[] pixelArray, int width, 
        int height)
    {
      InitParameters();
      Planet(new_layer, ref pixelArray, width, height);
    }

    //
    //  INITGAUSS  --  Initialise random number generators.  As given in
    //    Peitgen & Saupe, page 77.
    //

    void InitGauss()
    {
      // Range of random generator
      arand = Math.Pow(2.0, 15.0) - 1.0;
      gaussadd = Math.Sqrt(3.0 * nRand);
      gaussfac = 2 * gaussadd / (nRand * arand);
    }

    void InitParameters()
    {
      /* Set defaults when explicit specifications were not given.

         The  default  fractal  dimension  and  power  scale depend upon
         whether we're generating a planet or clouds. */
      arand = Math.Pow(2.0, 15.0) - 1.0;
      _random = new Random((int)_rseed);
      for(int i = 0; i < 7; i++)
        _random.Next();
      /*
         if(!dimspec)
         {
         fracdim = clouds ? Cast(1.9, 2.3) : Cast(2.0, 2.7);
         }
         if(!powerspec)
         powscale = clouds ? Cast(0.6, 0.8) : Cast(1.0, 1.5);
         if(!icespec)
         icelevel = Cast(0.2, 0.6);
         if(!glacspec)
         glaciers = Cast(0.6, 0.85);
         if(!starspec)
         starfraction = Cast(75, 125);
         if (!starcspec) 
         starcolour = Cast(100, 150);
         */

      if(!dimspec)
      {
        _fracdim = _clouds ? Cast(1.9, 2.3) : Cast(2.0, 2.7);
        if(_DimensionSpinButton != null)
          _DimensionSpinButton.Value = _fracdim;
      }
      if(!powerspec)
      {
        _powscale = _clouds ? Cast(0.6, 0.8) : Cast(1.0, 1.5);
        if(_PowerSpinButton != null)
          _PowerSpinButton.Value = _powscale;
      }
      if(!icespec)
      {
        _icelevel = Cast(0.2, 0.6);
        if(_IceSpinButton != null)
          _IceSpinButton.Value = _icelevel;
      }
      if(!glacspec)
      {
        _glaciers = Cast(0.6, 0.85);
        if(_GlaciersSpinButton != null)
          _GlaciersSpinButton.Value = _glaciers;
      }
      if(!starspec)
      {
        _starfraction = Cast(75, 125);
        if(_StarsSpinButton != null)
          _StarsSpinButton.Value = _starfraction;
      }
      if (!starcspec) 
      {
        _starcolour = Cast(100, 150);
        if(_SaturationSpinButton != null)
          _SaturationSpinButton.Value = _starcolour;
      }
    }

    /*  GAUSS  --  Return a Gaussian random number.  As given in Peitgen
        & Saupe, page 77. */
    double Gauss()
    {
      int i;
      double sum = 0.0;

      for (i = 1; i <= nRand; i++) {
        sum += (_random.Next() & 0x7FFF);
      }

      return gaussfac * sum - gaussadd;
    }

    void Swap(ref double a, ref double b)
    {
      double tempSwap = a;
      a = b;
      b = tempSwap;
    }

    double Cast(double low, double high)
    {
      return ((low)+(((high)-(low)) * (_random.Next() & 0x7FFF) / arand));
    }

    double Planck(double temperature, double lambda)  
    {
      double c1 = 3.7403e10;
      double c2 = 14384.0;
      double ret_val = (c1 * Math.Pow((double) lambda, -5.0));
      ret_val /= (Math.Pow(Math.E, c2 / (lambda * temperature)) - 1);

      return ret_val;
    }

    /*  TEMPRGB  --  Calculate the relative R, G, and B components	for  a
        black	body  emitting	light  at a given temperature.
        The Planck radiation equation is solved directly  for
        the R, G, and B wavelengths defined for the CIE  1931
        Standard    Colorimetric    Observer.	  The	colour
        temperature is specified in degrees Kelvin. */

    double[] TempRGB(double temp, double []rgb)
    {
      double      er, eg, eb, es;

      /* Lambda is the wavelength in microns: 5500 angstroms is 0.55 microns. */

      er = Planck(temp, 0.7000);
      eg = Planck(temp, 0.5461);
      eb = Planck(temp, 0.4358);

      es = (double)(1.0 / Math.Max(er, Math.Max(eg, eb)));

      rgb[0] = er*es;
      rgb[1] = eg*es;
      rgb[2] = eb*es;
      return rgb;
    }


    /*	FOURN  --  Multi-dimensional fast Fourier transform

        Called with arguments:

        data       A  one-dimensional  array  of  floats  (NOTE!!!	NOT
        DOUBLES!!), indexed from one (NOTE!!!   NOT  ZERO!!),
        containing  pairs of numbers representing the complex
        valued samples.  The Fourier transformed results	are
        returned in the same array.

        nn	      An  array specifying the edge size in each dimension.
        THIS ARRAY IS INDEXED FROM  ONE,	AND  ALL  THE  EDGE
        SIZES MUST BE POWERS OF TWO!!!

        ndim       Number of dimensions of FFT to perform.  Set to 2 for
        two dimensional FFT.

        isign      If 1, a Fourier transform is done; if -1 the  inverse
        transformation is performed.

        This  function  is essentially as given in Press et al., "Numerical
        Recipes In C", Section 12.11, pp.  467-470.
        */
    void FourierNDimensions(double []data, uint []nn, uint ndim, int isign)
    {
      uint i1, i2, i3;
      uint i2rev, i3rev;
      uint ip1, ip2, ip3;
      uint ifp1, ifp2;
      uint ibit, idim, k1, k2;
      uint nprev;
      uint nrem;
      uint n;
      uint ntot;
      double tempi, tempr;
      double theta, wi, wpi, wpr, wr, wtemp;

      ntot = 1;
      for (idim = 1; idim <= ndim; idim++)
        ntot *= nn[idim];
      nprev = 1;
      for (idim = ndim; idim >= 1; idim--) {
        n = nn[idim];
        nrem = ntot / (n * nprev);
        ip1 = nprev << 1;
        ip2 = ip1 * n;
        ip3 = ip2 * nrem;
        i2rev = 1;
        for (i2 = 1; i2 <= ip2; i2 += ip1) {
          if (i2 < i2rev) {
            for (i1 = i2; i1 <= i2 + ip1 - 2; i1 += 2) {
              for (i3 = i1; i3 <= ip3; i3 += ip2) {
                i3rev = i2rev + i3 - i2;
                // Swap data[i3] with data[i3rev]
                Swap(ref data[i3], ref data[i3rev]);
                // Swap data[i3 + 1] with data[i3rev + 1]
                Swap(ref data[i3+1], ref data[i3rev+1]);
              }
            }
          }
          ibit = ip2 >> 1;
          while (ibit >= ip1 && i2rev > ibit) {
            i2rev -= ibit;
            ibit >>= 1;
          }
          i2rev += ibit;
        }
        ifp1 = ip1;
        while (ifp1 < ip2) {
          ifp2 = ifp1 << 1;
          theta = isign * (Math.PI * 2) / (ifp2 / ip1);
          wtemp = Math.Sin(0.5 * theta);
          wpr = -2.0 * wtemp * wtemp;
          wpi = Math.Sin(theta);
          wr = 1.0;
          wi = 0.0;
          for (i3 = 1; i3 <= ifp1; i3 += ip1) {
            for (i1 = i3; i1 <= i3 + ip1 - 2; i1 += 2) {
              for (i2 = i1; i2 <= ip3; i2 += ifp2) {
                k1 = i2;
                k2 = k1 + ifp1;
                tempr = wr * data[k2] - wi * data[k2 + 1];
                tempi = wr * data[k2 + 1] + wi * data[k2];
                data[k2] = data[k1] - tempr;
                data[k2 + 1] = data[k1 + 1] - tempi;
                data[k1] += tempr;
                data[k1 + 1] += tempi;
              }
            }
            wr = (wtemp = wr) * wpr - wi * wpi + wr;
            wi = wi * wpr + wtemp * wpi + wi;
          }
          ifp1 = ifp2;
        }
        nprev *= n;
      }
    }


    /*  SPECTRALSYNTH  --  Spectrally  synthesised	fractal  motion in two
        dimensions.  This algorithm is given under  the
        name   SpectralSynthesisFM2D  on  page  108  of
        Peitgen & Saupe. */
    void SpectralSynth(ref double []x, uint n, double h)
    {
      uint bl;
      uint i, j, i0, j0;
      double rad, phase, rcos, rsin;
      double []a;
      uint []nsize = new uint[3];

      bl = ((n * n) + 1) * 2 ;
      a = new double[bl];
      x = a;

      for (i = 0; i <= n / 2; i++) {
        for (j = 0; j <= n / 2; j++) {
          phase = 2 * Math.PI * ((_random.Next() & 0x7FFF) / arand);
          if (i != 0 || j != 0) {
            rad = Math.Pow((double) (i * i + j * j), -(h + 1) / 2) * Gauss();
          } else {
            rad = 0;
          }
          rcos = rad * Math.Cos(phase);
          rsin = rad * Math.Sin(phase);
          // Real(a, i, j) = rcos;
          a[1 + (((i) * meshsize) + (j)) * 2] = rcos;
          // Imag(a, i, j) = rsin;
          a[2 + (((i) * meshsize) + (j)) * 2] = rsin;
          i0 = (i == 0) ? 0 : n - i;
          j0 = (j == 0) ? 0 : n - j;
          // Real(a, i0, j0) = rcos;
          a[1 + (((i0) * meshsize) + (j0)) * 2] = rcos;
          // Imag(a, i0, j0) = - rsin;
          a[2 + (((i0) * meshsize) + (j0)) * 2] = -rsin;
        }
      }
      // Imag(a, n / 2, 0) = 0;
      a[2 + (n * meshsize)] = 0;
      // Imag(a, 0, n / 2) = 0;
      a[2 + n] = 0;
      // Imag(a, n / 2, n / 2) = 0;
      a[2 + (n) * meshsize + n] = 0;
      for (i = 1; i <= n / 2 - 1; i++) {
        for (j = 1; j <= n / 2 - 1; j++) {
          phase = 2 * Math.PI * ((_random.Next() & 0x7FFF) / arand);
          rad = Math.Pow((double) (i * i + j * j), -(h + 1) / 2) * Gauss();
          rcos = rad * Math.Cos(phase);
          rsin = rad * Math.Sin(phase);
          // Real(a, i, n - j) = rcos;
          a[1 + (((i) * meshsize) + (n-j)) * 2] = rcos;
          // Imag(a, i, n - j) = rsin;
          a[2 + (((i) * meshsize) + (n - j)) * 2] = rsin;
          // Real(a, n - i, j) = rcos;
          a[1 + (((n - i) * meshsize) + (j)) * 2] = rcos;
          // Imag(a, n - i, j) = - rsin;
          a[2 + (((n - i) * meshsize) + (j)) * 2] = -rsin;
        }
      }

      nsize[0] = 0;
      nsize[1] = nsize[2] = n;	      /* Dimension of frequency domain array */
      FourierNDimensions(a, nsize, 2, -1);	      /* Take inverse 2D Fourier transform */
    }

    /*  ETOILE  --	Set a pixel in the starry sky.	*/
    void Etoile(ref Pixel rgbPixel)
    {
      const double starQuality = 0.5;	      /* Brightness distribution exponent */
      const double starIntensity = 8;	      /* Brightness scale factor */
      const double starTintExp =	0.5;	      /* Tint distribution exponent */
      if ((_random.Next() % 1000) < _starfraction) {
        double v = starIntensity * Math.Pow(1 / (1 - Cast(0, 0.9999)), starQuality);
        double temp;
        //r, g, b; 
        double []rgbDoubleArray = new double[3];

        if (v > 255) v = 255;

        /* We make a special case for star colour  of zero in order to
           prevent  floating  point  roundoff  which  would  otherwise
           result  in  more  than  256 star colours.  We can guarantee
           that if you specify no star colour, you never get more than
           256 shades in the image. */

        if (_starcolour == 0) 
        {
          rgbPixel = new Pixel((byte)v, (byte)v, (byte)v);
        } 
        else 
        {
          temp = 5500 + _starcolour *
            Math.Pow(1 / (1 - Cast(0, 0.9999)), starTintExp) *
            (((_random.Next() & 7) != 0) ? -1 : 1);
          /* Constrain temperature to a reasonable value: >= 2600K
             (S Cephei/R Andromedae), <= 28,000 (Spica). */
          temp = Math.Max(2600, Math.Min(28000, temp));
          TempRGB(temp, rgbDoubleArray);
          rgbPixel = new Pixel();
          for(int dummy_i = 0; dummy_i < 3; dummy_i++)
            rgbPixel.SetColor((Pixel.ColorName)dummy_i, 
                (byte)(rgbDoubleArray[dummy_i] * v + 0.499));
        }
      } 
      else 
      {
        rgbPixel = new Pixel(); // automatically constructs Pixel(0,0,0)
      }
    }

    //
    //  GENPLANET  --  Generate planet from elevation array.
    //

    void GenPlanet(Drawable drawable, ref byte[] pixelArray, int width, 
        int height, double []a, uint n)
    {
      double rgbQuant = 255; 
      double atthick = 1.03;   /* Atmosphere thickness as a 
                                  percentage of planet's diameter */
      byte []cp = null;
      byte []ap = null;
      double []u = null;
      double []u1 = null;
      uint []bxf = null;
      uint []bxc = null;
      uint ap_index = 0;

      uint  rpix_offset = 0; // Offset to simulate the pointer for rpix
      double athfac = Math.Sqrt(atthick * atthick - 1.0);
      double []sunvec = new double[3];
      const double starClose = 2;
      const double atSatFac = 1.0;

      double shang, siang;
      double r;
      double t = 0;
      double t1 = 0;
      double by, dy;
      double dysq = 0;
      double sqomdysq, icet;
      double svx = 0;
      double svy = 0; 
      double svz = 0; 
      double azimuth;
      int byf = 0;
      int byc = 0;
      int lcos = 0;
      double dx; 
      double dxsq;
      double ds, di, inx;
      double dsq, dsat;
      byte ir, ig, ib;
      PixelFetcher pf = null; 

      if (drawable != null)
        pf = new PixelFetcher(drawable, false);

      if (!_stars) {
        u = new double[width];
        u1 = new double[width];
        bxf = new uint[width];
        bxc = new uint[width];

        /* Compute incident light direction vector. */

        shang = hourspec ? _hourangle : Cast(0, 2 * Math.PI);
        siang = inclspec ? _inclangle : Cast(-Math.PI * 0.12, Math.PI * 0.12);

        sunvec[0] = Math.Sin(shang) * Math.Cos(siang);
        sunvec[1] = Math.Sin(siang);
        sunvec[2] = Math.Cos(shang) * Math.Cos(siang);

        /* Allow only 25% of random pictures to be crescents */

        if (!hourspec && ((_random.Next() % 100) < 75)) {
          sunvec[2] = Math.Abs(sunvec[2]);
        }

        /* Prescale the grid points into intensities. */

        cp = new byte[n * n];

        ap = cp;
        for (int i = 0; i < n; i++) 
        {
          for (int j = 0; j < n; j++) {
            ap[ap_index++] = (byte)(255.0 *  (a[1 + (i * meshsize + j) * 2]
                  + 1.0) / 2.0);
          }
        }

        /* Fill the screen from the computed  intensity  grid  by  mapping
           screen  points onto the grid, then calculating each pixel value
           by bilinear interpolation from  the	surrounding  grid  points.
           (N.b. the pictures would undoubtedly look better when generated
           with small grids if	a  more  well-behaved  interpolation  were
           used.)

           Before  we get started, precompute the line-level interpolation
           parameters and store them in an array so we don't  have  to  do
           this every time around the inner loop. */

        //#define UPRJ(a,size) ((a)/((size)-1.0))

        for (int j = 0; j < width; j++) 
        {
          //          double bx = (n - 1) * UPRJ(j, screenxsize);
          double bx = (n - 1) * (j/(width-1.0));

          bxf[j] = (uint)Math.Floor(bx);
          bxc[j] = bxf[j] + 1;
          u[j] = bx - bxf[j];
          u1[j] = 1 - u[j];
        }
      }
      Pixel[] pixels = new Pixel[width];

      for (int i = 0; i < height; i++) 
      {
        t = 0;
        t1 = 0;
        dysq = 0;
        svx = 0;
        svy = 0; 
        svz = 0; 
        byf = 0;
        byc = 0;
        lcos = 0;

        if (!_stars) {		      /* Skip all this setup if just stars */
          //#define UPRJ(a,size) ((a)/((size)-1.0))
          //          by = (n - 1) * UPRJ(i, screenysize);
          by = (n - 1) * ((double)i / ((double)height-1.0));
          dy = 2 * (((height / 2) - i) / ((double) height));
          dysq = dy * dy;
          sqomdysq = Math.Sqrt(1.0 - dysq);
          svx = sunvec[0];
          svy = sunvec[1] * dy;
          svz = sunvec[2] * sqomdysq;
          byf = (int)(Math.Floor(by) * n);
          byc = byf + (int)n;
          t = by - Math.Floor(by);
          t1 = 1 - t;
        }

        if (_clouds) 
        {

          // Render the FFT output as clouds.

          for (int j = 0; j < width; j++) 
          {
            r = 0;
            if((byf + bxf[j]) < cp.Length)
              r += t1 * u1[j] * cp[byf + bxf[j]]; 
            if((byc + bxf[j]) < cp.Length)
              r += t * u1[j] * cp[byc + bxf[j]]; 
            if((byc + bxc[j]) < cp.Length)
              r += t * u[j] * cp[byc + bxc[j]]; 
            if((byf + bxc[j]) < cp.Length)
              r += t1 * u[j] * cp[byf + bxc[j]]; 
            byte w = (byte)((r > 127.0) ? (rgbQuant * ((r - 127.0) / 128.0)) : 0);

            pixels[j] = new Pixel(w, w, (byte)rgbQuant);
          }
        } 
        else if (_stars) 
        {

          /* Generate a starry sky.  Note  that no FFT is performed;
             the output is  generated  directly  from  a  power  law
             mapping	of  a  pseudorandom sequence into intensities. */

          for (int j = 0; j < pixels.Length; j++) 
          {
            Etoile(ref pixels[j]);
          }
        } 
        else 
        {
          for (int j = 0; j < width; j++) 
            pixels[j] = new Pixel(); // Pixel(0,0,0)

          azimuth = Math.Asin((((double)i / (height - 1)) * 2) - 1);
          icet = Math.Pow(Math.Abs(Math.Sin(azimuth)), 1.0 / _icelevel) - 0.5;
          lcos = (int)((height / 2) * Math.Abs(Math.Cos(azimuth)));
          rpix_offset = (uint)(width/2 - lcos);

          for (int j = (int)((width / 2) - lcos); 
              j <= (int)((width / 2) + lcos); 
              j++) 
          {
            r = 0.0;
            ir = 0;
            ig = 0;
            ib = 0;

            r = 0;
            if((byf + bxf[j]) < cp.Length)
              r += t1 * u1[j] * cp[byf + bxf[j]]; 
            if((byc + bxf[j]) < cp.Length)
              r += t * u1[j] * cp[byc + bxf[j]]; 
            if((byc + bxc[j]) < cp.Length)
              r += t * u[j] * cp[byc + bxc[j]]; 
            if((byf + bxc[j]) < cp.Length)
              r += t1 * u[j] * cp[byf + bxc[j]]; 

            double ice;

            if (r >= 128) 
            {
              //#define ELEMENTS(array) (sizeof(array)/sizeof((array)[0]))
              //int ix = ((r - 128) * (ELEMENTS(pgnd) - 1)) / 127;
              int ix = (int)((r - 128) * (99 - 1)) / 127;

              /* Land area.  Look up colour based on elevation from
                 precomputed colour map table. */
              ir = pgnd[ix,0];
              ig = pgnd[ix,1];
              ib = pgnd[ix,2];
            } 
            else 
            {

              /* Water.  Generate clouds above water based on
                 elevation.  */

              ir = (byte)((r > 64) ? (r - 64) * 4 : 0);
              ig = ir;
              ib = 255;
            }

            /* Generate polar ice caps. */

            ice = Math.Max(0.0, icet + _glaciers * Math.Max(-0.5, 
                  (r - 128) / 128.0));
            if  (ice > 0.125) 
            {
              ir = ig = ib = 255;
            }

            /* Apply limb darkening by cosine rule. */

            {   
              dx = 2 * (((width / 2) - j) / ((double) height));
              dxsq = dx * dx;
              di = svx * dx + svy + svz * Math.Sqrt(1.0 - dxsq);
              //#define 	    PlanetAmbient  0.05
              if (di < 0)
                di = 0;
              di = Math.Min(1.0, di);

              ds = Math.Sqrt(dxsq + dysq);
              ds = Math.Min(1.0, ds);
              /* Calculate  atmospheric absorption  based on the
                 thickness of atmosphere traversed by  light  on
                 its way to the surface. */

              //#define 	    AtSatFac 1.0
              dsq = ds * ds;
              dsat = atSatFac * ((Math.Sqrt(atthick * atthick - dsq) -
                    Math.Sqrt(1.0 * 1.0 - dsq)) / athfac);
              //#define 	    AtSat(x,y) x = ((x)*(1.0-dsat))+(y)*dsat
              /*
                 AtSat(ir, 127);
                 AtSat(ig, 127);
                 AtSat(ib, 255);
                 */
              ir = (byte)((ir*(1.0-dsat))+127*dsat);
              ig = (byte)((ig*(1.0-dsat))+127*dsat);
              ib = (byte)((ib*(1.0-dsat))+255*dsat);

              inx = planetAmbient + (1.0 - planetAmbient) * di;
              ir = (byte)(ir * inx);
              ig = (byte)(ig * inx);
              ib = (byte)(ib * inx);
            }

            pixels[rpix_offset++].SetColor(ir, ig, ib);
          }

          /* Left stars */
          //#define StarClose	2
          for (int j = 0; j < width / 2 - (lcos + starClose); j++) 
          {
            Etoile(ref pixels[j]);
          }

          /* Right stars */
          for (int j = (int) (width / 2 + (lcos + starClose)); j < width; 
              j++) 
          {
            Etoile(ref pixels[j]);
          }
        }

        if (drawable != null)
        {
          for (int x = 0; x < pixels.Length; x++) 
            pf.PutPixel(x, i, pixels[x].ToByte());

          _progress.Update((double)i/height);
        }
        else
        {
          for (int x = 0; x < pixels.Length; x++) 
            pixels[x].ToByte().CopyTo(pixelArray, 3*(i*width+x));
        }
      }

      if (drawable != null)
      {
        pf.Dispose();

        drawable.Flush();
        drawable.Update(0, 0, width, height);
        Display.DisplaysFlush();
      }
    }

    //
    //  PLANET  --	Make a planet.
    //

    bool Planet(Drawable drawable, ref byte[] pixelArray, int width, 
        int height)
    {
      double[] a = null;

      InitGauss();

      if (!_stars) 
      {
        SpectralSynth(ref a, meshsize, 3.0 - _fracdim);

        if (a == null)
          return false;

        // Apply power law scaling if non-unity scale is requested.
        if (_powscale != 1.0) 
        {
          for (int i = 0; i < meshsize; i++) 
          {
            for (int j = 0; j < meshsize; j++) 
            {
              //        double r = Real(a, i, j);
              double r = a[1 + (i * meshsize + j) * 2];

              if (r > 0) 
              {
                //      Real(a, i, j) = Math.Pow(r, powscale);
                a[1 + (i * meshsize + j) * 2] =
                  Math.Pow(r, _powscale);
              }
            }
          }
        }

        // Compute extrema for autoscaling.

        double rmin = 1e50;
        double rmax = -1e50;

        for (int i = 0; i < meshsize; i++) 
        {
          for (int j = 0; j < meshsize; j++) 
          {
            //	    double r = Real(a, i, j);
            double r = a[1 + (i * meshsize + j) * 2];

            rmin = Math.Min(rmin, r);
            rmax = Math.Max(rmax, r);
          }
        }

        double rmean = (rmin + rmax) / 2;
        double rrange = (rmax - rmin) / 2;

        for (int i = 0; i < meshsize; i++) 
        {
          for (int j = 0; j < meshsize; j++) 
          {
            //	    Real(a, i, j) = (Real(a, i, j) - rmean) / rrange;
            a[1 + (i * meshsize + j) * 2] = 
              (a[1 + (i * meshsize + j) * 2] - rmean) / rrange;
          }
        }
      }

      GenPlanet(drawable, ref pixelArray, width, height, a, meshsize);

      return true;
    }
  }
}

