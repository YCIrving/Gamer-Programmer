using System;
using System.Linq;
using Boo.Lang;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class XCustomAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    {
        Debug.Log("Moving assets from Source path: " + sourcePath + "to Destination path: " + destinationPath + ".");
        AssetMoveResult assetMoveResult = AssetMoveResult.DidNotMove;
            
        if (sourcePath.EndsWith(".prefab") && XGlobalFlag.xGlobalFlag)
        {
            var guid = AssetDatabase.AssetPathToGUID(sourcePath);
//            try
//            {
//                xlock.LockFile(Guid, sourcePath);
//            }
//            catch (Exception e)f
//            {
//                assetMoveResult = AssetMoveResult.FailedMove;
//                EditorUtility.DisplayDialog("Move failed!", "You cannot lock the prefab: " + sourcePath + ".", "Ok");
//                return assetMoveResult;
//            }

            try
            {
                XLock.UnlockFile(guid, sourcePath);
            }
            catch (Exception e)
            {
                assetMoveResult = AssetMoveResult.FailedMove;
                EditorUtility.DisplayDialog("Move failed!", "You cannot unlock the prefab: " + sourcePath + ".", "Ok");
                return assetMoveResult;
            }

            try
            {
                guid = AssetDatabase.AssetPathToGUID(destinationPath);
                XLock.LockFile(guid, destinationPath);

            }
            catch (Exception e)
            {
                assetMoveResult = AssetMoveResult.FailedMove;
                EditorUtility.DisplayDialog("Move failed!", "You cannot lock the prefab: " + destinationPath + ".", "Ok");
                return assetMoveResult;
            }

        }
        return assetMoveResult;
    }

    private static AssetDeleteResult OnWillDeleteAsset(string prefabPath, RemoveAssetOptions opt)
    {
        Debug.Log("Deleting assets: " + prefabPath + ".");
        AssetDeleteResult assetDeleteResult = AssetDeleteResult.DidNotDelete;
        
        if (prefabPath.EndsWith(".prefab") && XGlobalFlag.xGlobalFlag)
        {
            var guid = AssetDatabase.AssetPathToGUID(prefabPath);
            try
            {
                XLock.UnlockFile(guid, prefabPath);
            }
            catch (Exception e)
            {
                assetDeleteResult = AssetDeleteResult.FailedDelete;
                EditorUtility.DisplayDialog("Deletion failed!", "You cannot unlock the prefab: " + prefabPath + ".", "Ok");
            }
        }
        
        return assetDeleteResult;
    }

    private static string[] OnWillSaveAssets(string[] paths)
    {
        Debug.Log("Saving assets...");
        var pathsToSave = new List<string>();
        foreach (string path in paths)
        {
            pathsToSave.Add(path);
            if (path.EndsWith(".prefab") && XGlobalFlag.xGlobalFlag )
            {
                var guid = AssetDatabase.AssetPathToGUID(path);
                try
                {
                    XLock.LockFile(guid, path);
                }
                catch (Exception e)
                {
                    pathsToSave.Remove(path);
                    EditorUtility.DisplayDialog("Deletion failed!", "You cannot lock the prefab: " + path + ".", "Ok");
                }
            }
        }
        return pathsToSave.ToArray();
    }
}