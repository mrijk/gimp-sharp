using System;

namespace Gimp.Ministeck
{
  public class MinisteckPalette
  {
    Palette _palette;

    public MinisteckPalette()
    {
      _palette = new Palette("Ministeck");
#if true
      _palette.AddEntry("first", new RGB(253, 254, 253));
      _palette.AddEntry("", new RGB(206, 153,  50));
      _palette.AddEntry("", new RGB(155, 101,  52));
      _palette.AddEntry("", new RGB( 50,  50,  50));
      _palette.AddEntry("", new RGB(  4,   3,  98));
      _palette.AddEntry("", new RGB(  2, 102,  54));
      
      _palette.AddEntry("", new RGB(  2,  50, 154));
      _palette.AddEntry("", new RGB(254,  50, 102));
      _palette.AddEntry("", new RGB(206, 154, 102));
      _palette.AddEntry("", new RGB(254, 254,  50));
      _palette.AddEntry("", new RGB(250,  90,   6));
      _palette.AddEntry("", new RGB( 55, 101,  53));
      
      _palette.AddEntry("", new RGB(103, 102, 101));
      _palette.AddEntry("", new RGB(206,   2,  50));
      _palette.AddEntry("", new RGB(254, 154,  54));
      _palette.AddEntry("", new RGB(102,  50,  50));
      _palette.AddEntry("", new RGB(253, 154, 154));
      _palette.AddEntry("", new RGB( 50, 102, 206));
      
      _palette.AddEntry("", new RGB(  3,  50,  56));
      _palette.AddEntry("", new RGB( 50,   2, 102));
      _palette.AddEntry("", new RGB(251, 155, 101));
      _palette.AddEntry("", new RGB(254, 254, 202));
      _palette.AddEntry("", new RGB(  4,   2,   2));
      _palette.AddEntry("last", new RGB(206, 206, 206));
#else
      _palette.AddEntry("black"     , new RGB(  7,   7,   7));
      _palette.AddEntry("dark blue" , new RGB( 64,  10, 121));
      _palette.AddEntry("light blue", new RGB( 59, 138, 207));
      _palette.AddEntry("light blue", new RGB( 59, 138, 207));
#endif 
    }

    public void Delete()
    {
      	_palette.Delete();
    }
  }
  }
