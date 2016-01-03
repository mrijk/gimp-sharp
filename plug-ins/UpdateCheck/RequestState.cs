// The UpdateCheck plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// RequestState.cs
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

using System.IO;
using System.Net;
using System.Text;

public class RequestState
{
  // This class stores the State of the request.
  const int BUFFER_SIZE = 1024;
  readonly StringBuilder _requestData = new StringBuilder("");
  readonly byte[] _bufferRead = new byte[BUFFER_SIZE];
  public HttpWebRequest Request {get;}
  public HttpWebResponse Response {get; set;}
  public Stream StreamResponse {get; set;}

  public RequestState(HttpWebRequest request)
  {
    Request = request;
    StreamResponse = null;
  }
  
  public StringBuilder RequestData => _requestData;

  public byte[] BufferRead => _bufferRead;
}
