using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Gimp.PicturePackage
{
  public delegate void SelectHandler(Layout layout);

  public class LayoutSet : IEnumerable
  {
    ArrayList _set = new ArrayList();
    Layout _selected = null;

    public LayoutSet()
    {
    }

    public void Load()
    {
      XmlDocument doc = new XmlDocument();

      Assembly myAssembly = Assembly.GetExecutingAssembly();
#if false
      String[] names = myAssembly.GetManifestResourceNames();
      Console.WriteLine(names[0]);
      Stream myStream = 
      myAssembly.GetManifestResourceStream("PicturePackage.picture-package.xml");
#else
      Stream myStream = 
      	myAssembly.GetManifestResourceStream("picture-package.xml");
#endif
      doc.Load(myStream);
      LoadXmlDocument(doc);

      // Next read from gimpdir
      try
	{
	string filename = Gimp.Directory() + "/" + "picture-package.xml";
	doc.Load(filename);
	LoadXmlDocument(doc);
	}
      catch (Exception e)
	{
	Console.WriteLine("Nothing to read! " + e.Message);
	}
    }
    
    void LoadXmlDocument(XmlDocument doc)
    {
      XmlNodeList nodeList;
      XmlElement root = doc.DocumentElement;

      nodeList = root.SelectNodes("/picture-package/layout");

      foreach (XmlNode layout in nodeList)
	{
	XmlAttributeCollection attributes = layout.Attributes;
	XmlAttribute name = (XmlAttribute) attributes.GetNamedItem("name");
	Add(new Layout(layout));
	}
    }

    public void Add(Layout layout)
    {
      _set.Add(layout);
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    public Layout this[int index]
    {
      get {return (Layout) _set[index];}
    }

    public event SelectHandler SelectEvent;

    public Layout Selected
    {
      set
	  {
	  _selected = value;
	  if (SelectEvent != null)
	    {
	    SelectEvent(value);
	    }
	  }

      get {return _selected;}
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

    public LayoutSet GetLayouts(PageSize pageSize, int resolution)
    {
      LayoutSet set = new LayoutSet();

      foreach (Layout layout in _set)
	{
	if (layout.GetPageSize(resolution).CompareTo(pageSize) == 0)
	  {
	  set.Add(layout);
	  }
	}
      return set;
    }
  }
  }
