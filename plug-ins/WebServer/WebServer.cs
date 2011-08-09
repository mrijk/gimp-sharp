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

using Manos;

using Gtk;

namespace Gimp.WebServer
{
  class WebServer : Plugin
  {
    ManosApp app;
    int Port = 8080;
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
      
      return dialog;
    }

    ManosApp LoadLibrary(string library)
    {
      Assembly a = Assembly.LoadFrom(library);
      
      foreach (Type t in a.GetTypes ()) {
	if (t.BaseType == typeof (ManosApp)) {
	  if (app != null)
	    throw new Exception ("Library contains multiple apps.");
	  app = CreateAppInstance (t);
	}
      }
      
      return app;
    }

    ManosApp CreateAppInstance (Type t)
    {
      int arg_count = 0; // Arguments.Count;
      ConstructorInfo [] constructors = t.GetConstructors ();
      
      foreach (ConstructorInfo ci in constructors.Where (c => c.GetParameters ().Count () == arg_count)) {

	object [] args = ArgsForParams (ci.GetParameters ());

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

    object [] ArgsForParams (ParameterInfo [] prms)
    {
      object [] res = new object [prms.Length];
      
      for (int i = 0; i < prms.Count (); i++) {
	try {
	  res [i] = Convert.ChangeType (Arguments [i], prms [i].ParameterType);
	} catch (Exception e) {
	  Console.Error.WriteLine ("Exception converting type: '{0}'.", prms [i].ParameterType);
	  Console.Error.WriteLine (e);
	  
	  return null;
	}
      }
      
      return res;
    }

    override protected void Render()
    {
      app = LoadLibrary (ApplicationAssembly);

      Console.WriteLine ("Running {0} on port {1}.", app, Port);
      
      AppHost.Port = Port;
      AppHost.Start(app);
    }
  }
}
