
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Net;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = System.Object;


[InitializeOnLoad]
class XLock
{
    private static UnityWebRequest _request;
    private static string _baseUrl = "http://10.0.1.237:8899/cmd";
    private static bool _goBackToPreviousStage = false;
    private static string _responseErr;
    // private static string _responseResult;
    private static string _responseString;
    private static string[] _responseStringArray;
    private static FileLock[] _responseFileLocks;
    private static LockLog[] _responseLogs;
    
    
    static XLock()
    {
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (_goBackToPreviousStage)
        {
            StageUtility.GoBackToPreviousStage();
            _goBackToPreviousStage = false;
        }

        if (_request == null)
            return;
  
        if (_request.isDone)
        {
//                _callback?.Invoke();
//                _callback = null;
                _request = null;
        }
    }
    
    static void PostRequest(string method, string body)
    {
        if (_request != null) return;

        var url = _baseUrl + "/" + method;
        _request = new UnityWebRequest(url, "POST");

        if (method.Equals("listAll"))
        {
            _request.downloadHandler = new DownloadHandlerBuffer();
            _request.SendWebRequest();
        }

        else
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            _request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            _request.downloadHandler = new DownloadHandlerBuffer();
            // _request.SetRequestHeader("Content-Type", "application/json");
            _request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            _request.SendWebRequest();
        }

