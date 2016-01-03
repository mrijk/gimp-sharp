// The UpdateCheck plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Renderer.cs
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

namespace Gimp.UpdateCheck
{
  public class Renderer : BaseRenderer
  {
    static ManualResetEvent allDone= new ManualResetEvent(false);
    const int BUFFER_SIZE = 1024;
    const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render()
    {
      bool enableProxy = GetValue<bool>("enable_proxy");
      string httpProxy = GetValue<string>("http_proxy");
      string port = GetValue<string>("port");

      if (enableProxy)
	{
	  Gimp.RcSet("update-enable-proxy", (enableProxy) ? "true" : "false");
	  Gimp.RcSet("update-http-proxy", httpProxy);
	  Gimp.RcSet("update-port", port);
	}

      var assembly = Assembly.GetAssembly(typeof(Plugin));
      Console.WriteLine(assembly.GetName().Version);

      var doc = new XmlDocument();

      try 
	{
	  var myRequest = (HttpWebRequest) 
	    WebRequest.Create("http://gimp-sharp.sourceforge.net/version.xml");

	  // Create a proxy object, needed for mono behind a firewall?!
	  if (enableProxy)
	    {
	      var myProxy = new WebProxy()
		{Address = new Uri(httpProxy + ":" + port)};
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

    static void TimeoutCallback(object state, bool timedOut) 
    { 
      if (timedOut) 
	{
	  var request = state as HttpWebRequest;
	  request?.Abort();
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
	  var requestState = (RequestState) asyncResult.AsyncState;
	  var responseStream = requestState.StreamResponse;

	  int read = responseStream.EndRead(asyncResult);
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
	      if (requestState.RequestData.Length > 1)
		{
		  string stringContent = requestState.RequestData.ToString();
		  Console.WriteLine(stringContent);
		}
	      responseStream.Close();
	    }
	}
      catch (WebException e)
	{
	  Console.WriteLine("\nReadCallBack Exception raised!");
	  Console.WriteLine("\nMessage:{0}",e.Message);
	  Console.WriteLine("\nStatus:{0}",e.Status);
	}
      allDone.Set();
    }
  }
}
