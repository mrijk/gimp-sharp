using System;

namespace Gimp.SliceTool
{
	public class PreferencesDialog : GimpDialog
	{
		GimpColorButton _active;
		GimpColorButton _inactive;

		public PreferencesDialog() : base("Slice Preferences", "SliceTool",
		IntPtr.Zero, 0, null, "SliceTool")
		{
			GimpTable table = new GimpTable(2, 2, false);
			table.BorderWidth = 12;
			table.ColumnSpacing = 6;
			table.RowSpacing = 6;
			VBox.PackStart(table, true, true, 0);

			RGB rgb = new RGB(255, 0, 0);
			_active = new GimpColorButton("", 16, 16, rgb.GimpRGB,
				ColorAreaType.COLOR_AREA_FLAT);
			_active.Update = true;
			table.AttachAligned(0, 0, "Active tile border color:", 0.0, 0.5, _active, 1, true);

			rgb = new RGB(0, 255, 0);
			_inactive = new GimpColorButton("", 16, 16, rgb.GimpRGB,
				ColorAreaType.COLOR_AREA_FLAT);
			_inactive.Update = true;
			table.AttachAligned(0, 1, "Inactive tile border color:", 0.0, 0.5, _inactive, 1, true);
		}

		public RGB ActiveColor
		{
			get {return _active.Color;}
			set {_active.Color = value;}
		}

		public RGB InactiveColor
		{
			get {return _inactive.Color;}
			set {_inactive.Color = value;}
		}
	}
}
