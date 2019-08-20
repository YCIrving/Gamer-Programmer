using System;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
internal class MvvmGameObject : TreeElement
{
    public bool enabled;
    public string text;
    public Material material;
    public string tag;
    public float floatValue1;
    
    public MvvmGameObject (string name, int depth, int id) : base (name, depth, id)
    {
        enabled = true;
        text = "";
        tag = "Untagged";
  
        floatValue1 = Random.value;

    }
    
    public MvvmGameObject (GameObject root, int depth, int id) : base (root.name, depth, id)
    {
        // read from prefab
        enabled = root.activeSelf;
        text = root.name;
        tag = root.tag;
        floatValue1 = Random.value;

    }
}
