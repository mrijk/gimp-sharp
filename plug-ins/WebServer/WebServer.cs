// The WebServer plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// WebServer.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Manos;

using Gtk;

namespace Gimp.WebServer
{
  class WebServer : Plugin
  {
    Variable<int> _port = new Variable<int>("port", _("Port number"), 8080);

    Label _statusLabel;

    ManosApp app;
    string application_assembly;

    public IList<string> Arguments {
      get;
      set;
    }
 
    public string ApplicationAssembly {
      get {
	if (application_assembly == null)
	  // return System.IO.Path.GetFileName(Directory.GetCurrentDirectory ()) + ".dll";
	  return System.IO.Path.GetFileName("routes.dll");
	return application_assembly;
      }
      set {
	if (value == null)
	  throw new ArgumentNullException ("value");
	application_assembly = value;
      }
    }
   
    static void Main(string[] args)
    {
      GimpMain<WebServer>(args);
    }

    override protected Procedure GetProcedure()
    {
      var inParams = new ParamDefList();
      return new Procedure("plug_in_webserver",
			   "Embedded Webserver",
			   "Embedded Webserver",
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2011",
			   "Webserver...",
			   "",
			   inParams)
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "WebServer.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("WebServer", true);

      var dialog = DialogNew("WebServer", "WebServer", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "WebServer");

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 4) {ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);
      
      CreatePort(table);
      CreateStatusLabel(table);
      CreateStartAndStopButtons(table);

      return dialog;
    }

    void CreatePort(GimpTable table)
    {
      var port = new GimpSpinButton(0, 9999, 1, _port);

      table.AttachAligned(0, 0, _("_Port:"), 0.0, 0.5, port, 2, true);
    }

    void CreateStatusLabel(GimpTable table)
    {
      _statusLabel = new Label(_("Server stopped"));

      table.AttachAligned(0, 1, _("Status:"), 0.0, 0.5, _statusLabel, 2, 
			  true);
    }

    void CreateStartAndStopButtons(GimpTable table)
    {
      var start = new Button(Stock.Execute);
      start.Clicked += delegate {StartServer();};
      table.Attach(start, 3, 4, 0, 1);

      var stop = new Button(Stock.Stop);
      stop.Clicked += delegate {StopServer();};
      table.Attach(stop, 3, 4, 1, 2);
    }

    Thread _thread;

    void StartServer()
    {
      if (app == null) 
	app = LoadLibrary(ApplicationAssembly);
      
      _statusLabel.Text = "Started on port " + _port.Value;

      var listenAddress = System.Net.IPAddress.Any;
      AppHost.ListenAt(new System.Net.IPEndPoint(listenAddress, 
						 _port.Value));

      _thread = new Thread(new ThreadStart(delegate {
	Console.WriteLine("Before Start!");
	AppHost.Start(app);
	Console.WriteLine("Duh");
      }));
      _thread.IsBackground = true;
      _thread.Start();
    }

    void StopServer()
    {
      AppHost.Stop();
      Gimp.Quit();

      Console.WriteLine("Before stop 1");

      _thread.Abort();
      Console.WriteLine("Before stop 2");
      _thread.Join();

      Console.WriteLine("Stopped!");

      _statusLabel.Text = "Server stopped";
    }

    ManosApp LoadLibrary(string library)
    {
      var types = Assembly.LoadFrom(library).GetTypes();
      var found = Array.FindAll(types,
				type => type.BaseType == typeof(ManosApp));
      if (found.Length > 1)
	throw new Exception("Library contains multiple apps.");
      else if (found.Length == 1)
	return CreateAppInstance(found[0]);

      return app;
    }

    ManosApp CreateAppInstance(Type t)
    {
      int arg_count = 0; // Arguments.Count;
      var constructors = t.GetConstructors();
      
      foreach (var ci in constructors.Where(c => c.GetParameters().Count () == arg_count)) {

	var args = ArgsForParams(ci.GetParameters());

	if (args == null)
	  continue;
	try {
	  return (ManosApp) Activator.CreateInstance(t, args);
	} catch (Exception e) {
	  Console.Error.WriteLine("Exception creating App Type: '{0}'.", t);
	  Console.Error.WriteLine(e);
	}
      }
      
      return null;
    }

    object[] ArgsForParams(ParameterInfo[] prms)
    {
      var res = new object[prms.Length];
      
      for (int i = 0; i < prms.Count(); i++) {
	try {
	  res[i] = Convert.ChangeType(Arguments[i], prms[i].ParameterType);
	} catch (Exception e) {
	  Console.Error.WriteLine("Exception converting type: '{0}'.", 
				  prms[i].ParameterType);
	  Console.Error.WriteLine(e);
	  
	  return null;
	}
      }
      
      return res;
    }

    override protected void Render()
    {
    }
  }
}
