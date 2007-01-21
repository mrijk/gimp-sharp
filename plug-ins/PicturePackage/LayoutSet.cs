// The PicturePackage plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// LayoutSet.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Gimp.PicturePackage
{
  public delegate void SelectHandler(Layout layout);

  public class LayoutSet
  {
    List<Layout> _set = new List<Layout>();
    Layout _selected = null;

    public void Load()
    {
      XmlDocument doc = new XmlDocument();

      Assembly myAssembly = Assembly.GetExecutingAssembly();
      Stream myStream = 
      	myAssembly.GetManifestResourceStream("picture-package.xml");
      doc.Load(myStream);
      LoadXmlDocument(doc);

      // Next read from gimpdir
      try
	{
	  doc.Load(Gimp.Directory + System.IO.Path.DirectorySeparatorChar +
		   "picture-package.xml");
	  LoadXmlDocument(doc);
	}
      catch (Exception)
	{
	  // No user defined picture package file
	  // Console.WriteLine("Nothing to read! " + e.Message);
	}
    }
    
    void LoadXmlDocument(XmlDocument doc)
    {
      XmlNodeList nodeList;
      XmlElement root = doc.DocumentElement;

      nodeList = root.SelectNodes("/picture-package/layout");

      foreach (XmlNode layout in nodeList)
	{
	  Add(new Layout(layout));
	}
    }

    public void Add(Layout layout)
    {
      _set.Add(layout);
    }

    public IEnumerator<Layout> GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    public Layout this[int index]
    {
      get {return _set[index];}
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
