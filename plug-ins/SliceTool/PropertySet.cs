using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Gimp.SliceTool
{
	public class PropertySet
	{
		Hashtable _set = new Hashtable();

		public PropertySet()
		{
			Add("URL");
			Add("AltText");
			Add("Target");
			Add("MouseOver");
			Add("MouseOut");
			Add("MouseClick");
			Add("MouseDoubleClick");
			Add("MouseUp");
			Add("MouseDown");
		}

		void Add(string name)
		{
			Property property = new Property(name);
			_set.Add(name, property);
		}

		Property GetProperty(string name)
		{
			Property property = (Property) _set[name];
			Debug.Assert(property != null, "Property not in hashset!");
			return property;
		}

		public void Set(string name, string value)
		{
			GetProperty(name).Value = value;
		}

		public string Get(string name)
		{
			return GetProperty(name).Value;
		}

		public bool Exists(string name)
		{
			return Get(name).Length > 0;
		}

		public void WriteHTML(StreamWriter w, string name)
		{
			GetProperty(name).WriteHTML(w);
		}

		public bool Changed
		{
			get
			{
				foreach (Property property in _set)
				{
					if (property.Changed)
					{
						return true;
					}
				}
				return false;
			}

			set
			{
				foreach (Property property in _set)
				{
					property.Changed = value;
				}
			}
		}
	}
}
