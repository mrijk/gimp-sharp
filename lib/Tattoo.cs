using System;

namespace Gimp
  {
    public class Tattoo
    {
      int _tattooID;

      public Tattoo(int tattooID)
      {
	_tattooID = tattooID;
      }

      public int ID
      {
	get {return _tattooID;}
      }
    }
  }
