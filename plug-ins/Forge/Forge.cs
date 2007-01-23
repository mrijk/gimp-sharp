// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
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
using System.Collections.Generic;

using Gtk;

namespace Gimp.Forge
{
  class Forge : PluginWithPreview
  {
    const double planetAmbient = 0.05;
    bool _randomSeed = true;
    bool _previewAllowed = false;

    RadioButton _PlanetRadioButton;
    RadioButton _CloudsRadioButton;
    RadioButton _NightRadioButton;
    ScaleEntry _dimensionEntry;
    ScaleEntry _glacierEntry;
    ScaleEntry _iceEntry;
    ScaleEntry _powerEntry;
    ScaleEntry _hourEntry;
    ScaleEntry _inclinationEntry;
    ScaleEntry _starsEntry;
    ScaleEntry _saturationEntry;
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
    private const int meshsize = 256;	      	// FFT mesh size

    Random _random;
    double _arand = Math.Pow(2.0, 15.0) - 1.0;

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

    static void Main(string[] args)
    {
      new Forge(args);
    }

    public Forge(string[] args) : base(args, "Forge")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList();
      inParams.Add(new ParamDef("clouds", false, typeof(bool), 
				_("Clouds (true), Planet or Stars (false)")));
      inParams.Add(new ParamDef("stars", false, typeof(bool), 
				_("Stars (true), Planet or Clouds (false)")));
      inParams.Add(new ParamDef("dimension", 2.4, typeof(double), 
				_("Fractal dimension factor")));
      inParams.Add(new ParamDef("power", 1.0, typeof(double), 
				_("Power factor")));
      inParams.Add(new ParamDef("glaciers", 0.75, typeof(double), 
				_("Glaciers factor")));
      inParams.Add(new ParamDef("ice", 0.4, typeof(double), 
				_("Ice factor")));
      inParams.Add(new ParamDef("hour", 0.0, typeof(double), 
				_("Hour factor")));
      inParams.Add(new ParamDef("inclination", 0.0, typeof(double), 
				_("Inclination factor")));
      inParams.Add(new ParamDef("stars", 100.0, typeof(double), 
				_("Stars factor")));
      inParams.Add(new ParamDef("saturation", 100.0, typeof(double), 
				_("Saturation factor")));
      inParams.Add(new ParamDef("seed", 0, typeof(uint), 
				_("Random generated seed")));

      Procedure procedure = new Procedure("plug_in_forge",
					  _("Creates an artificial world."),
					  _("Creates an artificial world."),
					  "Massimo Perga, Maurits Rijk",
					  "(C) Massimo Perga, Maurits Rijk",
					  "2006-2007",
					  _("Forge..."),
					  "RGB*",
					  inParams);
      procedure.MenuPath = "<Image>/Filters/Render";
      procedure.IconFile = "Forge.png";

      yield return procedure;
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Forge", true);

      GimpDialog dialog = DialogNew(_("Forge 0.2"), _("Forge"), IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, _("Forge"));

      HBox hbox = new HBox(false, 12);
      Vbox.PackStart(hbox);

      // Type
      GimpFrame frame = new GimpFrame(_("Type"));
      hbox.PackStart(frame, false, false, 0);

      VBox typeBox = new VBox(false, 1);
      frame.Add(typeBox);

      _PlanetRadioButton = CreateRadioButtonInVBox(typeBox, null,
          PlanetRadioButtonEventHandler, _("Pl_anet"));

      _CloudsRadioButton = CreateRadioButtonInVBox(typeBox, _PlanetRadioButton,
          CloudsRadioButtonEventHandler, _("C_louds"));

      _NightRadioButton = CreateRadioButtonInVBox(typeBox, _PlanetRadioButton,
          NightRadioButtonEventHandler, _("_Night"));

      // Random seed

      VBox randomBox = new VBox(false, 1);
      hbox.PackStart(randomBox, false, false, 0);

      RandomSeed seed = new RandomSeed(ref _rseed, ref _randomSeed);
      randomBox.PackStart(seed, false, false, 0);


      // Create the table widget
      GimpTable table = new GimpTable(4, 6, false);
      table.ColumnSpacing = 10;
      table.RowSpacing = 10;
      // table.BorderWidth = 10;
      // Vbox.PackStart(table, false, false, 0);
      Vbox.PackEnd(table);

      // Dimension

      _dimensionEntry = new ScaleEntry(table, 0, 0, 
				       _("_Dimension (0.0 - 3.0):"), 
				       150, 3, 
				       _fracdim, 0.0, 3.0, 
				       0.1, 1.0, 1,
				       true, 0, 0, null, null);
      _dimensionEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  if (forced > 0)
	    forced--;
	  else
	    dimspec = true;
	  _fracdim = _dimensionEntry.Value;
	};

