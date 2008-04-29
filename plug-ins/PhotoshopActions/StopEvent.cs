// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// StopEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class StopEvent : ActionEvent
  {
    [Parameter("Msge")]
    string _message;
    [Parameter("Cntn")]
    bool _continue;

    override public bool Execute()
    {
      MessageDialog message = 
	new MessageDialog(null, DialogFlags.DestroyWithParent,
			  MessageType.Info, ButtonsType.None, _message);
      if (_continue)
	{
	  message.AddButton("Continue", ResponseType.Ok);
	}
      message.AddButton("Stop", ResponseType.Cancel);

      ResponseType response = (ResponseType) message.Run();
      message.Destroy();

      return response == ResponseType.Ok;
    }
  }
}
