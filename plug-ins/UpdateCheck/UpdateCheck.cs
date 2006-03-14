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
using System.Text;
using System.Threading;
using System.Xml;

using Gtk;

namespace Gimp.UpdateCheck
{
  public class UpdateCheck : Plugin
  {
    static ManualResetEvent allDone= new ManualResetEvent(false);
    const int BUFFER_SIZE = 1024;
    const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

    bool _checkGimp = true;
    bool _checkGimpSharp = true;
    bool _checkUnstable = false;

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

      GimpTable table = new GimpTable(4, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, true, true, 0);

      CheckButton checkGimp = new CheckButton("Check _GIMP");
      checkGimp.Active = _checkGimp;
      checkGimp.Toggled += delegate(object sender, EventArgs args) {
	_checkGimp = checkGimp.Active;
      };
      table.Attach(checkGimp, 0, 1, 0, 1);

      CheckButton checkGimpSharp = new CheckButton("Check G_IMP#");
      checkGimpSharp.Active = _checkGimpSharp;
      checkGimpSharp.Toggled += delegate(object sender, EventArgs args) {
	_checkGimpSharp = checkGimpSharp.Active;
      };
      table.Attach(checkGimpSharp, 0, 1, 1, 2);

      CheckButton checkUnstable = new CheckButton("Check _Unstable Releases");
      checkUnstable.Active = _checkUnstable;
      checkUnstable.Toggled += delegate(object sender, EventArgs args) {
	_checkUnstable = checkUnstable.Active;
      };
      table.Attach(checkUnstable, 0, 1, 2, 3);

      dialog.ShowAll();
      return DialogRun();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      XmlDocument doc = new XmlDocument();

      try {
	HttpWebRequest myRequest = (HttpWebRequest) 
	  WebRequest.Create("http://gimp-sharp.sourceforge.net/version.xml");

	// Create a proxy object, needed for mono behind a firewall?!
	// WebProxy myProxy = new WebProxy();
	// myProxy.Address = new Uri("myProxy");
	// myRequest.Proxy=myProxy;
      
	RequestState requestState = new RequestState(myRequest);

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
      } catch (Exception e) 
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
	  HttpWebRequest request = state as HttpWebRequest;
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
	  RequestState requestState = 
	    (RequestState) asynchronousResult.AsyncState;
	  HttpWebRequest myHttpWebRequest = requestState.Request;
	  requestState.Response = (HttpWebResponse) 
	    myHttpWebRequest.EndGetResponse(asynchronousResult);
	  
	  // Read the response into a Stream object.
	  Stream responseStream = requestState.Response.GetResponseStream();
	  requestState.StreamResponse = responseStream;
	  
	  IAsyncResult asynchronousInputRead = 
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
