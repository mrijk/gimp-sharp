// The UpdateCheck plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Dialog.cs
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

using Gtk;

namespace Gimp.UpdateCheck
{
  public class Dialog : GimpDialog
  {
    public Dialog(VariableSet variables) : base(_("UpdateCheck"), variables)
    {
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 3)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, true, true, 0);
      
      table.Attach(new GimpCheckButton(_("Check _GIMP"), 
				       GetVariable<bool>("check_gimp")),
		   0, 1, 0, 1);

      table.Attach(new GimpCheckButton(_("Check G_IMP#"), 
				       GetVariable<bool>("check_gimp_sharp")),
		   0, 1, 1, 2);

      table.Attach(new GimpCheckButton(_("Check _Unstable Releases"),
				       GetVariable<bool>("check_unstable")),
		   0, 1, 2, 3);

      var enableProxy = GetVariable<bool>("enable_proxy");
      var httpProxy = GetVariable<string>("http_proxy");
      var port = GetVariable<string>("port");

      string tmp = Gimp.RcQuery("update-enable-proxy");
      enableProxy.Value = (tmp != null || tmp == "true");
      httpProxy.Value =  Gimp.RcQuery("update-http-proxy") ?? "";
      port.Value = Gimp.RcQuery("update-port") ?? "";

      var expander = new Expander(_("Proxy settings"));
      var proxyBox = new VBox(false, 12);

      proxyBox.Add(new GimpCheckButton(_("Manual proxy configuration"),
				       enableProxy));

      var hbox = new HBox(false, 12) {Sensitive = enableProxy.Value};
      proxyBox.Add(hbox);

      hbox.Add(new Label(_("HTTP Proxy:")));
      hbox.Add(new GimpEntry(httpProxy));

      hbox.Add(new Label(_("Port:")));
      hbox.Add(new GimpEntry(port) {WidthChars = 4});
      
      enableProxy.ValueChanged += delegate
	{
	  hbox.Sensitive = enableProxy.Value;
	};
      
      expander.Add(proxyBox);
      table.Attach(expander, 0, 1, 3, 4);
    }
  }
}