        while (!_request.isDone)
        {
            Thread.Sleep(1);
        }
    }

    static void CheckUnityUserName()
    {
        var unityUserName = PlayerPrefs.GetString("UnityUserName");
        if (string.IsNullOrEmpty(unityUserName))
        {
            EditorUtility.DisplayDialog("Please set your user name first.", "Please set your user name first.", "OK");
            XSetUserNameWindow.ShowWindow();
            throw new Exception("Please set your user name first.");
        }
    }
    
    // P1: LockFile
    public static void LockFile(string guid, string prefabPath)
    {
//        PlayerPrefs.DeleteKey("UnityUserName");
//        PlayerPrefs.Save();
//        xSetUserNameWindow.init();
        
        // S1: checking unity user name
        try
        {
            CheckUnityUserName();

        }
        catch (Exception e)
        {
            throw e;
        }
        
        var unityUserName = PlayerPrefs.GetString("UnityUserName");
        
        // S2: posting http request
        // string json = "{ \"UserName\": \"" + unityUserName + "\", \"GUID\": \"" + guid + "\", \"Note\": \"" + prefabPath +"\"}";
        var requestBody = "args= -userName " + "\"" + unityUserName  + "\"" + " -note " + "\"" + prefabPath + "\"" + " -lockName " + guid;
        PostRequest("lock", requestBody);
        
        
        Debug.Log("Lock function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        // S3: analysing received json
        XAnalyseJsonBasic jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonBasic>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseString = jsonAnalysed.result;
//        _callback = () =>
//        {
//            code = _request.responseCode;
//            result = _request.downloadHandler.text;
//        };

        // S4: Acting based on analysed json
        if (!string.IsNullOrEmpty(_responseErr))
        {
            EditorUtility.DisplayDialog("Lock file failed! " , _responseErr, "OK");
            throw new Exception("Lock file failed! " + _responseErr);
        }
        else
        {
            // EditorUtility.DisplayDialog("Lock file success!", "", "OK");
        }
    }
    static void OnPrefabStageOpened(PrefabStage prefabStage)
    {      
        if (!XGlobalFlag.xGlobalFlag) return;
        // lock prefab
        var prefabPath = prefabStage.prefabAssetPath;
        var guid = AssetDatabase.AssetPathToGUID(prefabPath);
        try
        {
            LockFile(guid, prefabPath);
        }
        catch (Exception e)
        {
            _goBackToPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    [MenuItem("Assets/XLock/Lock", true)]
    static bool ValidateRightMouseLock()
    {
        return Selection.activeGameObject != null && XGlobalFlag.xGlobalFlag;
    }
    [MenuItem("Assets/XLock/Lock")]
    static void RightMouseLock()
    {
        var prefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!prefabPath.EndsWith(".prefab")) return;
        var guid = AssetDatabase.AssetPathToGUID(prefabPath);
        try
        {
            LockFile(guid, prefabPath);
        }
        catch (Exception e)
        {
            _goBackToPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
        
    }
    
    // P2: Unlock file
    public static void UnlockFile(string guid, string prefabPath)
    {
        try
        {
            CheckUnityUserName();

        }
        catch (Exception e)
        {
            throw e;
        }
        
        var unityUserName = PlayerPrefs.GetString("UnityUserName");
        var prefabNameTemp = prefabPath.Split('/');
        var prefabName = prefabNameTemp[prefabNameTemp.Length - 1];
        
        var requestBody = "args= -userName " + "\"" + unityUserName  + "\"" + " -lockName " + guid;
        // string json = "{ \"UserName\": \"" + unityUserName + "\", \"GUID\": \"" + guid + "\", \"Note\": \"" + prefabPath +"\"}";
        
        PostRequest("unlock", requestBody);

        XAnalyseJsonBasic jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonBasic>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseString = jsonAnalysed.result;

        Debug.Log("Unlock function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("Unlock file failed! " ,  _responseErr, "OK");
            throw new Exception("Unlock file failed! " + _responseErr);
        }
        else
        {
            EditorUtility.DisplayDialog("Unlock success!", "Unlock the " + prefabName + " success!", "OK");
        }
    }
    
    [MenuItem("Assets/XLock/Unlock", true)]
    static bool ValidateRightMouseUnlock()
    {
        return Selection.activeGameObject != null && XGlobalFlag.xGlobalFlag;
    }
    [MenuItem("Assets/XLock/Unlock")]
    static void RightMouseUnlock()
    {
        var prefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!prefabPath.EndsWith(".prefab")) return;
        var guid = AssetDatabase.AssetPathToGUID(prefabPath);
        try
        {
            UnlockFile(guid, prefabPath);
        }
        catch (Exception e)
        {
            // _gotoPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    
    // P3: Steal lock
    static void StealLock(string guid, string prefabPath)
    {
        var prefabNameTemp = prefabPath.Split('/');
        var prefabName = prefabNameTemp[prefabNameTemp.Length - 1];
        if (!EditorUtility.DisplayDialog("", 
        "Are you sure to steal the lock of " + prefabName + "?", "OK","Cancel")) return;
        
        try
        {
            CheckUnityUserName();
        }
        catch (Exception e)
        {
            throw e;
        }
        
        var unityUserName = PlayerPrefs.GetString("UnityUserName");
        
        // string json = "{ \"UserName\": \"" + unityUserName + "\", \"GUID\": \"" + guid + "\", \"Note\": \"" + prefabPath +"\"}";
        string requestBody = "args= -userName " + "\"" + unityUserName  + "\"" + " -note " + "\"" + prefabPath + "\"" + " -lockName " + guid;
        
        // string requestBody = "args= -userName " + unityUserName + " -note " + prefabPath + " -lockName " + guid;
        
        PostRequest("stealLock", requestBody);

        XAnalyseJsonBasic jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonBasic>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseString = jsonAnalysed.result;

        Debug.Log("StealLock function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr))
        {
            EditorUtility.DisplayDialog("Steal lock failed! " , _responseErr, "OK");
            throw new Exception("Steal lock failed! " + _responseErr);
        }
        else
        {
            
            EditorUtility.DisplayDialog("Steal success!", "Steal the lock of " + prefabName + " success!", "OK");
        }
    }
    
    [MenuItem("Assets/XLock/Steal Lock", true)]
    static bool ValidateRightMouseStealLock()
    {
        return Selection.activeGameObject != null && XGlobalFlag.xGlobalFlag;
    }
    [MenuItem("Assets/XLock/Steal Lock")]
    static void RightMouseStealLock()
    {
        var prefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!prefabPath.EndsWith(".prefab")) return;
        var guid = AssetDatabase.AssetPathToGUID(prefabPath);
        try
        {
            StealLock(guid, prefabPath);
        }
        catch (Exception e)
        {
            _goBackToPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    
    // P4: InquireLockByGUID
    static void InquireLockByGUID(string guid, string prefabPath)
    {
        // string json = "{ \"GUID\": \"" + guid + "\"}";
        string requestBody = "args= -lockName " + guid;
        PostRequest("findLockOwner", requestBody);

        XAnalyseJsonWithString jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonWithString>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseString = jsonAnalysed.result;
        // _responseString = jsonAnalysed.data;

        Debug.Log("InquireLockByGUID function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("Inquire lock by GUID failed! ", _responseErr , "OK");
            throw new Exception("Inquire lock by GUID failed! " + _responseErr);
        }
        else
        {
            if (string.IsNullOrEmpty(_responseString))
            {
                EditorUtility.DisplayDialog(prefabPath + " is free! " , "", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("The lock of " + prefabPath + " is held by: " , _responseString, "OK"); 
            } 
        }
    }
    [MenuItem("Assets/XLock/Inquire lock of this file", true)]
    static bool ValidateRightMouseInquireLockByGUID()
    {
        return Selection.activeGameObject != null && XGlobalFlag.xGlobalFlag;
    }
    [MenuItem("Assets/XLock/Inquire lock of this file")]
    static void RightMouseInquireLockByGUID()
    {
        var prefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!prefabPath.EndsWith(".prefab")) return;
        var guid = AssetDatabase.AssetPathToGUID(prefabPath);
        
        try
        {
            InquireLockByGUID(guid, prefabPath);
        }
        catch (Exception e)
        {
            // _gotoPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    
    
    // P5: InquireLockByUserName
    public static void InquireLockByUserName(string unityUserName)
    {      
        // string json = "{ \"UserName\": \"" + unityUserName + "\"}";
        string requestBody = "args= -userName " + "\"" + unityUserName + "\"";
        
        PostRequest("listOwnLock", requestBody);

        XAnalyseJsonWithLocks jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonWithLocks>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseFileLocks = jsonAnalysed.result;
        // _responseFileLocks = jsonAnalysed.data;

        Debug.Log("InquireLockByUserName function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("Inquire lock by user name failed! " , _responseErr, "OK");
            throw new Exception("Inquire lock by user name failed! " + _responseErr);
        }
        else
        {
            string messageToShow = "";

            for (int i = 0; i < _responseFileLocks.Length; i++)
            {
                var l = _responseFileLocks[i];
                //messageToShow += (i+1).ToString() + ". " + AssetDatabase.GUIDToAssetPath(l.lockName) + "\n";
                messageToShow += (i+1).ToString() + ". " + l.note + "\n";
            }
            EditorUtility.DisplayDialog("Inquire success! ", unityUserName + " holds the lock(s) of: \n"+ messageToShow, "OK");
        }
    }

    [MenuItem("Assets/XLock/Inquire Lock By UserName", true)]
    static bool ValidateRightMouseInquireLockByUserName()
    {
        return XGlobalFlag.xGlobalFlag;
    }
    
    
    [MenuItem("Assets/XLock/Inquire Lock By UserName")]
    static void RightMouseInquireLockByUserName()
    {
        try
        {
            XInquireLockByUserNameWindow.showWindow();
        }
        catch (Exception e)
        {
//            _gotoPreviousStage = true;
            // EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    // P6: List all locks
    public static void ListAllLocks()
    {      
        // string json = "";
        string requestBody = "";
        
        PostRequest("listAll", requestBody);

        XAnalyseJsonWithLocks jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonWithLocks>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseFileLocks = jsonAnalysed.result;
        // _responseFileLocks = jsonAnalysed.data;

        Debug.Log("ListAllLocks function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("List all locks failed! ",  _responseErr, "OK");
            throw new Exception("List all locks failed! " + _responseErr);
        }
        else
        {
            string messageToShow = "";
            for (int i = 0; i < _responseFileLocks.Length; i++)
            {
                var l = _responseFileLocks[i];
                //messageToShow += (i+1).ToString()+ ". " + AssetDatabase.GUIDToAssetPath(l.lockName) + ": " + l.lockOwner + "\n";
                messageToShow += (i+1).ToString()+ ". " + l.note + ": " + l.lockOwner + "\n";
                
            }
            EditorUtility.DisplayDialog("List locks success!", messageToShow, "OK");
        }
    }
    
    [MenuItem("Assets/XLock/List all locks", true)]
    static bool ValidateRightMouseListAllLocks()
    {
        return XGlobalFlag.xGlobalFlag;
    }
    
    [MenuItem("Assets/XLock/List all locks")]
    static void RightMouseListAllLocks()
    {
        try
        {
            ListAllLocks();
        }
        catch (Exception e)
        {
//            _gotoPreviousStage = true;
//            EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    // P7: Unlock all my locks
    public static void UnlockAllMyFiles()
    {
        try
        {
            CheckUnityUserName();

        }
        catch (Exception e)
        {
            throw e;
        }
        
        var unityUserName = PlayerPrefs.GetString("UnityUserName");
        string requestBody = "args= -userName " + "\"" + unityUserName + "\"";
        PostRequest("listOwnLock", requestBody);
        
        XAnalyseJsonWithLocks jsonAnalysedWithLocks = JsonUtility.FromJson<XAnalyseJsonWithLocks>(_request.downloadHandler.text);

        _responseErr = jsonAnalysedWithLocks.err;
        _responseFileLocks = jsonAnalysedWithLocks.result;
        // _responseFileLocks = jsonAnalysed.data;

        Debug.Log("InquireLockByUserName function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("Inquire lock by user name failed! " , _responseErr, "OK");
            throw new Exception("Inquire lock by user name failed! " + _responseErr);
        }
        else
        {
            string messageToShow = "You have unlocked the following files: ";

            for (int i = 0; i < _responseFileLocks.Length; i++)
            {
                _request = null;
                var l = _responseFileLocks[i];
                var guid = l.lockName;
                var prefabPath = AssetDatabase.GUIDToAssetPath(l.lockName);
                
                requestBody = "args= -userName " + "\"" + unityUserName  + "\"" + " -lockName " + guid;
                PostRequest("unlock", requestBody);
                
                XAnalyseJsonBasic jsonAnalysedBasic = JsonUtility.FromJson<XAnalyseJsonBasic>(_request.downloadHandler.text);
                _responseErr = jsonAnalysedBasic.err;
                _responseString = jsonAnalysedBasic.result;
                Debug.Log("Unlock function:");
                Debug.Log("Sent: "+ requestBody);
                Debug.Log("Received: " + _request.downloadHandler.text);
        
                if (!string.IsNullOrEmpty(_responseErr ))
                {
                    EditorUtility.DisplayDialog("Unlock file failed! " ,  _responseErr, "OK");
                    throw new Exception("Unlock file failed! " + _responseErr);
                }
                else
                {
                    messageToShow = messageToShow + "\n" + (i+1).ToString() + ". " + prefabPath;
                    //EditorUtility.DisplayDialog("Success!", "Unlock the " + prefabPath+ " success!", "OK");
                }
            }
            
            EditorUtility.DisplayDialog("Unlock all files success! ", messageToShow, "OK");
        }
    }
    
    [MenuItem("Assets/XLock/Unlock all my files", true)]
    static bool ValidateRightMouseUnlockAllMyFiles()
    {
        return XGlobalFlag.xGlobalFlag;
    }
    
    [MenuItem("Assets/XLock/Unlock all my files")]
    static void RightMouseUnlockAllMyFiles()
    {
        try
        {
            UnlockAllMyFiles();
        }
        catch (Exception e)
        {
//            _gotoPreviousStage = true;
//            EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    // P8: List n logs
    public static void ListNLogs(int num)
    {      
        // string json = "{\"Num\":" + num + "}";
        var requestBody = "args= -num " + num;
        
        PostRequest("listOpLog", requestBody);

//        var downloadHandlerTemp = _request.downloadHandler.text.Replace("\n", "");
//        downloadHandlerTemp = downloadHandlerTemp.Replace("\\", "");
//        Debug.Log(downloadHandlerTemp);
//        Debug.Log(_request.downloadHandler.text);
        XAnalyseJsonWithStringArray jsonAnalysed = JsonUtility.FromJson<XAnalyseJsonWithStringArray>(_request.downloadHandler.text);

        _responseErr = jsonAnalysed.err;
        _responseStringArray = jsonAnalysed.result;
        // _responseStringArray = jsonAnalysed.data;

        Debug.Log("ListNLogs function:");
        Debug.Log("Sent: "+ requestBody);
        Debug.Log("Received: " + _request.downloadHandler.text);
        
        if (!string.IsNullOrEmpty(_responseErr ))
        {
            EditorUtility.DisplayDialog("List " + num + " logs failed! " , _responseErr, "OK");
            throw new Exception("List " + num + " logs failed! " + _responseErr);
        }
        else
        {
            var messageToShow = "";
            for (int i = 0; i < _responseStringArray.Length; i++)
            {
                var s = _responseStringArray[i];
                messageToShow += (i+1).ToString()+ ". " + s + "\n";
                
            }
            EditorUtility.DisplayDialog("Latest " + num + " Logs:", messageToShow, "OK");
        }
    }
    
    [MenuItem("Assets/XLock/List n logs", true)]
    static bool ValidateRightMouseListNLogs()
    {
        return XGlobalFlag.xGlobalFlag;
    }
    
    [MenuItem("Assets/XLock/List n logs")]
    static void RightMouseListNLogs()
    {
        try
        {
            XListNLogsWindow.ShowWindow();
        }
        catch (Exception e)
        {
//            _gotoPreviousStage = true;
//            EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
    // P9: Reset Xlock UserName
    
    [MenuItem("Assets/XLock/Reset Xlock username", true)]
    static bool ValidateRightMouseResetUsername()
    {
        return XGlobalFlag.xGlobalFlag;
    }
    
    [MenuItem("Assets/XLock/Reset Xlock username")]
    static void RightMouseResetUsername()
    {
        try
        {
            XSetUserNameWindow.ShowWindow();
        }
        catch (Exception e)
        {
//            _gotoPreviousStage = true;
//            EditorUtility.DisplayDialog("Error", e.ToString(), "OK");
        }
    }
    
}