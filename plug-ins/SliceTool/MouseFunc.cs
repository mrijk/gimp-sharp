using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class MouseFunc
  {
    protected Preview _preview;
    bool _useRelease, _useMove;

    public MouseFunc(Preview preview, bool useRelease, bool useMove)
    {
      _preview = preview;
      _useRelease = useRelease;
      _useMove = useMove;
    }

    protected void Redraw()
    {
      _preview.QueueDraw();
    }

    virtual protected void OnPress(int x, int y) {}
    virtual protected void OnRelease() {}
    virtual protected void OnMove(int x, int y) {}

    public void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      int x = (int) args.Event.X;
      int y = (int) args.Event.Y;

      if (_useRelease)
	{
	AddReleaseEvent();
	}

      if (_useMove)
	{
	AddMotionNotifyEvent();
	}
      OnPress(x, y);
    }

    protected void AddReleaseEvent()
    {
      _preview.ButtonReleaseEvent += 
	new ButtonReleaseEventHandler(OnButtonRelease);
    }

    protected void AddMotionNotifyEvent()
    {
      _preview.MotionNotifyEvent += 
	new MotionNotifyEventHandler(OnMotionNotify);
    }

    void OnButtonRelease(object o, ButtonReleaseEventArgs args)
    {
      _preview.MotionNotifyEvent -= 
	new MotionNotifyEventHandler(OnMotionNotify);
      _preview.ButtonReleaseEvent -= 
	new ButtonReleaseEventHandler(OnButtonRelease);
      OnRelease();
    }

    void OnMotionNotify(object o, MotionNotifyEventArgs args)
    {
      int x, y;
      EventMotion ev = args.Event;
      
      if (ev.IsHint) 
	{
	ModifierType s;
	ev.Window.GetPointer (out x, out y, out s);
	} 
      else 
	{
	x = (int) ev.X;
	y = (int) ev.Y;
	}

      OnMove(x, y);
    }
  }
  }
