using System;
using System.IO;

namespace Gimp.SliceTool
{
	public class Property
	{
		string _name;
		string _value;
		bool _changed = false;

		public Property(string name)
		{
			_name = name;
			_value = "";
		}

		public void WriteHTML(StreamWriter w)
		{
			if (_value.Length > 0)
			{
				w.Write(" {0}=\"{1}\"", _name, _value);
			}
		}

		public string Name
		{
			get {return _name;}
		}

		public string Value
		{
			get {return _value;}
			set 
			{
				if (value != _value)
				{
					_value = value;
					_changed = true;
				}
			}
		}

		public bool Changed
		{
			get {return _changed;}
			set {_changed = value;}
		}
	}
}
