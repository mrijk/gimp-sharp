using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Gimp.PicturePackage
{
  public class LayoutSet : IEnumerable
  {
    ArrayList _set = new ArrayList();
    
    public LayoutSet()
    {
    }

    public void Load()
    {
      XmlDocument doc = new XmlDocument();

      Assembly myAssembly = Assembly.GetExecutingAssembly();
      String[] names = myAssembly.GetManifestResourceNames();
      // Console.WriteLine(names[0]);
      // Stream myStream = 
      // myAssembly.GetManifestResourceStream("PicturePackage.picture-package.xml");
      Stream myStream = 
      	myAssembly.GetManifestResourceStream("picture-package.xml");

      doc.Load(myStream);

      XmlNodeList nodeList;
      XmlElement root = doc.DocumentElement;

      nodeList = root.SelectNodes("/picture-package/layout");

      foreach (XmlNode layout in nodeList)
	{
	XmlAttributeCollection attributes = layout.Attributes;
	XmlAttribute name = (XmlAttribute) attributes.GetNamedItem("name");
	_set.Add(new Layout(layout));
	}
      
    }
    
    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    public Layout this[int index]
    {
      get {return (Layout) _set[index];}
    }

    public PageSizeSet GetPageSizeSet(int resolution)
    {
      PageSizeSet set = new PageSizeSet();

      foreach (Layout layout in _set)
	{
	set.Add(layout.GetPageSize(resolution));
	}

      return set;
    }
  }
  }
