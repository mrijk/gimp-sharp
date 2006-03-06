// The UpdateCheck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// UpdateCheck.cs
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
using System.IO;
using System.Net;
using System.Xml;

using Gtk;

namespace Gimp.UpdateCheck
{
  public class UpdateCheck : Plugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new UpdateCheck(args);
    }

    public UpdateCheck(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_update_check",
					  "Check for updates",
					  "Check for updates",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Check for Updates...",
					  "",
					  in_params);
      procedure.MenuPath = "<Toolbox>/Xtns/Extensions";
      procedure.IconFile = "UpdateCheck.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("UpdateCheck", true);

      Dialog dialog = DialogNew("UpdateCheck", "UpdateCheck", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "UpdateCheck");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      XmlDocument doc = new XmlDocument();

      try {
	WebRequest myRequest = 
	  WebRequest.Create("http://gimp-sharp.sourceforge.net/version.xml");
      
	WebResponse myResponse = myRequest.GetResponse();
	
	Stream stream = myResponse.GetResponseStream();
	doc.Load(stream);

	myResponse.Close();
      } catch (Exception e) {
	Console.WriteLine("Exception!");
	Console.WriteLine(e.StackTrace);
	return;
      }

      XmlElement root = doc.DocumentElement;
      XmlNodeList nodeList = root.SelectNodes("/packages/package");
      
      foreach (XmlNode node in nodeList)
	{
	  XmlAttributeCollection attributes = node.Attributes;
	  XmlAttribute version = (XmlAttribute)
	    attributes.GetNamedItem("version");
	  Console.WriteLine(version.Value);
	}
      
    }
  }
}
