using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gimp
  {
    public class PersistentStorage
    {
      Plugin _plugin;
      string _name;

      public PersistentStorage(Plugin plugin)
      {
	_plugin = plugin;
	_name = plugin.Name;
      }

      BinaryFormatter _formatter = new BinaryFormatter();

      public void SetData()
      {
	MemoryStream memoryStream = new MemoryStream();
	Type type = _plugin.GetType();

	foreach (FieldInfo field 
		 in type.GetFields(BindingFlags.Instance |  
				   BindingFlags.NonPublic | 
				   BindingFlags.Public))
	  {
	  foreach (object attribute in field.GetCustomAttributes(true))
	    {
	    if (attribute is SaveAttribute)
	      {
	      _formatter.Serialize(memoryStream, field.GetValue(_plugin));
	      }
	    }
	  }
	gimp_procedural_db_set_data(_name, memoryStream.GetBuffer(),
				    (int) memoryStream.Length);		    
      }


      public void GetData()
      {
	int size = gimp_procedural_db_get_data_size(_name);
	if (size > 0)
	  {
	  byte[] data = new byte[size];
	  gimp_procedural_db_get_data(_name, data);

	  MemoryStream memoryStream = new MemoryStream(data);
	  Type type = _plugin.GetType();

	  foreach (FieldInfo field 
		   in type.GetFields(BindingFlags.Instance |  
				     BindingFlags.NonPublic | 
				     BindingFlags.Public))
	    {
	    foreach (object attribute in field.GetCustomAttributes(true))
	      {
	      if (attribute is SaveAttribute)
		{
		field.SetValue(_plugin, _formatter.Deserialize(memoryStream));
		}
	      }
	    }
	  }
      }

      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_procedural_db_set_data(string identifier, 
							    byte[] data,
							    int bytes);
      [DllImport("libgimp-2.0.so")]
      public static extern bool gimp_procedural_db_get_data(string identifier, 
							    byte[] data);
      [DllImport("libgimp-2.0.so")]
      public static extern int gimp_procedural_db_get_data_size(string identifier);
    }
  }
