// The UpdateCheck plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

using Gtk;

namespace Gimp.UpdateCheck
{
  class UpdateCheck : Plugin
  {
    static ManualResetEvent allDone= new ManualResetEvent(false);
    const int BUFFER_SIZE = 1024;
    const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

    Variable<bool> _checkGimp = new Variable<bool>("check-gimp", "", true);
    Variable<bool> _checkGimpSharp = new Variable<bool>("check-gimp-sharp", 
							"", true);
    Variable<bool> _checkUnstable = new Variable<bool>("check-unstable",
						       "", true);
    Variable<bool> _enableProxy = new Variable<bool>("enable-proxy", "", false);
    Variable<string> _httpProxy = new Variable<string>("http-proxy", "", "");
    Variable<string> _port = new Variable<string>("port", "", "");

    static void Main(string[] args)
    {
      GimpMain<UpdateCheck>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_update_check",
			   _("Check for updates"),
			   _("Check for updates"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Check for Updates..."),
			   "")
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "UpdateCheck.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("UpdateCheck", true);

      var dialog = DialogNew(_("UpdateCheck"), _("UpdateCheck"), 
			     IntPtr.Zero, 0, Gimp.StandardHelpFunc, 
			     _("UpdateCheck"));

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 3)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, true, true, 0);

      var checkGimp = new GimpCheckButton(_("Check _GIMP"), _checkGimp);
      table.Attach(checkGimp, 0, 1, 0, 1);

      var checkGimpSharp = new GimpCheckButton(_("Check G_IMP#"), 
					       _checkGimpSharp);
      table.Attach(checkGimpSharp, 0, 1, 1, 2);

      var checkUnstable = new GimpCheckButton(_("Check _Unstable Releases"),
					      _checkUnstable);
      table.Attach(checkUnstable, 0, 1, 2, 3);

      string tmp = Gimp.RcQuery("update-enable-proxy");
      _enableProxy.Value = (tmp != null || tmp == "true");
      _httpProxy.Value =  Gimp.RcQuery("update-http-proxy") ?? "";
      _port.Value = Gimp.RcQuery("update-port") ?? "";

      var expander = new Expander(_("Proxy settings"));
      var proxyBox = new VBox(false, 12);

      var enableProxy = new GimpCheckButton(_("Manual proxy configuration"),
					    _enableProxy);
      proxyBox.Add(enableProxy);

      var hbox = new HBox(false, 12) {Sensitive = _enableProxy.Value};
      proxyBox.Add(hbox);

      hbox.Add(new Label(_("HTTP Proxy:")));
      hbox.Add(new GimpEntry(_httpProxy));

      hbox.Add(new Label(_("Port:")));
      var port = new GimpEntry(_port) {WidthChars = 4};
      hbox.Add(port);
      
      enableProxy.Toggled += delegate
	{
	  hbox.Sensitive = enableProxy.Active;
	};
      
      expander.Add(proxyBox);
      table.Attach(expander, 0, 1, 3, 4);

      return dialog;
    }

    override protected void Render()
    {
      if (_enableProxy.Value)
	{
	  Gimp.RcSet("update-enable-proxy", 
		     (_enableProxy.Value) ? "true" : "false");
	  Gimp.RcSet("update-http-proxy", _httpProxy.Value);
	  Gimp.RcSet("update-port", _port.Value);
	}

      var assembly = Assembly.GetAssembly(typeof(Plugin));
      Console.WriteLine(assembly.GetName().Version);

      var doc = new XmlDocument();

      try 
	{
	  var myRequest = (HttpWebRequest) 
	    WebRequest.Create("http://gimp-sharp.sourceforge.net/version.xml");
	  
	  // Create a proxy object, needed for mono behind a firewall?!
	  if (_enableProxy.Value)
	    {
	      var myProxy = new WebProxy();
	      myProxy.Address = new Uri(_httpProxy.Value + ":" + _port.Value);
	      myRequest.Proxy = myProxy;
	    }
      
	  var requestState = new RequestState(myRequest);
	  
	  // Start the asynchronous request.
	  IAsyncResult result= (IAsyncResult) myRequest.BeginGetResponse
	    (new AsyncCallback(RespCallback), requestState);
	  
	  // this line implements the timeout, if there is a timeout, 
	  // the callback fires and the request becomes aborted
	  ThreadPool.RegisterWaitForSingleObject
	    (result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), 
	     myRequest, DefaultTimeout, true);
	  
	  // The response came in the allowed time. The work processing will 
	  // happen in the callback function.
	  allDone.WaitOne();
	  
	  // Release the HttpWebResponse resource.
	  requestState.Response.Close();
	} 
      catch (Exception e) 
	{
	  Console.WriteLine("Exception!");
	  Console.WriteLine(e.StackTrace);
	  return;
	}
    }

    // Abort the request if the timer fires.
    static void TimeoutCallback(object state, bool timedOut) 
    { 
      if (timedOut) 
	{
	  var request = state as HttpWebRequest;
	  if (request != null) 
	    {
	      request.Abort();
	    }
	}
    }

    static void RespCallback(IAsyncResult asynchronousResult)
    {  
      try
	{
	  // State of request is asynchronous.
	  var requestState = (RequestState) asynchronousResult.AsyncState;
	  var myHttpWebRequest = requestState.Request;
	  requestState.Response = (HttpWebResponse) 
	    myHttpWebRequest.EndGetResponse(asynchronousResult);
	  
	  // Read the response into a Stream object.
	  var responseStream = requestState.Response.GetResponseStream();
	  requestState.StreamResponse = responseStream;
	  
	  var asynchronousInputRead = 
	    responseStream.BeginRead(requestState.BufferRead, 0, 
				     BUFFER_SIZE, 
				     new AsyncCallback(ReadCallBack), 
				     requestState);
	  return;
	}
      catch(WebException e)
	{
	  Console.WriteLine("\nRespCallback Exception raised!");
	  Console.WriteLine("\nMessage:{0}",e.Message);
	  Console.WriteLine("\nStatus:{0}",e.Status);
	}
      allDone.Set();
    }

    static void ReadCallBack(IAsyncResult asyncResult)
    {
      try
	{
	  RequestState requestState = (RequestState) asyncResult.AsyncState;
	  Stream responseStream = requestState.StreamResponse;
	  int read = responseStream.EndRead(asyncResult);
	  // Read the HTML page and then print it to the console.
	  if (read > 0)
	    {
	      requestState.RequestData.Append
		(Encoding.ASCII.GetString(requestState.BufferRead, 0, read));
	      IAsyncResult asynchronousResult = 
		responseStream.BeginRead(requestState.BufferRead, 0, 
					 BUFFER_SIZE, 
					 new AsyncCallback(ReadCallBack), 
					 requestState);
	      return;
	    }
	  else
	    {
	      Console.WriteLine("\nThe contents of the Html page are : ");
	      if (requestState.RequestData.Length>1)
		{
		  string stringContent = requestState.RequestData.ToString();
		  Console.WriteLine(stringContent);
		  
		  // DumpVersion(stringContent);
		}
	      responseStream.Close();
	    }
	}
      catch(WebException e)
	{
	  Console.WriteLine("\nReadCallBack Exception raised!");
	  Console.WriteLine("\nMessage:{0}",e.Message);
	  Console.WriteLine("\nStatus:{0}",e.Status);
	}
      allDone.Set();
    }
  }
}
