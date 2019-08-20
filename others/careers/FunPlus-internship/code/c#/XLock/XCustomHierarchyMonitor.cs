using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoadAttribute]
public static class XCustomHierarchyMonitor
{
    // 尝试监听将prefab拖动到Hierarchy中的操作，但是目前这样还不行，所有修改prefab的操作都会被相应
//    static XCustomHierarchyMonitor()
//    {
//        EditorApplication.hierarchyChanged += OnHierarchyChanged;
//    }
//
//    static void OnHierarchyChanged()
//    {
//        if (Selection.activeGameObject != null &&
//            PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.PrefabInstance)
//            
//            Debug.Log(Selection.activeGameObject);
//    }
}