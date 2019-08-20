using System.Collections.Generic;
using UnityEngine;

public class MvvmPrefab : ScriptableObject
{
    [SerializeField] List<MvvmGameObject> _treeElements = new List<MvvmGameObject> ();

    internal List<MvvmGameObject> treeElements
    {
        get { return _treeElements; }
        set { _treeElements = value; }
    }

//    void Awake ()
//    {
//        if (_treeElements.Count == 0)
//        {
//            
//        }
//    }
}
