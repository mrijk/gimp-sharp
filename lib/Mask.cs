using System;

namespace Gimp
  {
    public class Mask
    {
      Int32 _maskID;

      public Mask(Int32 maskID)
      {
	_maskID = maskID;
      }

      public Int32 ID
      {
	get {return _maskID;}
      }
    }
  }
