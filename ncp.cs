using System;
using System.Collections;

using Gtk;

namespace Gimp
  {
    public class ncp : Plugin
    {
      Drawable drawable;
      GimpParam[] values = new GimpParam[1];
      UInt32 _seed;
      bool _random_seed;

      int _points = 12;
      int _closest = 1;
      const bool color = true;

      [STAThread]
      static void Main(string[] args)
      {
	ncp plugin = new ncp(args);
      }

      public ncp(string[] args) : base(args)
      {
      }

      struct Point 
      {
	public int x;
	public int y;
      }

      override protected void Query()
      {
	GimpParamDef[] args = new GimpParamDef[3];

	args[0].type = PDBArgType.INT32;
	args[0].name = "run_mode";
	args[0].description = "Interactive, non-interactive";

	args[1].type = PDBArgType.IMAGE;
	args[1].name = "image";
	args[1].description = "Input image (unused)";

	args[2].type = PDBArgType.DRAWABLE;
	args[2].name = "drawable";
	args[2].description = "Input drawable";

	InstallProcedure("plug_in_ncp",
			 "Generates 2D textures",
			 "Generates 2D textures",
			 "Maurits Rijk",
			 "Maurits Rijk",
			 "Today",
			 "NCP...",
			 "RGB*, GRAY*",
			 args);

	MenuRegister("plug_in_ncp",
		     "<Image>/Filters/Render");
      }

      override protected void Run(string name, GimpParam[] param,
				  out GimpParam[] return_vals)
      {
	values[0].type = PDBArgType.STATUS;
	values[0].data.d_status = PDBStatusType.PDB_SUCCESS;
	return_vals = values;

	drawable = new Drawable(param[2].data.d_drawable);

	gimp_ui_init("ncp", true);

	Dialog dialog = DialogNew("ncp", "ncp",
				  IntPtr.Zero, 0, null, "ncp", 
				  Stock.Cancel, ResponseType.Cancel,
				  Stock.Ok, ResponseType.Ok);

	VBox vbox = new VBox(false, 12);
	vbox.BorderWidth = 12;
	dialog.VBox.PackStart(vbox, true, true, 0);

	GimpTable table = new GimpTable(3, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	vbox.PackStart(table, false, false, 0);

	RandomSeed seed = new RandomSeed(ref _seed, ref _random_seed);
	Widget label = table.AttachAligned(0, 0, "Random _Seed:", 0.0, 0.5,
					  seed, 2, true);

	ScaleEntry entry = new ScaleEntry(table, 0, 1, "_Points:", 150, 3,
					  _points, 1.0, 256.0, 1.0, 8.0, 0,
					  true, 0, 0, null, null);
	entry.ValueChanged += new EventHandler(PointsUpdate);

	entry = new ScaleEntry(table, 0, 2, "C_lose to:", 150, 3,
			       _closest, 1.0, 256.0, 1.0, 8.0, 0,
			       true, 0, 0, null, null);
	entry.ValueChanged += new EventHandler(CloseToUpdate);
			       
	dialog.ShowAll();
	DialogRun();

	drawable.Detach();
      }


      void PointsUpdate(object sender, EventArgs e)
      {
	_points = (int) ((Adjustment)sender).Value;
      }

      void CloseToUpdate(object sender, EventArgs e)
      {
	_closest = (int) ((Adjustment)sender).Value;
      }
		
      Point[,] vp;

      int[] _distances;

      int bpp;
      bool has_alpha;
      int width, height;

      override protected void DoSomething()
      {
	int x1, y1, x2, y2;
	drawable.MaskBounds(out x1, out y1, out x2, out y2);

	Random random = new Random((int) _seed);

	bpp = drawable.Bpp;
	has_alpha = drawable.HasAlpha();
	if (has_alpha)
	  bpp--;

	width = x2 - x1;
	height = y2 - y1;

	int xmid = width / 2;
	int ymid = height / 2;

	_distances = new int[4 * _points];
	vp = new Point[bpp, 4 * _points];

	for (int b = 0; b < bpp; b++) 
	  {
	  for (int i = 0; i < _points; i++)
	    {
	    int px = random.Next(0, width - 1);
	    int py = random.Next(0, height - 1);

	    vp[b, i].x = px;
	    vp[b, i].y = py ;
	    vp[b, i + _points].x = (px < xmid) ? (vp[b, i].x + width) 
	      : (vp[b, i].x - width);
	    vp[b, i + _points].y = vp[b, i].y;
	    vp[b, i + 2 * _points].x = vp[b, i].x;
	    vp[b, i + 2 * _points].y = (py < ymid) ? (vp[b, i].y + height) 
	      : (vp[b, i].y - height);
	    vp[b, i + 3 * _points].x = (px < xmid) ? (vp[b, i].x + width) 
	      : (vp[b, i].x - width);
	    vp[b, i + 3 * _points].y = (py < ymid) ? (vp[b, i].y + height) 
	      : (vp[b, i].y - height);
	    }
	  }
				
	ProgressInit("NCP");
	RgnIterator iter = new RgnIterator(drawable, RunMode.INTERACTIVE);
	iter.Iterate(new RgnIterator.IterFuncDest(DoNCP));
			
	Display.DisplaysFlush();
      }

      int Select(int n)
      {
	int pivot = 0;
	ArrayList data = new ArrayList(_distances);
	
	while (true)
	  {
	  ArrayList under = new ArrayList();
	  ArrayList over = new ArrayList();
	  int pcount = 0;
	  
	  pivot = (int) data[0];
	  foreach (int elem in data)
	    {
	    if (elem < pivot)
	      under.Add(elem);
	    else if (elem > pivot)
	      over.Add(elem);
	    else
	      pcount++;
	    }
	  
	  if (n < under.Count)
	    {
	    data = under;
	    }
	  else if (n < under.Count + pcount)
	    {
	    return pivot;
	    }
	  else
	    {
	    data = over;
	    n -= under.Count + pcount;
	    }
	  }
	return pivot;
      }
      
      void DoNCP(int x, int y, ref byte[] dest)
      {
	for (int b = 0; b < bpp; b++) 
	  {
	  // compute distance to each point
	  for (int k = 0; k < _points * 4; k++) 
	    {
	    int x2 = x - vp[b, k].x;
	    int y2 = y - vp[b, k].y;
	    _distances[k] = x2 * x2 + y2 * y2;
	    }

	  byte val = (byte) (255.0 * Math.Sqrt((double) Select(_closest) / 
					       (width * height)));

	  // invert
	  val = (byte) (255 - val);
	  if (color) 
	    { 
	    dest[b] = val;
	    }
	  else 
	    {
	    for (int l = 0; l < bpp; l++) 
	      dest[l] = val;
	    break;
	    }
	  }
	if (has_alpha) 
	  dest[bpp]= 255;
      }
    }
  }
