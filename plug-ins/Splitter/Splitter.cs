using System;

using Gimp;
using Gtk;

namespace Gimp.Splitter
{
  public class Splitter : Plugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new Splitter(args);
    }

    public Splitter(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      InstallProcedure("plug_in_splitter",
		       "Splits an image.",
		       "Splits an image in separate parts using a formula of the form f(x, y) = 0",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "1999 - 2004",
		       "Splitter...",
		       "RGB*",
		       null);

      MenuRegister("plug_in_splitter", "<Image>/Filters/Generic");
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
      Entry entry = new Entry();
      hbox.Add(entry);
      hbox.Add(new Label("= 0"));

      GimpFrame frame1 = CreateLayerFrame("Layer 1");
      table.Attach(frame1, 0, 1, 1, 2);

      GimpFrame frame2 = CreateLayerFrame("Layer 2");
      table.Attach(frame2, 1, 2, 1, 2);

      CheckButton merge = new CheckButton("Merge visible layers");
      table.Attach(merge, 0, 1, 3, 4);

      dialog.ShowAll();
      return DialogRun();
    }

    GimpFrame CreateLayerFrame(string title)
    {
      GimpFrame frame = new GimpFrame(title);

      GimpTable table = new GimpTable(3, 3, false);
      table.BorderWidth = 12;
      table.RowSpacing = 12;
      table.ColumnSpacing = 12;
      frame.Add(table);

      SpinButton translateX = new SpinButton(int.MinValue, int.MaxValue, 1);
      translateX.Value = 0;
      translateX.WidthChars = 4;
      table.AttachAligned(0, 0, "Translate X:", 0.0, 0.5, translateX, 1, true);

      SpinButton translateY = new SpinButton(int.MinValue, int.MaxValue, 1);
      translateY.Value = 0;
      translateY.WidthChars = 4;
      table.AttachAligned(0, 1, "Translate Y:", 0.0, 0.5, translateY, 1, true);

      SpinButton rotate = new SpinButton(0, 360, 1);
      rotate.WidthChars = 4;
      table.AttachAligned(0, 2, "Rotate:", 0.0, 0.5, rotate, 1, true);

      return frame;
    }
  }
  }
