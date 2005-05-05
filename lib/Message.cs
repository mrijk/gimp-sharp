// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
//
// Message.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Message
    {
    public Message(string message)
      {
      if (!gimp_message(message))
        {
        throw new Exception();
        }
      }

    static MessageHandlerType Handler
      {
      get {return gimp_message_get_handler();}
      set {gimp_message_set_handler(value);}
      }

    [DllImport("libgimpwidgets-2.0.so")]
    static extern bool gimp_message (string message);
    [DllImport("libgimpwidgets-2.0.so")]
    static extern MessageHandlerType gimp_message_get_handler();
    [DllImport("libgimpwidgets-2.0.so")]
    static extern void gimp_message_set_handler(MessageHandlerType handler);
    }
  }
