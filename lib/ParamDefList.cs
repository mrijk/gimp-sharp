using System;
using System.Collections;

namespace Gimp
  {
    public class ParamDefList
    {
      ArrayList _set;

      public ParamDefList(params ParamDef[] list)
      {
	_set = new ArrayList(list);
      }
    }
  }