      // Power
      _powerEntry = new ScaleEntry(table, 3, 0, _("_Power:"), 
				   150, 3, 
				   _powscale, 0.0, Double.MaxValue, 
				   0.1, 1.0, 1,
				   true, 0, 0, null, null);
      _powerEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  if (forced > 0)
	    forced--;
	  else
            powerspec = true;
	  _powscale = _powerEntry.Value;
	};

      // Glaciers

      _glacierEntry = new ScaleEntry(table, 0, 1, _("_Glaciers"), 
				     150, 3, 
				     _glaciers, 0.0, Double.MaxValue, 
				     0.1, 1.0, 0,
				     true, 0, 0, null, null);
      _glacierEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  glacspec = true;
	  _glaciers = _glacierEntry.Value;
	  InvalidatePreview();
	};

      // Ice

      _iceEntry = new ScaleEntry(table, 3, 1, _("_Ice"), 
				 150, 3, 
				 _icelevel, 0.0, Double.MaxValue, 
				 0.1, 1.0, 1,
				 true, 0, 0, null, null);
      _iceEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  icespec = true;
	  _icelevel = _iceEntry.Value;
	  InvalidatePreview();
	};

      // Hour

      _hourEntry = new ScaleEntry(table, 0, 2, _("Ho_ur (0 - 24):"), 
				  150, 3, 
				  _hourangle, 0.0, 24.0, 0.1, 1.0, 0,
				  true, 0, 0, null, null);
      _hourEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  hourspec = true;
	  _hourangle = _hourEntry.Value;
	  InvalidatePreview();
	};

      // Inclination

      _inclinationEntry = new ScaleEntry(table, 3, 2, 
					 _("I_nclination (-90 - 90):"), 
					 150, 3, 
					 _inclangle, -90.0, 90.0, 1, 10.0, 0,
					 true, 0, 0, null, null);
      _inclinationEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  inclspec = true;
	  _inclangle = _inclinationEntry.Value;  
	  InvalidatePreview();
	};

      // Star percentage

      _starsEntry = new ScaleEntry(table, 0, 3, _("_Stars (0 - 100):"), 
					150, 3, 
					_starfraction, 1.0, 100.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      _starsEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  starspec = true;
	  _starfraction = _starsEntry.Value;
	  InvalidatePreview();
	};

      // Saturation

      _saturationEntry = new ScaleEntry(table, 3, 3, _("Sa_turation:"), 
					150, 3, 
					_starcolour, 0.0, Int32.MaxValue, 
					1.0, 8.0, 0,
					true, 0, 0, null, null);
      _saturationEntry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  starcspec = true;
	  _starcolour = _saturationEntry.Value;
	  InvalidatePreview();
	};

      return dialog;
    }

    RadioButton CreateRadioButtonInVBox(VBox vbox, 
        RadioButton radioButtonGroup,
        GenericEventHandler radioButtonEventHandler, 
        string radioButtonLabel)
    { 
      RadioButton radioButton = new RadioButton(radioButtonGroup, 
						radioButtonLabel);
      radioButton.Clicked += new EventHandler(radioButtonEventHandler);
      vbox.PackStart(radioButton, true, true, 10);

      return radioButton;
    }

    void SetDefaultValues()
    {
      _PlanetRadioButton.Active = true;
      _glacierEntry.Value = 0.75;
      _iceEntry.Value = 0.4;
      _hourEntry.Value = 0;
      _inclinationEntry.Value = 0;
      _starsEntry.Value = 100.0;
      _saturationEntry.Value = 125.0;
    }

    void PlanetRadioButtonEventHandler(object source, EventArgs e)
    {
      if (_PlanetRadioButton.Active == true)
      {
        if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.4;
        }
        if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 1.2;
        }
      }

      // Enable all the spin buttons
      _dimensionEntry.Sensitive = true; 
      _powerEntry.Sensitive = true; 
      _glacierEntry.Sensitive = true; 
      _iceEntry.Sensitive = true; 
      _hourEntry.Sensitive = true; 
      _inclinationEntry.Sensitive = true; 
      _starsEntry.Sensitive = true; 
      _saturationEntry.Sensitive = true; 

      if(_previewAllowed)
        InvalidatePreview();
    }

    void CloudsRadioButtonEventHandler(object source, EventArgs e)
    {
      if ((_clouds = _CloudsRadioButton.Active) == true)
      {
        if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.15;
        }
        if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 0.75;
        }
      }
      // Disable some spin buttons
      _dimensionEntry.Sensitive = true; 
      _powerEntry.Sensitive = true; 
      _glacierEntry.Sensitive = false; 
      _iceEntry.Sensitive = false; 
      _hourEntry.Sensitive = false; 
      _inclinationEntry.Sensitive = false; 
      _starsEntry.Sensitive = false; 
      _saturationEntry.Sensitive = false; 

      if(_previewAllowed)
        InvalidatePreview();
    }

    void NightRadioButtonEventHandler(object source, EventArgs e)
    {
      if ((_stars = _NightRadioButton.Active) == true)
      {
        if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.4;
        }
        if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 1.2;
        }
      }
      // Enable just the star spin button
      _dimensionEntry.Sensitive = false; 
      _powerEntry.Sensitive = false; 
      _glacierEntry.Sensitive = false; 
      _iceEntry.Sensitive = false; 
      _hourEntry.Sensitive = false; 
      _inclinationEntry.Sensitive = false; 
      _starsEntry.Sensitive = true; 
      _saturationEntry.Sensitive = false; 

      InvalidatePreview();
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      Console.WriteLine("UpdatePreview!");

      int width, height;
      preview.GetSize(out width, out height);

      byte[] pixelArray = new byte[width * height * 3];
      RenderForge(null, pixelArray, width, height);

      preview.DrawBuffer(pixelArray, width * 3);
    }

    override protected void Reset()
    {
      SetDefaultValues(); 
    }

    override protected void Render(Image image, Drawable drawable)
    {
      int width = drawable.Width;
      int height = drawable.Height;

      if (width < height)
      {
        new Message(_("This filter can be applied just if height <= width"));
        return;
      }

      Tile.CacheDefault(drawable);

      _progress = new Progress(_("Forge..."));
      RenderForge(drawable, null, width, height);
    }

    void RenderForge(Drawable drawable, byte[] pixelArray, int width, 
		     int height)
    {
      InitParameters();
      Planet(drawable, pixelArray, width, height);
    }

    void InitParameters()
    {
      // Set defaults when explicit specifications were not given.
      // The  default  fractal  dimension  and  power  scale depend upon
      // whether we're generating a planet or clouds.

      _random = new Random((int) _rseed);

      for (int i = 0; i < 7; i++)
	{
	  _random.Next();
	}

      if (!dimspec)
      {
        _fracdim = _clouds ? Cast(1.9, 2.3) : Cast(2.0, 2.7);
        if (_dimensionEntry != null)
          _dimensionEntry.Value = _fracdim;
      }

      if (!powerspec)
      {
        _powscale = _clouds ? Cast(0.6, 0.8) : Cast(1.0, 1.5);
        if(_powerEntry != null)
          _powerEntry.Value = _powscale;
      }

      if (!icespec)
	{
	  _icelevel = Cast(0.2, 0.6);
	  if(_iceEntry != null)
	    _iceEntry.Value = _icelevel;
	}

      if (!glacspec)
	{
	  _glaciers = Cast(0.6, 0.85);
	  if(_glacierEntry != null)
	    _glacierEntry.Value = _glaciers;
	}

      if (!starspec)
	{
	  _starfraction = Cast(75, 125);
	  if(_starsEntry != null)
	    _starsEntry.Value = _starfraction;
	}

      if (!starcspec) 
	{
	  _starcolour = Cast(100, 150);
	  if(_saturationEntry != null)
	    _saturationEntry.Value = _starcolour;
	}
      _previewAllowed = true;
    }

    double Cast(double low, double high)
    {
      return (low + ((high - low) * (_random.Next() & 0x7FFF) / _arand));
    }

    //
    //  GENPLANET  --  Generate planet from elevation array.
    //

    void GenPlanet(Drawable drawable, byte[] pixelArray, int width, 
		   int height, double[] a, uint n)
    {
      const double rgbQuant = 255; 
      const double atthick = 1.03;   /* Atmosphere thickness as a 
                                        percentage of planet's diameter */
      byte[] cp = null;
      byte[] ap = null;
      double[] u = null;
      double[] u1 = null;
      uint[] bxf = null;
      uint[] bxc = null;
      uint ap_index = 0;

      uint  rpix_offset = 0; // Offset to simulate the pointer for rpix
      double athfac = Math.Sqrt(atthick * atthick - 1.0);
      double[] sunvec = new double[3];
      const double starClose = 2;
      const double atSatFac = 1.0;

      double shang, siang;
      double r;
      double t = 0;
      double t1 = 0;
      double by, dy;
      double dysq = 0;
      double sqomdysq, icet;
      int lcos = 0;
      double dx; 
      double dxsq;
      double ds, di, inx;
      double dsq, dsat;

      PixelFetcher pf = null; 

      if (drawable != null)
        pf = new PixelFetcher(drawable, false);

      if (!_stars) 
	{
	  u = new double[width];
	  u1 = new double[width];
	  bxf = new uint[width];
	  bxc = new uint[width];

	  // Compute incident light direction vector.

	  shang = hourspec ? _hourangle : Cast(0, 2 * Math.PI);
	  siang = inclspec ? _inclangle : Cast(-Math.PI * 0.12, Math.PI * 0.12);

	  sunvec[0] = Math.Sin(shang) * Math.Cos(siang);
	  sunvec[1] = Math.Sin(siang);
	  sunvec[2] = Math.Cos(shang) * Math.Cos(siang);

	  // Allow only 25% of random pictures to be crescents

	  if (!hourspec && ((_random.Next() % 100) < 75)) 
	    {
	      sunvec[2] = Math.Abs(sunvec[2]);
	    }

	  // Prescale the grid points into intensities.

	  cp = new byte[n * n];

	  ap = cp;
	  for (int i = 0; i < n; i++) 
	    {
	      for (int j = 0; j < n; j++) 
		{
		  ap[ap_index++] = (byte)
		    (255.0 * (a[1 + (i * meshsize + j) * 2] + 1.0) / 2.0);
		}
	    }

	  /* Fill the screen from the computed  intensity  grid  by  mapping
	     screen  points onto the grid, then calculating each pixel value
	     by bilinear interpolation from  the surrounding  grid  points.
	     (N.b. the pictures would undoubtedly look better when generated
	     with small grids if a more well-behaved  interpolation  were
	     used.)

	     Before  we get started, precompute the line-level interpolation
	     parameters and store them in an array so we don't  have  to  do
	     this every time around the inner loop. */

	  //#define UPRJ(a,size) ((a)/((size)-1.0))

	  for (int j = 0; j < width; j++) 
	    {
	      //          double bx = (n - 1) * UPRJ(j, screenxsize);
	      double bx = (n - 1) * (j/(width-1.0));

	      bxf[j] = (uint) Math.Floor(bx);
	      bxc[j] = bxf[j] + 1;
	      u[j] = bx - bxf[j];
	      u1[j] = 1 - u[j];
	    }
	}

      StarFactory starFactory = new StarFactory(_random, _starfraction,
						_starcolour);
      Pixel[] pixels = new Pixel[width];

      for (int i = 0; i < height; i++) 
      {
        t = 0;
        t1 = 0;
        dysq = 0;
        double svx = 0;
        double svy = 0; 
        double svz = 0; 
        int byf = 0;
        int byc = 0;
        lcos = 0;

        if (!_stars) {	 // Skip all this setup if just stars
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
            pixels[j] = starFactory.Generate();
          }
        } 
        else 
        {
          for (int j = 0; j < width; j++) 
            pixels[j] = new Pixel(3);

          double azimuth = Math.Asin((((double) i / (height - 1)) * 2) - 1);
          icet = Math.Pow(Math.Abs(Math.Sin(azimuth)), 1.0 / _icelevel) 
            - 0.5;
          lcos = (int)((height / 2) * Math.Abs(Math.Cos(azimuth)));
          rpix_offset = (uint)(width/2 - lcos);

          for (int j = (int)((width / 2) - lcos); 
              j <= (int)((width / 2) + lcos); 
              j++) 
          {
            r = 0.0;
            byte ir = 0;
            byte ig = 0;
            byte ib = 0;

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

            pixels[rpix_offset++].Bytes = new byte[]{ir, ig, ib};
          }

          /* Left stars */
          //#define StarClose	2
          for (int j = 0; j < width / 2 - (lcos + starClose); j++) 
          {
            pixels[j] = starFactory.Generate();
          }

          /* Right stars */
          for (int j = (int) (width / 2 + (lcos + starClose)); j < width; 
              j++) 
          {
            pixels[j] = starFactory.Generate();
          }
        }

        if (drawable != null)
        {
          for (int x = 0; x < pixels.Length; x++) 
          {
            pf.PutPixel(x, i, pixels[x]);
          }

          _progress.Update((double)i/height);
        }
        else
        {
          for (int x = 0; x < pixels.Length; x++) 
          {
            pixels[x].CopyTo(pixelArray, 3 * (i * width + x));
          }
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

    void Planet(Drawable drawable, byte[] pixelArray, int width, int height)
    {
      double[] a = null;

      if (!_stars) 
	{
	  SpectralSynthesis spectrum = new SpectralSynthesis(_random);
	  a = spectrum.Synthesize(meshsize, 3.0 - _fracdim);

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
			  a[1 + (i * meshsize + j) * 2] = Math.Pow(r, _powscale);
			}
		    }
		}
	    }

	  // Compute extrema for autoscaling.

	  double rmin = double.MaxValue;
	  double rmax = double.MinValue;

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

      GenPlanet(drawable, pixelArray, width, height, a, meshsize);
    }
  }
}
