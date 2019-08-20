using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreeElement
{
    [SerializeField] int _id;
    [SerializeField] string _name;
    [SerializeField] int _depth;
    [NonSerialized] TreeElement _parent;
    [NonSerialized] List<TreeElement> _children;

    public int depth
    {
        get { return _depth; }
        set { _depth = value; }
    }

    public TreeElement parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public List<TreeElement> children
    {
        get { return _children; }
        set { _children = value; }
    }

    public bool hasChildren
    {
        get { return children != null && children.Count > 0; }
    }

    public string name
    {
        get { return _name; } set { _name = value; }
    }

    public int id
    {
        get { return _id; } set { _id = value; }
    }

    public TreeElement ()
    {
    }

    public TreeElement (string name, int depth, int id)
    {
        _name = name;
        _id = id;
        _depth = depth;
    }
}


