using System;

using Gtk;

namespace Gimp.SliceTool
{
	public class FileExistsDialog : MessageDialog
	{
		public FileExistsDialog(string filename) : 
			base(null, DialogFlags.DestroyWithParent,
			MessageType.Warning, ButtonsType.YesNo, 
			"File " + filename + " already exists!\n" + 
			"Do you want to overwrite this file?")
		{
		}

		public bool IsYes()
		{
			ResponseType response = (ResponseType) Run();
			Destroy();
			return (response == ResponseType.Yes);
		}
	}
}
