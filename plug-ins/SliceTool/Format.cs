using System;
using Gtk;

namespace Gimp.SliceTool
{
  public class Format : GimpFrame
  {
    OptionMenu _format;

    public Format() : base("Format")
    {
      _format = new OptionMenu();
      Add(_format);

      Menu menu = new Menu();
      menu.Append(new MenuItem("gif"));
      menu.Append(new MenuItem("jpg"));
      menu.Append(new MenuItem("png"));
      _format.Menu = menu;
    }

    public string Exension
    {
      set
	  {
	  if (value == "gif")
	    {
	    _format.SetHistory(0);
	    }
	  else if (value == "jpg" || value == "jpeg")
	    {
	    _format.SetHistory(1);
	    }
	  else if (value == "png")
	    {
	    _format.SetHistory(2);
	    }
	  else
	    {
	    _format.SetHistory(2);
	    }
	  }

      get
	  {
	  switch (_format.History)
	    {
	    case 0:
	      return "gif";
	    case 1:
	      return "jpg";
	    case 2:
	      return "png";
	    default:
	      return null;
	    }
	  }
    }
  }
  }
