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

      static MessageHandlerType HandlerType
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
