using System;

using Gtk;

namespace Gimp.RedEye
{
  public class RedEye : Plugin
  {
    DrawablePreview _preview;
    int _x, _y;

    [SaveAttribute]
    int _radius = 8;
    [SaveAttribute]
    int _threshold = 40;

    [STAThread]
    static void Main(string[] args)
    {
      new RedEye(args);
    }

    public RedEye(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      InstallProcedure("plug_in_red_eye",
		       "Red eye correction",
		       "Red eye correction",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2004-2005",
		       "Red Eye Correction...",
		       "RGB*",
		       null);

      MenuRegister("<Image>/Filters/Enhance");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("red_eye", true);
      Dialog dialog = DialogNew("Red Eye Correction", "red_eye",
				IntPtr.Zero, 0, null, "red eye");
      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += new EventHandler(UpdatePreview);
      _preview.ButtonPressEvent += new ButtonPressEventHandler(PreviewClicked);
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      ScaleEntry entry = new ScaleEntry(table, 0, 0, "_Radius:", 150, 3,
					_radius, 1.0, 100.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);

      entry = new ScaleEntry(table, 0, 1, "_Threshold:", 150, 3,
			     _threshold, 1.0, 256.0, 1.0, 8.0, 0,
			     true, 0, 0, null, null);

      dialog.ShowAll();
      return DialogRun();
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      int x, y, width, height;

      _preview.GetPosition(out x, out y);
      _preview.GetSize(out width, out height);
    }

    void PreviewClicked(object o, ButtonPressEventArgs args)
    {
      int x, y;

      _preview.GetPosition(out x, out y);

      _x = x + (int) args.Event.X;
      _y = y + (int) args.Event.Y;
      Console.WriteLine("Clicked {0}!", _x);
    }

    override protected void DoSomething(Image image, Drawable drawable)
    {
      // int x1, y1, x2, y2;
      // drawable.MaskBounds(out x1, out y1, out x2, out y2);

      image.UndoGroupStart();

      image.SetComponentActive(ChannelType.RED, false);
      image.SetComponentActive(ChannelType.BLUE, false);
      drawable.FuzzySelect(_x, _y, _threshold, ChannelOps.ADD, true, true, 
			   _radius, false);

      // Desaturate the red channel
      image.SetComponentActive(ChannelType.RED, true);
      image.SetComponentActive(ChannelType.GREEN, false);
      drawable.Desaturate();
      image.SetComponentActive(ChannelType.GREEN, true);
      image.SetComponentActive(ChannelType.BLUE, true);

      image.UndoGroupEnd();

      Display.DisplaysFlush();
    }
  }
  }
