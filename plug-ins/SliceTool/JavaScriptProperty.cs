using System;
using System.Collections;

namespace Gimp.SliceTool
  {
  public class JavaScriptProperty : Property
    {
    static Hashtable _preload = new Hashtable();

    public JavaScriptProperty(string name) : base(name)
      {
      }

    public override string Value
      {
      set 
          {
          if (value != base.Value)
            {
            if (_preload.ContainsKey(base.Value))
              {
              int refCount = (int) _preload[base.Value];
              refCount--;
              if (refCount == 0)
                {
                _preload.Remove(base.Value);
                }
              else
                {
                _preload[base.Value] = refCount;
                }
              }

            if (_preload.ContainsKey(value))
              {
              int refCount = (int) _preload[value];
              refCount++;
              _preload[value] = refCount;
              }
            else if (value.Length > 0)
              {
              _preload.Add(value, 1);
              }

            base.Value = value;
            }
          }
      }

    public static string[] Preload
      {
      get 
          {
          string[] array = new string[_preload.Count];
          _preload.Keys.CopyTo(array, 0);
          return array;
          }
      }
    }
  }
